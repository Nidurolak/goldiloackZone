using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Datamanager DM;
    public Text NameText;
    public Text AntiText;
    public Text AllowText;
    public Text tooltipText;
    public RectTransform backgroundRectTransform;
    public List<SelectedTilePanelSlot> BuildtimeAndCost = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> ProductionAndKeep = new List<SelectedTilePanelSlot>();
    public List<Sprite> Icons = new List<Sprite>();
    public SelectedTilePanelSlot ElecNeeds;
    public SelectedTilePanelSlot SteamNeeds;

    //슬롯을 포함한 여러 곳에 쓰일 툴팁을 만들고 연습하는 곳이야 07/20

    private void Awake()
    {
        backgroundRectTransform = gameObject.transform.Find("Background").GetComponent<RectTransform>();
        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
        //tooltipText = gameObject.transform.Find("Text").GetComponent<Text>();
    }


    public void ShowToolTipText(string NameString, string ShortContentsString, int Anti, int Allow)
    {
        gameObject.SetActive(true);

        //텍스트를 지정한 값으로 변경
        NameText.text = NameString;
        tooltipText.text = ShortContentsString;
        if (Anti != 99)
        {
            switch (Anti)
            {
                case 0: AntiText.text = "기본 방재 : " + "없음";
                    break;
                case 1:
                    AntiText.text = "기본 방재 : " + "매우 낮음";
                    break;
                case 2:
                    AntiText.text = "기본 방재 : " + "낮음";
                    break;
                case 3:
                    AntiText.text = "기본 방재 : " + "약간 낮음";
                    break;
                case 4:
                    AntiText.text = "기본 방재 : " + "보통";
                    break;
                case 5:
                    AntiText.text = "기본 방재 : " + "약간 높음";
                    break;
                case 6:
                    AntiText.text = "기본 방재 : " + "높음";
                    break;
                case 7:
                    AntiText.text = "기본 방재 : " + "매우 높음";
                    break;
                case 8:
                    AntiText.text = "기본 방재 : " + "최고 수준";
                    break;
            }
        }
        else
        {
            AntiText.text = "";
        }
        if (Allow != 99)
        {
            switch (Allow)
            {
                case 0:
                    AllowText.text = "한계 오염 : " + "그린";
                    break;
                case -1:
                    AllowText.text = "한계 오염 : " + "그린-";
                    break;
                case -2:
                    AllowText.text = "한계 오염 : " + "옐로우";
                    break;
                case -3:
                    AllowText.text = "한계 오염 : " + "옐로우-";
                    break;
                case -4:
                    AllowText.text = "한계 오염 : " + "오렌지";
                    break;
                case -5:
                    AllowText.text = "한계 오염 : " + "오렌즈-";
                    break;
                case -6:
                    AllowText.text = "한계 오염 : " + "레드";
                    break;
                case -7:
                    AllowText.text = "한계 오염 : " + "레드-";
                    break;
                case -8:
                    AllowText.text = "한계 오염 : " + "블랙";
                    break;
            }
        }
        else
        {
            AllowText.text = "";
        }
    }
    public void ShowToolTipBuildCost(List<int> CostType, List<float> CostValue, int Time)
    {
        if (Time > 0)
        {
            BuildtimeAndCost[0].text.text = Time.ToString();
        }
        else
        {
            BuildtimeAndCost[0].text.text = "즉시 건설";
        }

        for (int i = 0; i < CostType.Count; i++)
        {
            BuildtimeAndCost[i + 1].Icon.SetActive(false);
        }

        for (int i = 0; i < CostType.Count; i++)
        {
            BuildtimeAndCost[i + 1].Icon.SetActive(true);
            BuildtimeAndCost[i + 1].image.sprite = Icons[CostType[i]];
            BuildtimeAndCost[i + 1].text.text = CostValue[i].ToString();
        }
    }

    public void ShowToolTipNeeds(bool Elec, bool Steam)
    {
        if(Elec == false)
        {
            ElecNeeds.Icon.SetActive(false);
        }
        else
        {
            ElecNeeds.Icon.SetActive(true);
        }
        if (Steam == false)
        {
            SteamNeeds.Icon.SetActive(false);
        }
        else
        {
            SteamNeeds.Icon.SetActive(true);
        }
    }

    public void ShowToolTipProductionAndKeep(int BuildingType, int BuildingVersion, int ProductionType, float ProductionValue, List<int> UpkeepType, List<float> UpkeepValue)
    {
        if (ProductionType != -1)
        {
            ProductionAndKeep[0].Icon.SetActive(true);
            ProductionAndKeep[0].image.sprite = Icons[ProductionType];
            if (ProductionType == 17 && ProductionValue != 0)
            {
                ProductionAndKeep[0].text.text = "+" + (ProductionValue + DM.Tech_Production_Type_Integer_List[ProductionType] + DM.Tech_Production_Version_Integer_List[BuildingType].Version_List[BuildingVersion]).ToString();
            }
            else
            {
                ProductionAndKeep[0].text.text = "+" + (ProductionValue + DM.Tech_Production_Type_Integer_List[ProductionType] + DM.Tech_Production_Version_Integer_List[BuildingType].Version_List[BuildingVersion]).ToString() + "/턴";
            }
        }
        else
        {
            ProductionAndKeep[0].Icon.SetActive(false);
        }

        for(int i = 0; i < UpkeepType.Count; i++)
        {
            if (UpkeepType[i] != -1)
            {
                ProductionAndKeep[i + 1].Icon.SetActive(true);
                ProductionAndKeep[i + 1].image.sprite = Icons[UpkeepType[i]];
                ProductionAndKeep[i + 1].text.text = "-" + UpkeepValue[i].ToString() + "/턴";
            }
            else
            {
                ProductionAndKeep[i + 1].Icon.SetActive(false);
            }
        }
    }

    public void HideToolTip()
    {
        for(int i = 0; i < 4; i++)
        {
            ProductionAndKeep[i].Icon.SetActive(false);
            ProductionAndKeep[i].image.sprite = null;
            ProductionAndKeep[i].text.text = "";
        }

        for (int i = 1; i < 4; i++)
        {
            BuildtimeAndCost[i].Icon.SetActive(false);
            BuildtimeAndCost[i].image.sprite = null;
            BuildtimeAndCost[i].text.text = "";
        }
    }
}
