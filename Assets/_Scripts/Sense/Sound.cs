using UnityEngine;
using System.Collections.Generic;

public struct SoundData
{
    //redundancy w/MemoryData
    public Vector3 soundOrigin;
    public Transform originObj;
    public CharacterBase soundSource;
    public bool closeSound;
    public float soundDecayTime;
}

public class Sound : MonoBehaviour
{
    public float soundFarDistance = 15f;
    public float soundCloseDistance = 5f;
    public float decayTime = 5f;
    public LayerMask hearingLayerMask; 
    public LayerMask environmentLayerMask;

    public void EmitSound(CharacterBase newSoundSource)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, soundFarDistance, hearingLayerMask);

        foreach (Collider col in hitColliders)
        {
            IHear hearer = col.GetComponent<IHear>();
            if (hearer == null || hearer == gameObject.GetComponent<IHear>()) continue;

            Vector3 direction = (col.transform.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, col.transform.position);

            // Raycast to detect obstruction
            if (Physics.Raycast(transform.position + direction * 0.1f, direction, out RaycastHit hitInfo, soundFarDistance, environmentLayerMask))
            {
                if (hitInfo.collider.gameObject != gameObject || hitInfo.collider.gameObject != gameObject.transform.parent.gameObject && hitInfo.distance < distanceToTarget)
                {
                    SoundData obstructedSound = new SoundData
                    {
                        soundOrigin = transform.position,
                        originObj = transform,
                        soundSource = newSoundSource,
                        closeSound = false,
                        soundDecayTime = decayTime
                    };

                    hearer.HeardSound(obstructedSound);
                    continue;
                }
            }

            // Not obstructed, determine if close or far
            bool isClose = distanceToTarget <= soundCloseDistance;
    
            SoundData newSound = new SoundData
            {
                soundOrigin = transform.position,
                originObj = transform,
                soundSource = newSoundSource,
                soundDecayTime = decayTime,
                closeSound = isClose
            };
            hearer.HeardSound(newSound);
            Debug.Log("Made sound");
        }
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, soundFarDistance);
    //
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(transform.position, soundCloseDistance);
    // }
}
