using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class RuleManager : MonoBehaviour
{
    public Datamanager DM;
    public RuleTreePanel RTP;
    public RuleInfo Selected_Now;
    public RuleInfo Selected_Before;
    public List<RuleInfo> ruleInfos = new List<RuleInfo>();
    public List<bool> ruleInfos_Activating = new List<bool>();
    public List<bool> ruleInfos_Activating_first = new List<bool>();
    public List<RuleInfo> ruleInfos_On = new List<RuleInfo>();
    public UnityEvent RuleButton_Active;

    public UnityEvent RuleButton_TicketOpen;
    //public UnityEvent RuleButton_Active;


    //여기에 할당된 숫자의 법안 패널은 이미 완료됨
    public enum Side {None, Left, Middle, Right}
    public List<bool> UnlockedRulePanels_Left = new List<bool>();
    public List<bool> UnlockedRulePanels_Middle = new List<bool>();
    public List<bool> UnlockedRulePanels_Right = new List<bool>();

    [System.Serializable]
    public class RuleWorker
    {
        public int Duration;
        public int CoolTime;
        public int ManPower;
    }

    public int Rule_005_BaseCount_Max;
    public int Rule_005_BaseDuration_Max;
    public int Rule_005_BaseCoolTime_Max;
    public int Rule_005_BaseManPower_Max;
    public List<RuleWorker> Rule_005_List = new List<RuleWorker>();

    public void WorkerCalc()
    {
        for (int i = Rule_005_List.Count - 1; i >= 0; i--)
        {

            Rule_005_List[i].Duration--;
            if (Rule_005_List[i].Duration == 0)
            {
                DM.All_Resource_List[24] -= Rule_005_List[i].ManPower;
            }
            Rule_005_List[i].CoolTime--;
            if (Rule_005_List[i].CoolTime <= 0)
            {
                Rule_005_List.Remove(Rule_005_List[i]);
            }
        }
        //foreach(RuleWorker i in Rule_005_List)
        //{
        //    if(i.CoolTime == 0)
        //    {
        //        Rule_005_List.Remove(i);
        //    }
        //}
        DM.ManPower_Setting();
        DM.ManPower_Caculate();
    }


    public void AddRuleWorker(int duration, int coolTime, int manPower)
    {
        RuleWorker a = new RuleWorker();
        a.Duration = duration;
        a.CoolTime = coolTime;
        a.ManPower = manPower;

        DM.All_Resource_List[24] += manPower;

        Rule_005_List.Add(a);
    }

    public void Rule_active()
    {
        RuleButton_Active.Invoke();
    }
    public void Show_Rule_Selected(RuleInfo a)
    {
        RTP.NameText.text = a.Name;
        RTP.NameText2.text = a.Name2;
        RTP.ContentsText.text = a.Content;
        if(DM.All_Resource_List[54] > 0)
        {
            RTP.Seal.gameObject.SetActive(false);
            RTP.OkButton.interactable = false;
            RTP.WaitinhText.text = "시민들이 이전 법안에 적응 중입니다. " + DM.All_Resource_List[54] + "턴이 필요합니다.";
        }
        else if (a.NextRuleDelay > 0 && DM.All_Resource_List[54] ==0)
        {
            RTP.Seal.gameObject.SetActive(false);
            RTP.OkButton.interactable = true;
            RTP.WaitinhText.text = "시민들이 이 법안에 적응하는데 " + a.NextRuleDelay + "턴이 필요합니다.";
        }
        if(a.isWorking == true)
        {
            RTP.Seal_Active(false);
            RTP.OkButton.interactable = false;
            RTP.WaitinhText.text = "이 법안은 이미 발효되었습니다.";
        }
    }

    //언락된 특수기술들은 따로 스크립트 할당해야겠어. 추가배식, 묽은 죽 들어가는 전용패널 만들자
    //사용되지 않음
    public void On_Rule_Button()
    {
        if(DM.All_Resource_List[54] <= 0)
        {
            switch (Selected_Now.EC)
            {
                case RuleInfo.EffectCategory.UnlockSpacial:
                    switch (Selected_Now.Type)
                    {
                        //특수 기능해제
                        case 6:
                            //묽은 죽인데 이건 전용 버튼을 패널에 따로 할당해서 온 액티브 하자
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                        case 9:
                            break;
                    }
                    break;
            }
        }
    }
    
    public void Reset_Food_RuleActive(bool isReset)
    {
        //추가배식 체크
        if(ruleInfos_Activating[0] == true)
        {
            if(isReset == true)
            {
                RuleActive_001();
            }
            else
            {
                ruleInfos_Activating[0] = false;
            }
        }
        else if(ruleInfos_Activating[0] == false)
        {
            if (isReset == true)
            {
                ruleInfos_Activating[0] = true;
            }
            else
            {
                RuleActive_001();
            }
        }

        //배식제한 체크
        if (ruleInfos_Activating[1] == true)
        {
            if (isReset == true)
            {
                RuleActive_002();
            }
            else
            {
                ruleInfos_Activating[1] = false;
            }
        }
        else if (ruleInfos_Activating[1] == false)
        {
            if (isReset == true)
            {
                ruleInfos_Activating[1] = true;
            }
            else
            {
                RuleActive_002();
            }
        }
        
    }

    public void RuleActive_001_002_SetToNormal()
    {
        if(ruleInfos[0].isWorking == true)
        {
            RuleActive_001();
        }
        else if(ruleInfos[1].isWorking == true)
        {
            RuleActive_002();
        }
    }

    public void RuleActive_003_004_SetToNormal()
    {
        if (ruleInfos[2].isWorking == true)
        {
            RuleActive_003();
        }
        else if (ruleInfos[3].isWorking == true)
        {
            RuleActive_004();
        }
    }

    //추가배식
    public void RuleActive_001()
    {
        //추가배식이 꺼져있으면 킨다.
        if (ruleInfos_Activating[0] == false)
        {
            ruleInfos_Activating[0] = true;
            //체중관리
            if (DM.Spacial_Effect_Unlock[1].List[6] == true)
            {
                    DM.All_Resource_List[40] -= 1;
                    DM.ManPower_Setting();
                DM.ManPower_Caculate();
                ruleInfos_Activating_first[6] = false;
            }

            //배식량속이기
            if (DM.Spacial_Effect_Unlock[1].List[7] == true)
            {
                    DM.Rule_Food_Magnification -= 0.3f;
                    DM.All_Resource_PerTurn_List_Total[31] -= 1;
                ruleInfos_Activating_first[7] = false;
            }

            DM.Rule_Food_Magnification += 0.3f;
            DM.All_Resource_PerTurn_List_Total[32] -= 2;
        }

        //켜져있으면 끈다
        else if (ruleInfos_Activating[0] == true)
        {
            ruleInfos_Activating[0] = false;
            //체중관리
            if (DM.Spacial_Effect_Unlock[1].List[6] == true)
            {
                if (ruleInfos_Activating_first[6] == false)
                {
                    DM.All_Resource_List[40] += 1;
                    DM.ManPower_Setting();
                    DM.ManPower_Caculate();
                }
                ruleInfos_Activating_first[6] = false;
            }

            //배식량속이기
            if (DM.Spacial_Effect_Unlock[1].List[7] == true)
            {
                if (ruleInfos_Activating_first[7] == false)
                {
                    DM.Rule_Food_Magnification += 0.3f;
                    DM.All_Resource_PerTurn_List_Total[31] += 1;
                }
                ruleInfos_Activating_first[7] = false;
            }

            DM.Rule_Food_Magnification -= 0.3f;
            DM.All_Resource_PerTurn_List_Total[32] += 2;
        }
        DM.TM.ResetForTech();
    }

    //배식 제한
    public void RuleActive_002()
    {
        if (ruleInfos_Activating[1] == false)
        {
            ruleInfos_Activating[1] = true;
            DM.Rule_Food_Magnification -= 0.3f;
            DM.All_Resource_PerTurn_List_Total[32] += 2;
        }

        else if (ruleInfos_Activating[1] == true)
        {
            ruleInfos_Activating[1] = false;
            DM.Rule_Food_Magnification += 0.3f;
            DM.All_Resource_PerTurn_List_Total[32] -= 2;
        }
        DM.TM.ResetForTech();
    }

    //묽은 수프
    public void RuleActive_003()
    {
        if (ruleInfos_Activating[2] == false)
        {
            ruleInfos_Activating[2] = true;
            DM.Rule_Production_Type_Magnification_List[7] += 0.2f;
            DM.All_Resource_PerTurn_List_Total[32] -= 3;
        }

        else if (ruleInfos_Activating[2] == true)
        {
            ruleInfos_Activating[2] = false;
            DM.Rule_Production_Type_Magnification_List[7] -= 0.2f;
            DM.All_Resource_PerTurn_List_Total[32] += 3;

        }
        DM.TM.ResetForTech();
    }

    //푸짐한 식사
    public void RuleActive_004()
    {
        if (ruleInfos_Activating[3] == false)
        {
            ruleInfos_Activating[3] = true;
            DM.Rule_Production_Type_Magnification_List[7] -= 0.2f;
            DM.All_Resource_PerTurn_List_Total[32] -= 3;
        }

        else if (ruleInfos_Activating[3] == true)
        {
            ruleInfos_Activating[3] = false;
            DM.Rule_Production_Type_Magnification_List[7] += 0.2f;
            DM.All_Resource_PerTurn_List_Total[32] += 3;

        }
        DM.TM.ResetForTech();
    }

    //비상근무 
    public void RuleActive_005()
    {
        int a;
        int b;
        int c;

        DM.All_Resource_List[32] += 15;
        DM.All_Resource_List[31] -= 10;

        if (Rule_005_BaseCount_Max > Rule_005_List.Count)
        {
            a = 2;
            b = 4;
            c = 2;

            AddRuleWorker(a,b,c);
            DM.ManPower_Setting();
            DM.ManPower_Caculate();
        }
        else if(Rule_005_BaseCount_Max <= Rule_005_List.Count)
        {
            Debug.Log("워커 미작동");
        }
        DM.TM.ResetForTech();
    }

    //단축근무
    public void RuleActive_006()
    {
        if (ruleInfos_Activating[5] == false)
        {
            ruleInfos_Activating[5] = true;
            DM.Rule_Production_Type_Magnification_List[40] += 3;
            DM.All_Resource_PerTurn_List_Total[32] -= 2;
            DM.All_Resource_PerTurn_List_Total[30] += 2;
        }

        else if (ruleInfos_Activating[5] == true)
        {
            ruleInfos_Activating[5] = false;
            DM.Rule_Production_Type_Magnification_List[40] -= 3;
            DM.All_Resource_PerTurn_List_Total[32] += 2;
            DM.All_Resource_PerTurn_List_Total[30] -= 2;

        }

        DM.TM.ResetForTech();
        DM.ManPower_Setting();
        DM.ManPower_Caculate();
    }

    public void RuleActive_009()
    {
        Debug.Log("우헤헤");
        DM.SPM.SpacialUI[0].SetActive(true);
    }


    //환자 현장관리
    public void RuleActive_010()
    {
        if (ruleInfos_Activating[5] == false)
        {
            ruleInfos_Activating[5] = true;
            DM.Rule_Production_Type_Magnification_List[40] += 3;
            DM.All_Resource_PerTurn_List_Total[32] -= 2;
            DM.All_Resource_PerTurn_List_Total[30] += 2;
        }

        else if (ruleInfos_Activating[5] == true)
        {
            ruleInfos_Activating[5] = false;
            DM.Rule_Production_Type_Magnification_List[40] -= 3;
            DM.All_Resource_PerTurn_List_Total[32] += 2;
            DM.All_Resource_PerTurn_List_Total[30] -= 2;

        }

        DM.ManPower_Setting();
        DM.ManPower_Caculate();
    }

}
