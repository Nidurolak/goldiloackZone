using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour
{
    //여기서는 타일 생성에 관련된 함수를 실행하고 데이터를 모아 DataManager에 보내는 것이 목적이다.

    [Serializable]
    public class Count //자원 생성의 최소 최대값을 정하는 클래스
    {
        public int Minimum;
        public int Maximum;

        public Count(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    public MouseControll Ms;
    public Datamanager DM;
    [HideInInspector]
    public GameObject camera;

    //생성할 기본 타일을 선언
    public GameObject Floor_Prefab;
    public GameObject PopUp_Prefab;
    public GameObject PopUp_Canvas;
    public List<GameObject> PopUp_UI_List_OFF = new List<GameObject>();
    public List<GameObject> PopUp_UI_List_ON = new List<GameObject>();

    public GameObject SteelWhale;
    private new GameObject Instantiate;

    //최소 최대값으로 자원의 생성갯수를 정할 변수들. 사용이 잘 안되면 빠르게 지우자
    public Count Resource_Count_Goldion;
    public Count Resource_Count_Steel;
    public Count Resource_Count_Wood;
    public Count Resource_Count_Ore;
    public Count Resource_Count_Rock;
    
    [SerializeField]
    public List<Vector3> gridPositions = new List<Vector3>();//타일 설치가 가능한 벡터3의 리스트.

    public GameObject RandomCollider;
    public List<GameObject> RandomColliders_FloorTile = new List<GameObject>();

    public GameObject SteelWhaleMaker;

    public List<GameObject> OndisableTileList;//풀링 등등의 이유로 비활성화된 리스트들을 불러모은다.
    public List<GameObject> OnEnableTileList_ResourceLayoutOnly;
    public List<GameObject> OnEnableTileList;

    public List<Sprite> ChangeTileToGreen;//바닥타일의 버전을 변환시킬 때 사용할 것이다.

    public List<Sprite> Resource_Sprites_Gold;
    public List<Sprite> Resource_Sprites_Steel;
    public List<Sprite> Resource_Sprites_Wood;
    public List<Sprite> Resource_Sprites_Ore;
    public List<Sprite> Resource_Sprites_Rock;

    //바닥타일은 맵 스타일의 변화에 따라 달라질 것이기 때문에 123으로 구분한다.
    public List<Sprite> Floor_Types_1;
    public List<Sprite> Floor_Types_1_Grass;
    public List<Sprite> Floor_Types_2;
    public List<Sprite> Floor_Types_3;

    //일단은 2x2 사이즈의 미들만 구현하지만 나중에 2x3 2x4 짜리도 만들어야한다.
    public List<Sprite> Floor_Middle_Types_1;
    public List<Sprite> Floor_Middle_Types_2;
    public List<Sprite> Floor_Middle_Types_3;

    public List<Sprite> Building_Sprites_Test;

    public List<TileData> Default_Tile;
    public List<TileData> NearBy_Wood;
    public List<TileData> NearBy_Ore;
    public List<TileData> NearBy_Steel_Whale;
    public List<TileData> NearBy_House;
    public List<TileData> NearBy_Public;
    public List<TileData> NearBy_Idea;
    public List<TileData> NearBy_Elec_Grid;
    public List<TileData> NearBy_Steam_Grid;
    public List<TileData> On_Steel;
    public List<TileData> On_Wood;
    public List<TileData> On_Ore;
    public List<TileData> On_Goldion;
    public List<TileData> On_Rock;
    public List<TileData> On_Electricity;
    public List<TileData> On_Steam;
    public List<TileData> On_House;
    public List<TileData> On_Public;
    public List<TileData> On_Idea;

    //순서대로 건설중인 타일/작업 중인 타일/작업 중지된 타일
    public List<TileData> On_Build_Tile;
    public List<TileData> On_Distroy_Tile;
    public List<TileData> On_Working_Tile;
    public List<TileData> Off_Working_Tile;
    public List<BuildingData_ListWrapper_Deep> Grid_Tile_List;
        
    [System.Serializable]
    public class BuildingData_ListWrapper
    {
        public List<BuildingData_ListWrapper_Deep> Type_Building;
    }
    [System.Serializable]
    public class BuildingData_ListWrapper_Deep
    {
        public List<TileData> Buildings;
    }

    public List<BuildingData_ListWrapper> TypeAll_Building_List = new List<BuildingData_ListWrapper>();

    //Json으로 불러와 넣을 타일 관련 텍스트들
    public List<string> Name_Text_List = new List<string>();
    public List<string> TileName_Text_List = new List<string>();
    public List<string> ResourceName_Text_List = new List<string>();
    public List<string> ResourceContents_Text_List = new List<string>();

    //자원 생성, 로드 등등 Tile이 OnEnable하기 전에 데이터를 할당시켜야한다. 
    public List<int> Resource_Type = new List<int>();
    public List<int> Resource_Version = new List<int>();

    //TileData가 OnEnable할 때 참고할 자료들
    public List<string> ReText_List = new List<string>();
    public List<string> ReText_Contents_List = new List<string>();

    //이건 BuildingData에 사용될 것들, OnEnable하기 전에 데이터를 할당시켜야한다.
    [HideInInspector]
    public List<int> Building_Type = new List<int>();
    public List<int> Building_Version = new List<int>();

    //TileData가 OnEnable할 때 참고할 자료들 - 건물 텍스트 정보
    public List<string> BuildingText_List = new List<string>();
    public List<string> BuildingText_Contents_List = new List<string>();

    //TileData가 OnEnable할 때 참고할 자료들 - 이벤트 정보
    public List<int> Event_Type = new List<int>();
    public List<int> Event_Version = new List<int>();

    //TileData가 OnEnable할 때 참고할 자료들 - 이벤트 텍스트 정보
    public List<string> Event_Text_List = new List<string>();
    public List<string> Event_Text_Contents_List = new List<string>();

    //TileData가 OnEnable할 때 참고할 자료들 - 바닥타일 정보
    public int Floor_Type;
    public List<int> floor_Version = new List<int>();

    //맵 크기를 결정함
    public enum MapSize { small, middle, Large };
    public MapSize mapStyle;

    //해당 맵의 컨디션을 설정
    public enum MapCondition {red, orange, yellow, green, gold };
    public MapCondition mapCondition;

    //맵 바이옴을 결정함
    public enum MapBiom {Field, Desert, Frost};
    public MapBiom mapBiom;

    //InitialiseList함수에 사용됨. 
    public float z1 = 0;
    public float z2 = 0;
    public int columns;
    public int rows;

    //타일 리스트를 만들 때 쓰임.
    private Transform ColliderBox;
    private Transform TileBox;

    public bool PoolingCheck;
    public bool isMaking;
    public bool isLoading;

    public Image Zone_Color;
    public Text Zone_Color_Text;

    public Slider Will;
    public Slider Hope;
    public Slider Complaint;

    //비활성화 오브젝트를 만들고 그 리스트를 가져온다. 필요한 선행 함수 없음
    public void MakingOnDisableTileList()
    {
        TileBox = new GameObject("Tile").transform;
        ColliderBox = new GameObject("ColliderBox").transform;

        for (int i = 0; i < 600; i++)
        {
            Instantiate = Instantiate(Floor_Prefab) as GameObject;

            Instantiate.transform.SetParent(TileBox);

            OndisableTileList.Add(Instantiate);


            var a = Instantiate.GetComponent<TileData>();
            a.TM = this;
            a.MS = Ms;
        }
    }

    public void MapSizeChoose()//MapSize를 대중소로 구분한다.
    {
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                mapStyle = MapSize.small;
                break;
            case 1:
                mapStyle = MapSize.middle;
                break;
            default:
                mapStyle = MapSize.Large;
                break;
        }
    }

    public void MapConditionChoose(bool a)//MapCondition을 구분한다. 동시에 좌측 상단의 존 정보도 교체한다.
    {
        if (a == true)
        {
            int i = Random.Range(0, 11);
            switch (i)
            {
                case 0:
                    mapCondition = MapCondition.red;
                    Zone_Color.color = new Color32(225, 0, 0, 255);
                    break;
                case 1:
                    mapCondition = MapCondition.orange;
                    Zone_Color.color = new Color32(255, 189, 0, 255);
                    break;
                case 2:
                    mapCondition = MapCondition.orange;
                    Zone_Color.color = new Color32(255, 189, 0, 255);
                    break;
                case 3:
                    mapCondition = MapCondition.yellow;
                    Zone_Color.color = new Color32(255, 255, 0, 255);
                    break;
                case 4:
                    mapCondition = MapCondition.yellow;
                    Zone_Color.color = new Color32(255, 255, 0, 255);
                    break;
                case 5:
                    mapCondition = MapCondition.yellow;
                    Zone_Color.color = new Color32(255, 255, 0, 255);
                    break;
                case 6:
                    mapCondition = MapCondition.yellow;
                    Zone_Color.color = new Color32(255, 255, 0, 255);
                    break;
                case 7:
                    mapCondition = MapCondition.green;
                    Zone_Color.color = new Color32(0, 255, 0, 255);
                    break;
                case 8:
                    mapCondition = MapCondition.green;
                    Zone_Color.color = new Color32(0, 255, 0, 255);
                    break;
                case 9:
                    mapCondition = MapCondition.green;
                    Zone_Color.color = new Color32(0, 255, 0, 255);
                    break;
                case 10:
                    mapCondition = MapCondition.gold;
                    Zone_Color.color = new Color32(255, 191, 0, 255);
                    break;
            }
        }
        else switch (mapCondition)
            {
                case MapCondition.red:
                    Zone_Color.color = new Color32(225, 0, 0, 255);
                    break;
                case MapCondition.orange:
                    mapCondition = MapCondition.orange;
                    Zone_Color.color = new Color32(255, 189, 0, 255);
                    break;
                case MapCondition.yellow:
                    Zone_Color.color = new Color32(255, 255, 0, 255);
                    break;
                case MapCondition.green:
                    Zone_Color.color = new Color32(0, 255, 0, 255);
                    break;
                case MapCondition.gold:
                    Zone_Color.color = new Color32(255, 191, 0, 255);
                    break;
            }
    }

    //맵 컨디션에 따라 이벤트 빈도수를 정함, 그리고 오염 수치도
    public void MapConditionInitialise()
    {
        switch (mapCondition)
        {
            case MapCondition.red:
                for(int i = 0; i < OnEnableTileList.Count; i++)
                {
                    OnEnableTileList[i].GetComponent<TileData>().Event_Desaster = -5;
                    OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case MapCondition.orange:
                for (int i = 0; i < OnEnableTileList.Count; i++)
                {
                    OnEnableTileList[i].GetComponent<TileData>().Event_Desaster = -4;
                    OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case MapCondition.yellow:
                for (int i = 0; i < OnEnableTileList.Count; i++)
                {
                    OnEnableTileList[i].GetComponent<TileData>().Event_Desaster = -3;
                    OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case MapCondition.green:
                for (int i = 0; i < OnEnableTileList.Count; i++)
                {
                    OnEnableTileList[i].GetComponent<TileData>().Event_Desaster = -2;
                    OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;
            case MapCondition.gold:
                for (int i = 0; i < OnEnableTileList.Count; i++)
                {
                    OnEnableTileList[i].GetComponent<TileData>().Event_Desaster = 0;
                    OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
                }
                break;

        }


    }

    //기존의 gridPosition을 지우고 새 리스트를 채운다. 타일 재정렬 등에 사용.
    //필요한 선행 함수 - MapSizeChoose, MakingOnDisableTileList
    public void InitialiseList()
    {
        switch (mapStyle)//맵 스타일에 따라 타일의 가로세로 길이를 바꾼다.
        {
            case MapSize.small:
                var a = Random.Range(7, 9);
                columns = a;
                rows = Random.Range(a-1, a+1);
                DM.MaxTurn = 50;

                //카메라 경계선을 설정
                Ms.SettingLimit();
                break;
            case MapSize.middle:
                var b = Random.Range(8, 10);
                columns = b;
                rows = Random.Range(b - 1, b + 1);
                DM.MaxTurn = 60;

                //카메라 경계선을 설정
                Ms.SettingLimit();
                break;
            case MapSize.Large:
                var c = Random.Range(10, 12);
                columns = c;
                rows = Random.Range(c - 1, c + 1);
                DM.MaxTurn = 70;

                //카메라 경계선을 설정
                Ms.SettingLimit();
                break;
        }
        gridPositions.Clear();//기존 리스트를 청소

        var a1 = 0; 
        var a2 = 0;

        float b1 = 0;
        float b2 = 0;
        float b3 = 0;
        for(int i = 0; i < rows*2; i++)
        {
            //X축
            b1 = 1.275f;
            b1 *= -i;

            //Y축
            b2 = 0.65f;
            b2 *= i;

            //Z축
            b3 = 0.005f;
            b3 *= i;

            b3 = 0.005f;
            b3 *= i;

            for (int z = 0; z < columns*2; z++)
            {
                //OnEnableTileList[i + z].GetComponent<TileData>().Columns = z;
                //OnEnableTileList[i + z].GetComponent<TileData>().Rows = i;

                gridPositions.Add(new Vector3(b1, b2, b3));
                b1 += 1.275f;
                b2 += 0.65f;
                b3 += 0.005f;
            }
        }

        columns *= 2;
        rows *= 2;
        /*
        //여기를 고쳐서 원형이 나오게 해야겠어
        for (float y = -rows ; y <= rows; y += 1)
        {
            z2 += 0.01f;
            if(y < 0)
            {
                a1++;
            }
            else if(y >= 0)
            {
                a1--;
            }
            //x축을 한 번 루프할 때 마다 Y축 전체를 루프한다.
            for (float x = -columns - 0.5f; x <= columns; x += 1)
            {
                if (x < a1 && x > -a1)
                {
                    gridPositions.Add(new Vector3(x * 2.55f, y * 1.3f + 0.65f, z2 + 0.005f));
                }
                //타일들이 사용할 새로이 Vector3값을 그리드포지션스에 할당한다.
            }
            //a1 = 0;
        }
        z2 = 0;

        //y축 루프
        for (float y = -rows; y <= rows; y+=1 )
        {
            z1 += 0.01f;
            if (y < 0)
            {
                a2++;
            }
            else if (y > 0)
            {
                a2--;
            }
            //x축을 한 번 루프할 때 마다 Y축 전체를 루프한다.
            for (float x = -columns; x <= columns; x += 1)
            {
                //타일들이 사용할 새로이 Vector3값을 그리드포지션스에 할당한다.
                if (x < a2 && x > -a2)
                {
                    gridPositions.Add(new Vector3(x * 2.55f, y * 1.3f, z1));
                }
            }
        }*/
        z1 = 0;
    }

    //OnEnableTileList에 타일 오브젝트들을 할당
    public void PositionSetting()
    {

        OnEnableTileList.Clear();
        OnEnableTileList_ResourceLayoutOnly.Clear();

         for(int i = 0; i < gridPositions.Count; i++)//완성된 그리드 포지션만큼 OndisableTileList를 돌면서 포지션을 할당. TD의 벡터값을 어째야할지 아직 잘 모르겠어
         {
            Vector3 a = gridPositions[i];

            OnEnableTileList.Add(OndisableTileList[i]);//문제발견, OndisableTile 리스트에 문제가 발생했어. 7-09
            OnEnableTileList_ResourceLayoutOnly.Add(OndisableTileList[i]);
            OnEnableTileList[i].GetComponent<TileData>().transform.position = a;
            OnEnableTileList[i].GetComponent<TileData>().Tile_Number = i;
            OnEnableTileList[i].GetComponent<TileData>().FloorType = Floor_Type;
            //랜덤으로 타일 버전을 변경한다.
            OnEnableTileList[i].GetComponent<TileData>().FloorVersion = Random.Range(0, 3);
        }
    }

    //맵 크기에 따라 사이즈를 조절하는 함수, 지도 크기에 따라 각 자원의 최소 최대값을 늘린다. 필요한 선행 함수 - MapSizeChoose
    public void ResourceCountControll(MapSize size, Count count, int min_Default, int max_Default)
    {
        switch (size)
        {
            case MapSize.small: 
                count.Minimum = min_Default;
                count.Maximum = max_Default;
                break;
            case MapSize.middle:
                count.Minimum = (int)(min_Default + 1.5f);
                count.Maximum = (int)(max_Default * 2f);
                break;
            case MapSize.Large:
                count.Minimum = (int)(min_Default + 2f);
                count.Maximum = (int)(max_Default * 3f);
                break;
        }
    }

    //맵 크기에 따라 자원 민맥값을 정한 뒤 OnDisableTile 목록에다 값을 할당시킨다. 필요한 선행 함수 - MapSizeChoose
    public void ResourceCounControll_tSetting()
    {
        ResourceCountControll(mapStyle, Resource_Count_Goldion, 5, 10);
        ResourceCountControll(mapStyle, Resource_Count_Ore, 8, 13);
        ResourceCountControll(mapStyle, Resource_Count_Rock, 4, 19);
        ResourceCountControll(mapStyle, Resource_Count_Steel, 7, 9);
        ResourceCountControll(mapStyle, Resource_Count_Wood, 2, 5);
    }

    //정해진 자원을 맵 상에 흩뿌림, 뭉쳐서 생성해야할 자원은 랜덤콜라이더를 응용해보자.
    //필요한 선행 함수 - MakingOnDisableTileList, ResourceCountControll, PositionSetting
    public void ResourceLayout(List<Sprite> ResourceSprite, Count count, int ReType, bool Constructable_OnTile, bool Distructive, int TurnCount, int ManPower)
    {
        int ReCount = Random.Range(count.Minimum, count.Maximum);//미리 설정한 미니멈과 맥시멈 값을 두고 자원 생성값을 지정
        for (int i = 0; i <= ReCount; i++)
        {   
            //OnEnableTileList_ResourceLayoutOnly에서 한 타일의 Index를 지정함
            int randomIndex = Random.Range(0, OnEnableTileList_ResourceLayoutOnly.Count);

            //지정된 자원 스프라이트 중 하나를 고름
            var ReVersion = Random.Range(0, ResourceSprite.Count + 1);

            //타일을 불러옴
            GameObject tile = OnEnableTileList_ResourceLayoutOnly[randomIndex];

            //타일의 TD을 불러오고 타입과 버전을 할당함
            TileData TD = tile.GetComponent<TileData>();
            TD.ReType = ReType;
            TD.ReVersion = ReVersion;
            //TD.Workable_OnTile = Constructable_OnTile;
            TD.Distrucktive = Distructive;
            /*여기 고쳐
            switch (ReType)
            {
                case 4:
                    break;
                case 3:
                    break;
            }*/
            //if (DM.Spacial_Effect_Unlock[0] == false || DM.Spacial_Effect_Unlock[5] == false)
            if (DM.Spacial_Effect_Unlock[0].List[0] == false || DM.Spacial_Effect_Unlock[0].List[4] == false)
            {
                TD.Distrucktive = false;
            }
            TD.Turn_Count = TurnCount;
            TD.Needed_ManPower = ManPower;

            //중복 자원을 만들지 않기 위해 리스트에서 제거함. OnEnableTileList_ResourceLayoutOnly는 TD의 OnDisable에서 다시 돌아옴

            OnEnableTileList_ResourceLayoutOnly.RemoveAt(randomIndex);
        }
    }

    public void ResetForTech() 
    {
        for(int i = 0; i < On_Working_Tile.Count; i++)
        {
            On_Working_Tile[i].Check_TechorRule();
        }
    }

    //맵 전체를 훝으며 자원 타일만 골라 체크함
    public void Add_NearAndOn_All()
    {
        Default_Tile.Clear();
        On_Steel.Clear();
        On_Wood.Clear();
        On_Ore.Clear();
        On_Goldion.Clear();
        On_Rock.Clear();
        NearBy_Wood.Clear();
        NearBy_Ore.Clear();
        
        NearBy_Steel_Whale.Clear();
        
        NearBy_House.Clear();

        Debug.Log(OnEnableTileList.Count + "  ds");
        for (int i = 0; i< OnEnableTileList.Count; i++)
        {
         TileData a = OnEnableTileList[i].GetComponent<TileData>();
            switch (a.ReType)
            {
                case 1:
                    if(a.BuildingType == 0 && a.Resource_using == false)
                    {
                        On_Goldion.Add(OnEnableTileList[i].GetComponent<TileData>());
                    }
                    break;
                case 2:
                    if (a.BuildingType == 0 && a.Resource_using == false)
                    {
                        On_Wood.Add(OnEnableTileList[i].GetComponent<TileData>());
                        OnEnableTileList[i].GetComponent<TileData>().Distrucktive = true;
                    }
                    break;
                case 3:
                    if (a.BuildingType == 0 && a.Resource_using == false)
                    {
                        On_Ore.Add(OnEnableTileList[i].GetComponent<TileData>());
                        if(DM.Spacial_Effect_Unlock[0].List[1] == true)
                        {
                            OnEnableTileList[i].GetComponent<TileData>().Distrucktive = true;
                        }
                    }
                    break;
                case 4:
                    if (a.BuildingType == 0 && a.Resource_using == false)
                    {
                        On_Rock.Add(OnEnableTileList[i].GetComponent<TileData>());
                        if (DM.Spacial_Effect_Unlock[0].List[0] == true)
                        {
                            OnEnableTileList[i].GetComponent<TileData>().Distrucktive = true;
                        }
                    }
                    break;
                case 5:
                    if (a.BuildingType == 0 && a.Resource_using == false)
                    {
                        On_Steel.Add(OnEnableTileList[i].GetComponent<TileData>());
                    }
                    break;
                default:
                    if (a.BuildingType == 0)
                    {
                        Default_Tile.Add(OnEnableTileList[i].GetComponent<TileData>());
                    }
                    break;
            }
        }

        Check_On(On_Goldion, 1);
        Check_On(On_Wood, 2);
        Check_On(On_Ore, 3);
        Check_On(On_Rock, 4);
        Check_On(On_Steel, 5);
        Check_NearBy_Resource(On_Wood);
        Check_NearBy_Resource(On_Ore);
    }

    //자원 근처 타일을 활성화 한다.
    public void Check_NearBy_Resource(List<TileData> a)
    {
        Debug.Log("에베베");
        for (int i = 0; i <a.Count; i++)
        {
            if (a[i].Resource_using == false)
            {
                a[i].AreaCheck.SetActive(true);
            }
        }
    }

    //On 리스트, NearBy리스트, 넘버링, 0전기, 1증기, 2주택, 3공공, 4사상
    public void Check_NearBy_Online(List<TileData> a, List<TileData> b, int c)
    {
        switch (c)
        {
            case 0:
                for (int i = 0; i < b.Count; i++)
                {
                    b[i].Electricity_Online = false;
                }
                for (int z = 0; z < a.Count; z++)
                {
                    if (a[z].IsWorking_OnTile == true)
                    {
                        a[z].AreaCheck.GetComponent<TileChecker>().Check_Electricity = true;
                        a[z].AreaCheck.SetActive(true);
                    }
                }
                for (int i = 0; i < b.Count; i++)
                {
                    //b[i].Electricity_Online = false;
                    if (b[i].Electricity_Online == false &&
                        b[i].Need_Electricity == true &&
                        b[i].IsWorking_OnTile == true)
                    {
                        b[i].DoCheck();
                    }
                }
                    break;
            case 1:
                for (int i = 0; i < b.Count; i++)
                {
                    b[i].Steam_Online = false;
                }
                for (int z = 0; z < a.Count; z++)
                {
                    if (a[z].IsWorking_OnTile == true)
                    {
                        a[z].AreaCheck.GetComponent<TileChecker>().Check_Steam = true;
                        a[z].AreaCheck.SetActive(true);
                    }
                }
                for (int i = 0; i < b.Count; i++)
                {
                    //b[i].Electricity_Online = false;
                    if (b[i].Steam_Online == false &&
                        b[i].Need_Steam == true &&
                        b[i].IsWorking_OnTile == true)
                    {
                        b[i].DoCheck();
                    }
                }
                break;
            case 2:
                for (int i = 0; i < b.Count; i++)
                {
                    b[i].House_Online = false;
                }
                for (int z = 0; z < a.Count; z++)
                {
                    if (a[z].IsWorking_OnTile == true)
                    {
                        a[z].AreaCheck.GetComponent<TileChecker>().Check_House = true;
                        a[z].AreaCheck.SetActive(true);
                    }
                }
                for (int i = 0; i < b.Count; i++)
                {
                    //b[i].Electricity_Online = false;
                    if (b[i].House_Online == false &&
                        b[i].Need_House == true &&
                        b[i].IsWorking_OnTile == true)
                    {
                        b[i].DoCheck();
                    }
                }
                break;
            case 3:
                for (int i = 0; i < b.Count; i++)
                {
                    b[i].Public_Online = false;
                }
                for (int z = 0; z < a.Count; z++)
                {
                    if (a[z].IsWorking_OnTile == true)
                    {
                        a[z].AreaCheck.GetComponent<TileChecker>().Check_Public = true;
                        a[z].AreaCheck.SetActive(true);
                    }
                }
                for (int i = 0; i < b.Count; i++)
                {
                    //b[i].Electricity_Online = false;
                    if (b[i].Public_Online == false &&
                        b[i].Need_Public == true &&
                        b[i].IsWorking_OnTile == true)
                    {
                        b[i].DoCheck();
                    }
                }
                break;
            case 4:
                for (int i = 0; i < b.Count; i++)
                {
                    b[i].Idea_Online = false;
                }
                for (int z = 0; z < a.Count; z++)
                {
                    if (a[z].IsWorking_OnTile == true)
                    {
                        a[z].AreaCheck.GetComponent<TileChecker>().Check_Idea = true;
                        a[z].AreaCheck.SetActive(true);
                    }
                }
                for (int i = 0; i < b.Count; i++)
                {
                    //b[i].Electricity_Online = false;
                    if (b[i].Idea_Online == false &&
                        b[i].Need_Idea == true &&
                        b[i].IsWorking_OnTile == true)
                    {
                        b[i].DoCheck();
                    }
                }
                break;
        }
        b.Clear();
    }

    public void Check_NearBy_Part(GameObject a)
    {
        //Debug.Log("체크 니어바이 파트 발동");
        switch (a.GetComponent<TileData>().BuildingType)
        {
            case 1:
                //Debug.Log("빌딩 타입" + a.GetComponent<TileData>().BuildingType);
                On_Steel.Remove(a.GetComponent<TileData>());
                break;
            case 4:
                //Debug.Log("빌딩 타입" + a.GetComponent<TileData>().BuildingType);
                NearBy_Ore.Clear();
                Check_On(On_Ore, 3);
                Check_NearBy_Resource(On_Ore);
                break;
            case 5:
                //Debug.Log("빌딩 타입" + a.GetComponent<TileData>().BuildingType);
                On_Goldion.Remove(a.GetComponent<TileData>());
                break;
            case 9:
                //Debug.Log("빌딩 타입" + a.GetComponent<TileData>().BuildingType);
                NearBy_Wood.Clear();
                Check_On(On_Wood, 2);
                Check_NearBy_Resource(On_Wood);
                break;
            case 18:
                NearBy_House.Clear();
                Check_On(On_House, 0);
                Check_NearBy_Resource(On_House);
                break;
            
            default:
                //Debug.Log("빌딩 타입" + a.GetComponent<TileData>().BuildingType);
                Default_Tile.Remove(a.GetComponent<TileData>());
                break;
        }
    }

    public void Check_On(List<TileData> a, int Type)
    {
        a.Clear();
        for(int i =0; i< OnEnableTileList.Count ; i++)
        {
            if(OnEnableTileList[i].GetComponent<TileData>().ReType == Type 
                && a.Contains(OnEnableTileList[i].GetComponent<TileData>()) == false 
                && OnEnableTileList[i].GetComponent<TileData>().BuildingType == 0 &&
                    OnEnableTileList[i].GetComponent<TileData>().Resource_using ==false)
            {
                a.Add(OnEnableTileList[i].GetComponent<TileData>());
            }
        }
    }
    public void ResourceLayout_Total()//자원 생성을 한꺼번에 한다
    {
        ResourceLayout(Resource_Sprites_Gold, Resource_Count_Goldion, 1, true, true, 0,0);
        ResourceLayout(Resource_Sprites_Wood, Resource_Count_Wood, 2, true, true, 1,1);
        ResourceLayout(Resource_Sprites_Ore, Resource_Count_Ore, 3, true, true, 2,2);
        ResourceLayout(Resource_Sprites_Rock, Resource_Count_Rock, 4, true, true, 3,1);
        ResourceLayout(Resource_Sprites_Steel, Resource_Count_Steel, 5, false, false, 0,0);
    }

    //바닥 타일을 바꿀 랜덤 콜라이더를 지정한다. 필요한 선행 함수 없음
    public void MakeFloorTileChanger()
    {
        for(int i = 0; i < 30; i++)
        {
            GameObject RanCol_Tile = Instantiate(RandomCollider) as GameObject;
            RanCol_Tile.GetComponent<TileChanger>().WTC = TileChanger.What_To_Change_Type.Floor;
            switch (mapBiom)
            {
                case MapBiom.Field:
                    RanCol_Tile.GetComponent<TileChanger>().floorSprites = Floor_Types_1_Grass;
                    break;
                case MapBiom.Desert:
                    RanCol_Tile.GetComponent<TileChanger>().floorSprites = Floor_Types_1_Grass;
                    break;
                case MapBiom.Frost:
                    RanCol_Tile.GetComponent<TileChanger>().floorSprites = Floor_Types_1_Grass;
                    break;
            }
            RanCol_Tile.transform.SetParent(ColliderBox);
            RandomColliders_FloorTile.Add(RanCol_Tile);
        }
    }

    //랜덤 콜라이더를 맵 사이즈에 맞게 갯수를 조절하여 활성화시킨다. 필요한 선행 함수 - MakeFloorTileChanger
    public IEnumerator FloorTileVersionChange()
    {
        WaitForSeconds a = new WaitForSeconds(0.04f);
        int i = 0;
        switch (mapStyle)
        {
            case MapSize.small:
                i = Mathf.Min(columns, rows);
                break;
            case MapSize.middle:
                i = Mathf.Min(columns, rows) + 3;
                break;
            case MapSize.Large:
                i = Mathf.Min(columns, rows) + 5;
                break;
        }
        for (int z =0; z < i; z++)//포지션을 할당한 뒤 셋액티브를 킨다. 바닥타일 교체용
        {
            RandomColliders_FloorTile[z].transform.position = OnEnableTileList[Random.Range(0, OnEnableTileList.Count)].transform.position;
            RandomColliders_FloorTile[z].SetActive(true);
        }
        yield return a;

        //스틸웨일 설치
        SteelWhaleMaker.SetActive(true);
        yield return null;
    }

    public void MakeSteelWhale(int columns, int rows)
    {



    }

    public void FloorTile_Active()
    {
        StartCoroutine("FloorTile_OnEnable");
    }
    //모든 값이 할당된 바닥 타일들을 활성화 시킨다. 버튼작동
    public IEnumerator FloorTile_OnEnable()
    {
        int rows_ = 0;
        int columns_ = 0;
        List<BuildingData_ListWrapper_Deep> z = new List<BuildingData_ListWrapper_Deep>();

        if (isMaking == false)
        {
            if (isLoading == false)
            {
                MapSizeChoose();
                MapConditionChoose(true);
                InitialiseList();
                PositionSetting();
                ResourceCounControll_tSetting();
                ResourceLayout_Total();
            }
            /*for (int i = 0; i < rows; i++)
            {
                Grid_Tile_List.Add(null);
            }*/
            Debug.Log("로로롤우 " + rows + "컬러러러 " + columns);
            for (int i = 0; i < OnEnableTileList.Count; i++)
            {
                OnEnableTileList[i].SetActive(true);
                OnEnableTileList[i].GetComponent<TileData>().Change_Color();
                if (columns_ < columns-1)
                {
                    Grid_Tile_List[rows_].Buildings.Add(OnEnableTileList[i].GetComponent<TileData>());
                    columns_++;
                }
                else if (columns_ == columns-1)
                {
                    columns_ = 0;
                    Grid_Tile_List[rows_].Buildings.Add(OnEnableTileList[i].GetComponent<TileData>());
                    //columns_++;
                    rows_++;
                }
            }
            if (isLoading == false)
            {
               StartCoroutine(FloorTileVersionChange());
            }
            //Debug.Log("플로어 타일 작동 완료");

            MapConditionInitialise();
            PoolingCheck = false;
            isMaking = true;
            isLoading = false;
            CameraSetting();
            yield return new WaitForSeconds(0.04f);
            Add_NearAndOn_All();
            //Check_NearBy(On_Wood);
            //Check_NearBy(On_Ore);
        }
    }

    //타일들을 비활성화함과 동시에 타일 생성 리스트들을 싹다 지워야한다. 어떻게?
    public void FloorTile_OnDisable()
    {
        if (isMaking == true)
        {
            for (int i = OnEnableTileList.Count - 1; i >= 0; i--)
            {
                if(OnEnableTileList[i].GetComponent<TileData>().IsWorking_OnTile == true)
                {
                    OnEnableTileList[i].GetComponent<TileData>().Toggle_On();
                }
                OnEnableTileList[i].SetActive(false);
            }

            OnEnableTileList.Clear();
            OnEnableTileList_ResourceLayoutOnly.Clear();
            PoolingCheck = true;
            isMaking = false;
        }
        Zone_Color.color = new Color32(255, 255, 255, 0);
    }

    public void Editer_Updisaster()
    {
        for(int i = 0; i < OnEnableTileList.Count; i++)
        {
            OnEnableTileList[i].GetComponent<TileData>().Event_Desaster += 1;
            OnEnableTileList[i].GetComponent<TileData>().Current_Disaster += 1;
        }
    }
    public void Editer_Downdisaster()
    {
        for (int i = 0; i < OnEnableTileList.Count; i++)
        {
            OnEnableTileList[i].GetComponent<TileData>().Event_Desaster -= 1;
            OnEnableTileList[i].GetComponent<TileData>().Current_Disaster -= 1;
        }
        Debug.Log(OnEnableTileList.Count);
    }

    /*public void MakingPopUp_UI()
    {
        for(int i = 0; i <= OnEnableTileList.Count; i++)
        {
            //자원 팝업용 UI 겸사겸사 생성
            Instantiate = Instantiate(PopUp_Prefab) as GameObject;

            Instantiate.transform.SetParent(PopUp_Canvas.transform);

            Instantiate.GetComponent<PopUpUi>().c = this;

            PopUp_UI_List_OFF.Add(Instantiate);
        }

    }*/

    public IEnumerator BuildingPopup()
    {
        WaitForSeconds c = new WaitForSeconds(0.06f);
        if (On_Working_Tile.Count > 0)
        {
            for (int i = 0; i < On_Working_Tile.Count; i++)
            {
                if (On_Working_Tile[i].BuildingType != 0 &&
                    On_Working_Tile[i].BuildingType != 10 &&
                    On_Working_Tile[i].BuildingType != 11 &&
                    On_Working_Tile[i].BuildingType != 12 &&
                    On_Working_Tile[i].BuildingType != 14 &&
                    On_Working_Tile[i].BuildingType != 17 &&
                    On_Working_Tile[i].BuildingType != 18 &&
                    On_Working_Tile[i].BuildingType != 19 &&
                    On_Working_Tile[i].BuildingType != 21 )
                {
                    yield return c;
                    On_Working_Tile[i].PopUp.Logo.sprite = DM.Resource_Sprites_Logo_NonBackGround[On_Working_Tile[i].Production_Type];
                    On_Working_Tile[i].PopUp.This.SetActive(true);
                }
            }
        }

    }

    //카메라를 세팅한다.
    public void CameraSetting()
    {
        var a = camera.GetComponent<Camera>();
        switch (mapStyle)
        {
            case (MapSize)0:
                a.orthographicSize = 3.9f;
                break;
            case (MapSize)1:
                a.orthographicSize = 5.2f;
                break;
            case (MapSize)2:
                a.orthographicSize = 7.2f;
                break;
        }
    }

    public void Destroy_Tile_Building(TileData TD)
    {
        if (TD.IsWorking_OnTile)
        {
            TD.Change_Score();
        }
            OndisableTileList.Add(TD.transform.gameObject);
            TD.CurrentTilePosition = new Vector3(0, 0, 0);
            TD.Tile_Number = 0;
            TD.FloorType = 0;
            TD.FloorVersion = 0;
            TD.Resource_using = false;
            TD.Steam_Online = false;
            TD.Electricity_Online = false;
            TD.Production_Type = 0;
            TD.Production_Value = 0;
            TD.Upkeep_Type.Clear();
            TD.Upkeep_Value.Clear();
            TD.ReType = 0;
            TD.ReVersion = 0;
            TD.EventType = 0;
            TD.EventVersion = 0;
            TD.Anti_Disaster = 0;
            TD.Current_Disaster = 0;
            TD.Allow_Disaster = 0;
            TD.Event_Desaster = 0;
            TD.BuildingType = 0;
            TD.BuildingVersion = 0;
            TD.Building.sprite = null;
            TD.Needed_ManPower = 0;
            TD.Working_ManPower = 0;
            TD.polCol_Size = TileData.PolCol_Size.self;
            TD.IsWorking_OnTile = false;
            TD.Distrucktive = false;
            TD.Constructable_OnTile = false;
            TD.BuildCost_Type.Clear();
            TD.BuildCost_Value.Clear();
            TD.Turn_Count = 0;
            TD.Need_Electricity = false;
            TD.Need_Steam = false;
            TD.Event_Acidente = false;
            OndisableTileList.Add(TD.transform.gameObject);
        //이벤트로 인한 파괴가 아니고 테크 트리나 지부장 보너스를 받고 있을 때 자원회수
        if(TD.Event_Acidente == false)
        {
            //자원 회수
        }
    }
    public void Awake()
    {
            GameObject a = GameObject.Find("DataManager");
            DM = a.GetComponent<Datamanager>();
            a.GetComponent<Datamanager>().TM = this;
        if(DM.IsReset == true)
        {
            DM.Reset();
            DM.Hope = Hope;
            DM.Will = Will;
            DM.Complaint = Complaint;
            DM.Check_Mind();
            DM.ManPower_Setting();
        }
    }

    //해야할 것들 - 풀링 후 재생성 준비, 재생성 시 새로운 세팅이 만들어지도록 유도 후에 세이브로드 갱신 준비
    public void Start()
    {
        MakingOnDisableTileList();
        MakeFloorTileChanger();
        FloorTile_Active();
        //MakingPopUp_UI();
        /*MapSizeChoose();
        InitialiseList();
        PositionSetting();
        ResourceCounControll_tSetting();
        ResourceLayout_Total();
        MakeFloorTileChanger();*/
    }

    public void Test_Tile_Side_Change()
    {
        for(int i = 0; i < OnEnableTileList.Count; i++)
        {
            OnEnableTileList[i].GetComponent<TileData>().Side.color = new Color32(255, 255, 255, 0);
        }
        for (int i = 0; i < On_Working_Tile.Count; i++)
        {
            On_Working_Tile[i].Side.color = new Color32(0, 230, 0, 255);
        }
    }
}