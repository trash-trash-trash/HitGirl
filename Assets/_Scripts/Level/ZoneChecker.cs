using UnityEngine;
using System.Linq;

public class ZoneChecker : MonoBehaviour
{
    public Zone currentArea;

    public float zoneCheckRadius = 1f;

    public LayerMask zoneLayer;

    public bool CheckIsPlayerCharacterAllowedInArea(CharacterBase player)
    {
        UpdateCurrentArea(player.gameObject.transform);

        if (currentArea.allowedCharacters.Contains(player.clothes.clothesCharacter))
            return true;

        return false;
    }

    public void UpdateCurrentArea(Transform targetTransform)
    {
        Collider[] hits = Physics.OverlapSphere(targetTransform.position, zoneCheckRadius, zoneLayer);

        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.name}");
            Zone zone = hit.GetComponent<Zone>();
            if (zone != null)
            {
                currentArea = zone;
                return;
            }
        }

        currentArea = null;
    }
}
