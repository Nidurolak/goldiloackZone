using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnCount : MonoBehaviour
{
    public List<Image> Counts = new List<Image>();
    public List<Sprite> Numbers = new List<Sprite>();
    public Datamanager DM;

    public Slider gage;

    public int OldTurnNum;

    public IEnumerator Watching()
    {
        WaitForEndOfFrame a = new WaitForEndOfFrame();
        while (true)
        {
            yield return a;
            if(OldTurnNum != DM.CurrentTurn)
            {

                Setting();
                OldTurnNum = DM.CurrentTurn;
            }
        }

    }
    public void Setting()
    {
        var a = DM.CurrentTurn.ToString();
        gage.minValue = 0;
        gage.maxValue = DM.MaxTurn;
        gage.DOValue(DM.MaxTurn - DM.CurrentTurn, 3, false);
        if(DM.CurrentTurn < 10)
        {
            Counts[0].sprite = Numbers[0];
            Counts[1].sprite = Numbers[0];
            Counts[2].sprite = Numbers[DM.CurrentTurn];
        }
        else if (DM.CurrentTurn <= 99 && DM.CurrentTurn >= 10)
        {
            Counts[0].sprite = Numbers[0];
            Counts[1].sprite = Numbers[int.Parse(a.Substring(0, 1))];
            Counts[2].sprite = Numbers[int.Parse(a.Substring(1, 1))];
        }
        else if (DM.CurrentTurn >= 100)
        {
            Counts[0].sprite = Numbers[int.Parse(a.Substring(0, 1))];
            Counts[1].sprite = Numbers[int.Parse(a.Substring(1, 1))];
            Counts[2].sprite = Numbers[int.Parse(a.Substring(2, 1))];
        }
    }
    public void Awake()
    {
        StartCoroutine(Watching());
    }
}
