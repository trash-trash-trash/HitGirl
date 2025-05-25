using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class InteractableRegistry
{
    public static List<IInteractable> AllInteractables = new List<IInteractable>();

    public static void Register(IInteractable i)
    {
        if (!AllInteractables.Contains(i))
        { 
            if(i is Door door)
             Debug.Log("added door");

            AllInteractables.Add(i);
        }
    }

    public static void Unregister(IInteractable i)
    {
        AllInteractables.Remove(i);
    }
    
    public static IInteractable FindBlockingDoor(NavMeshPath path)
    {
        foreach (var interactable in AllInteractables)
        {
            if (interactable is Door door)
            {
                Debug.Log("checking door");
                Vector3 doorPosition = door.transform.position;
                float detectionRadius = 5f;
                
                for (int i = 0; i < path.corners.Length; i++)
                {
                    float distanceToDoor = Vector3.Distance(path.corners[i], doorPosition);
                    if (distanceToDoor <= detectionRadius)
                    {
                        Debug.Log($"Path corner near door at distance: {distanceToDoor}");
                        return door;
                    }
                }
            }
        }
        return null;
    }
}