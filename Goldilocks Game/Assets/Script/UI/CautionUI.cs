using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class CautionUI : MonoBehaviour
{
    private Sequence CautionSequence;

    public Color baseText;
    public Color targetText;
    //폰트 기본 색은 206 223 125 목표색은 255 0 0
    public Datamanager DM;
    public Button TurnButton;
    private WaitForSeconds wait = new WaitForSeconds(0.1f);

    public List<CautionIcon> CautionList_Off = new List<CautionIcon>();
    public List<CautionIcon> CautionList_On = new List<CautionIcon>();

    public CautionIcon Medi;
    public CautionIcon FoodConsume;

    public List<bool> Caution_Checker = new List<bool>();

    public void Check_CannotWorking_TurnButton()
    {
        CautionSequence.Kill();
        if (TurnButton.interactable == false)
        {
            for (int i = 0; i < CautionList_On.Count; i++)
            {
                if (CautionList_On[i].ResourceType <= 9 && CautionList_On[i].isDoColor == false) 
                {
                    StartCoroutine(Check_CannotWorking_TurnButton_Docoloring(CautionList_On[i]));
                }
            }
        }
    }

    public IEnumerator Check_CannotWorking_TurnButton_Docoloring(CautionIcon a)
    {
        a.isDoColor = true;
        a.Contents.DOColor(targetText, 0.3f).SetLoops(10, LoopType.Yoyo);
        yield return new WaitForSeconds(4f);
        a.isDoColor = false;
    }

    public IEnumerator Check_Caution()
    {
        while (true)
        {
            for (int i = 0; i < 9; i++)
            {
                if (DM.All_Resource_List[i] + DM.All_Resource_PerTurn_List_Total[i] < 0)
                {
                    if (i <= 9)
                    {
                        Caution_Active(CautionList_Off[0], true, i, DM.resources(i, false) + " 부족", "이번 턴에 소비하게 될 " + DM.resources(i, false) + DM.resources(i, true) + " 남아있는 양보다 많습니다.");
                    }
                }
                else if (DM.All_Resource_List[i] + DM.All_Resource_PerTurn_List_Total[i] >= 0 && Caution_Checker[i] == true)
                {
                    for(int j = 0; j < CautionList_On.Count; j++)
                    {
                        if (CautionList_On[j].ResourceType == i || i <= 9)
                        {
                            Caution_Active(CautionList_On[j], false, i, "", "");
                        }
                    }
                }
            }
            switch (DM.medicalState)
            {
                case Datamanager.MedicalState.Good:
                    Medi.Icon.SetActive(true);
                    Medi.Contents.text = "보건 양호";
                    Medi.TooltipText.text = "시민의 대부분이 건강한 상태입니다. 인구당 인력비가 약간 줄어듭니다.";
                    break;
                case Datamanager.MedicalState.Normal:
                    Medi.Icon.SetActive(false);
                    Medi.Contents.text = "";
                    Medi.TooltipText.text = "";
                    break;
                case Datamanager.MedicalState.Bad:
                    Medi.Icon.SetActive(true);
                    Medi.Contents.text = "보건 불량";
                    Medi.TooltipText.text = "희망이 감소하고 인구 당 인력 수치가 증가합니다. 인구당 인력비가 늘어납니다.";
                    break;
                case Datamanager.MedicalState.Collapse:
                    Medi.Icon.SetActive(true);
                    Medi.Contents.text = "의료 붕괴";
                    Medi.TooltipText.text = "희망이 감소하고 인구 당 인력 수치가 증가하며 병사자가 나옵니다.인구당 인력비가 늘어납니다.";
                    break;
            }
            //식량 체크만 따로 하자. 시민 소비량을 체크 못했어 위에건 엉터리야. 아래걸 참고하자

            int a = DM.All_Resource_List[6] + DM.All_Resource_PerTurn_List_Total[6] + DM.Consume_Food_Value();

            int b = 0;
            if (a < 0)
            {
                b = DM.All_Resource_List[5] + DM.All_Resource_PerTurn_List_Total[5] + a;
            }

            int c = 0;
            if(b < 0)
            {
                c = DM.All_Resource_List[7] + DM.All_Resource_PerTurn_List_Total[7] + b;
            }


            if (a < 0)
            {
                FoodConsume.Icon.SetActive(true);
                FoodConsume.Contents.text = "배식 불가";
                FoodConsume.TooltipText.text = "당장 시민들이 먹을 식량이 충분하지 않습니다. 배식을 받지 못한 시민은 통조림이나 식자재를 먹을 것입니다.";
            }
            else if (a >= 0)
            {
                FoodConsume.Icon.SetActive(false);
            }

            if (b < 0)
            {
                Debug.Log(b);

                FoodConsume.Icon.SetActive(true);
                FoodConsume.Contents.text = "식량 고갈";
                FoodConsume.TooltipText.text = "식량은 커녕 식자재도 고갈되었습니다. 당장 해결하지 않으면 다들 굶어죽을 것입니다.";
            }
            else if(b >= 0 && a >= 0)
            {
                FoodConsume.Icon.SetActive(false);
            }


            if (DM.OverCrowding == true)
            {
                Caution_Active(CautionList_Off[0], true, 17, "주거지 부족", "거주가능한 공간이 부족합니다. 사람들이 불만을 표시합니다.");
            }
            for(int i = 0; i < CautionList_On.Count; i++)
            {
                if (DM.OverCrowding == false)
                {
                    Caution_Active(CautionList_On[i], false, 17, "", "");
                }
            }
            if (Caution_Checker[0] == true ||
                Caution_Checker[1] == true ||
                Caution_Checker[2] == true ||
                Caution_Checker[3] == true ||
                Caution_Checker[4] == true ||
                Caution_Checker[8] == true)
            {
                TurnButton.interactable = false;
            }
            else
            {
                TurnButton.interactable = true;
            }
            yield return wait;
        }
    }

    public void Caution_Active(CautionIcon a, bool isOn, int b, string c, string d)
    {
        //경고등 작동
        if(isOn == true && Caution_Checker[b] == false)
        {
            a.Icon.SetActive(true);
            a.TooltipText.text = d;
            a.Contents.text = c;
            a.ResourceType = b;
            CautionList_Off.Remove(a);
            CautionList_On.Add(a);
            Caution_Checker[b] = true;
        }
        else if (isOn == false && Caution_Checker[b] == true)
        {
            a.Icon.SetActive(false);
            a.TooltipText.text = "";
            a.Contents.text = "";
            a.ResourceType = 0;
            CautionList_Off.Add(a);
            CautionList_On.Remove(a);
            Caution_Checker[b] = false;
        }
    }

    public void OnEnable()
    {
        StartCoroutine(Check_Caution());
        CautionSequence = DOTween.Sequence();
    }
}

[System.Serializable]
public class CautionIcon
{
    public GameObject Icon;
    public GameObject Tooltip;
    public Text Contents;
    public Text TooltipText;
    public int ResourceType;
    public bool isDoColor;
}