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

//턴 작동은 870번 줄 인근


public class Datamanager : MonoBehaviour
{
    public void Scenetest()
    {
        SceneManager.LoadScene("TrainTest");
    }

    //턴엔드 기능은 1100번줄 인근
    //이전의 Type-Style로 구분하던 방식을 Type-Version로 바꾼다. 모든 변수는 단어마다_를 사용하며 지역 변수 외에는 카멜 방식을 쓴다.
    //가능한 이곳에서는 세이브 로드, 그리고 업적 따위를 위해 데이터를 모으고 보내는 것 이상의 함수를 담지 않도록 한다.
    //public GameObject Save_Load_Text;
    public GameObject Original;

    public enum Difficulty {easy, normal, hard}
    public Difficulty difficulty;
    public float Difficulty_Production()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 1.3f;
                break;
            case Difficulty.normal:
                a = 1f;
                break;
            case Difficulty.hard:
                a = 0.7f;
                break;
        }
        return a;
    }
    public float Difficulty_Upkeep()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 0.8f;
                break;
            case Difficulty.normal:
                a = 1f;
                break;
            case Difficulty.hard:
                a = 1.2f;
                break;
        }
        return a;
    }
    public float Difficulty_FoodConsume()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = DM.All_Resource_List[43] * 0.02f;
                break;
            case Difficulty.normal:
                a = DM.All_Resource_List[43] * 0.03f;
                break;
            case Difficulty.hard:
                a = DM.All_Resource_List[43] * 0.04f;
                break;
        }
        return a;
    }
    public float Difficulty_Paitient()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = Random.Range(0, 3) * 0.1f;
                break;
            case Difficulty.normal:
                a = Random.Range(3, 5) * 0.1f;
                break;
            case Difficulty.hard:
                a = Random.Range(5, 7) * 0.1f;
                break;
        }
        return a;
    }
    public float Desaster_Building_Value(int i)
    {
        float a = 0;
        switch (i)
        {
            case 0:
                a = 0.1f;
                break;
            case -1:
                a = 0.2f;
                break;
            case -2:
                a = 0.4f;
                break;
            case -3:
                a = 0.5f;
                break;
            case -4:
                a = 0.6f;
                break;
            case -5:
                a = 0.7f;
                break;
            case -6:
                a = 0.8f;
                break;
            case -7:
                a = 1.0f;
                break;
            case -8:
                a = 1.2f;
                break;
        }
        return a;
    }
    public float Difficulty_BuildCost()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 0.9f;
                break;
            case Difficulty.normal:
                a = 1f;
                break;
            case Difficulty.hard:
                a = 1.3f;
                break;
        }
        return a;
    }
    public float Difficulty_IlltoDie()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 0.7f;
                break;
            case Difficulty.normal:
                a = 1f;
                break;
            case Difficulty.hard:
                a = 1.3f;
                break;
        }
        return a;
    }
    public float Difficulty_TechCost()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 0.7f;
                break;
            case Difficulty.normal:
                a = 1f;
                break;
            case Difficulty.hard:
                a = 1.5f;
                break;
        }
        return a;
    }
    public float Difficulty_RuleDelay()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 8f;
                break;
            case Difficulty.normal:
                a = 10f;
                break;
            case Difficulty.hard:
                a = 15f;
                break;
        }
        return a;
    }
    public float Difficulty_RuleEffect()
    {
        float a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 0.7f;
                break;
            case Difficulty.normal:
                a = 1f;
                break;
            case Difficulty.hard:
                a = 1.6f;
                break;
        }
        return a;
    }

    public string TicketValue()
    {
        float a = 0;
        string b = "";

        a = All_Resource_List[55] / All_Resource_List[38];

        if (a >= (All_Resource_List[56] * 0.1f))
        {
            Ticket = EconomyState.Lot; 
            b = "흔함";
        }
        else if ((All_Resource_List[56] * 0.1f) > a && a > (All_Resource_List[57] * 0.1f))
        {
            Ticket = EconomyState.Normal;
            b = "보통";
        }
        else 
        if (a <= (All_Resource_List[57] * 0.1f))
        {
            Ticket = EconomyState.Few;
            b = "귀함";
        }

        return b;
    }


    //스테이지 이동 후 첫 타이밍 - 적응기, 아무런 보너스가 없는 중반 - 여명기
    //모든 작업 보너스가 있는 후반 - 황혼기, 의지불만희망 모두 페널티가 심한 극후반 - 탈출기
    public enum Stage_Escalation { dawn, adaptive, twilight, exodus, None}
    public Stage_Escalation stage_Escalation;

    public UnityEvent TurnEnd_Explorer;
    public UnityEvent TurnEnd_Quest;
    public bool IsReset;

    //인구 수보다 주택 수가 적을 경우
    public bool OverCrowding;

    //이거 발동 중일 땐 모든 작동 금지
    public bool IsCinematic;

    //이게 트루면 게임 오버!
    public bool IsOver;

    //식권 경제
    public enum EconomyState {Few, Normal, Lot}
    public EconomyState Ticket;

    //의료붕괴 수치화, 인구 대비 0~5%일때는 Good, 6~20%일때는 Noraml, 21~40%일때는 Bad, 41% 부터는 collapse 
    public enum MedicalState {Good, Normal, Bad, Collapse, None}
    public MedicalState medicalState;

    public enum MindState {Max, High, Middle, Low, Min, None}
    public MindState WillState;
    public MindState ComplaintState;
    public MindState HopeState;

    public Datamanager DM;
    public RuleManager RM;
    public static Datamanager instance = null; 
    public TileManager TM;
    public TechTreeManager TTM;
    public MouseControll MS;
    public SoundManager SM;
    public ExplorerManager EXM;
    public EventManager EM;
    public ScorePanelManager SPM;
    public QuestManager QM;
    public AlramPanel AP;
    public AgeColor AC;
    public Guide Over;
    public GameObject Overobj;
    public GameObject SteelWhale;

    //이건 익스플로러에 연동되어 있는데 조만간 빼내야겠어.
    public Sprite[] Resource_Sprites_Logo_NonBackGround;
    public Sprite[] Resource_Sprites_Logo_BackGround;


    public List<FoundZone> FoundZone_List = new List<FoundZone>();

    public List<int> FloorTile_Count = new List<int>();//세이브, 풀링, 타일세팅 등등에 사용된다. 사용이 잘 안되면 빠르게 지우자.

    //전자는 세이브로드에 이용하고 중자는 실시간 자원 할당에 사용할 예정, 후자는 활성화된 타일 정보를 세이브할 때 사용
    public List<GameObject> FloorTile_List = new List<GameObject>();
    public List<GameObject> FloorTile_List_MakingResource;
    public List<GameObject> OnEableFloorTiles = new List<GameObject>();

    //Int 타입으로 분류한 자원 목록들을 한데 모아서 쓰는 장치, 각 순번에 따른 자원 항목은 보드나 엑셀 파일을 참고
    public List<int> All_Resource_List = new List<int>();
    //매 턴 더해지고 빼지는 리스트를 계산할 때 사용.
    public List<int> All_Resource_PerTurn_List_Total = new List<int>();
    
    //기술 유지비
    public float Tech_Upkeep_Discharge_Magnification_List;
    public List<float> Tech_Upkeep_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Tech_Upkeep_Version_Magnification_List = new List<Building_Version_List>();

    //법안 유지비
    public float Rule_Upkeep_Discharge_Magnification_List;
    public List<float> Rule_Upkeep_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Rule_Upkeep_Version_Magnification_List = new List<Building_Version_List>();

    //이벤트 유지비
    public float Event_Upkeep_Discharge_Magnification_List;
    public List<float> Event_Upkeep_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Event_Upkeep_Version_Magnification_List = new List<Building_Version_List>();

    //기술 건설비
    public float Tech_BuildingCost_Discharge_Magnification_List;
    public List<float> Tech_BuildingCost_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Tech_BuildingCost_Version_Magnification_List = new List<Building_Version_List>();

    //이벤트 건설비
    public float Event_BuildingCost_Discharge_Magnification_List;
    public List<float> Event_BuildingCost_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Event_BuildingCost_Version_Magnification_List = new List<Building_Version_List>();

    //계열 기술 생산, 개별 기술 생산
    public List<int> Tech_Production_Type_Integer_List = new List<int>();
    public List<Building_Version_List> Tech_Production_Version_Integer_List = new List<Building_Version_List>();

    //기술 생산 배율
    public float Tech_Production_Discharge_Magnification_List;
    public List<float> Tech_Production_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Tech_Production_Version_Magnification_List = new List<Building_Version_List>();

    //법안 생산 배율
    public float Rule_Production_Discharge_Magnification_List;
    public List<float> Rule_Production_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Rule_Production_Version_Magnification_List = new List<Building_Version_List>();

    //이벤트 생산 배율
    public float Event_Production_Discharge_Magnification_List;
    public List<float> Event_Production_Type_Magnification_List = new List<float>();
    public List<Building_Version_List> Event_Production_Version_Magnification_List = new List<Building_Version_List>();

    //기술에 따른 리스트 증감식, 턴수 감소나 특수한 효과에 꼭 필요하겠더라고
    public List<float> Tech_AllResource_Intger_List = new List<float>();

    //에스컬레이션
    public List<float> Escalation_Production_Magnification;

    public int HungryCount;

    public List<bool> OnActiveRuleList = new List<bool>();

    public List<int> Rule_Active_Max = new List<int>();
    public List<int> Rule_Active_Current = new List<int>();
    public List<int> Rule_Active_Delay_Max = new List<int>();
    public List<int> Rule_Active_Delay_Current = new List<int>();

    //의지 배율
    public float Will_Production_Magnification;

    //기술 특별 해금
    public List<Spacial_Effect_Unlock_List> Spacial_Effect_Unlock = new List<Spacial_Effect_Unlock_List>();

    [System.Serializable]
    public class Spacial_Effect_Unlock_List
    {
        public List<bool> List = new List<bool>();
    }


    public Button TurnButton;

    public Slider Will;
    public Slider Hope;
    public Slider Complaint;
    public Slider TurnGage;

    public Image Turn_Wheel;

    [SerializeField]
    public int Consume_Food_Value()
    { 
        int a = 0;
        a = (int)(All_Resource_List[38] *
            (Difficulty_FoodConsume() + Tech_Food_Integer + Rule_Food_Integer) *
            Tech_Food_Magnification *
            Rule_Food_Magnification);
        return -a;
    }

    [SerializeField]
    public int Consume_Food_By_Ticket(bool return_a)
    {
        int a = 0;
        float b = 0;
        if (RM.ruleInfos[8].isWorking == true)
        {
            switch (Ticket)
            {
                case EconomyState.Few:
                    a = 0;
                    break;
                case EconomyState.Normal:
                    b = (int)(All_Resource_List[55] * (All_Resource_List[58] * 0.1f));
                    a = (int)(Difficulty_FoodConsume() * b);
                    break;
                case EconomyState.Lot:

                    b = (int)(All_Resource_List[55] * (All_Resource_List[58] * 0.1f) * 1.3f);
                    a = (int)(Difficulty_FoodConsume() * b);
                    break;
            }
        }
        if (return_a == true)
        {
            return -a;
        }
        else
        {
            return (int)-b;
        }
    }

    [SerializeField]
    public int Consume_Food_Ticket()
    {
        int a = 0;
        if (RM.ruleInfos[8].isWorking == true)
        {
            switch (Ticket)
            {
                case EconomyState.Few:
                    a = 0;
                    break;
                case EconomyState.Normal:
                    a = All_Resource_List[55] * All_Resource_List[58];
                    break;
                case EconomyState.Lot:
                    a = (int)(All_Resource_List[55] * All_Resource_List[58] * 1.3f);
                    break;
            }
        }
        return a;
    }

    public float Tech_Food_Integer;
    public float Tech_Food_Magnification;
    public float Rule_Food_Integer;
    public float Rule_Food_Magnification;


    public int MaxTurn;
    public int CurrentTurn;

    [System.Serializable]
    public class Building_Version_List
    {
        public List<float> Version_List = new List<float>();
    }

    //타일데이터 저장을 위한 클래스 양식, 조건에 따라 첨삭 가능
    public class TileData_Save
    {
        public Vector3 OriginalPosition;

        public int Tile_Number;
        public int FloorType;
        public int FloorVersion;

        public bool Resource_using;
        public bool Steam_Online;
        public bool Electricity_Online;
        public bool House_Online;
        public bool Idea_Online;
        public bool Public_Online;

        public bool Event_Acidente;

        public int Production_Type;
        public float Production_Value;
        

        public int UpKeep1_Type;
        public float UpKeep1_Value;
        public int UpKeep2_Type;
        public float UpKeep2_Value;
        public int UpKeep3_Type;
        public float UpKeep3_Value;

        public int BuildCost1_Type;
        public float BuildCost1_Value;
        public int BuildCost2_Type;
        public float BuildCost2_Value;
        public int BuildCost3_Type;
        public float BuildCost3_Value;

        public int Buff1_Type;
        public int Buff2_Type;
        public int Buff3_Type;
        public float Buff1_Value;
        public float Buff2_Value;
        public float Buff3_Value;

        public int Turn_Count;

        public int Buff_Area_Size;

        public int ReType;
        public int ReVersion;

        public int EventType;
        public int EventVersion;

        public int Current_Disaster;
        public int Anti_Disaster;
        public int Allow_Disaster;
        public int Event_Desaster;

        public int BuildingType;
        public int BuildingVersion;

        public int Needed_ManPower;
        public int Working_ManPower;

        public bool Need_Steam;
        public bool Need_Electricity;
        public bool Need_House;
        public bool Need_Public;
        public bool Need_Idea;

        public bool Constructable_OnTile;
        public bool Workable_OnTile;
        public bool IsWorking_OnTile;
        public bool Distrucktive;
        public bool IsDistroy_OnTile;
    }

    //자원데이터 저장을 위한 클래스 양식, 조건에 따라 첨삭 가능
    public class ResourceData_Save
    {
        public List<int> All_Resource_List = new List<int>();
        public List<int> All_Resource_PerTurn_List_Total = new List<int>();

        public List<float> Tech_Production_Magnification_List = new List<float>();
        public List<float> Tech_Upkeep_Magnification_List = new List<float>();
        public List<float> Tech_BuildingCost_Magnification_List = new List<float>();
        public List<float> Skill_Production_Magnification_List = new List<float>();
        public List<float> Skill_Upkeep_Magnification_List = new List<float>();
        public List<float> Skill_BuildingCost_Magnification_List = new List<float>();
        public List<float> Event_Production_Magnification_List = new List<float>();
        public List<float> Event_Upkeep_Magnification_List = new List<float>();
        public List<float> Event_BuildingCost_Magnification_List = new List<float>();
        public List<float> Mind_Production_Magnification_List = new List<float>();


        public int CurrentTurn;
        public int MaxTurn;
    }

    [System.Serializable]
    public class BuildingData
    {
        public int Type;
        public int Version;
        public Sprite Image;
        public Sprite Construction_Image;
        public string Name;
        public string Contents;
        public string ShortContents;
        public AudioClip BuildingSound;
        public AudioClip ConstructSound;

        public int Working_ManPower;
        public bool Workable_Ontile;

        public int Production_Type;
        public float Production_Value;
        

        public List<int> Upkeep_Type = new List<int>(3);
        public List<float> Upkeep_Value = new List<float>(3);

        public List<int> BuildCost_Type = new List<int>(3);
        public List<float> BuildCost_Value = new List<float>(3);

        public List<int> Buff_Type = new List<int>(3);
        public List<float> Buff_Value = new List<float>(3);
        public int Build_TurnCount;

        public int Anti_Disaster;
        public int Allow_Disaster;
        

        public int Buff_Area_Size;

        public int Addon_Slot;

        public int limitNumber;

        public bool Need_Steam;
        public bool Need_Electricity;
        public bool Need_House;
        public bool Need_Public;
        public bool Need_Idea;
        public bool Constructable;
    }

    //인스펙터에 보익 위해 조립하는 클래스
    [System.Serializable]
    public class BuildingData_ListWrapper
    {
        public  List<BuildingData> BD_Version;
    }

    public List <BuildingData_ListWrapper> Bd = new List<BuildingData_ListWrapper>();

    [System.Serializable]
    public class UnlockBuildingData_ListWrapper
    {
        public List<bool> Unlock_Version;
    }

    public List<UnlockBuildingData_ListWrapper> Ub = new List<UnlockBuildingData_ListWrapper>();

    public List<UnlockBuildingData_ListWrapper> Uswb = new List<UnlockBuildingData_ListWrapper>();

    [System.Serializable]
    public class AddonData
    {
        public int Type;
        public int Version;

        public string Name;
        public string Contents;

        public int Buff_ProductType;
        public int Buff_ProductValue;

        public float Buff_ProductScale;

        public int Buff_UpkeepType;
        public float Buff_UpkeepValue;

        public float BuildCost1_Type;
        public float BuildCost1_Value;

        public float BuildCost2_Type;
        public float BuildCost2_Value;

        public float BuildCost3_Type;
        public float BuildCost3_Value;

        
    }

    [System.Serializable]
    public class AddonData_ListWrapper
    {
        public List<AddonData> AO_Version;
    }

    public List<AddonData_ListWrapper> AO = new List<AddonData_ListWrapper>();
    //타일 정보를 세이브하는 함수 07-12
    public void Tile_Save()
    {
        //타일이 생성되었는지 체크한 후 발동
        if (TM.isMaking == true)
        {
            var datalength = TM.OnEnableTileList.Count;//저장할 활성화된 타일 리스트의 총 갯수를 구한다.
            TileData_Save[] tileData_s = new TileData_Save[datalength];//위에서 구한 리스트 갯수만큼의 배열을 생성함

            //배열을 쭉 돌면서 각 인덱스에 정보를 주입한다.
            for (int i = 0; i < datalength; i++)
            {
                GameObject TileData_Save_Object = TM.OnEnableTileList[i];
                TileData TD = TileData_Save_Object.GetComponent<TileData>();
                tileData_s[i] = new TileData_Save
                {
                    OriginalPosition = TD.OriginalPosition,
                    Tile_Number = TD.Tile_Number,
                    FloorType = TD.FloorType,
                    FloorVersion = TD.FloorVersion,
                    Resource_using = TD.Resource_using,
                    Steam_Online = TD.Steam_Online,
                    Electricity_Online = TD.Electricity_Online,
                    House_Online = TD.House_Online,
                    Idea_Online = TD.Idea_Online,
                    Public_Online = TD.Public_Online,
                    Event_Acidente = TD.Event_Acidente,
                    

                    Production_Type = TD.Production_Type,
                    Production_Value = TD.Production_Value,

                    ReType = TD.ReType,
                    ReVersion = TD.ReVersion,

                    EventType = TD.EventType,
                    EventVersion = TD.EventVersion,

                    Current_Disaster = TD.Current_Disaster,
                    Allow_Disaster = TD.Allow_Disaster,
                    Anti_Disaster = TD.Anti_Disaster,
                    Event_Desaster = TD.Event_Desaster,

                    //UpKeep1_Type = TD.UpKeep1_Type,
                    //UpKeep1_Value = TD.UpKeep1_Value,
                    //UpKeep2_Type = TD.UpKeep2_Type,
                    //UpKeep2_Value = TD.UpKeep2_Value,
                    //UpKeep3_Type = TD.UpKeep3_Type,
                    //UpKeep3_Value = TD.UpKeep3_Value,

                    //BuildingType = TD.BuildingType,
                    //BuildingVersion = TD.BuildingVersion,
                    //BuildCost1_Type = TD.BuildCost1_Type,
                    //BuildCost1_Value = TD.BuildCost1_Value,
                    //BuildCost2_Type = TD.BuildCost2_Type,
                    //BuildCost2_Value = TD.BuildCost2_Value,
                    //BuildCost3_Type = TD.BuildCost3_Type,
                    //BuildCost3_Value = TD.BuildCost3_Value,

                    //Buff1_Type = TD.Buff1_Type,
                    //Buff1_Value = TD.Buff1_Value,
                    //Buff2_Type = TD.Buff2_Type,
                    //Buff2_Value = TD.Buff2_Value,
                    //Buff3_Type = TD.Buff3_Type,
                    //Buff3_Value = TD.Buff3_Value,
                    

                    Needed_ManPower = TD.Needed_ManPower,
                    Working_ManPower = TD.Working_ManPower,
                    Constructable_OnTile = TD.Constructable_OnTile,
                    IsWorking_OnTile = TD.IsWorking_OnTile,
                    Distrucktive = TD.Distrucktive,

                    Buff_Area_Size = TD.Buff_Area_Size,

                    Need_Steam = TD.Need_Steam,
                    Need_Electricity = TD.Need_Electricity,
                    Need_House = TD.Need_House,
                    Need_Public = TD.Need_Public,
                    Need_Idea = TD.Need_Idea,

                    IsDistroy_OnTile = TD.IsDistory_OnTile,
                    Turn_Count = TD.Turn_Count,
                };
            }
            //이전 세이브 데이터를 지우고 새 데이터를 만든다.
            ES3.DeleteFile(Application.dataPath + "/Save/SaveFile_Tile");
            //저장할 자료형, 키값, 
            ES3.Save<TileData_Save[]>("SaveFile_Tile", tileData_s, Application.dataPath + "/Save/SaveFile_Tile");
            
            //ES3.Save<int>("ZoneGrade_Save", TM.mapCondition, Application.dataPath + "/Save/SaveFile_Tile");
            //ES3.Save<int>("Columns_Save", TM.columns, Application.dataPath + "/Save/SaveFile_Tile");
            ES3.Save<int>("Rows_Save", TM.rows, Application.dataPath + "/Save/SaveFile_Tile");

        }

        //맵 사이즈 정보를 받아와 로드 시 카메라 컨트롤에 사용, 오류 있음
        //ES3.Save<int>("MapSize_Save", TM.mapStyle, Application.dataPath + "/Save/SaveFile_Tile");
        
        ResourceData_Save ReData_Save = new ResourceData_Save();
        ReData_Save.All_Resource_List = All_Resource_List;
        ReData_Save.MaxTurn = MaxTurn;
        ReData_Save.CurrentTurn = CurrentTurn;

        ES3.Save<ResourceData_Save>("Resource_Save", ReData_Save, Application.dataPath + "/Save/SaveFile_Tile");
        
    }

    //타일 정보를 로드하는 함수
    public void Tile_Load()
    {
        TM.FloorTile_OnDisable();
        TM.isLoading = true;
        var a = ES3.Load<TileData_Save[]>("SaveFile_Tile", Application.dataPath + "/Save/SaveFile_Tile");
        {
            var c = ES3.Load<ResourceData_Save>("Resource_Save", Application.dataPath + "/Save/SaveFile_Tile");

            for (int i = 0; i < SPM.Current_Production.Count; i++)
            {
                SPM.Current_Production[i] = "+0";
            }

            All_Resource_List = c.All_Resource_List;
            CurrentTurn = c.CurrentTurn;
            MaxTurn = c.MaxTurn;
            
        }
        //var d = ES3.Load<int>("ZoneGrade_Save", Application.dataPath + "/Save/SaveFile_Tile");
        TM.mapCondition = (TileManager.MapCondition)ES3.Load<int>("ZoneGrade_Save", Application.dataPath + "/Save/SaveFile_Tile");
        TM.MapConditionChoose(false);
        TM.columns = ES3.Load<int>("Columns_Save", Application.dataPath + "/Save/SaveFile_Tile");
        TM.rows = ES3.Load<int>("Rows_Save", Application.dataPath + "/Save/SaveFile_Tile");
        TM.Ms.SettingLimit();

        for (int i = 0; i < a.Length; i++)
        {
            TM.OnEnableTileList.Add(TM.OndisableTileList[i]);
            TM.OnEnableTileList_ResourceLayoutOnly.Add(TM.OndisableTileList[i]);
        }

        for (int i = 0; i < a.Length; i++)
        {
            if(a[i].FloorType != 99 && i != a.Length - 1)
            {
                var b = TM.OnEnableTileList[i].GetComponent<TileData>();
                b.OriginalPosition = a[i].OriginalPosition;
                b.Tile_Number = a[i].Tile_Number;
                b.FloorType = a[i].FloorType;
                b.FloorVersion = a[i].FloorVersion;
                b.ReType = a[i].ReType;
                b.ReVersion = a[i].ReVersion;
                b.EventType = a[i].EventType;
                b.EventVersion = a[i].EventVersion;
                b.BuildingType = a[i].BuildingType;
                b.BuildingVersion = a[i].BuildingVersion;
                b.Production_Type = a[i].Production_Type;
                b.Production_Value = a[i].Production_Value;
                //b.BuildCost1_Type = a[i].BuildCost1_Type;
                //b.BuildCost1_Value = a[i].BuildCost1_Value;
                //b.BuildCost2_Type = a[i].BuildCost2_Type;
                //b.BuildCost2_Value = a[i].BuildCost2_Value;
                //b.BuildCost3_Type = a[i].BuildCost3_Type;
                //b.BuildCost3_Value = a[i].BuildCost3_Value;
                //b.Buff1_Type = a[i].Buff1_Type;
                //b.Buff1_Value = a[i].Buff1_Value;
                //b.Buff2_Type = a[i].Buff2_Type;
                //b.Buff2_Value = a[i].Buff2_Value;
                //b.Buff3_Type = a[i].Buff3_Type;
                //b.Buff3_Value = a[i].Buff3_Value;
                //b.UpKeep1_Type = a[i].UpKeep1_Type;
                //b.UpKeep1_Value = a[i].UpKeep1_Value;
                //b.UpKeep2_Type = a[i].UpKeep2_Type;
                //b.UpKeep2_Value = a[i].UpKeep2_Value;
                //b.UpKeep3_Type = a[i].UpKeep3_Type;
                //b.UpKeep3_Value = a[i].UpKeep3_Value;
                //Debug.Log("i - 1 < 0로 작동");
                b.Current_Disaster = a[i].Current_Disaster;
                b.Event_Desaster = a[i].Event_Desaster;
                b.Allow_Disaster = a[i].Allow_Disaster;
                b.Anti_Disaster = a[i].Anti_Disaster;
                b.Buff_Area_Size = a[i].Buff_Area_Size;

                b.IsDistory_OnTile = a[i].IsDistroy_OnTile;
                b.IsWorking_OnTile = a[i].IsWorking_OnTile;
                b.Constructable_OnTile = a[i].Constructable_OnTile;
                b.Distrucktive = a[i].Distrucktive;

                b.Need_Electricity = a[i].Need_Electricity;
                b.Need_Steam = a[i].Need_Steam;
                b.Need_House = a[i].Need_House;
                b.Need_Idea = a[i].Need_Idea;
                b.Need_Public = a[i].Need_Public;

                b.Needed_ManPower = a[i].Needed_ManPower;
                b.Working_ManPower = a[i].Working_ManPower;
                

                b.Turn_Count = a[i].Turn_Count;
                b.Resource_using = a[i].Resource_using;
            }
            else if(a[i].FloorType == 99)
            {
                Debug.Log("스틸웨일 콜");
                TileData d = SteelWhale.GetComponent<TileData>();

                d.OriginalPosition = a[i].OriginalPosition;
                d.Tile_Number = a[i].Tile_Number;
                d.FloorType = a[i].FloorType;
                d.FloorVersion = a[i].FloorVersion;
                d.ReType = a[i].ReType;
                d.ReVersion = a[i].ReVersion;
                d.EventType = a[i].EventType;
                d.EventVersion = a[i].EventVersion;
                d.BuildingType = a[i].BuildingType;
                d.BuildingVersion = a[i].BuildingVersion;
                d.Production_Type = a[i].Production_Type;
                d.Production_Value = a[i].Production_Value;
                //d.BuildCost1_Type = a[i].BuildCost1_Type;
                //d.BuildCost1_Value = a[i].BuildCost1_Value;
                //d.BuildCost2_Type = a[i].BuildCost2_Type;
                //d.BuildCost2_Value = a[i].BuildCost2_Value;
                //d.BuildCost3_Type = a[i].BuildCost3_Type;
                //d.BuildCost3_Value = a[i].BuildCost3_Value;
                //d.Buff1_Type = a[i].Buff1_Type;
                //d.Buff1_Value = a[i].Buff1_Value;
                //d.Buff2_Type = a[i].Buff2_Type;
                //d.Buff2_Value = a[i].Buff2_Value;
                //d.Buff3_Type = a[i].Buff3_Type;
                //d.Buff3_Value = a[i].Buff3_Value;
                //d.UpKeep1_Type = a[i].UpKeep1_Type;
                //d.UpKeep1_Value = a[i].UpKeep1_Value;
                //d.UpKeep2_Type = a[i].UpKeep2_Type;
                //d.UpKeep2_Value = a[i].UpKeep2_Value;
                //d.UpKeep3_Type = a[i].UpKeep3_Type;
                //d.UpKeep3_Value = a[i].UpKeep3_Value;
                //Debug.Log("i - 1 < 0로 작동");
                d.Current_Disaster = a[i].Current_Disaster;
                d.Event_Desaster = a[i].Event_Desaster;
                d.Allow_Disaster = a[i].Allow_Disaster;
                d.Anti_Disaster = a[i].Anti_Disaster;

                d.Buff_Area_Size = a[i].Buff_Area_Size;

                d.IsDistory_OnTile = a[i].IsDistroy_OnTile;
                d.IsWorking_OnTile = a[i].IsWorking_OnTile;
                d.Constructable_OnTile = a[i].Constructable_OnTile;
                d.Distrucktive = a[i].Distrucktive;

                d.Need_Electricity = a[i].Need_Electricity;
                d.Need_Steam = a[i].Need_Steam;
                d.Needed_ManPower = a[i].Needed_ManPower;
                d.Working_ManPower = a[i].Working_ManPower;

                d.Turn_Count = a[i].Turn_Count;
                d.Resource_using = a[i].Resource_using;
                SteelWhale.SetActive(true);
                TM.OnEnableTileList.Remove(TM.OnEnableTileList[i]);
            }
        }
        TM.FloorTile_Active();

        //카메라 컨트롤을 위해 맵 사이즈 정보를 받아옴
        TM.mapStyle = (TileManager.MapSize)ES3.Load<int>("MapSize_Save", Application.dataPath + "/Save/SaveFile_Tile"); ;
        TM.CameraSetting();

    }


    //데이터를 저장하는 함수. 세이브 버튼과 연동, 데이터 로드 함수를 만들때 타일 로드를 데이터 로드 후에 둬야한다.
    public void Data_Save()
    {
        Tile_Save();
    }
    
    public void Check_Current_Product(int a, string b)
    {
        if (a > 0)
        {
            b = "(+" + a + ")";
        }
        else if (a < 0)
        {
            b = "(-" + a + ")";
        }
        else b = "";
    }

    
    public void Building_TurnCount()
    {
        for(int i = 0; i < TM.On_Build_Tile.Count; i++)
        {
            if (TM.On_Build_Tile[i].GetComponent<TileData>().Turn_Count > 0)
            {
                TM.On_Build_Tile[i].GetComponent<TileData>().Turn_Count -= 1;
            }
            else if(TM.On_Build_Tile[i].GetComponent<TileData>().Turn_Count == 0)
            {
                TM.On_Build_Tile[i].GetComponent<TileData>().IsWorking_OnTile = true;
                TM.On_Working_Tile.Add(TM.On_Build_Tile[i]);
                TM.On_Build_Tile.Remove(TM.On_Build_Tile[i]);
            }
        }
    }

    public void Check_Mind()
    {
        Will.maxValue = All_Resource_List[33];
        Hope.maxValue = All_Resource_List[34];
        Complaint.maxValue = All_Resource_List[35];
        Will.DOValue(All_Resource_List[30], 2, false);
        Hope.DOValue(All_Resource_List[31], 2, false);
        Complaint.DOValue(All_Resource_List[32], 2, false);
    }

    //자원을 전부 합하는 계산식
    public void Resource_Caculate()
    {
        //식량 체크용
        var a = 0;
        var b = 0;
        var c = 0;

        All_Resource_List[0] += All_Resource_PerTurn_List_Total[0];
        All_Resource_List[1] += All_Resource_PerTurn_List_Total[1];
        All_Resource_List[2] += All_Resource_PerTurn_List_Total[2];
        All_Resource_List[3] += All_Resource_PerTurn_List_Total[3];
        All_Resource_List[4] += All_Resource_PerTurn_List_Total[4];
        All_Resource_List[55] += All_Resource_PerTurn_List_Total[55] + Consume_Food_By_Ticket(false);

        //식량이 적게 남았다면
        if (All_Resource_List[6] + All_Resource_PerTurn_List_Total[6] + Consume_Food_Value() + Consume_Food_By_Ticket(true) < 0)
        {
            //소비해야할 식량
            a = All_Resource_List[6] + All_Resource_PerTurn_List_Total[6] + Consume_Food_Value() + Consume_Food_By_Ticket(true);
            //소비해야할 통조림
            if (a < 0)
            {
                b = All_Resource_List[7] + All_Resource_PerTurn_List_Total[7] + a;
            }
            //소비해야할 날식량
            if (b < 0)
            {
                c = All_Resource_List[5] + All_Resource_PerTurn_List_Total[5] + b;
            }
            //통조림마저 없다면
            if (b <= 0)
            {
                All_Resource_List[7] = All_Resource_List[7] + a;
                //날식량 마저 없다면;;;
                if (c < 0)
                {
                    All_Resource_List[5] = All_Resource_List[5] + b;
                    if (HungryCount <= 0)
                    {
                        ManDead(-c, 0);

                        //불만 증가
                        All_Resource_List[32] += 10;
                    }
                    else if(HungryCount > 0)
                    {
                        HungryCount -= 2;
                        All_Resource_List[32] += 5;
                    }
                }
                //날식량은 있다면
                else if (c >= 0)
                {
                    All_Resource_List[5] = All_Resource_List[5] + b;
                }
            }
            //통조림이 있다면
            else if (b >= 0)
            {
                All_Resource_List[7] = All_Resource_List[7] + a;
            }
        }
        All_Resource_List[6] += All_Resource_PerTurn_List_Total[6] + Consume_Food_Value() + Consume_Food_By_Ticket(true);

        All_Resource_List[5] += All_Resource_PerTurn_List_Total[5];;
        All_Resource_List[7] += All_Resource_PerTurn_List_Total[7];
        All_Resource_List[8] += All_Resource_PerTurn_List_Total[8];
        All_Resource_List[12] += All_Resource_PerTurn_List_Total[12];
        All_Resource_List[13] += All_Resource_PerTurn_List_Total[13];
        All_Resource_List[14] += All_Resource_PerTurn_List_Total[14];
        All_Resource_List[30] += All_Resource_PerTurn_List_Total[30];
        All_Resource_List[31] += All_Resource_PerTurn_List_Total[31];
        All_Resource_List[32] += All_Resource_PerTurn_List_Total[32];
        All_Resource_List[15] += All_Resource_PerTurn_List_Total[15];
        All_Resource_List[39] += (All_Resource_PerTurn_List_Total[39] - All_Resource_PerTurn_List_Total[19]);
        All_Resource_List[55] += Consume_Food_By_Ticket(false);

        for (int i = 0; i < All_Resource_List.Count; i++)
        {
            if(All_Resource_List[i] < 0)
            {
                All_Resource_List[i] = 0;
            }
        }
    }

    public void ManDead(int a, int cause)
    {
        All_Resource_List[38] = All_Resource_List[38] - a;
        if (All_Resource_List[38] <= a)
        {
            if (Over.CS.Contains(Guide.Casues.AllDead) != true)
            {
                Over.CS.Add(Guide.Casues.AllDead);
            }
        }
        switch (cause)
        {
            case 0:
                AP.Add_Alarm(a + "명이 아사했습니다.", true);
                break;
            case 1:
                AP.Add_Alarm(a + "명이 병사했습니다.", true);
                break;
            case 2:
                AP.Add_Alarm(a + "명이 살해당했습니다.", true);
                break;
            case 3:
                AP.Add_Alarm(a + "명이 외부에서 실종되었습니다.", true);
                break;
        }
    }

    public IEnumerator Check_GameOver()
    {
        WaitForSeconds a = new WaitForSeconds(0.1f);

        while(IsOver == false)
        {
            if (All_Resource_List[35] <= All_Resource_List[32] && Over.CS.Contains(Guide.Casues.MaxCompliment) == false)
            {
                Over.CS.Add(Guide.Casues.MaxCompliment);
                IsOver = true;
            }
            if (All_Resource_List[34] <= 0 && Over.CS.Contains(Guide.Casues.MinHope) == false)
            {
                Over.CS.Add(Guide.Casues.MinHope);
                IsOver = true;
            }
            if (All_Resource_List[38] <= 0 && Over.CS.Contains(Guide.Casues.AllDead) == false)
            {
                Over.CS.Add(Guide.Casues.AllDead);
                IsOver = true;
            }
            //나중에 탈출 가능한지 불체크하자
            if (MaxTurn <= CurrentTurn && Over.CS.Contains(Guide.Casues.CantEscape) == false)
            {
                Over.CS.Add(Guide.Casues.CantEscape);
                IsOver = true;
            }

            yield return a;
        }
        Overobj.SetActive(true);
        StartCoroutine(Over.GameOver());
    }

    public void ManPower_Setting()
    {
        float a = 0f;
        //나중에 병으로 인한 패널티도 넣자
        switch (medicalState)
        {
            case MedicalState.Good:
                a = 0.9f;
                break;
            case MedicalState.Normal:
                a = 1f;
                break;
            case MedicalState.Bad:
                a = 1.3f;
                break;
            case MedicalState.Collapse:
                a = 1.7f;
                break;
        }
        All_Resource_List[29] = (All_Resource_List[38] / (int)(All_Resource_List[40] * a)) + All_Resource_List[24] + All_Resource_List[25];
        All_Resource_List[28] = All_Resource_List[29] - All_Resource_List[23];
    }

    public void ManPower_Caculate()
    {
        if (All_Resource_List[28] < 0)
        {
            while (All_Resource_List[28] < 0)
            {
                TM.On_Working_Tile[TM.On_Working_Tile.Count-1].GetComponent<TileData>().Toggle_On();

            }
        }
    }

    //턴 끝났을 때 즉각적으로 반응
    public void Patient_Caculate()
    {
        float b = 0;
        int people = All_Resource_List[38];

        if (TM.On_Working_Tile.Count > 0)
        {
            for (int i = 0; i < TM.On_Working_Tile.Count; i++)
            {
                //파괴 중인 건물은 환자 체크 하지 않음
                if (TM.On_Working_Tile[i].IsDistory_OnTile == false)
                {
                    if (TM.On_Working_Tile[i].BuildingType == 18)
                    {
                        int j = TM.On_Working_Tile[i].PeoPle_Living;
                        people -= j;
                        b = b
                            + (j
                            * Desaster_Building_Value(TM.On_Working_Tile[i].Current_Disaster)
                            * 0.5f
                            * Difficulty_Paitient());
                    }
                    else
                    {
                        int j = TM.On_Working_Tile[i].Working_ManPower * All_Resource_List[40];
                        b = b
                            + (j
                            * Desaster_Building_Value(TM.On_Working_Tile[i].Current_Disaster)
                            * Difficulty_Paitient());
                    }
                }
            }
        }
        if(OverCrowding == true && stage_Escalation != Stage_Escalation.dawn)
        {
            //스틸웨일 오염값 참고하자, 거 뭐시냐
            b = b
              + (people
              * (Desaster_Building_Value(TM.OnEnableTileList[TM.OnEnableTileList.Count - 1].GetComponent<TileData>().Current_Disaster) + 0.2f)
              * Difficulty_Paitient()
              );
        }
        if (All_Resource_List[39] + (int)b > All_Resource_List[38])
        {
            All_Resource_List[39] = All_Resource_List[38];
        }
        All_Resource_List[39] = All_Resource_List[39] + (int)b;
        Debug.Log("환자 작동");
    }

    //환자 사망은 환자계산 - 자원계산(의료)를 적용한 후의 숫자이기 때문에 치료받지 못하고 남은 환자의 숫자다
    public void Check_ManDie_Medical()
    {
        //여기다 랜덤확률 박기
        var a = Random.Range(0, 101);
        var b = 0f;
        var c = 0f;
        switch (medicalState)
        {
            case MedicalState.Bad:
                switch (difficulty)
                {
                    case Difficulty.normal:
                        b = Random.Range(0.1f, 0.5f);
                        c = All_Resource_List[39] * b * Difficulty_IlltoDie();
                        //랜덤 돌려서 20% 확률로 사망
                        if (a > 80)
                        {
                            //집중치료 법안 해금시 사망률 절반
                            if (c > 0)
                            {
                                if (Spacial_Effect_Unlock[0].List[0] == true)
                                {
                                    All_Resource_List[39] -= (int)(c * 0.75f);
                                    ManDead((int)(c * 0.5f), 1);
                                }
                                else if (Spacial_Effect_Unlock[0].List[0] != true)
                                {
                                    All_Resource_List[39] -= (int)c;
                                    ManDead((int)c, 1);
                                }
                            }
                        }
                        break;
                    case Difficulty.hard:
                        b = Random.Range(0.1f, 0.5f);
                        c = All_Resource_List[39] * b * Difficulty_IlltoDie();
                        //랜덤 돌려서 30% 확률로 사망
                        if (a > 70)
                        {
                            //집중치료 법안 해금시 사망률 절반
                            if (c > 0)
                            {
                                if (Spacial_Effect_Unlock[0].List[0] == true)
                                {
                                    All_Resource_List[39] -= (int)(c * 0.5f);
                                    ManDead((int)(c * 0.5f), 1);
                                } 
                                else if (Spacial_Effect_Unlock[0].List[0] != true)
                                {
                                    All_Resource_List[39] -= (int)c;
                                    ManDead((int)c, 1);
                                }
                            }
                        }
                        break;
                }
                break;
            case MedicalState.Collapse:
                switch (difficulty)
                {
                    case Difficulty.easy:
                        b = Random.Range(0.3f, 0.7f);
                        c = All_Resource_List[39] * b * Difficulty_IlltoDie();
                        //랜덤 돌려서 40% 확률로 사망
                        if (a > 60)
                        {
                            //집중치료 법안 해금시 사망률 절반
                            if (c > 0)
                            {
                                if (Spacial_Effect_Unlock[0].List[0] == true)
                                {
                                    All_Resource_List[39] -= (int)(c * 0.5f);
                                    ManDead((int)(c * 0.5f), 1);
                                }
                                else if (Spacial_Effect_Unlock[0].List[0] != true)
                                {
                                    All_Resource_List[39] -= (int)c;
                                    ManDead((int)c, 1);
                                }
                            }
                        }
                        break;
                    case Difficulty.normal:
                        b = Random.Range(0.3f, 0.7f);
                        c = All_Resource_List[39] * b * Difficulty_IlltoDie();
                        //랜덤 돌려서 50% 확률로 사망
                        if (a > 50)
                        {
                            //집중치료 법안 해금시 사망률 절반
                            if (c > 0)
                            {
                                if (Spacial_Effect_Unlock[0].List[0] == true)
                                {
                                    All_Resource_List[39] -= (int)(c * 0.5f);
                                    ManDead((int)(c * 0.5f), 1);
                                }
                                else if(Spacial_Effect_Unlock[0].List[0] != true)
                                {
                                    All_Resource_List[39] -= (int)c;
                                    ManDead((int)c, 1);
                                }
                            }
                        }
                        break;
                    case Difficulty.hard:
                        b = Random.Range(0.3f, 0.7f);
                        c = All_Resource_List[39] * b * Difficulty_IlltoDie();
                        //랜덤 돌려서 60% 확률로 사망
                        if (a > 40)
                        {
                            //집중치료 법안 해금시 사망률 절반
                            if (c > 0)
                            {
                                if (Spacial_Effect_Unlock[0].List[0] == true)
                                {
                                    Debug.Log("에엥!");
                                    All_Resource_List[39] -= (int)(c * 0.5f);
                                    ManDead((int)(c * 0.5f), 1);
                                }
                                else if (Spacial_Effect_Unlock[0].List[0] != true)
                                {
                                    Debug.Log("에엥?");
                                    All_Resource_List[39] -= (int)c;
                                    ManDead((int)c, 1);
                                }
                            }
                        }
                        break;
                }
                break;
            case MedicalState.Good:
                break;
            case MedicalState.Normal:
                break;
        }
        ManPower_Setting();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Event_TurnCount()
    {
        //이벤트 매니저에서 이벤트 처리 함수를 만들고 턴 카운트 깎음
        //EM.Active_Slot
    }

    public void RuleActive_001(Toggle toggle, GameObject a)
    {
        if(toggle.isOn == true)
        {
            Debug.Log("ㅁㅇㅁㅇㅁ");
            a.GetComponent<Button>().interactable = false;
        }
        else if (toggle.isOn == false)
        {
            Debug.Log("zzzzz");
            a.GetComponent<Button>().interactable = true;
        }
    }
    public void RuleActive_002(bool isNormal)
    {

    }
    public void RuleActive_003(bool isNormal)
    {

    }
    public void RuleActive_004(bool isNormal)
    {

    }

    public IEnumerator TurnGage_Active()
    {
        TurnButton.gameObject.SetActive(false);
        TurnGage.gameObject.SetActive(true);
        TurnGage.value = 0;
        TurnGage.DOValue(TurnGage.maxValue, All_Resource_List[47], false);
        Turn_Wheel.transform.DORotate(new Vector3(0, 0, 360), All_Resource_List[47], RotateMode.LocalAxisAdd);
        yield return new WaitForSeconds(All_Resource_List[47]);
        TurnGage.gameObject.SetActive(false);
        TurnButton.gameObject.SetActive(true);
    }

    /*==========================================================================================*/
    /*==========================================================================================*/
    /*===================================턴 엔드================================================*/
    /*==========================================================================================*/
    /*==========================================================================================*/
    /*==========================================================================================*/
    //턴 버튼을 눌렀을 때 발동한다.
    public void Turn_End()
    {
        //TEST
        StartCoroutine(TM.BuildingPopup());


        if(All_Resource_List[54] > 0)
        {
            All_Resource_List[54] -= 1;
        }
        //스테이지 에스컬레이션 체크
        Setting_Stage_Escalation();

        CurrentTurn++;

        for(int i = 0; i < TM.OnEnableTileList.Count; i++)
        {
            if (TM.On_Build_Tile.Contains(TM.OnEnableTileList[i].GetComponent<TileData>()) || TM.On_Distroy_Tile.Contains(TM.OnEnableTileList[i].GetComponent<TileData>()))
            {
                TM.OnEnableTileList[i].GetComponent<TileData>().Turn_Prograss();
            }
        }
        if (MS.Tile_Selected != null)
        {
            if(MS.Tile_Selected.GetComponent<TileData>().UpCheck == true)
            {
               MS.Tile_Selected.GetComponent<TileData>().Tile_Click();
               MS.selectedTileScript.StartCoroutine(MS.selectedTileScript.Slot_Move_Coroutine(MS.selectedTileScript.gameObject, MS.selectedTileScript.DownPosition, MS.selectedTileScript.MoveCheck, 0.7f));
               MS.selectedTileScript.MoveCheck = false;
               MS.Tile_Selected = null;
            }
        }

        //환자 계산
        Patient_Caculate();

        //자원을 전부 합해서 계산
        Resource_Caculate();

        //활성화된 이벤트의 타이머 작동
        EM.Event_Turn_Count();

        //룰매니저의 턴제 작동
        RM.WorkerCalc();

        //탐사대 이벤트 작동
        TurnEnd_Explorer.Invoke();

        //환자 사망
        Check_ManDie_Medical();

        TurnEnd_Quest.Invoke();

        StartCoroutine(TurnGage_Active());

        TM.Add_NearAndOn_All();
        StartCoroutine(AP.Alarm_Call());
        
        Check_Mind();
    }

    public IEnumerator Check_City_State()
    {
        WaitForEndOfFrame a = new WaitForEndOfFrame();
        while (true)
        {
            Check_OverCrowding();
            Check_MedcialCondition();
            yield return a;
        }
    }

    public void Check_OverCrowding()
    {
        int a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 5;
                break;
            case Difficulty.normal:
                a = 7;
                break;
            case Difficulty.hard:
                a = 13;
                break;
        }
        if (stage_Escalation != Stage_Escalation.dawn)
        {
            if (OverCrowding == false && All_Resource_List[38] > All_Resource_List[17])
            {
                OverCrowding = true;
                All_Resource_PerTurn_List_Total[32] += a;
                Debug.Log("인구 과밀화");
            }
            else if (OverCrowding == true && All_Resource_List[38] <= All_Resource_List[17])
            {
                OverCrowding = false;
                All_Resource_PerTurn_List_Total[32] -= a;
                Debug.Log("인구 안정");
            }
        }
    }

    public void Check_Economy_Ticket()
    {
    }

    public void Check_MedcialCondition()
    {
        int a = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                a = 5;
                break;
            case Difficulty.normal:
                a = 7;
                break;
            case Difficulty.hard:
                a = 13;
                break;
        }
        int z = 0;
        switch (difficulty)
        {
            case Difficulty.easy:
                z = 13;
                break;
            case Difficulty.normal:
                z = 7;
                break;
            case Difficulty.hard:
                z = 5;
                break;
        }
        float b = 0;
        if(All_Resource_List[39] != 0)
        {
            b = (float)All_Resource_List[39] / (float)All_Resource_List[38];
        }

        MedicalState c = new MedicalState();

        if (b >= 0.41f)
        {
            c = MedicalState.Collapse;
        }
        else if (b >= 0.21f)
        {
            c = MedicalState.Bad;
        }
        else if (b >= 0.06f)
        {
            c = MedicalState.Normal;
        }
        else if(b >= 0)
        {
            c = MedicalState.Good;
        }

        if (medicalState != c)
        {
            switch (medicalState)
            {
                case MedicalState.Good:
                    All_Resource_PerTurn_List_Total[30] -= z;
                    break;
                case MedicalState.Normal:
                    break;
                case MedicalState.Bad:
                    All_Resource_PerTurn_List_Total[31] += a;
                    break;
                case MedicalState.Collapse:
                    All_Resource_PerTurn_List_Total[31] += a * 2;
                    break;
            }
            switch (c)
            {
                case MedicalState.Good:
                    All_Resource_PerTurn_List_Total[30] += z;
                    break;
                case MedicalState.Normal:
                    break;
                case MedicalState.Bad:
                    All_Resource_PerTurn_List_Total[31] -= a;
                    break;
                case MedicalState.Collapse:
                    All_Resource_PerTurn_List_Total[31] -= a * 2;
                    break;
            }
            medicalState = c;
        }
    }

    public float Escalation_Revise(Difficulty a)
    {
        float b = 0;
        switch (a)
        {
            case Difficulty.easy:
                b = 0.3f;
                break;
            case Difficulty.normal:
                b = 0.2f;
                break;
            case Difficulty.hard:
                b = 0.1f;
                break;
        }
        return b;
    }


    public void Setting_Stage_Escalation()
    {
        Stage_Escalation a = stage_Escalation;
        if (All_Resource_List[48] > CurrentTurn)
        {
            stage_Escalation = Stage_Escalation.dawn;
        }
        else if (CurrentTurn <= MaxTurn * 0.5f)
        {
            stage_Escalation = Stage_Escalation.adaptive;
        }
        else if (CurrentTurn <= MaxTurn * 0.8f)
        {
            stage_Escalation = Stage_Escalation.twilight;
        }
        else
        {
            stage_Escalation = Stage_Escalation.exodus;
        }
        if (a != stage_Escalation)
        {
            AC.Announce_Age();
        }
    }
    

    public void ReloadScene()
    {
        SceneManager.LoadScene("Main");
    }


    //세이브 로드 시 텍스트 이미지를 위 아래로 움직이게 할 함수. 당장은 쓸 필요 없다 07-12
    public IEnumerator Save_Load_Message()
    {
        return null;
    }

    public void Reset()
    {
        Datamanager a = Original.GetComponent<Datamanager>();
        DM.All_Resource_List = a.All_Resource_List;
    }

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            StartCoroutine(Check_GameOver());
            StartCoroutine(Check_City_State());
            if (gameObject != null)
            {
                //이건 해결법을 모르겠군
                //DontDestroyOnLoad(gameObject);
            }
            if (IsReset == false)
            {
                Check_Mind();
                ManPower_Setting();
                IsReset = true;
            }
            StartCoroutine(TurnGage_Active());
            QM.Quest_Allocation(QM.QuestList_All[0]);
        }
    }

    public void Awake()
    {

        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);


        DontDestroyOnLoad(gameObject);

    }

    public string resources(int i, bool ispostposition)
    {
        string a = "";
        switch (i)
        {
            case 0:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "철";
                }
                break;
            case 1:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "부품";
                }
                break;
            case 2:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "합금";
                }
                break;
            case 3:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "연료";
                }
                break;
            case 4:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "골디온";
                }
                break;
            case 5:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "식자재";
                }
                break;
            case 6:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "식사";
                }
                break;
            case 7:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "통조림";
                }
                break;
            case 8:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "목재";
                }
                break;
            case 9:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "증기 반경";
                }
                break;
            case 10:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "전기 반경";
                }
                break;
            case 11:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "버프 반경";
                }
                break;
            case 12:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "기술 포인트";
                }
                break;
            case 13:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "작업 포인트";
                }
                break;
            case 14:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "전기";
                }
                break;
            case 15:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "증기";
                }
                break;
            case 16:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                a = "사상반경";
                }
                break;
            case 17:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "제공 주거";
                }
                break;
            case 18:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "공공 반경";
                }
                break;
            case 19:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "제공 의료";
                }
                break;
            case 20:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "주거 반경";
                }
                break;
            case 21:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "건물 기본 방재";
                }
                break;
            case 22:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "건물 기본 건설 턴";
                }
                break;
            case 23:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "사용중 인력";
                }
                break;
            case 24:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "추가 인력";
                }
                break;
            case 25:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "수트맨 인력";
                }
                break;
            case 26:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "로이드 엔진";
                }
                break;
            case 27:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "에고 엔진";
                }
                break;
            case 28:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "현재 인력";
                }
                break;
            case 29:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "최대 인력";
                }
                break;
            case 30:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "의지";
                }
                break;
            case 31:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "희망";
                }
                break;
            case 32:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "불만";
                }
                break;
            case 33:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "의지";
                }
                break;
            case 34:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "희망";
                }
                break;
            case 35:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "불만";
                }
                break;
            case 36:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "현재 제공 의료";
                }
                break;
            case 37:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "현재 주거 제공";
                }
                break;
            case 38:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "인구";
                }
                break;
            case 39:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "환자";
                }
                break;
            case 40:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "인구 당 인력";
                }
                break;
            case 41:
                if (ispostposition == true)
                {
                }
                else
                {
                    a = "";
                }
                break;
            case 42:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "전체 오염";
                }
                break;
            case 43:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(3);
                }
                else
                {
                    a = "인구당 식량 소모";
                }
                break;
            case 44:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "탐사대 보급품";
                }
                break;
            case 45:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "탐사대 제한";
                }
                break;
            case 46:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "탐사대 기본 인력";
                }
                break;
            case 47:
                if (ispostposition == true)
                {
                    a = Check_PostPosition(2);
                }
                else
                {
                    a = "";
                }
                break;

        }
        return a;
    }
    public string Check_PostPosition(int b)
    {
        string a = "";

        if (b == 0)
        {
            a = "을";
        }
        else if (b == 1)
        {
            a = "를";
        }
        else if (b == 2)
        {
            a = "이";
        }
        else if (b == 3)
        {
            a = "가";
        }
        return a;
    }


}


[Serializable]
public class TutorialSave
{

}


[Serializable]
public class FoundZone
{
    public int ZoneGrade;
    public int ZoneSize;
    public int ZoneFertileGrade;

    public FoundZone(int Grade, int Size, int FertileGrade)
    {
        ZoneGrade = Grade;
        ZoneSize = Size;
        ZoneFertileGrade = FertileGrade;
    }
}
