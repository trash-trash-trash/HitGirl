using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shove : MonoBehaviour, IWeapon
{
    public Player player;
    
    [Header("Shove Settings")]
    //atm origin is player body, rotation is cameraArm
    public Transform shoveOrigin;
    public Transform shoveRotation;
    public Vector3 boxHalfExtents = new Vector3(1, 1, 1);
    public Vector3 boxOffset = Vector3.forward;
    public float shoveHorizontalForce = 10f;
    public float shoveVerticalForce = 5f;
    public float shoveTorque = 5f;
    public LayerMask shoveLayer;

    public bool pushedSomeone;
    
    public bool canShove = true;
    public float shoveCooldown = 0.5f;

    public void TryShove()
    {
        if (!canShove)
            return;
        
            //se the Y rotation for forward-facing direction
            Quaternion yRotation = Quaternion.Euler(0f, shoveRotation.eulerAngles.y + 90, 0f);

            //offset by z
            Vector3 forwardOffset = yRotation * Vector3.forward * boxHalfExtents.z;
            Vector3 center = shoveOrigin.position + forwardOffset;

            Collider[] hits = Physics.OverlapBox(center, boxHalfExtents, yRotation, shoveLayer);

            pushedSomeone = false;

            foreach (Collider hit in hits)
            {
                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null && rb != GetComponentInChildren<Rigidbody>())
                {
                    //assumes only player will use shove... change 
                    NPCAnthillBase brain = rb.gameObject.GetComponent<NPCAnthillBase>();
                    if (brain != null)
                    {
                        if (brain.sight.canSeePlayer)
                            brain.alert = true;
                    }
                    
                    Health hp = rb.gameObject.GetComponent<Health>();
                    if(hp!=null)
                        hp.ChangeHP(0);

                    rb.constraints = RigidbodyConstraints.None;
                    
                    //add force
                    Vector3 direction = (rb.position - shoveOrigin.position).normalized;
                    Vector3 shoveVelocity = direction * shoveHorizontalForce;
                    shoveVelocity.y = shoveVerticalForce;
                    rb.linearVelocity = shoveVelocity;
                    
                    //add torque
                    Vector3 shoveDirection = (rb.position - shoveOrigin.position).normalized;
                    Vector3 randomOffset = Random.insideUnitSphere * 0.2f;
                    Vector3 torqueAxis = Vector3.Cross(Vector3.up, shoveDirection + randomOffset).normalized;

                    rb.AddTorque(torqueAxis * shoveTorque, ForceMode.Impulse);

                    Debug.Log("Shoved " + rb.gameObject.name);
                    if (!pushedSomeone)
                        pushedSomeone = true;
            }
            StartCoroutine(ShoveCooldownCoro());
        }
    }

    IEnumerator ShoveCooldownCoro()
    {
        if(pushedSomeone)   
            player.AggroAction = true;
        
        canShove = false;
        yield return new WaitForSeconds(shoveCooldown);
        canShove = true;
        player.AggroAction = false;
        pushedSomeone = false;
    }

    public void Equip(WeaponSO weaponSO)
    {
        
    }

    public void Aim()
    {
        
    }

    public void Holster()
    {
        
    }

    public void AggroAction()
    {
        TryShove();
    }

    public IWeapon ReturnSelf()
    {
        return this;
    }
}
