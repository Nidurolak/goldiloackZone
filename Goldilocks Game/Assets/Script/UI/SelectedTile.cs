using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#pragma warning disable 0649

public class SelectedTile : UIMove
{
    //08/11 재난, 애드온 관련 슬라이더와 텍스트를 연결해야한다.
    public Vector2 DownPosition;
    public Vector2 UpPosition;
    public UISound uISound;

    public List<GameObject> FoodPanel = new List<GameObject>();
    public List<GameObject> WorkManPanel = new List<GameObject>();

    public List<Image> ShowTile = new List<Image>();
    public MouseControll MS;
    public TileManager TM;
    public Text Name_Text;
    public Text TileName_Text;
    public Text ResourceName_Text;
    public Text ResourceContents_Text;
    public Text BuildingName_Text;
    public Text BuildingContents_Text;
    public Text ProdutionORSupply_Text;
    public Text UpKeep_Text;
    public Text EventName_Text;
    public Text EventContents_Text;
    public Text CurrentDisaster_Text;
    public Text AllowDisaster_Text;
    public Slider CurrentDisaster_Slider;
    public Image CurrentDisaster_Slider_Color;
    public Text AddonName_Text;
    public GameObject OnOff_Working;
    public Button OnOff_Working_Button;
    public GameObject OnOff_Working_Icon;
    public Text OnOff_Working_Text;

    public Image Working;

    public SelectedTilePanelSlot Product;

    public List<SelectedTilePanelSlot> Cost = new List<SelectedTilePanelSlot>();
    //public SelectedTilePanelSlot Cost1;
    //public SelectedTilePanelSlot Cost2;
    //public SelectedTilePanelSlot Cost3;
    public SelectedTilePanelSlot Electricity;
    public SelectedTilePanelSlot Steam;

    public GameObject UnableWork;
    public Text UnableWork_Text;

    private string d;
    private string e;
    private string f;
    private string g;

    //public List<Sprite> Resource_Icon = new List<Sprite>();

    //Json으로 불러와 넣을 것들, 원래는 데이터 매니저에 할당해야한다.
    public List<string> TileName_Text_List = new List<string>();


    public List<Text> ShowText = new List<Text>();
    public bool MoveCheck;

    private Coroutine a;
    private Coroutine b;

    public void ChangeSpriteForSelectedTile(GameObject a)
    {//선택된 오브젝트를 이용해 스프라이트를 수정한다.
        List<SpriteRenderer> c = new List<SpriteRenderer>();
        
        //바닥 타일의 스프라이트를 수정
        var FloorTile = a.GetComponent<SpriteRenderer>();
        ShowTile[0].sprite = FloorTile.sprite;
        ShowTile[0].color = new Color32(255, 255, 255, 255);
        ShowTile[0].SetNativeSize();

        ShowTile[1].color = new Color32(255, 255, 255, 0);
        ShowTile[2].color = new Color32(255, 255, 255, 0);
        ShowTile[3].color = new Color32(255, 255, 255, 0);

        //자식 스프라이트들(건물, 이벤트, 자원)을 for문으로 돌리면서 각각 할당
        for (int i = 1; i < 4; i++)
        {
            var aChild = a.transform.GetChild(i - 1).gameObject;
            var aChildRen = aChild.GetComponent<SpriteRenderer>();
            //Debug.Log(i);
            if (aChildRen.sprite != null)
            {
                ShowTile[i].sprite = aChildRen.sprite;
                ShowTile[i].color = new Color32(255, 255, 255, 255);
                if(i == 2)
                {
                    ShowTile[i].rectTransform.sizeDelta = new Vector2(256, 256);
                }

                //ShowTile[i].SetNativeSize();
            }
        }
        //Debug.Log("선택된 타일 스프라이트 변경 작동");
    }

    public void SpriteSize_Setting(Sprite sprite)
    {
        //68번 줄에 삽입할 수 있도록 수정
    }

    //텍스트 리스트에서 각자 자료를 불러와 집어넣을 것들
    public void ChangeTextForSelectedTile(GameObject a)
    {
        var aTileData = a.GetComponent<TileData>();
        var b = aTileData.BuildingType;
        var c = aTileData.BuildingVersion;
        
        //이것도 나중에 타일 매니저에서 리스트를 따로 작성해야겠어
        //TileName_Text.text = TileName_Text_List[aTileData.FloorVersion];
        ResourceName_Text.text = TM.ResourceName_Text_List[aTileData.ReType];
        ResourceContents_Text.text = TM.ResourceContents_Text_List[aTileData.ReType];

        //빌딩 이름 텍스트
        if (aTileData.BuildingType != 0 && aTileData.Turn_Count > 0)
        {
            BuildingName_Text.text = "건설 중 ("+ TM.DM.Bd[aTileData.BuildingType - 1].BD_Version[aTileData.BuildingVersion - 1].Name + ")";
        }
        else if (aTileData.BuildingType != 0)
        {
            BuildingName_Text.text = TM.DM.Bd[aTileData.BuildingType - 1].BD_Version[aTileData.BuildingVersion - 1].Name;
        }
        else BuildingName_Text.text = "";
        //빌딩 내용 텍스트
        if (aTileData.BuildingType != 0 && aTileData.Turn_Count == 0)
        {
            BuildingContents_Text.text = TM.DM.Bd[aTileData.BuildingType - 1].BD_Version[aTileData.BuildingVersion - 1].Contents;
        }
        else BuildingContents_Text.text = "";

        //생산 혹은 주거구역 텍스트 체크
        if (aTileData.BuildingType != 0)
        {
            Product.Icon.SetActive(false);
            //건축이 안 끝났는지 체크
            if (aTileData.Production_Type != -1 && aTileData.Turn_Count > 0)
            {
                Product.Icon.SetActive(false);
            }
            else if (aTileData.Production_Type != -1)
            {
                if (TM.DM.Bd[aTileData.BuildingType - 1].BD_Version[aTileData.BuildingVersion - 1].Version != 11)
                {
                    Product.Icon.SetActive(true);
                    Product.image.sprite = TM.DM.Resource_Sprites_Logo_NonBackGround[aTileData.Production_Type];
                    Product.text.text = "+" + aTileData.Production_Value;
                }
                else
                {
                    Product.Icon.SetActive(true);
                    Product.image.sprite = TM.DM.Resource_Sprites_Logo_NonBackGround[aTileData.Production_Type];
                    Product.text.text = "+" + aTileData.Production_Value;
                    Debug.Log("T3");
                }
            }
            //유지비 체크인데 3바퀴 돌면서 정보가 없으면 셋엑티브 폴스 시키고 있으면 셋엑티브로 켜서 값할당해야겠어

            for (int i = 0; i < Cost.Count; i++)
            {
                Cost[i].Icon.SetActive(false);
            }
            for (int i = 0; i < aTileData.Upkeep_Type.Count; i++)
            {
                Cost[i].Icon.SetActive(true);
                Cost[i].image.sprite = TM.DM.Resource_Sprites_Logo_NonBackGround[aTileData.Upkeep_Type[i]];
                Cost[i].text.text = "-" + aTileData.Upkeep_Value[i];
            }
            if(aTileData.Need_Electricity  == true)
            {
                Electricity.Icon.SetActive(true);
                if(aTileData.Electricity_Online == true)
                {
                    Electricity.text.text = "연결됨";
                }
                else if(aTileData.Electricity_Online == false)
                {
                    Electricity.text.text = "연결되지 않음";
                }
            }
            else
            {
                Electricity.Icon.SetActive(false);
            }
            if (aTileData.Need_Steam == true)
            {
                Steam.Icon.SetActive(true);
                if (aTileData.Steam_Online == true)
                {
                    Steam.text.text = "연결됨";
                }
                else if (aTileData.Steam_Online == false)
                {
                    Steam.text.text = "연결되지 않음";
                }
            }
            else
            {
                Steam.Icon.SetActive(false);
            }
        }


        //타일 데이터에서 현재 재난 수치 공식을 짜야겠어
        CurrentDisaster_Slider.value = aTileData.Current_Disaster;
        switch (CurrentDisaster_Slider.value)
        {
            case -7: CurrentDisaster_Slider_Color.color = new Color32(225, 0, 0, 255);
                g = "레드-";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -6:
                CurrentDisaster_Slider_Color.color = new Color32(225, 0, 0, 255);
                g = "레드";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -5:
                CurrentDisaster_Slider_Color.color = new Color32(255, 189, 0, 255);
                g = "오렌지-";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -4:
                CurrentDisaster_Slider_Color.color = new Color32(255, 189, 0, 255);
                g = "오렌지";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -3:
                CurrentDisaster_Slider_Color.color = new Color32(255, 255, 0, 255);
                g = "옐로우-";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -2:
                CurrentDisaster_Slider_Color.color = new Color32(255, 255, 0, 255);
                g = "옐로우";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -1:
                CurrentDisaster_Slider_Color.color = new Color32(0, 255, 0, 255);
                g = "그린-";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case 0:
                CurrentDisaster_Slider_Color.color = new Color32(0, 255, 0, 255);
                g = "그린";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
            case -8:
                CurrentDisaster_Slider_Color.color = new Color32(0, 0, 0, 255);
                g = "블랙";
                CurrentDisaster_Text.text = "현재 오염 등급 : " + g + " (" + aTileData.Current_Disaster + ")";
                break;
        }

        if (aTileData.Allow_Disaster <= -1
            && aTileData.BuildingType != 0)
        {
            switch (aTileData.Allow_Disaster)
            {
                case -7:
                        AllowDisaster_Text.text = "요구 오염 최소치 : 레드- (" + aTileData.Allow_Disaster + ")";
                    break;
                case -6:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 레드 (" + aTileData.Allow_Disaster + ")";
                    break;
                case -5:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 오렌지- (" + aTileData.Allow_Disaster + ")";
                    break;
                case -4:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 오렌지 (" + aTileData.Allow_Disaster + ")";
                    break;
                case -3:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 옐로우- (" + aTileData.Allow_Disaster + ")";
                    break;
                case -2:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 옐로우 (" + aTileData.Allow_Disaster + ")";
                    break;
                case -1:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 그린- (" + aTileData.Allow_Disaster + ")";
                    break;
                case 0:
                    AllowDisaster_Text.text = "요구 오염 최소치 : 그린 (" + aTileData.Allow_Disaster + ")";
                    break;
            }
        }
        else AllowDisaster_Text.text = "";
    }
    
    //토글 버튼 관리 스크립트
    public void Check_WorkingButton(GameObject B, int BuildingType, bool IsWorking)
    {
        if  (
            MS.Tile_Selected.GetComponent<TileData>().BuildingType != 0
            || MS.Tile_Selected.GetComponent<TileData>().ReType != 0
            && MS.Tile_Selected.GetComponent<TileData>().Workable_OnTile() == true
            || MS.Tile_Selected.GetComponent<TileData>().Distrucktive == true
            //&& MS.Tile_Selected.GetComponent<TileData>().Turn_Count == 0
            )
        {
            B.SetActive(true);
            if (MS.Tile_Selected.GetComponent<TileData>().ReType == 1
               || MS.Tile_Selected.GetComponent<TileData>().ReType == 5)
               //|| MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0)
            {
                if (MS.Tile_Selected.GetComponent<TileData>().BuildingType == 0)
                {
                    B.SetActive(false);
                    Working.gameObject.SetActive(false);
                    Working.color = new Color(0, 0, 0, 0);
                }
                else
                {
                    //건물이 작업 중인데 토글이 꺼져있으면
                    if (IsWorking == true)
                    {
                        //스프라이트 교체는 관련 이미지를 받거든 시작함.
                        //Working.sprite = OnWorking;
                        Working.gameObject.SetActive(true);
                        Working.color = new Color(0, 180, 0, 255);
                    }
                    else if (IsWorking == false)
                    {
                        //스프라이트 교체는 관련 이미지를 받거든 시작함.
                        //Working.sprite = OFFWorking;
                        Working.gameObject.SetActive(true);
                        Working.color = new Color(114, 0, 0, 255);
                    }
                }
            }
            //건물이 작업 중인데 토글이 꺼져있으면
            else if (IsWorking == true)
            {
                //스프라이트 교체는 관련 이미지를 받거든 시작함.
                //Working.sprite = OnWorking;
                Working.gameObject.SetActive(true);
                Working.color = new Color(0, 180, 0, 255);
            }
            else if (IsWorking == false)
            {
                //스프라이트 교체는 관련 이미지를 받거든 시작함.
                //Working.sprite = OFFWorking;
                Working.gameObject.SetActive(true);
                Working.color = new Color(114, 0, 0, 255);
            }
        }
        else
        {
            B.SetActive(false);
            Working.gameObject.SetActive(false);
            Working.color = new Color(0, 0, 0, 0);
        }

        if (BuildingType != 0
            && MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0
            && MS.Tile_Selected.GetComponent<TileData>().ReType != 0)
        {
            B.SetActive(false);
            Working.gameObject.SetActive(false);
            Working.color = new Color(0, 0, 0, 0);
        }
    }

    public void Check_WorkingText(int BuildingType, int NeedPoint)
    {
        //건축 중인 건물
        if (BuildingType != 0 && MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0)
        {
            if (MS.Tile_Selected.GetComponent<TileData>().IsDistory_OnTile == false)
            {
                OnOff_Working_Text.text = MS.Tile_Selected.GetComponent<TileData>().Turn_Count + "턴 후 완성";
                OnOff_Working.SetActive(false);
            }
            else if (MS.Tile_Selected.GetComponent<TileData>().IsDistory_OnTile == true)
            {
                OnOff_Working_Text.text = "파괴 중\n" + MS.Tile_Selected.GetComponent<TileData>().Turn_Count + "턴 후 파괴\n" +
                    MS.Tile_Selected.GetComponent<TileData>().Working_ManPower + "인력 투입 중";
            }
            Working.gameObject.SetActive(false);
            Working.color = new Color(0, 0, 0, 0);
        }
        //파괴 가능한 자원
        else if(MS.Tile_Selected.GetComponent<TileData>().ReType !=0 && MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0
                && MS.Tile_Selected.GetComponent<TileData>().Resource_using ==false
                && MS.Tile_Selected.GetComponent<TileData>().Distrucktive == true)
        {
            OnOff_Working_Text.text = "파괴 가능\n" + MS.Tile_Selected.GetComponent<TileData>().Turn_Count + "턴 필요함\n" +
                NeedPoint + "인력 필요함";
        }
        else if (MS.Tile_Selected.GetComponent<TileData>().ReType != 0 && MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0
                && MS.Tile_Selected.GetComponent<TileData>().Resource_using == false
                && MS.Tile_Selected.GetComponent<TileData>().Distrucktive == false)
        {
            OnOff_Working_Text.text = "파괴 불가\n" + "관련 연구 필요함";
        }
        else if (MS.Tile_Selected.GetComponent<TileData>().ReType != 0 && MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0
                && MS.Tile_Selected.GetComponent<TileData>().Resource_using == true
                && MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile == false)
        {
            OnOff_Working_Text.text = "파괴 불가";
        }
        else if (MS.Tile_Selected.GetComponent<TileData>().ReType != 0 && MS.Tile_Selected.GetComponent<TileData>().Turn_Count > 0
                && MS.Tile_Selected.GetComponent<TileData>().Resource_using == true
                && MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile == true)
        {
            OnOff_Working_Text.text = "파괴 중\n" + MS.Tile_Selected.GetComponent<TileData>().Turn_Count + "턴 후 파괴\n" +
                NeedPoint + "인력 투입 중";
        }
        else if (BuildingType != 0 && MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile == true
                && MS.Tile_Selected.GetComponent<TileData>().IsDistory_OnTile == false)
        {
            if(NeedPoint > 0)
            {
                OnOff_Working_Text.text = "작업 중\n" + NeedPoint + "인력 투입 중";
            }
            else
            {
                OnOff_Working_Text.text = "가동 중";
            }
        }
        else if (BuildingType != 0 && MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile == false)
        {
            if (NeedPoint > 0)
            {
                OnOff_Working_Text.text = NeedPoint + "인력 필요함";
            }
            else
            {
                OnOff_Working_Text.text = "";
            }
        }
        else OnOff_Working_Text.text = "";
    }

    public void Check_AbleToWork(GameObject Label, Text Cause)
    {
            Label.SetActive(false);
        //if (MS.Tile_Selected.GetComponent<TileData>().Workable_OnTile == false)
        //{
            //Label.SetActive(false);
            if (MS.Tile_Selected.GetComponent<TileData>().Needed_ManPower > TM.DM.All_Resource_List[28]
                || MS.Tile_Selected.GetComponent<TileData>().Allow_Disaster > MS.Tile_Selected.GetComponent<TileData>().Current_Disaster
                || MS.Tile_Selected.GetComponent<TileData>().Resource_using == true
                || MS.Tile_Selected.GetComponent<TileData>().Event_Acidente == true
                || MS.Tile_Selected.GetComponent<TileData>().Distrucktive == true
                || MS.Tile_Selected.GetComponent<TileData>().BuildingType != 0
                && MS.Tile_Selected.GetComponent<TileData>().Turn_Count == 0)
                
            {
                Label.SetActive(true);
                //Debug.Log("세부 예외처리 발동");

                if(MS.Tile_Selected.GetComponent<TileData>().Event_Acidente == true)
                {
                    Cause.text = "사고 발생";
                }
                else if (MS.Tile_Selected.GetComponent<TileData>().Resource_using == true && MS.Tile_Selected.GetComponent<TileData>().IsDistory_OnTile == false)
                {
                    //Debug.Log("점유 중 예외처리 발동");
                    Cause.text = "사용 중인 자원";
                }
                else if (MS.Tile_Selected.GetComponent<TileData>().Needed_ManPower > TM.DM.All_Resource_List[28]
                && MS.Tile_Selected.GetComponent<TileData>().Allow_Disaster > MS.Tile_Selected.GetComponent<TileData>().Current_Disaster
                && MS.Tile_Selected.GetComponent<TileData>().ReType == 0)
                {
                    //Debug.Log("인력/오염 예외처리 발동");
                    Cause.text = "인력 부족 \n" + "오염 심각";
                }
                else if (MS.Tile_Selected.GetComponent<TileData>().Allow_Disaster > MS.Tile_Selected.GetComponent<TileData>().Current_Disaster
                && MS.Tile_Selected.GetComponent<TileData>().BuildingType != 0)
                {
                    //Debug.Log("오염 예외처리 발동");
                    Cause.text = "오염 심각";
                }
                else if (MS.Tile_Selected.GetComponent<TileData>().Needed_ManPower > TM.DM.All_Resource_List[28]
                && MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile == false)
                {
                    //Debug.Log("인력 예외처리 발동");
                    Cause.text = "인력 부족";
                }
                else if (MS.Tile_Selected.GetComponent<TileData>().Need_Steam == true
                && MS.Tile_Selected.GetComponent<TileData>().Steam_Online == false)
                {
                    //Debug.Log("증기 예외처리 발동");
                    Cause.text = "증기망 필요";
                }
                else if (MS.Tile_Selected.GetComponent<TileData>().Need_Electricity == true
                && MS.Tile_Selected.GetComponent<TileData>().Electricity_Online == false)
                {
                    //Debug.Log("전기 예외처리 발동");
                    Cause.text = "전력망 필요";
                }
            else { Label.SetActive(false); }
            }
    }

    public void Toggle_On()
    {
        if (MS.Tile_Selected.GetComponent<TileData>().Workable_OnTile() == true)
        {
            MS.Tile_Selected.GetComponent<TileData>().Toggle_On();
            Check_WorkingButton(OnOff_Working, MS.Tile_Selected.GetComponent<TileData>().BuildingType, MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile);
            Check_WorkingText(MS.Tile_Selected.GetComponent<TileData>().BuildingType, MS.Tile_Selected.GetComponent<TileData>().Needed_ManPower);
        }
    }

    public void SidePanelOnOFF(int buildingType, int buildingVersion, bool isOn)
    {
        //식량 법안 초기화
        for(int i = 0; i < FoodPanel.Count; i++)
        {
            FoodPanel[i].SetActive(false);
        }
        //인력 법안 초기화
        for (int i = 0; i < WorkManPanel.Count; i++)
        {
            WorkManPanel[i].SetActive(false);
        }


        if (isOn == true)
        {
            switch (buildingType)
            {
                case 7:
                    //묽은 수프
                    if (MS.SM.DM.RM.ruleInfos[0].isWorking == true)
                    {
                        FoodPanel[0].SetActive(true);
                        FoodPanel[1].SetActive(true);
                        FoodPanel[2].SetActive(true);
                    }
                    //푸짐한 식사
                    else if (MS.SM.DM.RM.ruleInfos[1].isWorking == true)
                    {
                        FoodPanel[0].SetActive(true);
                        FoodPanel[1].SetActive(true);
                        FoodPanel[3].SetActive(true);
                    }

                    //배식 제한
                    if (MS.SM.DM.RM.ruleInfos[2].isWorking == true)
                    {
                        FoodPanel[0].SetActive(true);
                        FoodPanel[4].SetActive(true);
                        FoodPanel[5].SetActive(true);
                    }
                    //배식 증량
                    else if (MS.SM.DM.RM.ruleInfos[3].isWorking == true)
                    {
                        FoodPanel[0].SetActive(true);
                        FoodPanel[4].SetActive(true);
                        FoodPanel[6].SetActive(true);
                    }
                    break;
                case 18:
                    //비상 근무
                    if (MS.SM.DM.RM.ruleInfos[4].isWorking == true)
                    {
                        WorkManPanel[0].SetActive(true);
                        WorkManPanel[1].SetActive(true);
                        WorkManPanel[2].SetActive(true);
                    }
                    //단축 근무
                    else if (MS.SM.DM.RM.ruleInfos[5].isWorking == true)
                    {
                        WorkManPanel[0].SetActive(true);
                        WorkManPanel[1].SetActive(true);
                        WorkManPanel[3].SetActive(true);
                    }

                    //일용직 고용
                    if (MS.SM.DM.RM.ruleInfos[11].isWorking == true)
                    {
                        WorkManPanel[0].SetActive(true);
                        WorkManPanel[4].SetActive(true);
                        WorkManPanel[5].SetActive(true);
                    }
                    //특근 수당
                    else if (MS.SM.DM.RM.ruleInfos[12].isWorking == true)
                    {
                        WorkManPanel[0].SetActive(true);
                        WorkManPanel[4].SetActive(true);
                        WorkManPanel[6].SetActive(true);
                    }
                    break;
            }
        }
    }

    public void ChangeNumber(int a)
    {
        if(a < 0)
        {
            a = 0;
        }
    }

    public override void Slot_Move()
    {
        if (MoveCheck == false)
        {
            ChangeTextForSelectedTile(MS.Tile_Selected);
            ChangeSpriteForSelectedTile(MS.Tile_Selected);
            if(b != null)
            {
                StopCoroutine(b);
            }
            a = StartCoroutine(Slot_Move_Coroutine(gameObject, UpPosition, MoveCheck, 0.7f));
            Check_WorkingButton(OnOff_Working, MS.Tile_Selected.GetComponent<TileData>().BuildingType, MS.Tile_Selected.GetComponent<TileData>().IsWorking_OnTile);
            Check_WorkingText(MS.Tile_Selected.GetComponent<TileData>().BuildingType, MS.Tile_Selected.GetComponent<TileData>().Needed_ManPower);
            Check_AbleToWork(UnableWork, UnableWork_Text);

            SidePanelOnOFF(MS.Tile_Selected.GetComponent<TileData>().BuildingType, MS.Tile_Selected.GetComponent<TileData>().BuildingVersion, true);
  
            //건물에 따라 팝업창 등장하고 사라지는거

            MoveCheck = true;
        }
        else if (MoveCheck == true)
        {
            if (a != null)
            {
                StopCoroutine(a);
            }
            b = StartCoroutine(Slot_Move_Coroutine(gameObject, DownPosition, MoveCheck, 0.7f));
            Product.Icon.SetActive(false);
            for(int i = 0; i < Cost.Count; i++)
            {
                Cost[i].Icon.SetActive(false);
            }
            Electricity.Icon.SetActive(false);
            Steam.Icon.SetActive(false);
            SidePanelOnOFF(MS.Tile_Selected.GetComponent<TileData>().BuildingType, MS.Tile_Selected.GetComponent<TileData>().BuildingVersion, false);


            MoveCheck = false;
        }
    }
}
