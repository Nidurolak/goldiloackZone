using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScorePanelManager : MonoBehaviour
{
    //스코어라고 했지만 일단은 자원관리하는 패널이다.
    //자식 텍스트와 아이콘으로 이루어져 있다.
    //자식 텍스트는 금속, 나무, 연료, 음식 총 4가지를 대변한다. 가장 기본적으로 표시되는 건 일반 자원
    //각 자원 텍스트는 "현재 수치" + "+-"(If문을 돌려서 체크) + "턴 당 자원 증감량"으로 표시
    //증감량 텍스트는 현재 수치 텍스트보다 작게 표시하며 이득일 경우 초록색, 손해일 경우 빨간색으로 표시
    //생각 중인 기믹은 마우스를 올리면 고급 자원의 증감량을 위에서 아래로 내려오는 패널로 표시하도록 하는 것이다.
    //타일 데이터나 데이터 매니저, 이벤트 매니저 등에서 자원 증감량에 영향을 줄 경우 패널의 정보는 수시로 바뀐다.


    public Text Metal_Basic_Text;
    public Text Metal_Normal_Text;
    public Text Metal_Special_Text;
    public Text Wood_Basic_Text;
    public Text Fuel_Basic_Text;
    public Text Fuel_Normal_Text;
    public Text Fuel_Spacial_Text;
    public Text Food_Basic_Text;
    public Text Food_Normal_Text;
    public Text Food_Special_Text;
    public Text Other_Steam_Text;
    public Text Other_Lloyd_Engine_Text;
    public Text Other_ManPower_Text;
    public Text Other_House_Text;
    public Text Other_Population_Text;
    public Text Other_Paintient_Text;
    public Text Other_Zone_Grade_Text;
    public Text Other_Tech_Point_Text;
    public Text Other_Work_Point_Text;
    public Text Fuel_Steam_Text;
    public Text Economy_Ticket_Text;
    public Text Turn;

    public List<GameObject> SpacialUI = new List<GameObject>();

    public int OldValue_ManPower;
    public int OldValue_House;
    public int OldValue_Paitiant;
    public int OldValue_People;

    public int OldValue_Metal;
    public int OldValue_Gear;
    public int OldValue_Alloy;

    public int OldValue_Fuel;
    public int OldValue_Elec;
    public int OldValue_Goldion;

    public int OldValue_Wood;
    public int OldValue_Steam;
    public int OldValue_TechPoint;
    public int OldValue_WorkPoint;

    public int OldValue_Food;
    public int OldValue_Raw;
    public int OldValue_Can;

    public Datamanager DM;

    public string Metal_Current_Product;
    public string Parts_Current_Product;
    public string Alloy_Current_Product;
    public string Wood_Current_Product;
    public string Fuel_Current_Product;
    public string Electricity_Current_Product;
    public string Goldion_Current_Product;
    public string Food_Current_Product;
    public string Meal_Current_Product;
    public string Can_Current_Product;
    public string People_CurrentManPower_Text;
    public string People_MaxManPower_Text;
    public string People_House_Text;
    public string People_Ratio_ToManPower;
    public string People_Medical;

    public void Awake()
    {

        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
        a.GetComponent<Datamanager>().SPM = this;
    }

    public List<string> Current_Production = new List<string>();

    private string a(int b)
    {
        var a = "";

        if (b >= 0)
        {
            a = "+" + b;
        }
        else
        {
            a = b.ToString();
        }

        return a;
    }

    public int HouseOn_Count()
    {
        int c = 0;
        for (int i = 0; i < DM.TM.On_House.Count; i++)
        {
            if(DM.TM.On_House[i].IsWorking_OnTile == true)
            {
                c++;
            }
        }
        return c;
    }

    public void LateUpdate()
    {
        //증감 추가
        Metal_Basic_Text.text = DM.All_Resource_List[0] + " / " + a(DM.All_Resource_PerTurn_List_Total[0]);
        Metal_Normal_Text.text = DM.All_Resource_List[1] + " / " + a(DM.All_Resource_PerTurn_List_Total[1]);
        Metal_Special_Text.text = DM.All_Resource_List[2] + " / " + a(DM.All_Resource_PerTurn_List_Total[2]);

        Wood_Basic_Text.text = DM.All_Resource_List[8] + " / " + a(DM.All_Resource_PerTurn_List_Total[8]);

        Fuel_Basic_Text.text = DM.All_Resource_List[3] + " / " + a(DM.All_Resource_PerTurn_List_Total[3]);
        Fuel_Normal_Text.text = DM.All_Resource_List[14] + " / " + a(DM.All_Resource_PerTurn_List_Total[14]);
        Fuel_Spacial_Text.text = DM.All_Resource_List[4] + " / " + a(DM.All_Resource_PerTurn_List_Total[4]);

        Other_House_Text.text = DM.All_Resource_List[17] + " / " + HouseOn_Count();

        Food_Basic_Text.text = DM.All_Resource_List[5] + " / " + a(DM.All_Resource_PerTurn_List_Total[5]);
        Food_Normal_Text.text = DM.All_Resource_List[6] + " / " + a(DM.All_Resource_PerTurn_List_Total[6] + DM.Consume_Food_Value() + DM.Consume_Food_By_Ticket(true));
        Food_Special_Text.text = DM.All_Resource_List[7] + " / " + a(DM.All_Resource_PerTurn_List_Total[7]);
        Other_ManPower_Text.text = DM.All_Resource_List[28] + " / " + DM.All_Resource_List[29];
        Other_Population_Text.text = DM.All_Resource_List[38] + " / " + DM.All_Resource_List[40];
        Other_Paintient_Text.text = DM.All_Resource_List[39] + " / " + a(DM.All_Resource_PerTurn_List_Total[39]);
        Other_Steam_Text.text = DM.All_Resource_List[15] + " / " + a(DM.All_Resource_PerTurn_List_Total[15]);
        Other_Work_Point_Text.text = DM.All_Resource_List[13] + " / " + a(DM.All_Resource_PerTurn_List_Total[13]);
        Other_Tech_Point_Text.text = DM.All_Resource_List[12] + " / " + a(DM.All_Resource_PerTurn_List_Total[12]);

        Economy_Ticket_Text.text = DM.All_Resource_List[55] + " / " + DM.TicketValue();
        //Turn.text = DM.CurrentTurn() + " / " + DM.MaxTurn();
    }
}
