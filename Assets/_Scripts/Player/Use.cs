using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Use : MonoBehaviour, IInteract
{
    public PlayerInputHandler playerInputs;

    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask interactableLayers;

    public Transform originPoint;

    public event Action<IInteractable> AnnounceInteractableFound;
    public event Action AnnounceDoneChecking;

    public event Action AnnounceCloseMenu;

    public void Awake()
    {
        playerInputs.AnnounceUseAction += TryUse;
    }

    //open radial context menu
    //cursor probably needs more robust control
    private void TryUse(InputAction.CallbackContext context)
    {
        if(context.performed)
            InteractInSphere();
        else if (context.canceled)
        {
            AnnounceCloseMenu?.Invoke();
            Cursor.visible = false;
        }
    }

    public void InteractInSphere()
    {
        bool foundSomething = false;
        Collider[] hits = Physics.OverlapSphere(originPoint.position, radius, interactableLayers);

        foreach (Collider col in hits)
        {
            //is this dumb?
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable == null)
            {
                // Try on parent if not found on current collider
                interactable = col.GetComponentInParent<IInteractable>();
            }

            if (interactable != null)
            {
                //interactable.Interact(this);

                AnnounceInteractableFound?.Invoke(interactable);
                
                //only target one at a time
               // break;
               foundSomething = true;
            }
        }

        if (foundSomething)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        AnnounceDoneChecking?.Invoke();
    }
}
