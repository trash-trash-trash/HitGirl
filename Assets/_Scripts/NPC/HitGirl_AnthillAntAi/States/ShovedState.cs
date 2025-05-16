using System.Collections;
using UnityEngine;

public class ShovedState : NPCAnthillStateBase
{
    private Rigidbody rb;

    public override void Enter()
    {
        base.Enter();
        rb = scenarioBrain.GetComponent<Rigidbody>();
        scenarioBrain.navMeshAgent.enabled = false;
        
        //emit specific "shoved sound"
        scenarioBrain.sound.EmitSound();
        
        scenarioBrain.sight.CanSee = false;
        scenarioBrain.characterBase.CanHear = false;
        StartCoroutine(WaitForGetUp());
    }

    //change to be interruptible later
    IEnumerator WaitForGetUp()
    {
        yield return new WaitForSeconds(2f);
        //wait until velocity is low enough
        while (rb.linearVelocity.magnitude > 1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        scenarioBrain.debugText.SetText("Getting up");
        //freeze everything and attempt stand up
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Quaternion uprightRotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
        float elapsed = 0f;
        float duration = 0.5f;
        Quaternion startRotation = rb.rotation;

        while (elapsed < duration)
        {
            rb.MoveRotation(Quaternion.Slerp(startRotation, uprightRotation, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MoveRotation(uprightRotation);

        scenarioBrain.sight.CanSee = true;
        scenarioBrain.characterBase.CanHear = true;
        scenarioBrain.navMeshAgent.enabled = true;
        scenarioBrain.shoved = false;
    }
}
