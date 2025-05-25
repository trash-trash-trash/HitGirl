using UnityEngine;

public class ClothesItem : MonoBehaviour, IInteractable
{
    public Clothes clothes;
    private bool canInteract = true;

    public void Interact(IInteract interactee, CharacterActions actionType)
    {
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
