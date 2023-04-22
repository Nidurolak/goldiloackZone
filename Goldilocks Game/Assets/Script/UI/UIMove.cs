using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class UIMove : MonoBehaviour
{
    public Sequence sequence = null;

    public void Start()
    {
        sequence = DOTween.Sequence();
    }

    public IEnumerator Slot_Move_Coroutine(GameObject Target, Vector2 TargetPos, bool MoveCheck, float MoveTime)
    {
        RectTransform a = (RectTransform)Target.transform;
        sequence.Kill();
        if (MoveCheck == false)
        {
            sequence.Append(a.DOAnchorPos(TargetPos, MoveTime, false));
            sequence.Play();
            yield return null;
        }
        else if (MoveCheck == true)
        {
            sequence.Append(a.DOAnchorPos(TargetPos, MoveTime, false));
            sequence.Play();
            yield return null;
        }
    }

    public abstract void Slot_Move();


}
