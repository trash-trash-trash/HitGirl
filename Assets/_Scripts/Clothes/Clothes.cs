using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    public Character clothesCharacter;

    private Color skinColor;
    
    public Dictionary<Character, Color> clothesColorDict = new Dictionary<Character, Color>();

    //fix later with models
    public MeshRenderer head;
    public MeshRenderer body;
    
    private bool clothed = true;

    public bool Clothed
    {
        get { return clothed; }
        set { clothed = value; }
    }

    public SkinTones skinTones;

    public void Awake()
    {
        clothesColorDict.Add(Character.Civilian, Color.green);
        clothesColorDict.Add(Character.Guard, Color.blue);
        clothesColorDict.Add(Character.Target, Color.red);
        clothesColorDict.Add(Character.Player, Color.black);

        int rand = Random.Range(0, skinTones.skinTones.Count);
        skinColor = skinTones.skinTones[rand];
        head.material.color = skinColor;

        body.material.color = clothesColorDict[clothesCharacter];
    }

    public void Undress()
    {
        Clothed = false;
        body.material.color = skinColor;
    }
}
