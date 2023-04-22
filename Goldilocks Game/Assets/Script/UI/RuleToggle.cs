using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleToggle : MonoBehaviour
{
    public Button thisButton;
    public Button RivalButton0;
    public Button RivalButton1;

    public void ButtonSwap()
    {
        thisButton.interactable = false;
        if (RivalButton0 != null)
        {
            RivalButton0.interactable = true;
        }
        if (RivalButton1 != null)
        {
            RivalButton1.interactable = true;
        }
    }

}
