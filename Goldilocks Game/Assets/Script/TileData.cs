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

public class TileData : MonoBehaviour
{
    private Sequence SideSequence;
    private Sequence XmarkSequence;
    //이건 TM에서 OnDisEnable 리스트를 만들 때 추가해야해. 프리팹 단계에선 추가하지 못해
    public TileManager TM;

    public PopUpUi PopUp;

    public GameObject BuildOrCrash_Particle;
    public GameObject BuildComplite_Particle;
    public GameObject BuildingWoriking_Particle;

    public MouseControll MS;

    public SpriteRenderer Floor;
    public SpriteRenderer Event;
    public SpriteRenderer Building;
    public SpriteRenderer Resource;
    public SpriteRenderer Side;

    public Vector3 OriginalPosition;//Onenable할 때만 값을 할당하여 세이브 로드할 때만 이용
    public Vector3 CurrentTilePosition;
    public GameObject AreaCheck;
    public GameObject Xmark;

    public int Columns;
    public int Rows;

    Coroutine FadeIn;
    Coroutine FadeOut;
    Coroutine SideClear;
    Coroutine DeadCount;
    Coroutine ParticleCheck;

    public int Tier;
    public int CanUpgrade;
    public int Sickness;
    public int Tile_Number;
    public int FloorType;
    public int FloorVersion;
    public AudioSource BuildingSound;
    
    public bool Resource_using;
    public bool Event_Acidente;

    //플롯 계산값은 후에 반올림 처리
    public int Production_Type;
    public float Production_Value;
    
    public List<int> Upkeep_Type = new List<int>(3);
    public List<float> Upkeep_Value = new List<float>(3);

    public List<int> BuildCost_Type = new List<int>(3);
    public List<float> BuildCost_Value = new List<float>(3);

    public List<int> Buff_Type = new List<int>(3);
    public List<float> Buff_Value = new List<float>(3);

    public int Buff_Area_Size;

    public int Turn_Count;

    //OnEnable할 때 If문으로 돌려서 DataManager의 TextList 값을 받아와 할당할 것이다.
    public int ReType;
    public int ReVersion;

    public int EventType;
    public int EventVersion;

    //재난 수치, 방재수치, 허용 방재수치, 이벤트 재난 수치
    public int Current_Disaster;
    public int Anti_Disaster;
    public int Allow_Disaster;
    public int Event_Desaster;

    //건물 타입, 버전, 필요 맨파워
    public int BuildingType;
    public int BuildingVersion;

    public int Needed_ManPower;
    public int Working_ManPower;
    public int PeoPle_Living;

    public bool SuitMan_Working;
    public bool ServeArm_Working;
    public bool Overtime_Working;
    public bool EmergencyWork_Working;

    public bool UpCheck;
    public bool CR_Running;
    //사상, 공공, 집이 근처에 있는지 체크, 만약 있다면 추가 보너스를 얻게 해야한다.
    public bool Steam_Online;
    public bool Electricity_Online;
    public bool House_Online;
    public bool Idea_Online;
    public bool Public_Online;
    public bool SuitMan_Online;

    public bool Need_Steam;
    public bool Need_Electricity;
    public bool Need_House;
    public bool Need_Public;
    public bool Need_Idea;

    //폴리곤 크기 체크를 할 때의 범위값
    public enum PolCol_Size { self, small, middle, large };
    public PolCol_Size polCol_Size;

    //타일 위 건설 가능, 작업 가능, 작업 중 값
    public bool Constructable_OnTile;

    public bool Workable_OnTile()
    {
        bool a = false;

        //자원 체크해서 goto, 기본 자원 사용은 제한없이 가능하니깐
        if (ReType == 2)
        {
            if (Resource_using == false || ReType == 4)
            {
                goto TrueOut;
            }
            else
            {
                goto FalseOut;
            }
        }

        if (ReType == 3 || BuildingType == 0)
        {
            goto FalseOut;
        }
        else if (ReType == 3 || BuildingType == 1)
        {
            a = true;
        }

        //재난 체크해서 goto, 재난 체크에 실패하면 폴스로 직행이니깐
        if (Current_Disaster >= Allow_Disaster)
        {
            a = true;
        }
        else if (Current_Disaster < Allow_Disaster)
        {
            goto FalseOut;
        }

        if (Need_Electricity == true && Electricity_Online == true)
        {
            a = true;
        }
        else if(Need_Electricity == true && Electricity_Online == false)
        {
            goto FalseOut;
        }

        if (Need_House == true && House_Online == true)
        {
            a = true;
        }
        else if (Need_House == true && House_Online == false)
        {
            goto FalseOut;
        }

        if (Need_Idea == true && Idea_Online == true)
        {
            a = true;
        }
        else if(Need_Idea == true && Idea_Online == false)
        {
            goto FalseOut;
        }

        if (Need_Public == true && Public_Online == true)
        {
            a = true;
        }
        else if (Need_Public == true && Public_Online == false)
        {
            goto FalseOut;
        }

        if (Need_Steam == true && Steam_Online == true)
        {
            a = true;
        }
        else if(Need_Steam == true && Steam_Online == false)
        {
            goto FalseOut;
        }


    TrueOut: a = true;
        return a;
    FalseOut: a = false;
        return a;
    }
    public bool IsWorking_OnTile;
    public bool Distrucktive;
    public bool IsDistory_OnTile;


    public int a;

    delegate int Product_Sum();
    Product_Sum GetProduct_Sum;
    

    //클리커게임 DataManager 291번 줄의 TileSetting함수를 보면 끝자락에 ReType을 할당한다. 이걸 이용하자.
    //Onenable할 때 타입을 체크해서 스프라이트를 할당한다. 이제 TM에서 해야할 일들을 하면 돼, 비활성 오브젝트 리스트작성, 자원할당, 작동시키기
    public void Check_Resource_Type_Version(int Type, int Version, SpriteRenderer sprite)
    {
        var Resource_Type = new List<Sprite>();
        Resource.color = new Color32(255, 255, 255, 255);
        if (BuildingType == 0) {
            switch (Type)
            {
                case 1:
                    Resource_Type = TM.Resource_Sprites_Gold;
                    Distrucktive = false;
                    break;
                case 2:
                    Resource_Type = TM.Resource_Sprites_Wood;
                    Distrucktive = true;
                    break;
                case 3:
                    Resource_Type = TM.Resource_Sprites_Ore;
                    if (TM.DM.Spacial_Effect_Unlock[0].List[4] == true)
                    {
                        Distrucktive = true;
                    }
                    else
                    {
                        Distrucktive = false;
                    }
                    break;
                case 4:
                    Resource_Type = TM.Resource_Sprites_Rock;
                    if (TM.DM.Spacial_Effect_Unlock[0].List[0] == true)
                    {
                        Distrucktive = true;
                    }
                    else
                    {
                        Distrucktive = false;
                    }
                    break;
                case 5:
                    Resource_Type = TM.Resource_Sprites_Steel;
                    Distrucktive = false;
                    break;
                case 0:
                    sprite.sprite = null;
                    break;
            }
            if (Type != 0)
            {
                if (Version != 0 && Version != 1)
                {
                    sprite.sprite = Resource_Type[Version - 1];
                }
                else
                    sprite.sprite = Resource_Type[Version];
            }
        }
        else if (BuildingType != 0)
        {
            sprite.sprite = null;
        }

    }

    public void Check_Floor_Type_Version(int Type, int Version, SpriteRenderer sprite)
    {
        var Floor_Type = new List<Sprite>();
        switch (Type)
        {
            case 0:
                Floor_Type = TM.Floor_Types_1;
                break;
            case 1:
                Floor_Type = TM.Floor_Types_1;
                break;
            case 2:
                Floor_Type = TM.Floor_Types_1;
                break;
            default:
                Floor_Type = TM.Floor_Types_1;
                break;
        }

        //흙 기본 타일을 활성화한다.
        var a = Random.Range(0, Floor_Type.Count);
        Floor.sprite = Floor_Type[a];

        if (Type != 0 && Type != 99)
        {
            if (Version != 0 && Version != 1)
            {
                sprite.sprite = Floor_Type[Version - 1];
            }
        }
        //임시로 스틸웨일 4칸으로 설정한 함수. 02-28
        else if(Type == 99)
        {
            sprite.sprite = TM.SteelWhaleMaker.GetComponent<BigTileMaker>().TilesMaker.GetComponent<SteelWhaleMaker>().Floor_T1[0];
        }
        else
        {
            sprite.sprite = Floor_Type[Version];
        }
    }

    public void Check_Building_Type_Version(int Type, int Version, SpriteRenderer sprite)
    {
        if (Type - 1 >= 0)
        {
            if (Turn_Count > 0)
            {
                Resource.sprite = null;
                Building.sprite = TM.DM.Bd[Type - 1].BD_Version[Version - 1].Construction_Image;
            }
            else
            {
                Resource.sprite = null;
                Building.sprite = TM.DM.Bd[Type - 1].BD_Version[Version - 1].Image;
            }
        }
    }

    public IEnumerator Tile_Jump()
    {
        CR_Running = true;
        if (UpCheck == false)
        {
            UpCheck = true;
            for (int i = 0; i < 5; i++)
            {
                CurrentTilePosition = new Vector3(CurrentTilePosition.x, CurrentTilePosition.y + 0.03f, CurrentTilePosition.z);
                gameObject.transform.position = CurrentTilePosition;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (UpCheck == true || transform.position != OriginalPosition)
        {
            UpCheck = false;
            for (int i = 0; i < 5; i++)
            {
                CurrentTilePosition = new Vector3(CurrentTilePosition.x, CurrentTilePosition.y - 0.03f, CurrentTilePosition.z);
                gameObject.transform.position = CurrentTilePosition;
                yield return new WaitForSeconds(0.01f);
            }
        }

        CR_Running = false;
        yield return null;
    }

    //타일이 클릭되면 발동, 마우스 컨트롤 스크립트에서 사용함
    public IEnumerator Tile_Clicked()
    {
        gameObject.transform.DOLocalMoveY(OriginalPosition.y - 0.07f, 0.07f).SetLoops(2, LoopType.Yoyo);
        yield return null; 
    }

    //이벤트 트리거에 의해 타일 점프 코루틴을 발동
    public void Tile_Click()
    {
        if //(CR_Running == false || gameObject.activeSelf == true)
        (gameObject.activeSelf == true)
        {
            StartCoroutine(Tile_Jump());
        }
    }

    public void Check_Disaster()
    {
        if (BuildingType != 0)
        {
            Current_Disaster = (Anti_Disaster + Event_Desaster) + TM.DM.All_Resource_List[21];
        }
        else
        {
            Current_Disaster = (Anti_Disaster + Event_Desaster);
        }
        if (Current_Disaster < Allow_Disaster)
        {
            if(IsWorking_OnTile == true)
            {
                Change_Score();
                IsWorking_OnTile = false;
            }
        }
    }

    public void Check_TechorRule()
    {
        if(IsWorking_OnTile == true)
        {
            IsWorking_OnTile = false;

            for(int i = 0; i < Upkeep_Type.Count; i++)
            {
                Change_Score_UpKeep(i);
            }
            Change_Production();

            IsWorking_OnTile = true;
            if (BuildingType != 0)
            {
                Change_Score();
            }
        }

    }

    public void Check_Acident()
    {
        if(Event_Acidente == true)
        {
            if(IsWorking_OnTile == true)
            {
                Change_Score();
                IsWorking_OnTile = false;
            }
        }
    }

    public void Check_List()
    {
        if (Turn_Count > 0 && BuildingType != 0)
        {
            TM.On_Build_Tile.Add(this);
        }
        if (IsWorking_OnTile == true)
        {
            TM.On_Working_Tile.Add(this);
        }
    }

    public void Distroy_Resource()
    {
        IsWorking_OnTile = true;
        IsDistory_OnTile = true;
        Resource_using = true;
        TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] - Needed_ManPower;
        TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] + Needed_ManPower;
        TM.On_Distroy_Tile.Add(this);
        TM.On_Working_Tile.Add(this);

        //파괴 파티클 작동
        Play_DistructionOrBuild_Particle_On(true);
            switch (ReType)
            {
                case 1:
                    TM.On_Goldion.Remove(this);
                    AreaCheck.SetActive(true);
                    break;
                case 2:
                    TM.On_Wood.Remove(this);
                    AreaCheck.SetActive(true);
                    break;
                case 3:
                    TM.On_Ore.Remove(this);
                    AreaCheck.SetActive(true);
                    break;
                case 4:
                    TM.On_Rock.Remove(this);
                    AreaCheck.SetActive(true);
                    break;
                case 5:
                    TM.On_Steel.Remove(this);
                    AreaCheck.SetActive(true);
                    break;
            }
    }
    public void UnDo_Distroy_Resource()
    {
        IsDistory_OnTile = false;
        IsWorking_OnTile = false;
        Resource_using = false;
        TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] + Needed_ManPower;
        TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] - Needed_ManPower;
        TM.On_Distroy_Tile.Remove(this);
        TM.On_Working_Tile.Remove(this);
        switch (ReType)
        {
            case 1:
                TM.On_Goldion.Add(this);
                AreaCheck.SetActive(true);
                break;
            case 2:
                TM.On_Wood.Add(this);
                AreaCheck.SetActive(true);
                break;
            case 3:
                TM.On_Ore.Add(this);
                AreaCheck.SetActive(true);
                break;
            case 4:
                TM.On_Rock.Add(this);
                AreaCheck.SetActive(true);
                break;
            case 5:
                TM.On_Steel.Add(this);
                AreaCheck.SetActive(true);
                break;
        }
    }

    public void Distroy_Building()
    {
        IsDistory_OnTile = true;
        Resource_using = true;
        if (IsWorking_OnTile == true)
        {
            Toggle_On();
        }
        Change_Color();
        TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] - 1;
        TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] + 1;
        TM.On_Distroy_Tile.Add(this);
        TM.On_Working_Tile.Add(this);
        Turn_Count = TM.DM.All_Resource_List[49] - TM.DM.All_Resource_PerTurn_List_Total[49];
        //Play_DistructionOrBuild_Particle_On();
        IsWorking_OnTile = true;
        //Play_DistructionOrBuild_Particle_On();
    }

    public void UnDo_Distroy_Building()
    {
        IsDistory_OnTile = false;
        Resource_using = false;
        TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] + 1;
        TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] - 1;
        TM.On_Distroy_Tile.Remove(this);
        TM.On_Working_Tile.Remove(this);
        Turn_Count = 0;
    }

    //셀렉티드 타일에서 작업 토글을 누르면 작동함
    public void Toggle_On()
    {
        if (TM.DM.All_Resource_List[28] >= Needed_ManPower)
        {
            if (IsWorking_OnTile == false)
            {

                //자원 파괴 시작
                if (ReType != 0 && BuildingType == 0 && TM.DM.All_Resource_List[28] >= Needed_ManPower)
                {
                    IsWorking_OnTile = true;
                    Distroy_Resource();
                }

                //건물 작업 시작
                else if (Workable_OnTile() == true && BuildingType != 0)
                {
                    IsWorking_OnTile = true;
                    Change_Score();
                    TM.On_Working_Tile.Add(this);
                    TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] - Needed_ManPower;
                    TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] + Needed_ManPower;
                    //건물 파티클 작동
                    //StartCoroutine();
                    TM.Off_Working_Tile.Remove(this);

                    BuildingSound.Play();

                    //변전소일 경우
                    if (BuildingType == 15 && BuildingVersion == 1)
                    {
                        AreaCheck.GetComponent<TileChecker>().Check_Electricity = true;
                        TM.Check_NearBy_Online(TM.On_Electricity, TM.NearBy_Elec_Grid, 0);
                    }
                    //증기돔일 경우
                    if (BuildingType == 16 && BuildingVersion == 1)
                    {
                        AreaCheck.GetComponent<TileChecker>().Check_Steam = true;
                        TM.Check_NearBy_Online(TM.On_Steam, TM.NearBy_Steam_Grid, 1);
                    }
                    //주택일 경우
                    if (BuildingType == 18)
                    {
                        AreaCheck.GetComponent<TileChecker>().Check_House = true;
                        TM.Check_NearBy_Online(TM.On_House, TM.NearBy_House, 2);
                    }
                    //사상건물일 경우
                    if (BuildingType == 10 || BuildingType == 11)
                    {
                        AreaCheck.GetComponent<TileChecker>().Check_Idea = true;
                        TM.Check_NearBy_Online(TM.On_Idea, TM.NearBy_Idea, 4);
                    }
                    //공공시설일 경우
                    if (BuildingType == 19)
                    {
                        AreaCheck.GetComponent<TileChecker>().Check_Public = true;
                        TM.Check_NearBy_Online(TM.On_Public, TM.NearBy_Public, 3);
                    }
                    //Check_Sickness();
                }
                Change_Color();
            }
            //타일 작업 종료
            else
            {
                Toggle_OFF();
            }

        }
        else if (TM.DM.All_Resource_List[28] <= 0)
        {
            Toggle_OFF();
        }

    }

    public void Toggle_OFF()
    {
        IsWorking_OnTile = false;
        BuildingSound.Stop();

        if (ReType != 0 && BuildingType == 0)
        {
            UnDo_Distroy_Resource();
            //Play_DistructionOrBuild_Particle_On();
        }

        if (Workable_OnTile() == true && BuildingType != 0)
        {
            Change_Score();
            TM.Off_Working_Tile.Add(this);
            TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] - Needed_ManPower;
            TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] + Needed_ManPower;
            TM.On_Working_Tile.Remove(this);
            //변전소일 경우
            if (BuildingType == 15 && BuildingVersion == 1)
            {
                AreaCheck.GetComponent<TileChecker>().Check_Electricity = false;
                TM.Check_NearBy_Online(TM.On_Electricity, TM.NearBy_Elec_Grid, 0);
            }
            //증기돔일 경우
            if (BuildingType == 16 && BuildingVersion == 1)
            {
                AreaCheck.GetComponent<TileChecker>().Check_Steam = false;
                TM.Check_NearBy_Online(TM.On_Steam, TM.NearBy_Steam_Grid, 1);
            }
            //주거시설일 경우
            if (BuildingType == 18)
            {
                AreaCheck.GetComponent<TileChecker>().Check_House = false;
                TM.Check_NearBy_Online(TM.On_House, TM.NearBy_House, 2);
            }
            //공공시설일 경우
            if (BuildingType == 19)
            {
                AreaCheck.GetComponent<TileChecker>().Check_Public = false;
                TM.Check_NearBy_Online(TM.On_Public, TM.NearBy_Public, 3);
            }
            //사상시설일 경우
            if (BuildingType == 11)
            {
                AreaCheck.GetComponent<TileChecker>().Check_Idea = false;
                TM.Check_NearBy_Online(TM.On_Idea, TM.NearBy_Idea, 4);
            }
            if (BuildingType == 10)
            {
                AreaCheck.GetComponent<TileChecker>().Check_Idea = false;
                TM.Check_NearBy_Online(TM.On_Idea, TM.NearBy_Idea, 4);
            }
            //Check_Sickness();

        }
        Change_Color();
    }

    //생산과 데이터값을 바꾸는 함수. 로드 시에도 발동한다. 즉 로드할 때 0에서 시작하여 타일 로드를 이용하여 값을 갱신한다.
    public void Change_Score()
    {
        Check_Production();
        a = 0;
        for(int i = 0; i < Upkeep_Type.Count; i++)
        {
            Check_Upkeep(i);
            Change_Score_UpKeep(i);
        }
        Change_Production();
    }

    public void Check_Upkeep(int i)
    {
        var a = TM.DM.Bd[BuildingType - 1].BD_Version[BuildingVersion - 1].Upkeep_Value[i];
        Upkeep_Value[i] =
             a * TM.DM.Difficulty_Upkeep() *
            (1f + TM.DM.Tech_Upkeep_Discharge_Magnification_List + TM.DM.Tech_Upkeep_Type_Magnification_List[BuildingType] + TM.DM.Tech_Upkeep_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]) *
            (1f + TM.DM.Rule_Upkeep_Discharge_Magnification_List + TM.DM.Rule_Upkeep_Type_Magnification_List[BuildingType] + TM.DM.Rule_Upkeep_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]) *
            (1f + TM.DM.Event_Upkeep_Discharge_Magnification_List + TM.DM.Event_Upkeep_Type_Magnification_List[BuildingType] + TM.DM.Event_Upkeep_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]);
    }

    public void Check_Production()
    {
        var a = TM.DM.Bd[BuildingType - 1].BD_Version[BuildingVersion - 1].Production_Value;
        //var a = TM.DM.Bd[BuildingType - 1].BD_Version[BuildingVersion - 1].Production_Value;
        //일단 주거, 공공, 사상계열 건물이 아닌 경우만 체크
        if (BuildingType != 18 || BuildingType != 19 || BuildingType != 10 || BuildingType != 11 || BuildingType != 12)
        {
            Production_Value =
                 (a * TM.DM.Difficulty_Production() + TM.DM.Tech_Production_Type_Integer_List[BuildingType] + TM.DM.Tech_Production_Version_Integer_List[BuildingType].Version_List[BuildingVersion]) *
                 (1f + TM.DM.Tech_Production_Discharge_Magnification_List + TM.DM.Tech_Production_Type_Magnification_List[BuildingType] + TM.DM.Tech_Production_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]) *
                 (1f + TM.DM.Rule_Production_Discharge_Magnification_List + TM.DM.Rule_Production_Type_Magnification_List[BuildingType] + TM.DM.Rule_Production_Version_Magnification_List[BuildingVersion].Version_List[BuildingVersion]) *
                 (1f + TM.DM.Event_Production_Discharge_Magnification_List + TM.DM.Event_Production_Type_Magnification_List[BuildingType] + TM.DM.Event_Production_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]) *
                 TM.DM.Will_Production_Magnification *
                 TM.DM.Escalation_Production_Magnification[BuildingVersion];
        }
        //주거단지일 경우
        else if(BuildingType == 18)
        {
            Production_Value =
                 (a + TM.DM.Tech_Production_Type_Integer_List[BuildingType] + TM.DM.Tech_Production_Version_Integer_List[BuildingType].Version_List[BuildingVersion]) *
                 (1f + TM.DM.Tech_Production_Discharge_Magnification_List + TM.DM.Tech_Production_Type_Magnification_List[BuildingType] + TM.DM.Tech_Production_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]) *
                 (1f + TM.DM.Rule_Production_Discharge_Magnification_List + TM.DM.Rule_Production_Type_Magnification_List[BuildingType] + TM.DM.Rule_Production_Version_Magnification_List[BuildingVersion].Version_List[BuildingVersion]) *
                 (1f + TM.DM.Event_Production_Discharge_Magnification_List + TM.DM.Event_Production_Type_Magnification_List[BuildingType] + TM.DM.Event_Production_Version_Magnification_List[BuildingType].Version_List[BuildingVersion]);
        }
    }
    
    public void Change_Production()
    {
        if (Production_Type != -1)
        {
            if (IsWorking_OnTile == true)
            {
                //불만 희망 의지, 환자 변동치 체크
                if(Production_Type == 36 || Production_Type == 33 || Production_Type == 34 || Production_Type == 35)
                {
                    TM.DM.All_Resource_PerTurn_List_Total[Production_Type] -= (int)Production_Value;
                }
                //주거 체크하는 식
                else if (Production_Type == 17)
                {
                    TM.DM.All_Resource_List[17] += (int)Production_Value;
                    //인구가 제공주거보다 많다면
                    if(TM.DM.All_Resource_List[38] > TM.DM.All_Resource_List[17])
                    {
                        TM.DM.All_Resource_List[37] = TM.DM.All_Resource_List[17];
                        PeoPle_Living = (int)Production_Value;
                    }
                    else if (TM.DM.All_Resource_List[38] <= TM.DM.All_Resource_List[17])
                    {
                        TM.DM.All_Resource_List[37] = TM.DM.All_Resource_List[38];
                        PeoPle_Living = TM.DM.All_Resource_List[38] - TM.DM.All_Resource_List[17];
                    }
                }
                TM.DM.All_Resource_PerTurn_List_Total[Production_Type] += (int)Production_Value;
            }
            else if (IsWorking_OnTile == false)
            {
                if (Production_Type == 36 || Production_Type == 33 || Production_Type == 34 || Production_Type == 35)
                {
                    TM.DM.All_Resource_PerTurn_List_Total[Production_Type] += (int)Production_Value;
                }
                else if (Production_Type == 17)
                {
                    TM.DM.All_Resource_List[17] -= (int)Production_Value;
                    PeoPle_Living = 0;
                }
                TM.DM.All_Resource_PerTurn_List_Total[Production_Type] -= (int)Production_Value;
            }
        }
    }

    public void Change_Score_UpKeep(int i)
    {
            if(IsWorking_OnTile == true)
            {
                TM.DM.All_Resource_PerTurn_List_Total[Upkeep_Type[i]] -= (int)Upkeep_Value[i];
            }
            else if(IsWorking_OnTile == false)
        {
            TM.DM.All_Resource_PerTurn_List_Total[Upkeep_Type[i]] += (int)Upkeep_Value[i];
        }
    }

    public void Change_Color()
    { 
        if (Turn_Count > 0 && BuildingType != 0)
        {
            Building.color = new Color32(255, 255, 255, 255);
            Resource.color = new Color32(255, 255, 255, 255);
        }
        else if (BuildingType != 0 && IsWorking_OnTile == false)
        {
            Building.color = new Color32(150, 150, 150, 255);
        }
        else if (BuildingType != 0 && IsWorking_OnTile == true)
        {
            Building.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            Building.color = new Color32(255, 255, 255, 255);
            Resource.color = new Color32(255, 255, 255, 255);
        }
        if (IsDistory_OnTile == true)
        {
            Resource.color = new Color32(255, 50, 50, 255);
            Building.color = new Color32(255, 50, 50, 255);
        }
    } 

    //안쓰임
    public void Check_Sickness()
    {
        TM.DM.All_Resource_PerTurn_List_Total[39] -= Sickness;
        Sickness = 0;
        if (Current_Disaster != 0)
        {
            var min = -Current_Disaster * Needed_ManPower;
            var max = -Current_Disaster * Needed_ManPower * 4;
            Sickness += Random.Range(min, max);
            TM.DM.All_Resource_PerTurn_List_Total[39] += Sickness;
        }
    }

    public void Reset_Resource_Sprite()
    {

    }

    public void Turn_Prograss()
    { 
        if(Turn_Count > 0)
        {
            Turn_Count -= 1;
            //턴 카운트가 끝나면 발동
            if (Turn_Count == 0)
            {
                //파괴 중일 경우의 체크식
                if (TM.On_Distroy_Tile.Contains(this))
                {
                    //건물 파괴 공식
                    if(BuildingType != 0)
                    {
                        TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] + 1;
                        TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] - 1;
                        TM.TypeAll_Building_List[BuildingType - 1].Type_Building[BuildingVersion - 1].Buildings.Remove(this);
                        Resource_using = false;
                        Resource.color = new Color32(255, 255, 255, 255);
                        Building.sprite = null;
                        BuildingType = 0;
                        BuildingVersion = 0;
                        Production_Type = 0;
                        Production_Value = 0;
                        Change_Color();

                        Upkeep_Type.Clear();
                        Upkeep_Value.Clear();
                        //후에 업글을 통하던 뭘 하던 건축 자원을 환급할 수 있는 식을 짜야한다.
                        BuildCost_Type.Clear();
                        BuildCost_Value.Clear();

                        Need_Electricity = false;
                        Need_Steam = false;
                        if(ReType == 3 || ReType == 4 || ReType == 2)
                        {
                            Distrucktive = true;
                        }
                        else
                        {
                            Distrucktive = false;
                        }
                        Check_Resource_Type_Version(ReType, ReVersion, Resource);
                        IsDistory_OnTile = false;
                        IsWorking_OnTile = false;
                        TM.On_Distroy_Tile.Remove(this);
                        TM.Off_Working_Tile.Remove(this);

                        switch (ReType)
                        {
                            case 1:
                                TM.On_Goldion.Add(this);
                                AreaCheck.SetActive(true);
                                break;
                            case 2:
                                TM.On_Wood.Add(this);
                                AreaCheck.SetActive(true);
                                break;
                            case 3:
                                TM.On_Ore.Add(this);
                                AreaCheck.SetActive(true);
                                break;
                            case 4:
                                TM.On_Rock.Add(this);
                                AreaCheck.SetActive(true);
                                break;
                            case 5:
                                TM.On_Steel.Add(this);
                                AreaCheck.SetActive(true);
                                break;
                        }
                        Play_DistructionOrBuild_Particle_On(false);
                    }

                    else if (BuildingType == 0)
                    {
                        Debug.Log("자원뻥");
                        TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] + Needed_ManPower;
                        TM.DM.All_Resource_List[23] = TM.DM.All_Resource_List[23] - Needed_ManPower;
                        if (TM.On_Wood.Contains(this))
                        {
                            TM.On_Wood.Remove(this);
                        }
                        else if (TM.On_Ore.Contains(this))
                        {
                            TM.On_Ore.Remove(this);
                        }
                        else if (TM.On_Rock.Contains(this))
                        {
                            TM.On_Rock.Remove(this);
                        }
                        ReType = 0;
                        ReVersion = 0;
                        Distrucktive = false;
                        Resource_using = false;
                        Resource.sprite = null;
                        TM.On_Distroy_Tile.Remove(this);
                        Needed_ManPower = 0;
                        TM.Default_Tile.Add(this);
                        IsDistory_OnTile = false;
                        IsWorking_OnTile = false;
                        Change_Color();
                    }
                }

                if (TM.On_Build_Tile.Contains(this))
                {
                    Building.sprite = TM.DM.Bd[BuildingType - 1].BD_Version[BuildingVersion - 1].Image;
                    Building.color = new Color32(155, 155, 155, 255);
                    TM.On_Build_Tile.Remove(this);
                    Play_DistructionOrBuild_Particle_On(false);
                }

                if (BuildingType == 15 && BuildingVersion == 1)
                {
                    TM.On_Electricity.Add(this);
                }
                if (BuildingType == 16 && BuildingVersion == 1)
                {
                    TM.On_Steam.Add(this);
                }
                if (BuildingType == 18)
                {
                    TM.On_House.Add(this);
                }
                if (BuildingType == 19)
                {
                    TM.On_Public.Add(this);
                }
                if (BuildingType == 11)
                {
                    TM.On_Idea.Add(this);
                }
                if (BuildingType == 10)
                {
                    TM.On_Idea.Add(this);
                }

            }
        }

    }

    public void DoCheck()
    {
        /*if (DeadCount != null)
        {
            StopCoroutine(DeadCount);
        }
        else if (DeadCount == null)
        {
            DeadCount = StartCoroutine(Check());
        }
        */
        DeadCount = StartCoroutine(Check());
    }

    //온워킹인거 다른 스크립트에서 미리 체크하고 들어가자.
    public IEnumerator Check()
    {
        yield return new WaitForSeconds(0.05f);
        if(Workable_OnTile() == true)
        {
            Toggle_On();
        }
        /*
        if (Need_Electricity == true && Electricity_Online == false)
        {
            Toggle_On();
        }
        if (Need_Steam == true && Steam_Online == false)
        {
            Toggle_On();
        }
        if (Need_Public == true && Public_Online == false)
        {
            Toggle_On();
        }
        if (Need_Idea == true && Idea_Online == false)
        {
            Toggle_On();
        }
        if (Need_House == true && House_Online == false)
        {
            Toggle_On();
        }*/
    }
    
    public void Test3()
    {

    }

    //작동 불능, X표 켜짐
    public IEnumerator Test()
    {
        //Debug.Log("테스트00");
        yield return new WaitUntil(() => Turn_Count == 0);
        if(IsDistory_OnTile == false)
        {
            yield return new WaitUntil(() => Allow_Disaster > Current_Disaster && BuildingType != 0);
        }
        Xmark.SetActive(true);
        if(Workable_OnTile() == true)
        {
            Toggle_On();
        }
        StartCoroutine(Test2());
        FadeOut = StartCoroutine(MarkFadeOut());
        yield return null;
    }
    //작동 가능, X표 꺼짐
    public IEnumerator Test2()
    {
        yield return new WaitUntil(() => Turn_Count == 0);
        if(IsDistory_OnTile == false)
        {
            yield return new WaitUntil(() => Allow_Disaster <= Current_Disaster && BuildingType != 0);
        }
        Xmark.SetActive(false);
        StartCoroutine(Test());

        StopCoroutine(FadeIn);
        StopCoroutine(FadeOut);
    }
    public IEnumerator MarkFadeIn()
    {
        SpriteRenderer a = Xmark.GetComponent<SpriteRenderer>();
        a.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);
        yield return null;
        //yield return new WaitForSeconds(1f);
        //FadeOut = StartCoroutine(MarkFadeOut());
    }
    public IEnumerator MarkFadeOut()
    {
        SpriteRenderer a = Xmark.GetComponent<SpriteRenderer>();
        a.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
        FadeIn = StartCoroutine(MarkFadeIn());
    }
    public void ElecOn_side()
    {
        Side.color = new Color(255, 255, 0, 0);
        SideSequence.Kill();
        if(SideClear != null)
        {
            StopCoroutine(SideClear);
        }
        SideClear = StartCoroutine(ClearSide());
    }
    public void SteamOn_side()
    {
        Side.color = new Color(193, 193, 193, 0);
        SideSequence.Kill();
        if (SideClear != null)
        {
            StopCoroutine(SideClear);
        }
        SideClear = StartCoroutine(ClearSide());
    }
    public IEnumerator ClearSide()
    {
        SideSequence.Append(Side.DOFade(1, 0.9f).SetLoops(2, LoopType.Yoyo));
        SideSequence.Play();
        yield return new WaitForSeconds(1.8f);
        Side.DOFade(0, 0.9f);
        //Side.color = new Color(255, 255, 255, 0);
    }

    public void Play_DistructionOrBuild_Particle_On(bool OnOff)
    {
        if (OnOff == false)
        {
            BuildOrCrash_Particle.SetActive(false);
            BuildComplite_Particle.SetActive(true);
        }
        else if (OnOff == true)
        {
            BuildOrCrash_Particle.SetActive(true);
        }
    }

    public void Play_DistructionOrBuild_Particle(bool OnOff)
    {
        if (OnOff == false)
        {
            BuildOrCrash_Particle.SetActive(false);
            BuildComplite_Particle.SetActive(true);
        }
        else if (OnOff == true)
        {
            BuildOrCrash_Particle.SetActive(true);
        }
    }

    public void OnEnable()
    {
        //Xmark.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        
        StartCoroutine(Test());
        if(TM == null)
        {
            TM = GameObject.Find("TileManager").GetComponent<TileManager>();
        }
        CR_Running = false;
        //7-17 로딩 여부를 따져 포지션을 어찌 잡을 지 정한다.
        if (TM.isLoading == false)
        {
            OriginalPosition = gameObject.transform.position;
        }
        else if (TM.isLoading == true)
        {
            gameObject.transform.position = OriginalPosition;
        }

        CurrentTilePosition = gameObject.transform.position;
        TM.OndisableTileList.Remove(gameObject);
        Check_Resource_Type_Version(ReType, ReVersion, Resource);
        Check_Floor_Type_Version(FloorType, FloorVersion, Floor);
        Check_Building_Type_Version(BuildingType, BuildingVersion, Building);
        Check_Disaster();
        //Check_Production();


        //온에이블 시 타일 상태에 따라 타일 매니저에 어느 리스트에 포함될 건지 전달한다.
        if (Turn_Count > 0 && BuildingType != 0)
        {
            TM.On_Build_Tile.Add(this);
        }
        if (BuildingType != 0 && IsWorking_OnTile == false)
        {
            TM.Off_Working_Tile.Add(this);
            TM.TypeAll_Building_List[BuildingType-1].Type_Building[BuildingVersion - 1].Buildings.Add(this);
        }
        else if (BuildingType != 0 && IsWorking_OnTile == true)
        {
            TM.On_Working_Tile.Add(this);
            TM.TypeAll_Building_List[BuildingType - 1].Type_Building[BuildingVersion - 1].Buildings.Add(this);
            Change_Score();
            TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] - Needed_ManPower;
        }
        else if (IsWorking_OnTile == true && IsDistory_OnTile == true)
        {
            TM.On_Distroy_Tile.Add(this);
            TM.DM.All_Resource_List[28] = TM.DM.All_Resource_List[28] - Needed_ManPower;
        }

        if(BuildingType == 15 && BuildingVersion == 1)
        {
            TM.On_Electricity.Add(this);
        }
        if (BuildingType == 16 && BuildingVersion == 1)
        {
            TM.On_Steam.Add(this);
        }
        if (BuildingType == 18)
        {
            TM.On_House.Add(this);
        }
        if (BuildingType == 19)
        {
            TM.On_Public.Add(this);
        }
        if (BuildingType == 11)
        {
            TM.On_Idea.Add(this);
        }
        if (BuildingType == 10)
        {
            TM.On_Idea.Add(this);
        }

        if (TM.OnEnableTileList.Contains(gameObject) == false)
        {
            TM.OnEnableTileList.Add(gameObject);
        }
    }

    public void OnDisable()
    {
        TM.OndisableTileList.Add(gameObject);
        TM.OnEnableTileList.Remove(gameObject);
        CurrentTilePosition = new Vector3(0,0,0);
        Tier = 0;
        Tile_Number = 0;
        FloorType = 0;
        FloorVersion = 0;
        Resource_using = false;
        Steam_Online = false;
        Electricity_Online = false;
        Public_Online = false;
        House_Online = false;
        Idea_Online = false;
        BuildingSound.clip = null;
        Production_Type = 0;
        Production_Value = 0;
        ReType = 0;
        ReVersion = 0;
        EventType = 0;
        EventVersion = 0;
        Anti_Disaster = 0;
        Current_Disaster = 0;
        Allow_Disaster = 0;
        Event_Desaster = 0;
        BuildingType = 0;
        BuildingVersion = 0;
        Building.sprite = null;
        Needed_ManPower = 0;
        Working_ManPower = 0;
        polCol_Size = PolCol_Size.self;
        IsWorking_OnTile = false;
        Distrucktive = false;
        Constructable_OnTile = false;
        Upkeep_Type.Clear();
        Upkeep_Value.Clear();
        BuildCost_Type.Clear();
        BuildCost_Value.Clear();
        Turn_Count = 0;
        Need_Electricity = false;
        Need_Steam = false;
        Need_House = false;
        Need_Idea = false;
        Need_Public = false;
    }


    private float total_product;
    public float Total_product
    {
        get
        {
            return Production_Value;
        }
        set
        {
            Production_Value = value;
        }
    }
    

}
