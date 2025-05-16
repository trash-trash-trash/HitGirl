using UnityEngine;
using System.Collections.Generic;

public struct SoundData
{
    public Vector3 soundOrigin;
    public Transform originObj;
    public bool closeSound;
    public float decayTime;
}

public class Sound : MonoBehaviour
{
    public float soundFarDistance = 15f;
    public float soundCloseDistance = 5f;
    public float soundDecayTime = 5f;
    public LayerMask hearingLayerMask; 
    public LayerMask environmentLayerMask;

    public void EmitSound()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, soundFarDistance, hearingLayerMask);

        SoundData newSound = new SoundData
        {
            soundOrigin = transform.position,
            originObj = transform,
            closeSound = false,
            decayTime = soundDecayTime
        };
        
        foreach (Collider col in hitColliders)
        {
            Debug.Log("hit "+col.name);
            IHear hearer = col.GetComponent<IHear>();
            if (hearer == null|| hearer == gameObject.GetComponent<IHear>()) continue;

            Debug.Log(hearer+" hears "+gameObject.name);
            
            Vector3 direction = (col.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, col.transform.position);

            // Raycast to detect obstruction
            if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, soundFarDistance, environmentLayerMask))
            {
                // If environment is between source and hearer
                if (hitInfo.distance < distanceToTarget)
                {
                    hearer.HeardSound(newSound);
                    continue;
                }
            }

            // No obstruction or target is closer than obstruction
            if (distanceToTarget <= soundCloseDistance)
            {
                newSound.closeSound = true;
                hearer.HeardSound(newSound);
            }
            else
            {
                hearer.HeardSound(newSound);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soundFarDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, soundCloseDistance);
    }
}
