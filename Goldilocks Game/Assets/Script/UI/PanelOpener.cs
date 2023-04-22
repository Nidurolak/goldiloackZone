using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelOpener : UIMove
{
    public bool DownCheck;
    public bool WorkingAtOther;
    public UISound uISound;
    public GameObject Panel;
    
    public Vector2 DownPosition;
    public Vector2 UpPosition;

    public float x;
    public float y;

    public void Checking()
    {
        if (DownCheck == false)
        {
            DownCheck = true;
        }
        else if (DownCheck == true)
        {
            DownCheck = false;
        }
    }

    public override void Slot_Move()
    {
        if (WorkingAtOther == false)
        {
            if (DownCheck == false)
            {
                StartCoroutine(Slot_Move_Coroutine(Panel, UpPosition, DownCheck, 0.7f));
                DownCheck = true;
            }
            else if (DownCheck == true)
            {
                StartCoroutine(Slot_Move_Coroutine(Panel, DownPosition, DownCheck, 0.7f));
                DownCheck = false;
            }
        }
    }
}
