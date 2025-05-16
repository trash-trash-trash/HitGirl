using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, IWeapon
{
    public Transform pointA;  
    public Transform pointB;  
    public LayerMask hitLayers;

    public Player player;
    public float shootCooldownTime;
    
    public bool canShoot = true;
    
    //assumes only player will shoot...
    //aggroAction should pass along CharacterBase 
    void Shoot()
    {
        if (!canShoot)
            return; 
        if (pointA != null && pointB != null) 
        { 
            Vector3 direction = pointB.position - pointA.position; 
            float distance = direction.magnitude;

            RaycastHit hit;
            if (Physics.Raycast(pointA.position, direction.normalized, out hit, distance, hitLayers))
            {
                Debug.Log("Hit object: " + hit.collider.name);
                Debug.DrawLine(pointA.position, hit.point, Color.red);

                Health hp = hit.collider.gameObject.GetComponent<Health>();
                if (hp != null)
                {
                    hp.ChangeHP(-1);
                }
            }
            else
            {
                Debug.DrawLine(pointA.position, pointB.position, Color.green);
            }

            StartCoroutine(ShootCooldownCoro());
        }
        
    }
    
    IEnumerator ShootCooldownCoro()
    { 
        player.AggroAction = true;
        
        canShoot = false;
        yield return new WaitForSeconds(shootCooldownTime);
        canShoot = true;
        player.AggroAction = false;
    }

    public void AggroAction()
    {
        Shoot();
    }
}
