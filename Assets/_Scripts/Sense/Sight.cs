using System;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour, ISee
{
    private bool canSee = true;

    public bool CanSee
    {
        get { return canSee; }
        set { canSee = value; }
    }
    
    public bool canSeePlayer = false;

    public List<CharacterBase> charactersInVision = new List<CharacterBase>();
    public List<GameObject> objectsInVision = new List<GameObject>();
    
    public event Action<bool> AnnounceCanSee;
    public event Action<CharacterBase> AnnounceCanSeeCharacter;
    public event Action<bool> AnnounceCanSeePlayer;
    
    public LayerMask layersThatCanBlock;
    public LayerMask layersToIgnore;

    public Transform eyes;
    
    void OnTriggerStay(Collider other)
    {
        if (!canSee) return;
        TryProcessVision(other);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canSee) return;
        TryProcessVision(other);
    }

    private void TryProcessVision(Collider other)
    {
        GameObject otherGameObject = other.gameObject;

        if (otherGameObject == gameObject)
            return;

        if (((1 << otherGameObject.layer) & layersToIgnore) != 0)
            return;

        bool hasLOS = CheckLineOfSight(eyes, other.transform, layersThatCanBlock);

        if (hasLOS)
        {
            if (!objectsInVision.Contains(otherGameObject))
                objectsInVision.Add(otherGameObject);

            CharacterBase icharacter = otherGameObject.GetComponent<CharacterBase>();
            if (icharacter != null)
                AddRemoveCharacterFromVision(icharacter, true);
        }
        else
        {
            if (objectsInVision.Contains(otherGameObject))
            {
                objectsInVision.Remove(otherGameObject);

                CharacterBase icharacter = otherGameObject.GetComponent<CharacterBase>();
                if (icharacter != null)
                    AddRemoveCharacterFromVision(icharacter, false);
            }
        }
    }

    private bool CheckLineOfSight(Transform startTransform, Transform targetTransform, LayerMask blockingMask)
    {
        Vector3 direction = targetTransform.position - startTransform.position;
        float distance = direction.magnitude;
        Ray ray = new Ray(startTransform.position, direction.normalized);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, blockingMask))
        {
            if (hit.transform != targetTransform && !hit.transform.IsChildOf(targetTransform))
                return false;
        }

        return true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;

        if (objectsInVision.Contains(otherGameObject))
        {
            objectsInVision.Remove(otherGameObject);

            CharacterBase icharacter = otherGameObject.GetComponent<CharacterBase>();
            if (icharacter != null)
                AddRemoveCharacterFromVision(icharacter, false);
        }
    }
    
    public void AddRemoveCharacterFromVision(CharacterBase character, bool isVisible)
    {
        bool alreadyInVision = charactersInVision.Contains(character);

        if (isVisible)
        {
            if (!alreadyInVision)
            {
                charactersInVision.Add(character);

                if (character.character == Character.Player)
                    ChangeCanSeePlayer(character, true);
            }

            AnnounceCanSeeCharacter?.Invoke(character);
        }
        else
        {
            if (alreadyInVision)
            {
                charactersInVision.Remove(character);

                if (character.character == Character.Player)
                    ChangeCanSeePlayer(null, false);
            }
        }
    }


    
    public void ChangeCanSee(bool input)
    {
        canSee = input;
        ChangeCanSeePlayer(null, false);
        objectsInVision.Clear();
        charactersInVision.Clear();
        AnnounceCanSee?.Invoke(canSee);
    }

    public void ChangeCanSeePlayer(CharacterBase player, bool input)
    {
        canSeePlayer = input;

        if (input)
        {
            Player newPlayer = player.GetComponent<Player>();

            if (newPlayer.AggroAction)
                AnnounceCanSeePlayer?.Invoke(canSeePlayer);
        }
    }

    public bool ReturnCanSee()
    {
        return canSee;
    }
    
    public bool ReturnCanSeePlayer()
    {
        return canSeePlayer;
    }
}