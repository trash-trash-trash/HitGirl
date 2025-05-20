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
            direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            head.rotation = Quaternion.RotateTowards(head.rotation, targetRotation, headTurnSpeed * Time.deltaTime);
        }
    }
}
