using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public Transform pointOfInterest;
    
    private void Awake()
    {
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }
}
