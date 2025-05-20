using System.Collections;
using UnityEngine;

public class ShovedState : NPCAnthillStateBase
{
    private Rigidbody rb;
    private Coroutine getUpCoroutine;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool interrupted = false;

    public override void Enter()
    {
        base.Enter();

        rb = scenarioBrain.GetComponent<Rigidbody>();

        // Capture the current rotation (flatten Y for upright logic)
        Vector3 euler = rb.rotation.eulerAngles;
        startRotation = Quaternion.Euler(euler.x, euler.y, euler.z);
        targetRotation = Quaternion.Euler(0f, euler.y, 0f);

        interrupted = false;
        getUpCoroutine = StartCoroutine(WaitForGetUp());
    }

    public void InterruptGetUp()
    {
        interrupted = true;
        if (getUpCoroutine != null)
            StopCoroutine(getUpCoroutine);
    }
    public override void Exit()
    {
        base.Exit();
            InterruptGetUp();
    }

    IEnumerator WaitForGetUp()
    {
        float countdownTime = 2f;
        float countdownElapsed = 0f;

        // Interruptible countdown phase
        while (countdownElapsed < countdownTime)
        {
            if (interrupted) yield break;
            countdownElapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        //wait for velocity to drop
        while (rb.linearVelocity.magnitude > 1f)
        {
            if (interrupted) yield break;
            yield return new WaitForSeconds(0.1f);
        }

        // Begin get-up rotation
        scenarioBrain.debugText.SetText("Getting up");

        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (interrupted) yield break;
            rb.MoveRotation(Quaternion.Slerp(startRotation, targetRotation, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MoveRotation(targetRotation);
        scenarioBrain.navMeshAgent.enabled = true;
        scenarioBrain.shoved = false;
    }
}
