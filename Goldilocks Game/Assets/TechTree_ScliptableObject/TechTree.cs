using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "TechData", menuName = "SpawnTechData", order = 1)]
public class TechTree : MonoBehaviour
{
    public string ID;
    public string Name;
    public string Contents;
    public Sprite Thumbnail;
    public int TechPowerNeeded;
    public int WorkPowerNeeded;
    public int BonusType1;
    public int BonusType2;
    public float BonusValue1;
    public float BonusValue2;

    public enum TechCategory {UnlockBuilding, Building_Production, Building_Upkeep, Building_BuildCost,
                              Building_BuffAreaSize, Building_TurnCount, UpgradeSteelWhale}
    public TechCategory TC;
}
