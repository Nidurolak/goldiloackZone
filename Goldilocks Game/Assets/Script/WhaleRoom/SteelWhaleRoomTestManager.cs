using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class SteelWhaleRoomTestManager : MonoBehaviour
{
    public int Ver;
    public int Hor;
    public static SteelWhaleRoomTestManager instance = null;
    public GameObject roomInfoPrefab;
    public List<RoomInfo_Hor> RoomInfo_Ver = new List<RoomInfo_Hor>();
    public SteelWhaleManager SWM;

    public void SceneMove()
    {
        SceneManager.LoadScene("Main");
    }

    [System.Serializable]
    public class RoomInfo_Hor
    {
        public List<RoomInfo> hor;
    }

    public void settingRoom_Position(RoomInfo room, int x, int y)
    {
        room.This.transform.position = new Vector3(x * 5.2f, y * 2.7f, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TrainTest")
        {
            GameObject a = GameObject.Find("SteelWhaleManager");
            SWM = a.GetComponent<SteelWhaleManager>();

        }
        for (int i = 0; i < SWM.Line_Horizontal.Count; i++)
        {
            for(int y = 0; y < SWM.Line_Horizontal[i].ID.Count; y++)
            {
                //ID돌아가며 체크하면서 생성하는 코드
                GameObject a = null;
                a = Instantiate(roomInfoPrefab) as GameObject;
                RoomInfo b = a.GetComponent<RoomInfo>();
                settingRoom_Position(b, i, y);
                RoomInfo_Ver[i].hor.Add(b);
            }
        }
    }

    public void Room_ID_Setting_MovingRoom(SteelWhaleManager.Room_Info room_Info, string ID)
    {

        int RoomX = int.Parse(ID.Substring(0, 2));
        int RoomY = int.Parse(ID.Substring(2, 2));

        if (RoomX != 99)
        {
            List<SteelWhaleManager.AddOn> c = new List<SteelWhaleManager.AddOn>();
            //룸 정보 딥카피
            Change_Room_Info(room_Info, SWM.Swbd[RoomX].BD_Version[RoomY]);

            int Addon_0 = int.Parse(ID.Substring(4, 2));
            if (Addon_0 != 0)
            {
                Debug.Log("asdsdadssdddvvvv" + Addon_0);
                c.Add(SWM.Change_AddOn_Info(SWM.Addon_Info_List[Addon_0]));
            }
            int Addon_1 = int.Parse(ID.Substring(6, 2));
            if (Addon_1 != 0)
            {
                c.Add(SWM.Change_AddOn_Info(SWM.Addon_Info_List[Addon_1]));
            }
            int Addon_2 = int.Parse(ID.Substring(8, 2));
            if (Addon_2 != 0)
            {
                c.Add(SWM.Change_AddOn_Info(SWM.Addon_Info_List[Addon_2]));
            }
            int Addon_3 = int.Parse(ID.Substring(10, 2));
            if (Addon_3 != 0)
            {
                c.Add(SWM.Change_AddOn_Info(SWM.Addon_Info_List[Addon_3]));
            }
            int Addon_4 = int.Parse(ID.Substring(12, 2));
            if (Addon_4 != 0)
            {
                c.Add(SWM.Addon_Info_List[Addon_4]);
            }
            int Addon_5 = int.Parse(ID.Substring(14, 2));
            if (Addon_5 != 0)
            {
                c.Add(SWM.Change_AddOn_Info(SWM.Addon_Info_List[Addon_5]));
            }

            room_Info.AddOns = new List<SteelWhaleManager.AddOn>(c.ToArray());

        }
        else if (RoomX == 99)
        {
            room_Info.ID = "9999";
        }

        
    }

    public void Change_Room_Info(SteelWhaleManager.Room_Info a, SteelWhaleManager.Room_Info b)
    {
        a.Name = b.Name;
        a.Name1 = b.Name1;
        a.ID = b.ID;
        a.Contents = b.Contents;

        a.CanBuild = b.CanBuild;
        a.OnlyOne = b.OnlyOne;
        a.Need_To_Build_ID = b.Need_To_Build_ID;

        a.Room_Logo = b.Room_Logo;
        a.Room_Image = b.Room_Image;
        a.buffArea = b.buffArea;

        a.BuildCost_Type = new List<int>(b.BuildCost_Type.ToArray());
        a.BuildCost_Value = new List<float>(b.BuildCost_Value.ToArray());
        a.Upkeep_Type = new List<int>(b.Upkeep_Type.ToArray());
        a.Upkeep_Value = new List<float>(b.Upkeep_Value.ToArray());
        a.Product_Type = new List<int>(b.Product_Type.ToArray());
        a.Product_Value = new List<float>(b.Product_Value.ToArray());

        a.AddOn_Capacity = b.AddOn_Capacity;

        if (b.AddOns.Count != 0)
        {
            List<SteelWhaleManager.AddOn> c = new List<SteelWhaleManager.AddOn>();

            for (int i = 0; i < b.AddOns.Count; i++)
            {
                c.Add(SWM.Change_AddOn_Info(b.AddOns[i]));
            }

            a.AddOns = new List<SteelWhaleManager.AddOn>(c.ToArray());

        }

        a.Default_Anti_Disaster = b.Default_Anti_Disaster;
        a.Cog_Needed = b.Cog_Needed;
        a.Operation_Needed = b.Operation_Needed;
        a.People_Capacity = b.People_Capacity;
        a.Battler_Capacity = b.Battler_Capacity;
        a.patient_Capacity = b.patient_Capacity;
        a.Attack_Bonus = b.Attack_Bonus;
        a.Deffense_Bonus = b.Deffense_Bonus;
        a.Room_HP = b.Room_HP;
        a.Room_Moving_Time = b.Room_Moving_Time;
    }
}
