using System;
using System.Collections.Generic;
using UnityEngine;

//based on https://www.youtube.com/watch?v=tdkdRguH_dE
public class RadialMenu : MonoBehaviour
{
    public RadialMenuButton lastSelectedButton;
    public List<RadialMenuButton> menuButtons = new List<RadialMenuButton>();
    public float radius = 300f;

    public GameObject menuButtonPrefab;

    public Use use;

    private float angle=90f;

    private void Start()
    {
        use.AnnounceInteractableFound += AddButton;
        use.AnnounceDoneChecking += Rearrange;
        use.AnnounceCloseMenu += CloseMenu;
    }

    public void AddButton(IInteractable interactable)
    {
        GameObject butt = Instantiate(menuButtonPrefab, transform);
        RadialMenuButton rmb = butt.GetComponent<RadialMenuButton>();
        menuButtons.Add(rmb);
        //IInteractable returns sprite etc?

        rmb.AnnouncePointerEntry += SetButton;
        
        butt.SetActive(true);
    }

    private void SetButton(RadialMenuButton arg1, bool input)
    {
        if (input)
            lastSelectedButton = arg1;
        else
            lastSelectedButton = null;
    }

    void Rearrange()
    {
        int count = menuButtons.Count;
        float angleStep = 360f / count;

        float startingAngle = count % 2 == 1 ? angle : angle + (angleStep / 2f);

        for (int i = 0; i < count; i++)
        {
            float angleDegrees = startingAngle + angleStep * i;
            float angleRadians = angleDegrees * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleRadians) * radius;
            float y = Mathf.Sin(angleRadians) * radius;

            menuButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }


    public void CloseMenu()
    {
        if(lastSelectedButton!=null)
            //lastSelectedButton.interactable.Interact(use);
        
        foreach (RadialMenuButton butt in menuButtons)
        {
            Destroy(butt.gameObject);
        }
        menuButtons.Clear();
    }
}
