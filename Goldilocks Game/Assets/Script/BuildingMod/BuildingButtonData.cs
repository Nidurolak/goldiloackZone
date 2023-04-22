using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingButtonData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image BuildingImage;
    public GameObject TooltipUI;
    public GameObject MS;
    public GameObject UpsideMessage;
    public SelectedTile ST;
    public SlotManager SM;
    
    public Sprite Construction_Image;
    public Sprite Image;
    public string Name;
    public string Contents;
    public string ShortContents;

    public int Production_Type;
    public float Production_Value;
    //public float Production_Value_Tooltip;
    
    public int BuildingType;
    public int BuildingVersion;

    public List<int> Upkeep_Type = new List<int>(3);
    public List<float> Upkeep_Value = new List<float>(3);

    public List<int> BuildCost_Type = new List<int>(3);
    public List<float> BuildCost_Value = new List<float>(3);
    public List<string> BuildCost_Text = new List<string>(3);

    public List<int> Buff_Type = new List<int>(3);
    public List<float> Buff_Value = new List<float>(3);

    public int Build_TurnCount;

    public int Anti_Disaster;
    public int Allow_Disaster;
    

    public int Buff_Area_Size;

    public int Working_ManPower;

    public AudioClip BuildingSound;

    public bool Need_Steam;
    public bool Need_Electricity;
    public bool Need_House;
    public bool Need_Idea;
    public bool Need_Public;
    public bool Constructable;
    public bool Workable_OnTile;

    private string Production;
    private string Disaster;

    public List<bool> BuildingCost_bool = new List<bool>();
    public bool OnlyOne;
    public List<TileData> Check_Constructable_List = new List<TileData>();

    public bool CanBuild()
    {
        bool a = false;
        var b = BuildingCost_bool.Count;
        var c = 0;
        for (int i = 0; i < BuildingCost_bool.Count; i++)
        {
            if (BuildingCost_bool[i] == true)
            {
                c++;
            }
        }
        if (b == c)
        {
            a = true;
        }
        return a;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Check_Cost_Building();
        check_OnlyOne();
        if(BuildingType  != 99)
        {
            RectTransform rt = (RectTransform)TooltipUI.transform;
            rt.anchoredPosition = new Vector2(0, 240);
            var a = TooltipUI.GetComponent<Tooltip>();
            a.ShowToolTipText(Name, ShortContents, Anti_Disaster, Allow_Disaster);
            a.ShowToolTipBuildCost(BuildCost_Type, BuildCost_Value, Build_TurnCount);
            a.ShowToolTipNeeds(Need_Electricity, Need_Steam);
            if (BuildingType != 18 && BuildingType != 10 && BuildingType != 11)
            {
                a.ShowToolTipProductionAndKeep(BuildingType, BuildingVersion, Production_Type, Production_Value, Upkeep_Type, Upkeep_Value);
            }
            else if (BuildingType == 10 || BuildingType == 11 && BuildingVersion == 1)
            {
                a.ShowToolTipProductionAndKeep(BuildingType, BuildingVersion, 17, Production_Value, Upkeep_Type, Upkeep_Value);
            }
            else
            {
                a.ShowToolTipProductionAndKeep(BuildingType, BuildingVersion, 17, Production_Value, Upkeep_Type, Upkeep_Value);
            }
        }
        //a.ShowToolTipText(Name+ "\n" + Contents + "\n\n"+ BuildingCost + BuildingCost1 + BuildingCost2 + BuildingCost3 +
       //             "  /  " + "필요 턴 수 : " + Build_TurnCount + "턴  / "+ "작업 인력 : " + Working_ManPower + "\n" + Production +
       //             Upkeep_ + Upkeep  + Upkeep1 + Upkeep2 + Upkeep3 + "       " + Buff + Buff1 + Buff2 + Buff3 + "  /  " + Disaster, 858, 200);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.transform.position = new Vector2(-1000, -1000);
        var a = TooltipUI.GetComponent<Tooltip>();
        a.HideToolTip();
    }

    //건축 모드 진입
    public void ConstructMode_Building()
    {
        Check_Cost_Building();
        check_OnlyOne();
        var a = MS.GetComponent<MouseControll>();
        a.BuildingAndSkill = gameObject;
        if (CanBuild() ==true & OnlyOne == false)
        {
            a.BuildingAndSkill_bool = true;
            SM.DM.MS.BBM = this;
            SM.DM.MS.BM = MouseControll.BuildingMod.Build;
        }
        UpsideMessage_Move(BuildingCost_bool);
    }
    //건축 모드 탈출
    public void ConstructMode_Building_Out()
    {
        var a = MS.GetComponent<MouseControll>();
        a.BuildingAndSkill = null;
        a.BuildingAndSkill_bool = false;
        SM.DM.MS.BBM = null;
        SM.DM.MS.BM = MouseControll.BuildingMod.None;
    }

    public void DistructiveMode_On()
    {
        Check_Distructable();
        var a = MS.GetComponent<MouseControll>();
        a.BuildingAndSkill = gameObject;

        a.BuildingAndSkill_bool = true;
        SM.DM.MS.BBM = this;
        SM.DM.MS.BM = MouseControll.BuildingMod.Destroy;
        UpsideMessage_Move(BuildingCost_bool);
    }

    public void DistructiveMode_Out()
    {
        var a = MS.GetComponent<MouseControll>();
        a.BuildingAndSkill = null;
        a.BuildingAndSkill_bool = false;
        SM.DM.MS.BBM = null;
        SM.DM.MS.BM = MouseControll.BuildingMod.None;
    }

    public void Check_Text_Production(int ProductionType, float ProductionValue)
    {
        Production = "생산 : " + ProductionValue + " " + SM.DM.resources(ProductionType, false);
    }

    public void Check_Text_BuildCost(List<int> BuildCostType, List<float> BuildCostValue, ref string a)
    {
        var b = 0f;
        for(int i = 0; i < BuildCostType.Count; i++)
        {
            b =
                BuildCost_Value[i] * SM.DM.Difficulty_BuildCost() *
                (1 + SM.DM.Tech_BuildingCost_Discharge_Magnification_List + SM.DM.Tech_BuildingCost_Type_Magnification_List[BuildingType - 1] + SM.DM.Tech_BuildingCost_Version_Magnification_List[BuildingType - 1].Version_List[BuildingVersion - 1]) *
                (1 + SM.DM.Event_BuildingCost_Discharge_Magnification_List + SM.DM.Event_BuildingCost_Type_Magnification_List[BuildingType - 1] + SM.DM.Event_BuildingCost_Version_Magnification_List[BuildingType - 1].Version_List[BuildingVersion - 1]);
        }
    }

    public void Check_Text_Upkeep(int UpkeepType, float UpkeepCost, ref string a)
    {
        a = "-" + UpkeepCost + " " + SM.DM.resources(UpkeepType, false);
    }

    public void Check_Text_Buff(int BuffType, float BuffValue, ref string a)
    {
        string c = "";
        if (BuffValue > 0)
        {
            c = " 증가 ";
        }
        else if(BuffValue < 0)
        {
            c = " 감소 ";
        }
        else if(BuffValue == 0)
        {
            c = "";
        }

        //여기서 이프문으로 증가 감소 텍스트 추가
        switch (BuffType)
        {
            case 12:
                a = "불만" + c;
                break;
            case 13:
                a = "희망" + c;
                break;
            case 14:
                a = "의지" + c;
                break;
        }
    }
    

    public void Check_Text_Disaster(int Allow, int Anti)
    {
        Disaster = "방재|허용 오염수치 : " + Anti + "/" + Allow;
    }

    public void Check_Cost_Building()
    {
        for (int i = 0; i < BuildCost_Type.Count; i++)
        {
            var a =
                BuildCost_Value[i] *
                (1 + SM.DM.Tech_BuildingCost_Discharge_Magnification_List + SM.DM.Tech_BuildingCost_Type_Magnification_List[BuildingType - 1] + SM.DM.Tech_BuildingCost_Version_Magnification_List[BuildingType - 1].Version_List[BuildingVersion - 1]) *
                (1 + SM.DM.Event_BuildingCost_Discharge_Magnification_List + SM.DM.Event_BuildingCost_Type_Magnification_List[BuildingType - 1] + SM.DM.Event_BuildingCost_Version_Magnification_List[BuildingType - 1].Version_List[BuildingVersion - 1]) *
                SM.DM.Difficulty_BuildCost();
            if (a > SM.DM.All_Resource_List[BuildCost_Type[i]])
            {
                BuildingCost_bool[i] = false;
            }
            else BuildingCost_bool[i] = true;
        }
    }

    //건설이 가능한지 매번 체크해야하므로 마우스 컨트롤에서도 불려간다.
    public void Check_Constructable()
    {

        //건물 타입을 두루두루 돌면서 작동, 각 건물 타입 별로 작동함
        switch (BuildingType)
        {
            case 1:
                Check_Constructable_List = SM.TM.On_Steel;
                for(int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if(Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 4:
                Check_Constructable_List = SM.TM.NearBy_Ore;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 5:
                Check_Constructable_List = SM.TM.On_Goldion;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 9:
                Check_Constructable_List = SM.TM.NearBy_Wood;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 10:
                List<TileData> a = new List<TileData>();
                for(int i = 0; i < SM.TM.NearBy_House.Count; i++)
                {
                    if (SM.TM.NearBy_House[i].GetComponent<TileData>().ReType == 0 &&
                        SM.TM.NearBy_House[i].GetComponent<TileData>().BuildingType == 0)
                    {
                        a.Add(SM.TM.NearBy_House[i]);
                    }
                }
                Check_Constructable_List = a;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 11:
                List<TileData> b = new List<TileData>();
                for (int i = 0; i < SM.TM.NearBy_House.Count; i++)
                {
                    if (SM.TM.NearBy_House[i].GetComponent<TileData>().ReType == 0 &&
                        SM.TM.NearBy_House[i].GetComponent<TileData>().BuildingType == 0)
                    {
                        b.Add(SM.TM.NearBy_House[i]);
                    }
                }
                Check_Constructable_List = b;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 14:
                Check_Constructable_List = SM.TM.NearBy_Steel_Whale;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 19:
                List<TileData> c = new List<TileData>();
                for (int i = 0; i < SM.TM.NearBy_House.Count; i++)
                {
                    if (SM.TM.NearBy_House[i].GetComponent<TileData>().ReType == 0 &&
                        SM.TM.NearBy_House[i].GetComponent<TileData>().BuildingType == 0)
                    {
                        c.Add(SM.TM.NearBy_House[i]);
                    }
                }
                Check_Constructable_List = c;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 20:
                List<TileData> d = new List<TileData>();
                for (int i = 0; i < SM.TM.NearBy_House.Count; i++)
                {
                    if (SM.TM.NearBy_House[i].GetComponent<TileData>().ReType == 0 &&
                        SM.TM.NearBy_House[i].GetComponent<TileData>().BuildingType == 0)
                    {
                        d.Add(SM.TM.NearBy_House[i]);
                    }
                }
                Check_Constructable_List = d;
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            case 99:
                for (int z = 0; z < SM.TM.OnEnableTileList.Count; z++)
                {
                    Check_Constructable_List.Add(SM.TM.OnEnableTileList[z].GetComponent<TileData>());
                }
                for (int i = 0; i < Check_Constructable_List.Count; i++)
                {
                    if (Check_Constructable_List[i].GetComponent<TileData>().Distrucktive == false &&
                        Check_Constructable_List[i].GetComponent<TileData>().Resource_using == true)
                    {
                        Check_Constructable_List.Remove(Check_Constructable_List[i]);
                    }
                }
                break;
            default:
                if(OnlyOne == false)
                {
                    Check_Constructable_List = SM.TM.Default_Tile;
                    for (int i = 0; i < Check_Constructable_List.Count; i++)
                    {
                        if (Check_Constructable_List[i].GetComponent<TileData>().BuildingType != 0)
                        {
                            Check_Constructable_List.Remove(Check_Constructable_List[i]);
                        }
                    }
                }
                break;
        }
    }

    public void Check_Distructable()
    {
        Check_Constructable_List.Clear();
        /*for (int z = 0; z < SM.TM.OnEnableTileList.Count; z++)
        {
            Check_Constructable_List.Add(SM.TM.OnEnableTileList[z].GetComponent<TileData>());
        }
        for (int i = 0; i < Check_Constructable_List.Count; i++)
        {
            if (Check_Constructable_List[i].GetComponent<TileData>().Distrucktive == false ||
                Check_Constructable_List[i].GetComponent<TileData>().Resource_using == true)
            {
                Check_Constructable_List.Remove(Check_Constructable_List[i]);
            }
        }*/
        for (int z = 0; z < SM.TM.OnEnableTileList.Count; z++)
        {
            if(SM.TM.OnEnableTileList[z].GetComponent<TileData>().Distrucktive == true)
            {
                Check_Constructable_List.Add(SM.TM.OnEnableTileList[z].GetComponent<TileData>());
            }
        }
    }

    public void Show_Constructable_tile()
    {
        SM.TM.Ms.ClearTileSide();

        if (CanBuild() == true & OnlyOne == false)
        {
            for(int i = 0; i < Check_Constructable_List.Count; i++)
            {
                Check_Constructable_List[i].GetComponent<TileData>().Side.color = new Color32(0, 230, 0, 255);
            }
        }
    }

    public void Show_Distructable_Tile()
    {
        SM.TM.Ms.ClearTileSide();
        for (int i = 0; i < Check_Constructable_List.Count; i++)
        {
            if (Check_Constructable_List[i].GetComponent<TileData>().Distrucktive == true &&
                Check_Constructable_List[i].GetComponent<TileData>().Resource_using == false)
            {
                Check_Constructable_List[i].GetComponent<TileData>().Side.color = new Color32(255, 0, 0, 255);
            }
            else if(Check_Constructable_List[i].GetComponent<TileData>().IsDistory_OnTile == true)
            {
                Check_Constructable_List[i].GetComponent<TileData>().Side.color = new Color32(50, 50, 50, 255);
            }
        }
    }

    //건물 가격을 코스트 타입 123 전부 확인해서 처리
    public void Cost_Pay_Building()
    {
        for(int i = 0; i < BuildCost_Type.Count; i++)
        {
            var a =
                BuildCost_Value[i] *
                (1 + SM.DM.Tech_BuildingCost_Discharge_Magnification_List + SM.DM.Tech_BuildingCost_Type_Magnification_List[BuildingType - 1] + SM.DM.Tech_BuildingCost_Version_Magnification_List[BuildingType - 1].Version_List[BuildingVersion - 1]) *
                (1 + SM.DM.Event_BuildingCost_Discharge_Magnification_List + SM.DM.Event_BuildingCost_Type_Magnification_List[BuildingType - 1] + SM.DM.Event_BuildingCost_Version_Magnification_List[BuildingType - 1].Version_List[BuildingVersion - 1]) *
                SM.DM.Difficulty_BuildCost();
            SM.DM.All_Resource_List[BuildCost_Type[i]] -= (int)a;
        }
    }

    public void check_OnlyOne()
    {
        switch (BuildingType)
        {
            //의무
            case 10:
                for (int i = 1; i < SM.TM.TypeAll_Building_List[10].Type_Building.Count; i++)
                {
                    if (SM.TM.TypeAll_Building_List[10].Type_Building[i].Buildings.Count != 0)
                    {
                        OnlyOne = true;
                    }
                }
                break;
            //사명
            case 11:
                for (int i = 1; i < SM.TM.TypeAll_Building_List[11].Type_Building.Count; i++)
                {
                    if (SM.TM.TypeAll_Building_List[11].Type_Building[i].Buildings.Count != 0)
                    {
                        OnlyOne = true;
                    }
                }
                break;

            case 12:
                for(int i = 0; i < SM.TM.TypeAll_Building_List[12].Type_Building.Count; i++)
                {
                    if (SM.TM.TypeAll_Building_List[12].Type_Building[i].Buildings.Count != 0)
                    {
                        OnlyOne = true;
                    }
                }
                break;

            case 19:
                for (int i = 0; i < SM.TM.TypeAll_Building_List[19].Type_Building.Count; i++)
                {
                    if (SM.TM.TypeAll_Building_List[19].Type_Building[i].Buildings.Count != 0)
                    {
                        OnlyOne = true;
                    }
                }
                break;
            default:
                OnlyOne = false;
                break;
        }
    }

    //종합 데이터 변화
    public void Change_TileData(TileData a)
    {
        //비용 검사
        Check_Cost_Building();
        check_OnlyOne();
        //조건에 부합할 경우 건설
        if (CanBuild() == true & OnlyOne == false)
        {
            Cost_Pay_Building();
            a.BuildingType = BuildingType;
            a.BuildingVersion = BuildingVersion;
            a.Production_Type = Production_Type;
            a.Production_Value  = Production_Value;
            a.Upkeep_Type = Upkeep_Type;
            a.Upkeep_Value = Upkeep_Value;
            a.BuildCost_Type = BuildCost_Type;
            a.Turn_Count = Build_TurnCount;
            a.Anti_Disaster = Anti_Disaster;
            a.Allow_Disaster = Allow_Disaster;
            a.BuildingSound.clip = BuildingSound;
            a.Buff_Type = Buff_Type;
            a.Buff_Value = Buff_Value;
            a.Buff_Area_Size = Buff_Area_Size;
            a.Needed_ManPower = Working_ManPower;
            a.Need_Steam = Need_Steam;
            a.Need_House = Need_House;
            a.Need_Idea = Need_Idea;
            a.Need_Public = Need_Public;
            a.Need_Electricity = Need_Electricity;
            a.Constructable_OnTile = Constructable = false;
            a.Distrucktive = true;
            a.Resource.sprite = null;

            //건물 리스트에 추가
            SM.TM.TypeAll_Building_List[BuildingType - 1].Type_Building[BuildingVersion - 1].Buildings.Add(a);

            if (a.Turn_Count > 0)
            {
                a.Building.sprite = Construction_Image;
                SM.TM.On_Build_Tile.Add(a);
            }
            else if(a.Turn_Count == 0)
            {
                a.Building.sprite = Image;
                a.Building.color = new Color32(155, 155, 155, 255);
                SM.TM.Off_Working_Tile.Add(a);
            }

            //에리어 체크를 실시하여 주변 자원을 쏘옥 쏙쏙 빼간다.
            a.AreaCheck.GetComponent<TileChecker>().Check_Building = true;
            a.AreaCheck.SetActive(true);
            Check_Constructable();
            Show_Constructable_tile();
            a.Check_Disaster();
        }

        //다시 검사 및 텍스트 교체. 건설 후 자원이 부족할 경우 설정
        Check_Cost_Building();
        check_OnlyOne();
        UpsideMessage_Move(BuildingCost_bool);
    }

    public void List_Initialize(int Type, TileData a)
    {
        SM.TM.TypeAll_Building_List[Type].Type_Building[a.BuildingVersion].Buildings.Add(a);
    }

    public void UpsideMessage_Move(List<bool> c)
    {
        if (UpsideMessage.GetComponent<UpsideMessage>().MoveCheck == false)
        {
            UpsideMessage.GetComponent<UpsideMessage>().Slot_Move();
        }

        if (CanBuild() == true & OnlyOne == false)
        {
            UpsideMessage.GetComponent<UpsideMessage>().MessageChange("타일을 클릭하여 " + Name + "을(를) 건설하세요." + "\n 우클릭으로 건축 모드 해제");
        }
        else if(CanBuild() == false)
        { 
            UpsideMessage.GetComponent<UpsideMessage>().MessageChange("자원이 부족합니다.");
            StartCoroutine(TestMove());
        }
        if(OnlyOne == true)
        {
            UpsideMessage.GetComponent<UpsideMessage>().MessageChange(Name + "은(는) 하나만 건설할 수 있습니다.");
        }
        if(BuildingType == 99)
        {
            UpsideMessage.GetComponent<UpsideMessage>().MessageChange("파괴가능한 타일을 클릭하여 철거를 진행할 수 있습니다" + "\n 건물 철거는 취소할 수 없으며 1인력을 사용합니다." + "\n 우클릭으로 철거 모드 해제");
        }
        
    }

    IEnumerator TestMove()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (UpsideMessage.GetComponent<UpsideMessage>().MoveCheck == true)
        {
            UpsideMessage.GetComponent<UpsideMessage>().Slot_Move();
        }
    }
    
    public void DeepCopyType(ref List<int> a, ref List<int> b)
    {
        a = b;
    }

    public void OnEnable()
    {
        check_OnlyOne();
        Check_Constructable();
        BuildingImage.sprite = Image;
        /*Check_Text_BuildCost(BuildCost_Type, BuildCost_Value, ref BuildingCost1);
        Check_Text_BuildCost(BuildCost_Type, BuildCost_Value, ref BuildingCost2);
        Check_Text_BuildCost(BuildCost_Type, BuildCost_Value, ref BuildingCost3);
        Check_Text_Disaster(Allow_Disaster, Anti_Disaster);
        Check_Text_Production(Production_Type, Production_Value);
        Check_Text_Upkeep(Upkeep_Type, Upkeep_Value, ref Upkeep1);
        Check_Text_Upkeep(UpKeep2_Type, UpKeep2_Value, ref Upkeep2);
        Check_Text_Upkeep(UpKeep3_Type, UpKeep3_Value, ref Upkeep3);
        Check_Text_Buff(Buff1_Type, Buff1_Value, ref Buff1);
        Check_Text_Buff(Buff2_Type, Buff2_Value, ref Buff2);
        Check_Text_Buff(Buff3_Type, Buff3_Value, ref Buff3);
        Check_Text_Upkeep(UpKeep1_Type, Buff1_Type);*/
    }

    public void OnDisable()
    {
        BuildingType = 0;
        BuildingVersion = 0;
        Image = null;
        Name = null;
        Contents = null;
        Production_Type = 0;
        Production_Value = 0;
        Upkeep_Type.Clear();
        Upkeep_Value.Clear();
        BuildCost_Type.Clear();
        BuildCost_Value.Clear();
        for (int i = 0; i < BuildingCost_bool.Count; i++)
        {
            BuildingCost_bool[i] = true;
        }
        Build_TurnCount = 0;
        Anti_Disaster = 0;
        Allow_Disaster = 0;
        Buff_Type.Clear();
        Buff_Value.Clear();
        Buff_Area_Size = 0;
        Working_ManPower = 0;
        Need_Steam = false;
        Need_Electricity = false;
        Need_Public = false;
        Need_Idea = false;
        Need_House = false;
        BuildingSound = null;
        Constructable = false;
        Production = "";
        Disaster = "";
    }
}
