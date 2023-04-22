using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : UIMove
{
    /*
         * 빌딩 스크롤바와 연계하는 스크립트
         * 빌딩 버튼들은 비활성화 상태에서 하나의 리스트에 묶은 채로 관리
         * 빌딩이 연결되었을 때에만 리스트를 꺼냄
         * 빌딩 버튼에 마우스를 올리면 툴팁으로 간단한 설명, 필수 건물, 필요 자원, 유지비로 쓰이는 자원, 생산하는 자원이 표시됨
         * 마우스를 빼면 원상복구
         * 조건을 충족시키지 못하면 버튼은 짙은 회색으로 표시되며 툴팁에 부족한 조건을 빨간색으로 표시함
         * 빌딩 버튼을 클릭하면 건축모드로 진행, 이 상태가 되면 건축 조건에 맞는 타일을 찾아내어 약간 들어올린 채 흰색으로 표시함
         * 조건에 맞는 타일을 클릭하면 건축시작하며 건축모드에서 나온다. 조건에 맞지 않는 타일을 클릭하면 상단 메시지 창에 표시
         * 버튼을 다시 클릭하면 건축 모드에서 나온다. 다른 건축 버튼을 누르면 조건을 다시 설정 후 건축 모드 시작
    */
    public Vector2 DownPosition;
    public Vector2 UpPosition;
    public UISound uiSound;

    public TileManager TM;
    public Datamanager DM;
    public Vector3 OriginalPosition;
    public bool UpCheck;
    
    //담당하는 버튼들을 한 곳에 모아서 처리한다.
    public List<GameObject> ButtonList1 = new List<GameObject>();
    public List<GameObject> ButtonList2 = new List<GameObject>();
    public List<GameObject> ButtonList3 = new List<GameObject>();

   // public List<ButtonData> Category_Test = new List<ButtonData>();
    private bool isOpen;

    //건물 슬롯 1234에 할당
    public void Slot_Initialise1(List<GameObject> ButtonList, Datamanager DM, int Building_Type)
    {
        switch (Building_Type)
        {
            case 1:
                DM.TM.Check_On(DM.TM.On_Steel, 5);
                break;
            case 3:
                DM.TM.Check_NearBy_Resource(DM.TM.On_Ore);
                break;
            case 4:
            DM.TM.Check_On(DM.TM.On_Goldion, 1);
                break;
            case 9:
            DM.TM.Check_NearBy_Resource(DM.TM.On_Wood);
                break;
            default:
                break;
        }

        var a = DM.Bd[Building_Type-1];

        for (int i = 0; i< a.BD_Version.Count; i++)
        {
            var d = DM.Ub[Building_Type-1];
            if (d.Unlock_Version[i] == true)
            {
                var b = ButtonList[i].GetComponent<BuildingButtonData>();
                var c = a.BD_Version[i];
                b.BuildingType = c.Type;
                b.BuildingVersion = c.Version;
                b.Image = c.Image;
                b.Construction_Image = c.Construction_Image;
                b.Name = c.Name;
                b.Contents = c.Contents;
                b.ShortContents = c.ShortContents;
                b.Workable_OnTile = c.Workable_Ontile;
                b.Production_Type = c.Production_Type;
                b.Production_Value = c.Production_Value;
                b.Upkeep_Type = new List<int>(c.Upkeep_Type.ToArray());
                b.Upkeep_Value = new List<float>(c.Upkeep_Value.ToArray());
                b.BuildCost_Type = new List<int>(c.BuildCost_Type.ToArray());
                b.BuildCost_Value = new List<float>(c.BuildCost_Value.ToArray());
                b.Build_TurnCount = c.Build_TurnCount;
                b.Anti_Disaster = c.Anti_Disaster;
                b.Allow_Disaster = c.Allow_Disaster;
                b.Buff_Type = new List<int>(c.Buff_Type.ToArray());
                b.Buff_Value = new List<float>(c.Buff_Value.ToArray());
                b.Buff_Area_Size = c.Buff_Area_Size;
                b.Working_ManPower = c.Working_ManPower;
                b.Need_Steam = c.Need_Steam;
                b.Need_Electricity = c.Need_Electricity;
                b.Need_House = c.Need_House;
                b.Need_Public = c.Need_Public;
                b.Need_Idea = c.Need_Idea;
                b.BuildingSound = c.BuildingSound;
                b.Constructable = c.Constructable;
                b.Check_Constructable();
                b.check_OnlyOne();

                if (b.BuildingType != 0)
                {
                    ButtonList[i].SetActive(true);
                }
            }
        }
    }

    public void Slot_SetActive_False(List<GameObject> ButtonList)
    {
        for (int i = 0; i < 4; i++)
        {
            ButtonList[i].SetActive(false);
        }

    }


    public override void Slot_Move()
    {
        if (UpCheck == false)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, UpPosition, UpCheck, 1.2f));
            UpCheck = true;
            uiSound.PlaySound();
        }
        else if (UpCheck == true)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, DownPosition, UpCheck, 1.2f));
            UpCheck = false;
            uiSound.PlaySound();
        }
    }
    public  void Slot_Move2()
    {
        if (UpCheck == true)
        {
            StartCoroutine(Slot_Move_Coroutine(gameObject, DownPosition, UpCheck, 1.2f));
            UpCheck = false;
            uiSound.PlaySound();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OriginalPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }
}
