using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Category_Change : MonoBehaviour
{
    public SlotManager SM;
    public int Type1;
    public int Type2;
    public int Type3;

    public void SlotData_ReMove_(int i, List<GameObject> List)
    {
        var a = List[i].GetComponent<BuildingButtonData>();
        a.BuildingType = 0;
        a.BuildingVersion = 0;
        a.Image = null;
        a.Construction_Image = null;
        a.Name = null;
        a.Contents = null;
        a.Production_Type = 0;
        a.Production_Value = 0;
        a.Upkeep_Type.Clear();
        a.Upkeep_Value.Clear();
        a.BuildCost_Type.Clear();
        a.BuildCost_Value.Clear();
        a.Build_TurnCount = 0;
        a.Anti_Disaster = 0;
        a.Allow_Disaster = 0;
        a.Buff_Type.Clear();
        a.Buff_Value.Clear();
        a.Buff_Area_Size = 0;
        a.Working_ManPower = 0;
        a.Need_Steam = false;
        a.Need_Electricity = false;
        a.Constructable = false;
        SM.ButtonList1[i].SetActive(false);
    }

    public void SlotData_Remove()
    {
        var b = SM.ButtonList1;
        var c = SM.ButtonList2;
        var d = SM.ButtonList3;
        for (int i =0; i <4; i++)
        {
            SlotData_ReMove_(i,b);
            SlotData_ReMove_(i,c);
            SlotData_ReMove_(i,d);
        }
        SM.Slot_SetActive_False(b);
        SM.Slot_SetActive_False(c);
        SM.Slot_SetActive_False(d);

    }



    public void Slot_Change1()
    {
        SM.Slot_Initialise1(SM.ButtonList1, SM.DM, Type1);
    }
    public void Slot_Change2()
    {
        SM.Slot_Initialise1(SM.ButtonList2, SM.DM, Type2);
    }
    public void Slot_Change3()
    {
        SM.Slot_Initialise1(SM.ButtonList3, SM.DM, Type3);
    }

}
