using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCHeadLook : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float headTurnSpeed = 50f;

    public Vector3 pointOfInterest;
    public bool lookingAtPointOfInterest;

    [SerializeField] private NavMeshAgent agent;

    public float lookingAroundWaitTime = 2f;
    
    public bool lookingAround=false;

    public void FlipLookingAt(Vector3 newPOI, bool input)
    {
        if(input)
            pointOfInterest = newPOI;
        
        lookingAtPointOfInterest = input;
    }

    private void Update()
    {
        Vector3 direction = lookingAtPointOfInterest ? pointOfInterest - head.position : agent.velocity;
        RotateHead(direction);
    }

    private void RotateHead(Vector3 direction)
    {
        if(!lookingAtPointOfInterest)
            direction.y = transform.rotation.y;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            head.rotation = Quaternion.RotateTowards(head.rotation, targetRotation, headTurnSpeed * Time.deltaTime);
        }
    }

    public void LookAround()
    {
        lookingAround = true;
        StartCoroutine(LookAroundCoro());
    }

    IEnumerator LookAroundCoro()
    {
        Vector3 neutralLookPoint = head.position + transform.forward * 2f;
        Vector3 leftDirection = Quaternion.Euler(0, -65, 0) * head.transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0, 65, 0) * head.transform.forward;
        
        Vector3 leftLookPoint = head.position + leftDirection * 2f;
        Vector3 rightLookPoint = head.position + rightDirection * 2f;

        bool lookLeftFirst = Random.value < 0.5f;

        FlipLookingAt(lookLeftFirst ? leftLookPoint : rightLookPoint, true);
        yield return new WaitUntil(() => HeadFacingTarget());
        yield return new WaitForSeconds(lookingAroundWaitTime);

        // second look
        FlipLookingAt(lookLeftFirst ? rightLookPoint : leftLookPoint, true);
        yield return new WaitUntil(() => HeadFacingTarget());
        yield return new WaitForSeconds(lookingAroundWaitTime);
        
        FlipLookingAt(neutralLookPoint, true);
        yield return new WaitUntil(() => HeadFacingTarget());

        FlipLookingAt(Vector3.zero, false);
        lookingAround = false;
    }

    private bool HeadFacingTarget()
    {
        Vector3 direction = pointOfInterest - head.position;

        if (direction.sqrMagnitude < 0.01f) return false;

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        float angleDifference = Quaternion.Angle(head.rotation, targetRotation);
        return angleDifference < 1f;
    }

}
