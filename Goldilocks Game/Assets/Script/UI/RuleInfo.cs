using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class RuleInfo : MonoBehaviour
{
    public RuleManager RM;
    public RuleInfo RivalRule;
    public List<RuleInfo> BeforeRules = new List<RuleInfo>();
    public List<RuleInfo> NextRules = new List<RuleInfo>();

    public Button thisButton;
    public GameObject OutLine_On;
    public GameObject OutLine_Off;

    public string ID;
    public string Name;
    public string Name2;
    public string Flavor;
    public string Content;
    public string Effect;
    public bool isWorking;
    public bool canOpen;

    public int UnlockPanelsNumber_Tier;
    public RuleManager.Side side;


    //이게 전부 온 워킹해야 법안 해금됨
    public List<int> UnlockTarget = new List<int>();
    //언락타겟의 사이드를 정함
    public List<RuleManager.Side> UnlockTarget_side = new List<RuleManager.Side>();
    //언락 체크할 법안 숫자를 따짐.
    public int UnlockCount;

    public List<Slider> UnlockSlider = new List<Slider>();
    public List<GameObject> UnlockSlider_XIcon = new List<GameObject>();

    //룰 인포랑 액티브 법안은 따로 뺴야겠어.
    public enum RuleCategory
    {
        None, Passive, Active, UnlockBuilding, ManPower, Food, Resoucre, UnlockSpacial, UnlockUI, Hope, Comp, Will
    }
    public RuleCategory RC;

    public enum EffectCategory
    {
        None, UnlockSpacial
    }
    public EffectCategory EC;

    public enum ActiveType
    {
        None, Random, Continue, Onetime
    }
    ActiveType AT;
    public int NextRuleDelay;
    public int CoolTime;
    public int ActiveTime;

    public List<int> RandomMinMax = new List<int>();


    public int Type;
    public int Value;

    public int Will;
    public int Hope;
    public int Complaint;

    public void Call_Selected_Rule_This()
    {
        RM.RuleButton_Active.RemoveAllListeners();
        if (RM.Selected_Now != null) 
        {
            RM.Selected_Now.OutLine_Off.SetActive(false);

            if (RM.Selected_Now.UnlockSlider_XIcon.Count != 0 && RM.Selected_Now.isWorking == false)
            {
                for (int i = 0; i < RM.Selected_Now.UnlockSlider_XIcon.Count; i++)
                {
                    RM.Selected_Now.UnlockSlider_XIcon[i].SetActive(false);
                }
            }
            

            RM.Selected_Now = RM.Selected_Before;
        }
        RM.Selected_Now = this;

        if (isWorking == true)
        {
            RM.RTP.Seal_Active(false);
            if (RM.Selected_Now.UnlockSlider_XIcon.Count != 0)
            {
                for (int i = 0; i < RM.Selected_Now.UnlockSlider_XIcon.Count; i++)
                {
                    RM.Selected_Now.UnlockSlider_XIcon[i].SetActive(true);
                }
            }

            OutLine_On.SetActive(true);
        }
        else if(isWorking == false)
        {
            RM.RTP.Seal.gameObject.SetActive(false);
            if (RM.Selected_Now.UnlockSlider_XIcon.Count != 0)
            {
                for (int i = 0; i < RM.Selected_Now.UnlockSlider_XIcon.Count; i++)
                {
                    RM.Selected_Now.UnlockSlider_XIcon[i].SetActive(true);
                }
            }

            OutLine_Off.SetActive(true);
        }

        RM.Show_Rule_Selected(this);
        RM.RuleButton_Active.AddListener(Rule_Enforcement);
    }

    //대기시간이 0이 아닐 때도 만들어야해
    //여기서 텍스트 변화하고 이것저것
    public void Rule_Enforcement()
    {
        switch (side)
        {
            case RuleManager.Side.Left:
                RM.UnlockedRulePanels_Left[UnlockPanelsNumber_Tier] = true;
                break;
            case RuleManager.Side.Middle:
                RM.UnlockedRulePanels_Middle[UnlockPanelsNumber_Tier] = true;
                break;
            case RuleManager.Side.Right:
                RM.UnlockedRulePanels_Right[UnlockPanelsNumber_Tier] = true;
                break;
        }
        Debug.Log("법안 발동");
        RM.RTP.Seal_Active(true);
        RM.DM.All_Resource_List[54] += NextRuleDelay;
        RM.RTP.OkButton.interactable = false;

        if(RivalRule != null)
        {
            RivalRule.thisButton.interactable = false;
        }

        for(int i = 0; i < UnlockSlider.Count; i++)
        {
            UnlockSlider[i].DOValue(UnlockSlider[i].maxValue, 2f, false);
        }

        isWorking = true;
        OutLine_Off.SetActive(false);
        OutLine_On.SetActive(true);

        RM.DM.All_Resource_List[31] += Hope;
        RM.DM.All_Resource_List[32] += Complaint;
        RM.DM.All_Resource_List[30] += Will;
        RM.DM.Check_Mind();

        switch (RC)
        {
            case RuleCategory.Active:
                break;
            case RuleCategory.Passive:
                break;
            case RuleCategory.ManPower:
                RM.DM.All_Resource_List[40] += Value;
                break;
            case RuleCategory.UnlockBuilding:
                RM.DM.Ub[Type].Unlock_Version[Value] = true;
                break;
            case RuleCategory.Food:
                StartCoroutine(RC_Food_Reset());
                break;
            case RuleCategory.Resoucre:
                switch (Type)
                {
                    //건물 기본 방재
                    case 21:
                        RM.DM.All_Resource_List[21] += Value;
                        for (int i = 0; i < RM.DM.TM.OnEnableTileList.Count; i++)
                        {
                            RM.DM.TM.OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                        }
                        
                        break;
                    case 40:
                        RM.DM.All_Resource_List[40] += Value;
                        RM.DM.ManPower_Setting();
                        RM.DM.ManPower_Caculate();
                        break;
                    default:
                        RM.DM.Rule_Production_Type_Magnification_List[Type] += Value;
                        break;
                }
                break;
                
            case RuleCategory.UnlockSpacial:

                break;

            case RuleCategory.UnlockUI:

                switch (Value)
                {
                    case 9:
                        RM.RuleActive_009();
                        break;
                }
                break;
            case RuleCategory.Hope:
                RM.DM.All_Resource_List[33] += Type;
                break;
            case RuleCategory.Comp:
                RM.DM.All_Resource_List[34] += Type;
                break;
            case RuleCategory.Will:
                RM.DM.All_Resource_List[35] += Type;
                break;
        }
        switch (EC)
        {
            //언락 스페셜은 데이터매니저 목록의 스페셜 언락을 해금한다. 
            case EffectCategory.UnlockSpacial:
                RM.DM.Spacial_Effect_Unlock[1].List[RM.ruleInfos.IndexOf(this)] = true;
                break;
        }
    }

    public void Start()
    {
        Image b = OutLine_Off.GetComponent<Image>();
        b.DOFade(0.7f, 3f).SetLoops(-1, LoopType.Yoyo);
    }

    public void Update()
    {
        canOpen = true;
        int b = 0;
        if(BeforeRules.Count > 0)
        {
            for (int i = 0; i < BeforeRules.Count; i++)
            {
                if (BeforeRules[i].isWorking == true)
                {
                    b++; 
                }
            }
            if(UnlockCount == b)
            {
                canOpen = true;
            }
            else
            {
                canOpen = false;
            }
        }
        if (RivalRule.isWorking == true)
        {
            canOpen = false;
        }
        if(isWorking == true)
        {
            canOpen = true;
        }
        if (canOpen == false)
        {
            thisButton.interactable = false;
        }
        else if(canOpen == true)
        {
            thisButton.interactable = true;
        }
    }


    public IEnumerator RC_Food_Reset()
    {
        RM.Reset_Food_RuleActive(true);
        yield return new WaitForSeconds(0.05f);
        RM.DM.Spacial_Effect_Unlock[1].List[Value] = true;
        yield return new WaitForSeconds(0.05f);
        RM.Reset_Food_RuleActive(false);

        yield return null;
    }
}
