using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class RadialMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite spriteImage;
    public TMP_Text text;
    public Button button;

    public IInteractable interactable;

    public event Action<RadialMenuButton, bool> AnnouncePointerEntry;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnnouncePointerEntry?.Invoke(this, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnnouncePointerEntry?.Invoke(this, false);
    }
}
