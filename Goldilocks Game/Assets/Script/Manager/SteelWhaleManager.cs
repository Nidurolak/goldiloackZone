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

public class SteelWhaleManager : MonoBehaviour
{
    public Datamanager DM;
    public StealWhalePanel SWP;
    public GameObject TestBlock;
    private new GameObject Instantiate;
    public static SteelWhaleManager instance = null;

    public List<Sprite> Room_Logo = new List<Sprite>();
    public List<Sprite> AddOn_Logo = new List<Sprite>();

    public List<int> Line_Total = new List<int>();
    public List<Line_Vertical> Line_Horizontal = new List<Line_Vertical>();

    public List<string> All_Room_Info_Test = new List<string>();

    public List<Room_Info> Room_Info_List = new List<Room_Info>();
    public List<AddOn> Addon_Info_List = new List<AddOn>();

    [System.Serializable]
    public class SteelWhaleRoomData_ListWrapper
    {
        public List<Room_Info> BD_Version;
    }

    public List<SteelWhaleRoomData_ListWrapper> Swbd = new List<SteelWhaleRoomData_ListWrapper>();

    [System.Serializable]
    public class AddOn
    {
        public Sprite Logo;
        public string Name;
        [TextArea]
        public string Contents;

        public enum BuffArea { None, Self, Near, Around, All, Map }
        public BuffArea buffArea;

        public List<int> BuildCost_Type = new List<int>();
        public List<float> BuildCost_Value = new List<float>();

        public int BuildTime;

        public int Upkeep_Type;
        public float Upkeep_Value;

        public int Product_Type;
        public float Product_Value;

        public int Default_Anti_Disaster;
        public int Cog_Needed;

        public int People_Capacity;
        public int Battler_Capacity;
        public int patient_Capacity;

        public float Attack_Bonus;
        public float Deffense_Bonus;

        public int Room_HP;

        public int Room_Moving_Time;
    }

    [System.Serializable]
    public class Line_Vertical
    {
        public List<string> ID = new List<string>();
    }

    public void Check_Product_All_Room()
    {
        for(int i = 0; i < Line_Horizontal.Count; i++)
        {
            for(int z = 0; z < Line_Horizontal[i].ID.Count; z++)
            {
                if (SWP.Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos.ID != "9999")
                {
                    SWP.Check_Product(SWP.Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos);
                    Debug.Log(i + "번 줄 " + z + "번 객실 체크");
                }
            }
        }
    }

    //아이디 체크해서 룸 정보를 할당한다.
    public void Room_ID_Setting(Room_Info room_Info, string ID)
    {
        int RoomX = int.Parse(ID.Substring(0, 2));
        int RoomY = int.Parse(ID.Substring(2, 2));

        //완전 비어있는, 없는 방 체크, X가 99면 아예 없는 방이 된다.
        if (RoomX != 99)
        {
            List<AddOn> c = new List<AddOn>();

            //룸 정보 딥카피
            Change_Room_Info(room_Info, Swbd[RoomX].BD_Version[RoomY]);

            int Addon_0 = int.Parse(ID.Substring(4, 2));
            if (Addon_0 != 0)
            {
                Debug.Log("asdsdadssdddvvvv" + Addon_0);
                c.Add(Change_AddOn_Info(Addon_Info_List[Addon_0]));
            }
            int Addon_1 = int.Parse(ID.Substring(6, 2));
            if (Addon_1 != 0)
            {
                c.Add(Change_AddOn_Info(Addon_Info_List[Addon_1]));
            }
            int Addon_2 = int.Parse(ID.Substring(8, 2));
            if (Addon_2 != 0)
            {
                c.Add(Change_AddOn_Info(Addon_Info_List[Addon_2]));
            }
            int Addon_3 = int.Parse(ID.Substring(10, 2));
            if (Addon_3 != 0)
            {
                c.Add(Change_AddOn_Info(Addon_Info_List[Addon_3]));
            }
            int Addon_4 = int.Parse(ID.Substring(12, 2));
            if (Addon_4 != 0)
            {
                c.Add(Addon_Info_List[Addon_4]);
            }
            int Addon_5 = int.Parse(ID.Substring(14, 2));
            if (Addon_5 != 0)
            {
                c.Add(Change_AddOn_Info(Addon_Info_List[Addon_5]));
            }

            room_Info.AddOns = new List<AddOn>(c.ToArray());

        }
        else if (RoomX == 99)
        {
            room_Info.ID = "9999";
        }
        //차후 애드온 정보를 만들면 추가한다.
    }


    //룸 정보 딥카피하는 함수, 추후 추가 조건 추가할 예정
    public void Change_Room_Info(Room_Info a, Room_Info b)
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
            List<AddOn> c = new List<AddOn>();

            for(int i = 0; i < b.AddOns.Count; i++)
            {
               c.Add(Change_AddOn_Info(b.AddOns[i]));
            }

            a.AddOns = new List<AddOn>(c.ToArray());

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

    //애드온 딥카피하는 함수, 추후 추가 조건 추가할 예정
    public AddOn Change_AddOn_Info(AddOn b)
    {
        AddOn a = new AddOn();
        a.Name = b.Name;
        a.Contents = b.Contents;
        a.Logo = b.Logo;
        a.buffArea = b.buffArea;

        a.BuildCost_Type = new List<int>(b.BuildCost_Type.ToArray());
        a.BuildCost_Value = new List<float>(b.BuildCost_Value.ToArray());
        a.Upkeep_Type = b.Upkeep_Type;
        a.Upkeep_Value = b.Upkeep_Value;
        a.Product_Type = b.Product_Type;
        a.Product_Value = b.Product_Value;
        
        a.Default_Anti_Disaster = b.Default_Anti_Disaster;
        a.Cog_Needed = b.Cog_Needed;
        a.People_Capacity = b.People_Capacity;
        a.Battler_Capacity = b.Battler_Capacity;
        a.patient_Capacity = b.patient_Capacity;
        a.Attack_Bonus = b.Attack_Bonus;
        a.Deffense_Bonus = b.Deffense_Bonus;
        a.Room_HP = b.Room_HP;
        a.Room_Moving_Time = b.Room_Moving_Time;
        return a;
    }

    public void Lines_Make()
    {
        for(int i = 0; i < Line_Total.Count; i++)
        {
            for(int z = 0; z < Line_Total[i]; z++)
            {
                Instantiate = Instantiate(TestBlock) as GameObject;
                Instantiate.transform.position = new Vector3(i * 1.1f, z * 1.1f);
            }
        }
    }

    public class Area_Info
    {
        public int X;
        public int Y;

        public Room_Info Room;
    }
    [System.Serializable]
    public class Room_Info
    {
        public string Name;
        public string Name1;
        public string Contents;

        //ID 정보는 1~2글자 x축, 3~4글자 y축, 5~ 차량 정보, 애드온 정보를 포함한다.
        public string ID;

        public Sprite Room_Logo;
        public Sprite Room_Image;

        public bool OnOff;
        public bool CanBuild;
        public bool Unlock;
        public bool OnlyOne;

        public List<int> Need_To_Build_ID = new List<int>();

        public enum BuffArea { None, Self, Near, Around, All}
        public BuffArea buffArea;

        public List<int> BuildCost_Type = new List<int>();
        public List<float> BuildCost_Value = new List<float>();

        public int BuildTime;

        public List<int> Upkeep_Type = new List<int>();
        public List<float> Upkeep_Value = new List<float>();

        public List<int> Product_Type = new List<int>();
        public List<float> Product_Value = new List<float>();

        public List<int> Upkeep_Type_Current = new List<int>();
        public List<float> Upkeep_Value_Current = new List<float>();

        public List<int> Product_Type_Current = new List<int>();
        public List<float> Product_Value_Current = new List<float>();

        public List<int> Buff_Type = new List<int>();


        public int AddOn_Capacity;
        public List<AddOn> AddOns = new List<AddOn>();

        public int Default_Anti_Disaster;
        public int Cog_Needed;

        //자원 13번인가 작업 포인트 말하는거, 이거 배율해서 수리 포인트 배정될거
        public int Operation_Needed;

        public int People_Capacity;
        public int Battler_Capacity;
        public int patient_Capacity;

        public float Attack_Bonus;
        public float Deffense_Bonus;

        public int Room_HP;

        public int Room_Moving_Time;
    }

    public bool Check_Construct(int a, int b)
    {
        bool z = true;

        for (int i = 0; i < Swbd[a].BD_Version[b].BuildCost_Type.Count; i++)
        {
            //자원이 적으면 false체크
            if (DM.All_Resource_List[Swbd[a].BD_Version[b].BuildCost_Type[i]] < Swbd[a].BD_Version[b].BuildCost_Value[i])
            {
                z = false;
            }
            
        }
        return z;
    }

    public void Awake()
    {

        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);

        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TrainTest")
        {
            GameObject a = GameObject.Find("DataManager");
            DM = a.GetComponent<Datamanager>();

        }
    }
    
}
