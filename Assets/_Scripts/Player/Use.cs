using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Use : MonoBehaviour
{
    public PlayerInputHandler playerInputs;

    public Clothes playerClothes;
    public Clothes targetClothes;
    
    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask interactableLayers;

    public Transform originPoint;

    public event Action<IInteractable> AnnounceInteracting;

    public void Awake()
    {
        playerInputs.AnnounceUseAction += TryUse;
    }

    //open radial context menu
    private void TryUse(InputAction.CallbackContext context)
    {
        if(context.canceled)
           InteractInSphere();
    }

    public void InteractInSphere()
    {
        Collider[] hits = Physics.OverlapSphere(originPoint.position, radius, interactableLayers);

        foreach (Collider col in hits)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable == null)
            {
                // Try on parent if not found on current collider
                interactable = col.GetComponentInParent<IInteractable>();
            }

            if (interactable != null)
            {
                //assumes clothes for now
                //TODO: change
                targetClothes = col.gameObject.GetComponent<Clothes>();
                if (targetClothes == null)
                    targetClothes = col.gameObject.GetComponentInParent<Clothes>();
                if (targetClothes != null )
                {
                    if (!targetClothes.Clothed)
                    {
                        //fix
                        playerClothes.clothesCharacter = targetClothes.clothesCharacter;
                        playerClothes.body.material.color =
                            playerClothes.clothesColorDict[targetClothes.clothesCharacter];
                    }
                    else
                        interactable.Interact(CharacterActions.Undress);
                }

                AnnounceInteracting?.Invoke(interactable);
            }
        }
    }
}
