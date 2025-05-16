using TMPro;
using UnityEngine;

public class DebugStateText : MonoBehaviour
{
    public TMP_Text stateText;

    public void SetText(string input)
    {
        stateText.text = input;
    }
}
