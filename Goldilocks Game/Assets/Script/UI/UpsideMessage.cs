using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpsideMessage : UIMove
{
    public Vector2 DownPosition;
    public Vector2 UpPosition;
    public bool MoveCheck;
    public Text Message;

    public override void Slot_Move()
    {
        if (MoveCheck == false)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, UpPosition, MoveCheck,0.7f));
            MoveCheck = true;
        }
        else if (MoveCheck == true)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, DownPosition, MoveCheck, 0.7f));
            MoveCheck = false;
        }
    }

    public void MessageChange(string newMessage)
    {
        Message.text = newMessage;


    }
}
