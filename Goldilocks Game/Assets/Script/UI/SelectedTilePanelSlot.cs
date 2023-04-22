using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SelectedTilePanelSlot : MonoBehaviour
{
    public GameObject Icon;
    public Image image;
    public Text text;

    private void OnDisable()
    {
        image.sprite = null;
        text.text = null;
    }
}
