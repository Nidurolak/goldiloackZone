using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class TechTreeManager : MonoBehaviour
{
    public Datamanager DM;
    public Text Tech_Name;
    public Text Tech_Category;

    public Text WorkingPowerText;
    public Text TechPowerText;

    public GameObject PassivePanel;
    public Text Tech_Passive;
    public GameObject UnlockPanel;
    public GameObject ButtonPanel;
    public Text Tech_Unlock;
    public Image Tech_Unlock_Image;
    public Text Tech_Flavor;
    public Text Tech_Flavor2;
    public Button UpgradeButton;
    public List<GameObject> CostPanel = new List<GameObject>();
    public List<Text> CostPanelText = new List<Text>();
    public UnityEvent UpgradeButton_Active;
    public List<TechInfo> TechList = new List<TechInfo>();
    public List<bool> TechList_Active = new List<bool>();


    private TechInfo TI;

    Coroutine a;
    Coroutine b;

    public void ShowInfo(TechInfo techInfo)//Sprite image, string name, string contents, int techPowerNeede, int WorkPowerNeeded)
    {
        TI = techInfo;
        UpgradeButton_Active.RemoveAllListeners();

        Tech_Name.text = techInfo.Name;
        switch (techInfo.TC1)
        {
            case TechInfo.TechCategory.UnlockTech:
                Tech_Category.text = "기술 해금";
                break;
            case TechInfo.TechCategory.UnlockBuilding:
                Tech_Category.text = "건물 해금";
                break;
            case TechInfo.TechCategory.Building_Production:
                Tech_Category.text = "생산량 증감";
                break;
            case TechInfo.TechCategory.Building_TurnCount:
                Tech_Category.text = "건설속도 감소";
                break;
            case TechInfo.TechCategory.Building_Upkeep:
                Tech_Category.text = "유지비 증감";
                break;
            case TechInfo.TechCategory.Building_BuildCost:
                Tech_Category.text = "건설비용 증감";
                break;
            case TechInfo.TechCategory.Building_BuffAreaSize:
                Tech_Category.text = "영향범위 증대";
                break;
            case TechInfo.TechCategory.UpgradeSteelWhale:
                Tech_Category.text = "스틸웨일 개량";
                break;
            case TechInfo.TechCategory.Building_Anti:
                Tech_Category.text = "방재 강화";
                break;
            case TechInfo.TechCategory.ManPower:
                Tech_Category.text = "인력비 증감";
                break;
        }
        switch (techInfo.TC2)
        {
            case TechInfo.TechCategory.UnlockTech:
                Tech_Category.text += ", 기술 해금";
                break;
            case TechInfo.TechCategory.UnlockBuilding:
                Tech_Category.text += ", 건물 해금";
                break;
            case TechInfo.TechCategory.Building_Production:
                Tech_Category.text += ", 생산량 증감";
                break;
            case TechInfo.TechCategory.Building_TurnCount:
                Tech_Category.text += ", 건설속도 감소";
                break;
            case TechInfo.TechCategory.Building_Upkeep:
                Tech_Category.text += ", 유지비 증감";
                break;
            case TechInfo.TechCategory.Building_BuildCost:
                Tech_Category.text += ", 건설비용 증감";
                break;
            case TechInfo.TechCategory.Building_BuffAreaSize:
                Tech_Category.text += ", 영향범위 증대";
                break;
            case TechInfo.TechCategory.UpgradeSteelWhale:
                Tech_Category.text += ", 스틸웨일 개량";
                break;
            case TechInfo.TechCategory.None:
                break;
        }
        switch (techInfo.TC3)
        {
            case TechInfo.TechCategory.UnlockTech:
                Tech_Category.text += ", 기술 해금";
                break;
            case TechInfo.TechCategory.UnlockBuilding:
                Tech_Category.text += ", 건물 해금";
                break;
            case TechInfo.TechCategory.Building_Production:
                Tech_Category.text += ", 생산량 증감";
                break;
            case TechInfo.TechCategory.Building_TurnCount:
                Tech_Category.text += ", 건설속도 감소";
                break;
            case TechInfo.TechCategory.Building_Upkeep:
                Tech_Category.text += ", 유지비 증감";
                break;
            case TechInfo.TechCategory.Building_BuildCost:
                Tech_Category.text += ", 건설비용 증감";
                break;
            case TechInfo.TechCategory.Building_BuffAreaSize:
                Tech_Category.text += ", 영향범위 증대";
                break;
            case TechInfo.TechCategory.UpgradeSteelWhale:
                Tech_Category.text += ", 스틸웨일 개량";
                break;
            case TechInfo.TechCategory.ManPower:
                Tech_Category.text += ", 인력비 증감";
                break;
            case TechInfo.TechCategory.Building_Anti:
                Tech_Category.text += ", 건물 방재 증감";
                break;
            case TechInfo.TechCategory.None:
                break;
        }
        if (techInfo.Thumbnail2 == null)
        {
            PassivePanel.SetActive(true);
            UnlockPanel.SetActive(false);
            Tech_Passive.text = techInfo.Contents;
        }
        else if (techInfo.Thumbnail2 != null)
        {
            PassivePanel.SetActive(false);
            UnlockPanel.SetActive(true);
            Tech_Unlock_Image.sprite = techInfo.Thumbnail2;
            Tech_Unlock.text = techInfo.Contents;
        }
        bool c = CheckToUnlock(techInfo);
        Debug.Log(c);
        if (DM.All_Resource_List[12] < techInfo.TechPowerNeeded ||
            DM.All_Resource_List[13] < techInfo.WorkPowerNeeded ||
            c == false)
        {
            UpgradeButton.interactable = false;
        }
        else
        {
            UpgradeButton.interactable = true;
        }

        if(techInfo.TechPowerNeeded != 0)
        {
            CostPanel[0].SetActive(true);
            CostPanelText[0].text = techInfo.TechPowerNeeded.ToString();
        }
        else
        {
            CostPanel[0].SetActive(false);
        }

        if (techInfo.WorkPowerNeeded != 0)
        {
            CostPanel[1].SetActive(true);
            CostPanelText[1].text = techInfo.WorkPowerNeeded.ToString();
        }
        else
        {
            CostPanel[1].SetActive(false);
        }

        if (a != null && b != null)
        {
            StopCoroutine(a);
            StopCoroutine(b);
        }
        //a = StartCoroutine(Contents(Tech_Flavor, techInfo.Flavor));
        //b = StartCoroutine(Contents(Tech_Flavor2, techInfo.Flavor2));
        Tech_Flavor.text = techInfo.Flavor;
        Tech_Flavor2.text = techInfo.Flavor2;

        if (techInfo.isWorking == true)
        {
            ButtonPanel.SetActive(false);
        }
        else if (techInfo.isWorking == false)
        {
            ButtonPanel.SetActive(true);
            UpgradeButton_Active.AddListener(techInfo.Upgrade);
        }
    }

    public bool CheckToUnlock(TechInfo teckInfo)
    {
        var a = 0;
        if (teckInfo.NeedToUnlock.Count > 0)
        {
            for (int i = 0; i < teckInfo.NeedToUnlock.Count; i++)
            {
                if (teckInfo.NeedToUnlock[i].isWorking == true)
                {
                    a++;
                }
            }
        }
        bool b = false;

        Debug.Log(a + "   " + teckInfo.NeedToUnlock.Count);

        if (a == teckInfo.NeedToUnlock.Count)
        {
            b = true;
        }
        return b;
    }

    public void DoUpgrade()
    {
        UpgradeButton_Active.Invoke();
        //타일매니저의 리셋포테크를 적용시키는 거 잊지말고
    }

    public void Awake()
    {
        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
        a.GetComponent<Datamanager>().TTM = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (DM.All_Resource_List[12] < TI.TechPowerNeeded ||
                DM.All_Resource_List[13] < TI.WorkPowerNeeded ||
                CheckToUnlock(TI) == true)
            {
                DoUpgrade();
            }
        }
        WorkingPowerText.text = DM.All_Resource_List[13] + " / " + c(DM.All_Resource_PerTurn_List_Total[13]);
        TechPowerText.text = DM.All_Resource_List[12] + " / " + c(DM.All_Resource_PerTurn_List_Total[12]);
    }

    public IEnumerator Contents(Text Flavor, string TargetFlavor)
    {
        int a = 0;
        string b = "";

        for(int i =0; i < TargetFlavor.Length; i++)
        {
            b += TargetFlavor[i];
            Flavor.text = b;
            yield return new WaitForSeconds(0.025f);
        }
    }
    private string c(int b)
    {
        var c = "";

        if (b > 0)
        {
            c = "+" + b;
        }
        else
        {
            c = b.ToString();
        }

        return c;
    }

}
