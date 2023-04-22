using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.IO;
using System;

public class ExplorerPanel : UIMove
{
    private Coroutine a_Corutine;
    private Coroutine b_Corutine;
    public UISound uiSound;

    public Vector2 DownPosition;
    public Vector2 UpPosition;
    public bool UpCheck;
    

    public override void Slot_Move()
    {
        Debug.Log("작동");
        if (UpCheck == false)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, UpPosition, UpCheck, 1.3f));
            uiSound.PlaySound();
            UpCheck = true;
        }
        else if (UpCheck == true)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, DownPosition, UpCheck, 1.3f));
            uiSound.PlaySound();
            UpCheck = false;
        }
    }
    public void Slot_Move2()
    {
        if (UpCheck == true)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, DownPosition, UpCheck, 1.3f));
            uiSound.PlaySound();
            UpCheck = false;
        }
    }
}
