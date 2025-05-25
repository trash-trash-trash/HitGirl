using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Door : MonoBehaviour, IInteractable
{
    public bool opensInwards=false;
    public bool open = false;
    public bool isOpening=false;
    private bool canInteract = true;
    
    public Transform pivot;
    public Quaternion initialRotation;

    public float doorRotation = 90f;
    public float doorMoveTime = 1f;
    private IInteractable _interactableImplementation;

    void Start()
    { 
        InteractableRegistry.Register(this);
        initialRotation = pivot.rotation;
    }
    
    public void OpenCloseDoor()
    {
        StopCoroutine(OpenCloseDoorCoro());
        StartCoroutine(OpenCloseDoorCoro());
    }

    IEnumerator OpenCloseDoorCoro()
    {
        isOpening = true;

        Quaternion targetRotation;
        if (!open)
        {
            float yRotate = opensInwards ? doorRotation : -doorRotation;
            targetRotation = initialRotation * Quaternion.Euler(0, yRotate, 0);
        }
        else
        {
            targetRotation = initialRotation;
        }

        float time = 0f;
        float duration = doorMoveTime;
        Quaternion startRotation = pivot.rotation;

        while (time < duration)
        {
            pivot.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        pivot.rotation = targetRotation;

        if (pivot.rotation == initialRotation)
            open = false;
        else
            open = true;

        isOpening = false;
    }

    public void Interact(IInteract interactee, CharacterActions actionType)
    {
        OpenCloseDoor();
    }

    public GameObject ReturnSelf()
    {
        return gameObject;
    }

    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }
    public bool ReturnCanInteract()
    {
        return CanInteract;
    }
}
