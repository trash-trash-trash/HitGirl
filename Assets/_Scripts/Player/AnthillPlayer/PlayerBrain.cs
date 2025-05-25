using Anthill.AI;
using UnityEngine;

public class PlayerBrain : MonoBehaviour, ISense, IInteract
{
    public Health hp;
    public MouseLook mouseLook;
    public PlayerMove move;
    public Use use;
    
    public bool channelling = false;
    public bool interactMenu = false;
    public bool alive = true;
    public bool escaped = false;

    public void Start()
    {
        use.AnnounceInteractableFound += Interact;
        use.AnnounceCloseMenu += StopInteract;
    }

    //put logic in states?
    private void Interact(IInteractable obj)
    {
        interactMenu = true;
        mouseLook.canLook = false;
    }
    private void StopInteract()
    {
        interactMenu = false;
        mouseLook.canLook = true;
    }


    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
        aWorldState.Set(PlayerScenario.Channelling, channelling);
        aWorldState.Set(PlayerScenario.InteractMenu, interactMenu);
        aWorldState.Set(PlayerScenario.Alive, hp.Alive);
        aWorldState.Set(PlayerScenario.Escaped, escaped);
    }
}
