using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TechInfo : MonoBehaviour
{
    public TechTreeManager TTM;
    public string ID;
    public string Name;
    public string Contents;
    public string Flavor;
    public string Flavor2;
    public Image BackLight;
    public Image Thumbnail;
    public Sprite Thumbnail2;
    public int TechPowerNeeded;
    public int WorkPowerNeeded;
    public TechCategory TC1;
    public int BonusType1;
    public float BonusValue1;
    public TechCategory TC2;
    public int BonusType2;
    public float BonusValue2;
    public TechCategory TC3;
    public int BonusType3;
    public float BonusValue3;
    public bool isWorking;

    public enum TechCategory
    {
        None, UnlockSpacial, UnlockTech, UnlockBuilding, Building_Production, Building_Upkeep, Building_BuildCost,
        Building_BuffAreaSize, Building_TurnCount, Building_Anti, ManPower,UpgradeSteelWhale
    }
    public List<TechInfo> NeedToUnlock = new List<TechInfo>();
    public List<GameObject> UnlockTarget = new List<GameObject>();
    public List<Slider> UnlockSlider = new List<Slider>();

    public void Upgrade()
    {
        Debug.Log("발동");
        if (TTM.DM.All_Resource_List[12] >= TechPowerNeeded && TTM.DM.All_Resource_List[13] >= WorkPowerNeeded)
        {
            TTM.DM.All_Resource_List[12] -= TechPowerNeeded;
            TTM.DM.All_Resource_List[13] -= WorkPowerNeeded;
            TTM.UpgradeButton.interactable = false;
            BackLight.color = new Color(255,255,255, 255);
        }
        if (UnlockSlider.Count != 0)
        {
            for (int i = 0; i < UnlockSlider.Count; i++)
            {
                UnlockSlider[i].DOValue(1, 3, false);
            }
        }
        isWorking = true;

        switch (TC1)
        {
            case TechCategory.UnlockSpacial:
                TTM.DM.Spacial_Effect_Unlock[0].List[(int)BonusValue1] = true;
                if (BonusType1 == 3)
                {
                    switch ((int)BonusValue1)
                    {
                        case 0:
                            for (int i = 0; i < TTM.DM.TM.On_Rock.Count; i++)
                            {
                                TTM.DM.TM.On_Rock[i].Distrucktive = true;
                            }
                            break;
                        case 1:
                            for (int i = 0; i < TTM.DM.TM.On_Ore.Count; i++)
                            {
                                TTM.DM.TM.On_Ore[i].Distrucktive = true;
                            }
                            break;
                    }
                }
                break;
            case TechCategory.Building_BuildCost :
                if(BonusType1 != -1)
                {
                    TTM.DM.Tech_BuildingCost_Type_Magnification_List[BonusType1] += BonusValue1;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Production:
                if (BonusType1 != -1)
                {
                    TTM.DM.Tech_Production_Type_Magnification_List[BonusType1] += BonusValue1;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_TurnCount:
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Upkeep:
                if (BonusType1 != -1)
                {
                    TTM.DM.Tech_Upkeep_Type_Magnification_List[BonusType1] += BonusValue1;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.UnlockBuilding:
                if (BonusType1 != -1)
                {
                    TTM.DM.Ub[BonusType1].Unlock_Version[(int)BonusValue1] = true;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_BuffAreaSize:
                if (BonusType1 != -1)
                {
                    TTM.DM.Ub[BonusType1].Unlock_Version[(int)BonusValue1] = true;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Anti:
                TTM.DM.All_Resource_List[21] += (int)BonusValue1;
                for(int i = 0; i < TTM.DM.TM.OnEnableTileList.Count; i++)
                {
                    TTM.DM.TM.OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case TechCategory.ManPower:
                TTM.DM.All_Resource_List[40] += (int)BonusValue1;
                TTM.DM.ManPower_Setting();
                TTM.DM.ManPower_Caculate();
                break;
            case TechCategory.UnlockTech:
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.UpgradeSteelWhale:
                TTM.DM.TM.ResetForTech();
                break;
        }
        switch (TC2)
        {
            case TechCategory.UnlockSpacial:
                TTM.DM.Spacial_Effect_Unlock[0].List[(int)BonusValue2] = true;
                if (BonusType2 == 3)
                {
                    switch ((int)BonusValue2)
                    {
                        case 0:
                            for (int i = 0; i < TTM.DM.TM.On_Rock.Count; i++)
                            {
                                TTM.DM.TM.On_Rock[i].Distrucktive = true;
                            }
                            break;
                        case 1:
                            for (int i = 0; i < TTM.DM.TM.On_Ore.Count; i++)
                            {
                                TTM.DM.TM.On_Ore[i].Distrucktive = true;
                            }
                            break;
                    }
                }
                break;
            case TechCategory.Building_BuildCost:
                if (BonusType2 != -1)
                {
                    TTM.DM.Tech_BuildingCost_Type_Magnification_List[BonusType2] += BonusValue2;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Production:
                if (BonusType2 != -1)
                {
                    TTM.DM.Tech_Production_Type_Magnification_List[BonusType2] += BonusValue2;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_TurnCount:
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Upkeep:
                if (BonusType2 != -1)
                {
                    TTM.DM.Tech_Upkeep_Type_Magnification_List[BonusType2] += BonusValue2;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.UnlockBuilding:
                if (BonusType2 != -1)
                {
                    TTM.DM.Ub[BonusType2].Unlock_Version[(int)BonusValue2] = true;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_BuffAreaSize:
                if (BonusType2 != -1)
                {
                    TTM.DM.Ub[BonusType2].Unlock_Version[(int)BonusValue2] = true;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Anti:
                TTM.DM.All_Resource_List[21] += (int)BonusValue2;
                for (int i = 0; i < TTM.DM.TM.OnEnableTileList.Count; i++)
                {
                    TTM.DM.TM.OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case TechCategory.ManPower:
                TTM.DM.All_Resource_List[40] += (int)BonusValue2;
                TTM.DM.ManPower_Setting();
                TTM.DM.ManPower_Caculate();
                break;
            case TechCategory.UnlockTech:
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.UpgradeSteelWhale:
                TTM.DM.TM.ResetForTech();
                break;
        }
        switch (TC3)
        {
            case TechCategory.UnlockSpacial:
                TTM.DM.Spacial_Effect_Unlock[0].List[(int)BonusValue3] = true;
                if (BonusType3 == 3)
                {
                    switch ((int)BonusValue3)
                    {
                        case 0:
                            for (int i = 0; i < TTM.DM.TM.On_Rock.Count; i++)
                            {
                                TTM.DM.TM.On_Rock[i].Distrucktive = true;
                            }
                            break;
                        case 1:
                            for (int i = 0; i < TTM.DM.TM.On_Ore.Count; i++)
                            {
                                TTM.DM.TM.On_Ore[i].Distrucktive = true;
                            }
                            break;
                    }
                }
                break;
            case TechCategory.Building_BuildCost:
                if (BonusType3 != -1)
                {
                    TTM.DM.Tech_BuildingCost_Type_Magnification_List[BonusType3] += BonusValue3;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Production:
                if (BonusType3 != -1)
                {
                    TTM.DM.Tech_Production_Type_Magnification_List[BonusType3] += BonusValue3;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_TurnCount:
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Upkeep:
                if (BonusType3 != -1)
                {
                    TTM.DM.Tech_Upkeep_Type_Magnification_List[BonusType3] += BonusValue3;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.UnlockBuilding:
                if (BonusType3 != -1)
                {
                    TTM.DM.Ub[BonusType3].Unlock_Version[(int)BonusValue3] = true;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_BuffAreaSize:
                if (BonusType3 != -1)
                {
                    TTM.DM.Ub[BonusType3].Unlock_Version[(int)BonusValue3] = true;
                }
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.Building_Anti:
                TTM.DM.All_Resource_List[21] += (int)BonusValue3;
                for (int i = 0; i < TTM.DM.TM.OnEnableTileList.Count; i++)
                {
                    TTM.DM.TM.OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case TechCategory.ManPower:
                TTM.DM.All_Resource_List[40] += (int)BonusValue3;
                TTM.DM.ManPower_Setting();
                TTM.DM.ManPower_Caculate();
                break;
            case TechCategory.UnlockTech:
                TTM.DM.TM.ResetForTech();
                break;
            case TechCategory.UpgradeSteelWhale:
                TTM.DM.TM.ResetForTech();
                break;
        }
    }
}
