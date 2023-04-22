using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public List<EventDataScriptableObject> EVDSO = new List<EventDataScriptableObject>();

    public TileManager TM ;
    public Datamanager DM;

    public GameObject EventPanel;
    public Image Event_Sprite;
    public Text Event_Name;
    public Text Event_Contents;
    public GameObject ClickControll;

    public Text Notification_Text;
    public GameObject CloseButton;
    public Image Logo;

    public GameObject EventPanel2;
    public Text Event_Name2;
    public Text Event_Contents2;

    public Text Notification_Text2;
    public GameObject CloseButton2;
    public Image Logo2;


    public GameObject Choose;
    public List<GameObject> Choice_Button = new List<GameObject>();
    public List<Event_Quest_Notice_Message> Side_Slot = new List<Event_Quest_Notice_Message>();

    //활성화된 이벤트는 따로 클래스를 만들고 정보를 전달한 다음 리스트에 넣고 관리해야겠어

    public List<GameObject> Event_Slot = new List<GameObject>();
    public List<GameObject> Quest_Slot = new List<GameObject>();
    public List<GameObject> Notice_Slot = new List<GameObject>();

    public List<int> Event_Actived_Building = new List<int>();

    [SerializeField]
    public List<EventData> Active_Slot = new List<EventData>();
    [SerializeField]
    public List<EventData> Test_Slot = new List<EventData>();

    private EventData EV_Selected;

    public void Select_Event()
    {
        EventPanel.SetActive(true);
        ClickControll.SetActive(true);
        int a = Random.Range(0, 101);

        switch (TM.mapCondition)
        {
            case TileManager.MapCondition.red:
                if (a >= 50)
                {
                    //오렌지 이벤트
                    Change_Vaule(EV[2].EV_Type[Random.Range(0, EV[2].EV_Type.Count)]);
                }
                else if (a >= 40)
                {
                    //그레이 이벤트
                    Change_Vaule(EV[0].EV_Type[Random.Range(0, EV[0].EV_Type.Count)]);
                }
                else if (a >= 20)
                {
                    //다크 그레이 이벤트
                    Change_Vaule(EV[1].EV_Type[Random.Range(0, EV[1].EV_Type.Count)]);
                }
                else if (a >= 15)
                {
                    //레드 이벤트
                    Change_Vaule(EV[4].EV_Type[Random.Range(0, EV[4].EV_Type.Count)]);
                }
                else if (a >= 0)
                {
                    //그린 이벤트
                    Change_Vaule(EV[3].EV_Type[Random.Range(0, EV[3].EV_Type.Count)]);
                }
                break;

            case TileManager.MapCondition.orange:
                if (a >= 60)
                {
                    //오렌지 이벤트
                    Change_Vaule(EV[2].EV_Type[Random.Range(0, EV[2].EV_Type.Count)]);
                }
                else if (a >= 50)
                {
                    //그레이 이벤트
                    Change_Vaule(EV[0].EV_Type[Random.Range(0, EV[0].EV_Type.Count)]);
                }
                else if (a >= 30)
                {
                    //다크 그레이 이벤트
                    Change_Vaule(EV[1].EV_Type[Random.Range(0, EV[1].EV_Type.Count)]);
                }
                else if (a >= 25)
                {
                    //레드 이벤트
                    Change_Vaule(EV[4].EV_Type[Random.Range(0, EV[4].EV_Type.Count)]);
                }
                else if (a >= 0)
                {
                    //그린 이벤트
                    Change_Vaule(EV[3].EV_Type[Random.Range(0, EV[3].EV_Type.Count)]);
                }
                break;

            case TileManager.MapCondition.yellow:
                if (a >= 60)
                {
                    //오렌지 이벤트
                    Change_Vaule(EV[2].EV_Type[Random.Range(0, EV[2].EV_Type.Count)]);
                }
                else if (a >= 45)
                {
                    //그레이 이벤트
                    Change_Vaule(EV[0].EV_Type[Random.Range(0, EV[0].EV_Type.Count)]);
                }
                else if (a >= 25)
                {
                    //다크 그레이 이벤트
                    Change_Vaule(EV[1].EV_Type[Random.Range(0, EV[1].EV_Type.Count)]);
                }
                else if (a >= 20)
                {
                    //레드 이벤트
                    Change_Vaule(EV[4].EV_Type[Random.Range(0, EV[4].EV_Type.Count)]);
                }
                else if (a >= 0)
                {
                    //그린 이벤트
                    Change_Vaule(EV[3].EV_Type[Random.Range(0, EV[3].EV_Type.Count)]);
                }
                break;

            case TileManager.MapCondition.green:
                if (a >= 70)
                {
                    //오렌지 이벤트
                    Change_Vaule(EV[2].EV_Type[Random.Range(0, EV[2].EV_Type.Count)]);
                }
                else if (a >= 50)
                {
                    //그레이 이벤트
                    Change_Vaule(EV[0].EV_Type[Random.Range(0, EV[0].EV_Type.Count)]);
                }
                else if (a >= 40)
                {
                    //다크 그레이 이벤트
                    Change_Vaule(EV[1].EV_Type[Random.Range(0, EV[1].EV_Type.Count)]);
                }
                else if (a >= 35)
                {
                    //레드 이벤트
                    Change_Vaule(EV[4].EV_Type[Random.Range(0, EV[4].EV_Type.Count)]);
                }
                else if (a >= 0)
                {
                    //그린 이벤트
                    Change_Vaule(EV[3].EV_Type[Random.Range(0, EV[3].EV_Type.Count)]);
                }
                break;

            case TileManager.MapCondition.gold:
                if (a >= 80)
                {
                    //오렌지 이벤트
                    Change_Vaule(EV[2].EV_Type[Random.Range(0, EV[2].EV_Type.Count)]);
                }
                else if (a >= 60)
                {
                    //그레이 이벤트
                    Change_Vaule(EV[0].EV_Type[Random.Range(0, EV[0].EV_Type.Count)]);
                }
                else if (a >= 40)
                {
                    //다크 그레이 이벤트
                    Change_Vaule(EV[1].EV_Type[Random.Range(0, EV[1].EV_Type.Count)]);
                }
                else if (a >= 35)
                {
                    //레드 이벤트
                    Change_Vaule(EV[4].EV_Type[Random.Range(0, EV[4].EV_Type.Count)]);
                }
                else if (a >= 0)
                {
                    //그린 이벤트
                    Change_Vaule(EV[3].EV_Type[Random.Range(0, EV[3].EV_Type.Count)]);
                }
                break;
        }

    }

    //이벤트 활성화
    public void Change_Vaule(EventData EV)
    {
        int a = Random.Range(0, 6);
        //DeepCopy(EV_Selected, EV);
        EV_Selected = EV;

        Event_Sprite.sprite = EV.EventSprite;
        Event_Name.text = EV.EventName;
        Event_Contents.text = EV.EventContents;
        Logo.sprite = EV.Event_Logo_Sprite;

        //선택지 초기화
        for (int i = 0; i < Choice_Button.Count; i++)
        {
            Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = "";
            Choice_Button[i].SetActive(false);
        }
        Notification_Text.text = "";
        Notification_Text.gameObject.SetActive(false);
        CloseButton.SetActive(false);

        //선택지 나열
        switch (EV.event_Type)
        {
            case EventData.Event_Type.Map_Notice:
                CloseButton.SetActive(true);
                CloseButton.GetComponent<Button>().onClick.AddListener(Event_Active1);
                Notification_Text.text = EV.ValueChange_Text;
                Notification_Text.gameObject.SetActive(true);
                break;
            case EventData.Event_Type.Map_Choice:

                if (EV.choice.Count > 0)
                {
                    for (int i = 0; i < EV.choice.Count; i++)
                    {
                        Choice_Button[i].SetActive(true);
                        Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = EV.choice[i].ChoiceName;
                        Choice_Button[i].transform.GetChild(1).GetComponent<Text>().text = EV.choice[i].ChoiceContents;
                        switch (i)
                        {
                            case 0:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active2);
                                break;
                            case 1:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active3);
                                break;
                            case 2:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active4);
                                break;
                        }
                    }
                }
                break;

            case EventData.Event_Type.Building_Choice:

                if (EV.choice.Count > 0)
                {
                    for (int i = 0; i < EV.choice.Count; i++)
                    {
                        Choice_Button[i].SetActive(true);
                        Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = EV.choice[i].ChoiceName;
                        Choice_Button[i].transform.GetChild(1).GetComponent<Text>().text = EV.choice[i].ChoiceContents;
                        switch (i)
                        {
                            case 0:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active2);
                                break;
                            case 1:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active3);
                                break;
                            case 2:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active4);
                                break;
                        }
                    }
                }
                break;

            case EventData.Event_Type.Building_Notice:
                CloseButton.SetActive(true);
                CloseButton.GetComponent<Button>().onClick.AddListener(Event_Active1);
                Notification_Text.text = EV.ValueChange_Text;
                Notification_Text.gameObject.SetActive(true);
                break;

            case EventData.Event_Type.Resource_Choice:
                if (EV.choice.Count > 0)
                {
                    for (int i = 0; i < EV.choice.Count; i++)
                    {
                        Choice_Button[i].SetActive(true);
                        Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = EV.choice[i].ChoiceName;
                        Choice_Button[i].transform.GetChild(1).GetComponent<Text>().text = EV.choice[i].ChoiceContents;
                        switch (i)
                        {
                            case 0:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active2);
                                break;
                            case 1:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active3);
                                break;
                            case 2:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active4);
                                break;
                        }
                    }
                }
                break;

            case EventData.Event_Type.Resource_Notice:
                CloseButton.SetActive(true);
                CloseButton.GetComponent<Button>().onClick.AddListener(Event_Active1);
                Notification_Text.text = EV.ValueChange_Text;
                Notification_Text.gameObject.SetActive(true);
                break;

            case EventData.Event_Type.exploration_Choice:
                if (EV.choice.Count > 0)
                {
                    for (int i = 0; i < EV.choice.Count; i++)
                    {
                        Choice_Button[i].SetActive(true);
                        Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = EV.choice[i].ChoiceName;
                        Choice_Button[i].transform.GetChild(1).GetComponent<Text>().text = EV.choice[i].ChoiceContents;
                        switch (i)
                        {
                            case 0:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active2);
                                break;
                            case 1:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active3);
                                break;
                            case 2:
                                Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active4);
                                break;
                        }
                    }
                }
                break;

            case EventData.Event_Type.Exploration_Notice:
                CloseButton.SetActive(true);
                CloseButton.GetComponent<Button>().onClick.AddListener(Event_Active1);
                Notification_Text.text = EV.ValueChange_Text;
                Notification_Text.gameObject.SetActive(true);
                break;

            case EventData.Event_Type.MakeTile_Choice:
                for (int i = 0; i < EV.choice.Count; i++)
                {
                    Choice_Button[i].SetActive(true);
                    Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = EV.choice[i].ChoiceName;
                    Choice_Button[i].transform.GetChild(1).GetComponent<Text>().text = EV.choice[i].ChoiceContents;
                    switch (i)
                    {
                        case 0:
                            Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active2);
                            break;
                        case 1:
                            Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active3);
                            break;
                        case 2:
                            Choice_Button[i].GetComponent<Button>().onClick.AddListener(Event_Active4);
                            break;
                    }
                }
                break;
            case EventData.Event_Type.MakeTile_Notice:
                CloseButton.SetActive(true);
                CloseButton.GetComponent<Button>().onClick.AddListener(Event_Active1);
                Notification_Text.text = EV.ValueChange_Text;
                Notification_Text.gameObject.SetActive(true);

                break;
        }
    }

    public void DeepCopy(EventData EV, EventData CopyTo)
    {
        EV.Event_Logo_Sprite = CopyTo.Event_Logo_Sprite;
        EV.Event_Active_Sprite = CopyTo.Event_Active_Sprite;
        EV.event_Type = CopyTo.event_Type;
        EV.Active_Type = CopyTo.Active_Type;
        EV.Trigger_Type = CopyTo.Trigger_Type;
        EV.Active_Area = CopyTo.Active_Area;
        EV.Area_List_Number = CopyTo.Area_List_Number;
        EV.EventName = CopyTo.EventName;
        EV.EventContents = CopyTo.EventContents;
        EV.ValueChange_Text = CopyTo.ValueChange_Text;
        EV.Event_ID = CopyTo.Event_ID;
        EV.IsPercent = CopyTo.IsPercent;
        EV.IsProportion = CopyTo.IsProportion;
        EV.ReType = CopyTo.ReType;
        EV.ReValue = CopyTo.ReValue;
        EV.EventSprite = CopyTo.EventSprite;
        EV.Integer_RandomRange = CopyTo.Integer_RandomRange;
        EV.TypeNumber = CopyTo.TypeNumber;
        EV.Next_Series_Number = CopyTo.Next_Series_Number;
        EV.DestroyBuilding = CopyTo.DestroyBuilding;
        EV.Turn_Count = CopyTo.Turn_Count;
        EV.EventNumber_Goto = CopyTo.EventNumber_Goto;

        EV.choice = new List<EventData.Choice>(CopyTo.choice);
        if (CopyTo.choice.Count > 0)
        {
            for (int i = 0; i < CopyTo.choice.Count; i++)
            {
                EV.choice[i] = CopyTo.choice[i].Clone();
            }
        }
    }

    public void Event_Active1()
    {
        var a = new EventData();
        DeepCopy(a, EV_Selected);
        if (a.Turn_Count > 0)
        {
            Active_Slot.Add(a);
            Side_Slot[Active_Slot.Count -1].Connected_EventData = a;
            Side_Slot[Active_Slot.Count - 1].gameObject.SetActive(true);
        }
        Active_Event(a, 0);
        StartCoroutine(ClickControll_Off());
        CloseButton.GetComponent<Button>().onClick.RemoveListener(Event_Active1);
    }

    public void Event_Active2()
    {
        var a = new EventData();
        DeepCopy(a, EV_Selected);
        if (a.choice[0].Turn_Count > 0)
        {
            Active_Slot.Add(a);
            a.Choice_Number = 0;
            Side_Slot[Active_Slot.Count - 1].Connected_EventData = a;
            Side_Slot[Active_Slot.Count - 1].gameObject.SetActive(true);
        }
        Active_Event(a, 0);
        a.Choice_Number = 0;
        //Active_Event_Choice(EV_Selected, EV_Selected.choice[0]);
        StartCoroutine(ClickControll_Off());
        Choice_Button[0].GetComponent<Button>().onClick.RemoveListener(Event_Active2);
    }
    public void Event_Active3()
    {
        var a = new EventData();
        DeepCopy(a, EV_Selected);
        if (a.choice[1].Turn_Count > 0)
        {
            Active_Slot.Add(a);
            a.Choice_Number = 1;
            Side_Slot[Active_Slot.Count - 1].Connected_EventData = a;
            Side_Slot[Active_Slot.Count - 1].gameObject.SetActive(true);
        }
        Active_Event(a, 1);
        //Active_Event_Choice(EV_Selected, EV_Selected.choice[1]);
        a.Choice_Number = 1;
        StartCoroutine(ClickControll_Off());
        Choice_Button[1].GetComponent<Button>().onClick.RemoveListener(Event_Active3);
    }
    public void Event_Active4()
    {
        var a = new EventData();
        DeepCopy(a, EV_Selected);
        if (a.choice[2].Turn_Count > 0)
        {
            Active_Slot.Add(a);
            a.Choice_Number = 2;
            Side_Slot[Active_Slot.Count - 1].Connected_EventData = a;
            Side_Slot[Active_Slot.Count - 1].gameObject.SetActive(true);
        }
        Active_Event(a, 2);
        //Active_Event_Choice(EV_Selected, EV_Selected.choice[2]);
        a.Choice_Number = 2;
        StartCoroutine(ClickControll_Off());
        Choice_Button[2].GetComponent<Button>().onClick.RemoveListener(Event_Active4);
    }

    //이벤트 타입에 따라 발동, 선택형일 경우 다른 함수를 발동해야함
    public void Active_Event(EventData ev, int choiceNumber)
    {
        switch (ev.event_Type)
        {
            case EventData.Event_Type.Resource_Notice:
                switch (ev.Active_Type)
                {
                    case EventData.Event_Active_Type.None:
                        Value_Change_Resource_Integer(ev, false, false, 0);
                        break;
                    case EventData.Event_Active_Type.During_Countdown:
                        Value_Change_Resource_Integer(ev, false, false, 0);
                        break;
                }
                break;
            case EventData.Event_Type.Resource_Choice:
                switch (ev.Active_Type)
                {
                    case EventData.Event_Active_Type.None:
                        Value_Change_Resource_Integer(ev, false, true, choiceNumber);
                        break;
                    case EventData.Event_Active_Type.During_Countdown:
                        Value_Change_Resource_Integer(ev, false, true, choiceNumber);
                        break;
                }
                break;
            case EventData.Event_Type.Building_Notice:
                if (ev.Active_Area != EventData.Event_Active_Area.Building_All)
                {
                    switch (ev.Active_Area)
                    {
                        case EventData.Event_Active_Area.Building_Production:
                            Value_Change_Building_Production_Upkeep_Buildcost(ev, false, false, 0);
                            break;
                        case EventData.Event_Active_Area.Building_Upkeep:
                            Value_Change_Building_Production_Upkeep_Buildcost(ev, false, false, 0);
                            break;
                        case EventData.Event_Active_Area.Building_BuildCost_:
                            Value_Change_Building_Production_Upkeep_Buildcost(ev, false, false, 0);
                            break;
                        case EventData.Event_Active_Area.Building_All:
                            Value_Change_Building_Destroy(ev, false, false, 0);
                            break;
                    }
                }
                break;

            case EventData.Event_Type.Building_Choice:
                    switch (ev.choice[choiceNumber].Active_Area)
                    {
                        case EventData.Choice.Event_Active_Area.Building_Production:
                            Value_Change_Building_Production_Upkeep_Buildcost(ev, false, true, choiceNumber);
                            break;
                        case EventData.Choice.Event_Active_Area.Building_Upkeep:
                            Value_Change_Building_Production_Upkeep_Buildcost(ev, false, true, choiceNumber);
                            break;
                        case EventData.Choice.Event_Active_Area.Building_BuildCost_:
                            Value_Change_Building_Production_Upkeep_Buildcost(ev, false, true, choiceNumber);
                            break;
                        case EventData.Choice.Event_Active_Area.Building_All:
                            Value_Change_Building_Destroy(ev, false, true, choiceNumber);
                            break;
                    }
                break;

            case EventData.Event_Type.Map_Notice:
                switch (ev.Active_Area)
                {
                    case EventData.Event_Active_Area.Map:
                        Value_Change_Map(ev, false, false, 0);
                        break;
                }
                break;
        }
    }
    //통보형 이벤트의 정수값 변화, 정상 작동 확인, 후에 인구대비 조절값을 맥여야겠음. 아래 것과 합칠 방법 논의
    public void Value_Change_Resource_Integer(EventData ev, bool isReturn, bool isChoice, int choiceNumber)
    {
        List<int> ReType_Array = ev.ReType;
        List<float> ReValue_Array = ev.ReValue;

        if (isChoice == true)
        {
            ReType_Array = ev.choice[choiceNumber].ReType;
            ReValue_Array = ev.choice[choiceNumber].ReValue;
        }

        if (isReturn == false)
        {
            for (int i = 0; i < ReType_Array.Count; i++)
            {

                if (ReType_Array[i] == 43)
                {
                }

                if (ReType_Array[i] != -1)
                {
                    DM.All_Resource_List[ReType_Array[i]] += (int)ReValue_Array[i];
                }

                if (ReType_Array[i] == 43)
                {
                }

                if (ReType_Array[i] == 11 ||ReType_Array[i] == 12 || ReType_Array[i] == 13 ||
                   ReType_Array[i] == 33 || ReType_Array[i] == 34 || ReType_Array[i] == 35)
                {
                    DM.Check_Mind();
                }
            }
        }
        if (isReturn == true)
        {
            for (int i = 0; i < ReType_Array.Count; i++)
            {
                if (ReType_Array[i] == 43)
                {
                }

                if (ReType_Array[i] != -1)
                {
                    DM.All_Resource_List[ReType_Array[i]] -= (int)ReValue_Array[i];
                }

                if (ReType_Array[i] == 43)
                {
                    //DM.Check_Consume_Food(true);
                }
            }
        }
    }

    //배율로 고치는 것, 이것은 차후 이벤트 데이터에 배율로 할 목록을 추가하자. int랑 enum으로 지정하는거야.
    public void Value_Change_Resource_Percent(EventData ev)
    {
        List<int> ReType_Array = ev.ReType;
        List<float> ReValue_Array = ev.ReValue;

        for (int i = 0; i < ReType_Array.Count; i++)
        {
            if (ReType_Array[i] != -1)
            {
                DM.All_Resource_List[ReType_Array[i]] += (int)ReValue_Array[i];
            }
        }
    }

    public void Value_Change_Resource_Percent_Choice(EventData.Choice ev)
    {
        List<int> ReType_Array = ev.ReType;
        List<float> ReValue_Array = ev.ReValue;

        for (int i = 0; i < ReType_Array.Count; i++)
        {
            if (ReType_Array[i] != -1)
            {
                DM.All_Resource_List[ReType_Array[i]] += (int)ReValue_Array[i];
            }
        }
    }

    //건물의 생산, 소모, 건설비용의 이벤트 배율을 고친다. 이거 자체가 발동이 안돼. 위에 발동식을 뒤져봐야겠어
    public void Value_Change_Building_Production_Upkeep_Buildcost(EventData ev, bool isReturn, bool isChoice, int choiceNumber)
    {
        /*
        Debug.Log("벨류체인지_빌딩_프로덕션_업킵_빌드코스트 발동");
        int x = ev.ReType1;
        float y = ev.ReValue1;
        int z = (int)ev.Active_Area;
        int b = ev.TypeNumber;
        List<TileData> c = new List<TileData>();

        if (isChoice == true)
        {
            x = ev.choice[choiceNumber].ReType1;
            y = ev.choice[choiceNumber].ReValue1;
            z = (int)ev.choice[choiceNumber].Active_Area;
            b = ev.choice[choiceNumber].TypeNumber;
            Debug.Log(z);
        }
        
        switch (z)
        {
            case 5:
                Debug.Log("체크 업킵 작동");
                //활성화된 건물만 따로 모아둔 다음 계산한다.
                for (int i = 0; i < TM.TypeAll_Building_List[x].Type_Building.Count; i++)
                {
                    for (int a = 0; a < )
                    {

                    }
                    //작업 중인 타일을 잠깐 꺼놓고 수치만 고친다
                    if (TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == true)
                    {
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = false;
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().Change_Score();
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = false;
                        c.Add(TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>());
                        Debug.Log(c.Count);
                    }
                    else
                    {
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = true;
                    }
                }

                if (isReturn == false)
                {
                    //DM.Event_Upkeep_Magnification_List[x] += y;
                }
                else if (isReturn == true)
                {
                    //DM.Event_Upkeep_Magnification_List[x] -= y;
                }

                for (int i = 0; i < TM.TypeAll_Building_List[b].Type_Building.Count; i++)
                {
                    if (TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == false)
                    {
                        Debug.Log("리턴 발동");
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = true;
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().Change_Score();
                    }
                    else if (TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == true)
                    {
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = false;
                    }
                }
                break;

            case 4:
                Debug.Log("체크 프로덕트 작동");
                for (int i = 0; i < TM.TypeAll_Building_List[b].Type_Building.Count; i++)
                {
                    if (TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == true)
                    {
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = false;
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().Change_Score();
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = false;
                        c.Add(TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>());
                        Debug.Log(c.Count);
                    }
                    else if (TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == false)
                    {
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = true;
                    }
                }

                if (isReturn == false)
                {
                    //DM.Event_Production_Magnification_List[x] += y;
                }
                else if (isReturn == true)
                {
                    //DM.Event_Production_Magnification_List[x] -= y;
                }

                for (int i = 0; i < TM.TypeAll_Building_List[b].Type_Building.Count; i++)
                {
                    //if(c.Contains(TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>()))
                    if (TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == false)
                    {
                        Debug.Log("리턴 발동");
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = true;
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().Change_Score();
                    }
                    else if(TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile == true)
                    {
                        TM.TypeAll_Building_List[b].Type_Building[i].GetComponent<TileData>().IsWorking_OnTile = false;
                    }
                }

                break;

            case 6:
                Debug.Log("체크 빌드코스트 작동");
                if (isReturn == false)
                {
                    //DM.Event_BuildingCost_Magnification_List[x] += y;
                }
                else if (isReturn == true)
                {
                    //DM.Event_BuildingCost_Magnification_List[x] -= y;
                }
                break;
            default:
                Debug.Log("에잉 실패다");
                break;
        }*/
    }

    //건물을 파괴할 지 정지할 지를 정한다. 리소스타입1로 체크할 건물을 선택, 리소스 벨류2로 타일 번호 확인
    public void Value_Change_Building_Destroy(EventData ev, bool isReturn, bool isChoice, int choiceNumber)
    {
        /*
        bool x = ev.DestroyBuilding;
        int y = ev.ReType1;
        bool z = ev.DestroyBuilding;
        float b = ev.ReValue2;

        if (isChoice == true)
        {
            x = ev.choice[choiceNumber].DestroyBuilding;
            y = ev.choice[choiceNumber].ReType1;
            z = ev.choice[choiceNumber].DestroyBuilding;
            //b = ev.choice[choiceNumber].ReValue2;
        }

        Debug.Log(y);

        if (x == false)
        {
            var a = Random.Range(0, TM.TypeAll_Building_List[y].Type_Building.Count);
            Debug.Log(a);
            if (isReturn == true)
            {
                for (int i = 0; i < TM.OnEnableTileList.Count; i++)
                {
                    if (TM.OnEnableTileList[i].GetComponent<TileData>().Tile_Number == b)
                    {
                        TM.OnEnableTileList[i].GetComponent<TileData>().Event_Acidente = false;
                    }
                }
            }
            else if (isReturn == false)
            {
                TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>().Event_Acidente = true;
                TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>().Check_Acident();
            }
            //이벤트 걸린 타일의 번호를 찾아온다
            Event_Actived_Building.Add(TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>().Tile_Number);
            ev.ReValue2 = TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>().Tile_Number;
        }
        else if (x == true)
        {
            var a = Random.Range(0, TM.TypeAll_Building_List[y].Type_Building.Count);
            TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>().Event_Acidente = true;
            TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>().Check_Acident();
            TM.Destroy_Tile_Building(TM.TypeAll_Building_List[y].Type_Building[a].GetComponent<TileData>());
        }
        */
    }

    public void Value_Change_Map(EventData ev, bool isReturn, bool isChoice, int choiceNumber)
    {
        /*
        if (isReturn == false)
        {
            for (int i = 0; i < TM.OnEnableTileList.Count; i++)
            {
                TM.OnEnableTileList[i].GetComponent<TileData>().Event_Desaster -= (int)ev.ReValue3;
                TM.OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
            }
        }
        else if (isReturn == true)
        {
            for (int i = 0; i < TM.OnEnableTileList.Count; i++)
            {
                TM.OnEnableTileList[i].GetComponent<TileData>().Event_Desaster += (int)ev.ReValue3;
                TM.OnEnableTileList[i].GetComponent<TileData>().Check_Disaster();
            }
        }
        */
    }

    //통보형 이벤트의 범위를 체크, 가장 선행되어야함
    public void Value_Change_Building(EventData ev, bool isReturn, int choiceNumber)
    {
        switch (ev.event_Type)
        {
            case EventData.Event_Type.Building_Notice:
                if (ev.Active_Area == EventData.Event_Active_Area.Building_Upkeep ||
                   ev.Active_Area == EventData.Event_Active_Area.Building_Production ||
                   ev.Active_Area == EventData.Event_Active_Area.Building_BuildCost_)
                {
                    Value_Change_Building_Production_Upkeep_Buildcost(ev, isReturn, false, choiceNumber);
                }
                else Value_Change_Building_Destroy(ev, isReturn, false, choiceNumber);
                break;

            case EventData.Event_Type.Building_Choice:
                if (ev.Active_Area == EventData.Event_Active_Area.Building_Upkeep ||
                   ev.Active_Area == EventData.Event_Active_Area.Building_Production ||
                   ev.Active_Area == EventData.Event_Active_Area.Building_BuildCost_)
                {
                    Value_Change_Building_Production_Upkeep_Buildcost(ev, isReturn, true, choiceNumber);
                }
                else Value_Change_Building_Destroy(ev, isReturn, true, choiceNumber);
                break;
        }
    }

    //트리거 타입을 체크한다. 당장은 시리즈물에 쓰일 예정, 이벤트데이터에 적힌 Next_Series_Number를 끌어올 예정
    public void Check_Trigger_Type(EventData ev)
    {
        switch (ev.Trigger_Type)
        {
            case EventData.Event_Trigger_Type.Single:

                break;

            case EventData.Event_Trigger_Type.Single_Story:

                break;

            case EventData.Event_Trigger_Type.Series:

                break;

            case EventData.Event_Trigger_Type.Series_Story:

                break;
        }
    }

    //지속형 이벤트일 경우 사이드 슬롯에 등재한다. 이미지와 텍스트 등만 등재예정
    public void Side_Slot_Record_Event(EventData ev)
    {

    }

    //DM에서 연계
    public void Event_Turn_Count()
    {
        for (int i = 0; i < Active_Slot.Count; i++)
        {
            //통보형일 때 발동
            if (Active_Slot[i].event_Type != EventData.Event_Type.Building_Choice &&
               Active_Slot[i].event_Type != EventData.Event_Type.Resource_Choice &&
               Active_Slot[i].event_Type != EventData.Event_Type.Map_Choice)
            {
                Active_Slot[i].Turn_Count -= 1;

                Side_Slot[i].Time.text = (int.Parse(Side_Slot[i].Time.text) - 1).ToString();
                StartCoroutine(Side_Slot[i].Refresh_Event());

                if (Active_Slot[i].Active_Type == EventData.Event_Active_Type.Each_Turn)
                {
                    switch (Active_Slot[i].Active_Area)
                    {
                        case EventData.Event_Active_Area.Building_All:
                            Value_Change_Building_Destroy(Active_Slot[i], false, false, Active_Slot[i].Choice_Number);
                            break;

                        case EventData.Event_Active_Area.Building_BuildCost_:
                            Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, false, 0);
                            break;

                        case EventData.Event_Active_Area.Building_Production:
                            Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, false, 0);
                            break;

                        case EventData.Event_Active_Area.Building_Upkeep:
                            Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, false, 0);
                            break;
                        case EventData.Event_Active_Area.Data:
                            //차후 배율이 나오면 교체
                            Value_Change_Resource_Integer(Active_Slot[i], false, false, 0);
                            break;
                        case EventData.Event_Active_Area.Map:
                            Value_Change_Map(Active_Slot[i], false, false, 0);
                            break;
                    }
                }

                //카운트가 0일 경우 +액티브 타입에 따라 효과 발동 후 삭제 실행
                if (Active_Slot[i].Turn_Count < 1)
                {
                    switch (Active_Slot[i].Active_Type)
                    {
                        case EventData.Event_Active_Type.Each_Turn:
                            Side_Slot[i].gameObject.SetActive(false);
                            Active_Slot.Remove(Active_Slot[i]);
                            break;

                        case EventData.Event_Active_Type.End_Countdown:
                            switch (Active_Slot[i].Active_Area)
                            {
                                case EventData.Event_Active_Area.Building_All:
                                    Value_Change_Building_Destroy(Active_Slot[i], false, false, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Event_Active_Area.Building_BuildCost_:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, false, 0);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Event_Active_Area.Building_Production:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, false, 0);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Event_Active_Area.Building_Upkeep:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, false, 0);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Event_Active_Area.Data:
                                    //차후 배율이 나오면 교체
                                    Value_Change_Resource_Integer(Active_Slot[i], false, false, 0);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;
                                case EventData.Event_Active_Area.Map:
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    Value_Change_Map(Active_Slot[i], false, false, 0);
                                    break;
                            }
                            break;
                        case EventData.Event_Active_Type.During_Countdown:
                            switch (Active_Slot[i].Active_Area)
                            {
                                case EventData.Event_Active_Area.Building_All:
                                    Value_Change_Building_Destroy(Active_Slot[i], true, false, 0);
                                    break;

                                case EventData.Event_Active_Area.Building_BuildCost_:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], true, false, 00);
                                    break;

                                case EventData.Event_Active_Area.Building_Production:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], true, false, 0);
                                    break;

                                case EventData.Event_Active_Area.Building_Upkeep:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], true, false, 0);
                                    break;

                                case EventData.Event_Active_Area.Data:
                                    //차후 배율이 나오면 교체
                                    Value_Change_Resource_Integer(Active_Slot[i], true, false, 0);
                                    break;

                                case EventData.Event_Active_Area.Map:
                                    Value_Change_Map(Active_Slot[i], true, false, 0);
                                    break;
                            }
                            Side_Slot[i].gameObject.SetActive(false);
                            Active_Slot.Remove(Active_Slot[i]);
                            break;

                        case EventData.Event_Active_Type.Active_End:
                            //이건 퀘스트 만들면서
                            break;

                        default:
                            Side_Slot[i].gameObject.SetActive(false);
                            Active_Slot.Remove(Active_Slot[i]);
                            break;
                    }
                }
            }
            //선택형일 때 발동
            else if (Active_Slot[i].event_Type != EventData.Event_Type.Building_Notice &&
                    Active_Slot[i].event_Type != EventData.Event_Type.Resource_Notice &&
                    Active_Slot[i].event_Type != EventData.Event_Type.Map_Notice)
                 {
                 Active_Slot[i].choice[Active_Slot[i].Choice_Number].Turn_Count -= 1;

                 Side_Slot[i].Time.text = (int.Parse(Side_Slot[i].Time.text) - 1).ToString();
                 StartCoroutine(Side_Slot[i].Refresh_Event());

                 if (Active_Slot[i].choice[Active_Slot[i].Choice_Number].Active_Type == EventData.Choice.Event_Active_Type.Each_Turn)
                 {
                    switch (Active_Slot[i].choice[Active_Slot[i].Choice_Number].Active_Area)
                    {
                        case EventData.Choice.Event_Active_Area.Building_All:
                            Value_Change_Building_Destroy(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                            break;

                        case EventData.Choice.Event_Active_Area.Building_BuildCost_:
                            Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                            break;

                        case EventData.Choice.Event_Active_Area.Building_Production:
                            Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                            break;

                        case EventData.Choice.Event_Active_Area.Building_Upkeep:
                            Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                            break;
                        case EventData.Choice.Event_Active_Area.Data:
                            //차후 배율이 나오면 교체
                            Value_Change_Resource_Integer(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                            break;

                        case EventData.Choice.Event_Active_Area.Map:
                            Value_Change_Map(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                            break;
                    }
                 }


                //카운트가 0일 경우 액티브 타입에 따라 효과 발동 후 삭제 실행
                 if (Active_Slot[i].choice[Active_Slot[i].Choice_Number].Turn_Count < 1)
                 {
                    switch (Active_Slot[i].choice[Active_Slot[i].Choice_Number].Active_Type)
                    {
                        case EventData.Choice.Event_Active_Type.Each_Turn:
                            Side_Slot[i].gameObject.SetActive(false);
                            Active_Slot.Remove(Active_Slot[i]);
                            break;

                        case EventData.Choice.Event_Active_Type.End_Countdown:
                            switch (Active_Slot[i].choice[Active_Slot[i].Choice_Number].Active_Area)
                            {
                                case EventData.Choice.Event_Active_Area.Building_All:
                                    Value_Change_Building_Destroy(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Building_BuildCost_:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Building_Production:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Building_Upkeep:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Data:
                                    //차후 배율이 나오면 교체
                                    Value_Change_Resource_Integer(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Map:
                                    Value_Change_Map(Active_Slot[i], false, true, Active_Slot[i].Choice_Number);
                                    break;
                            }
                            Side_Slot[i].gameObject.SetActive(false);
                            Active_Slot.Remove(Active_Slot[i]);
                            break;

                        case EventData.Choice.Event_Active_Type.During_Countdown:
                            switch (Active_Slot[i].choice[Active_Slot[i].Choice_Number].Active_Area)
                            {
                                case EventData.Choice.Event_Active_Area.Building_All:
                                    Value_Change_Building_Destroy(Active_Slot[i], true, true, Active_Slot[i].Choice_Number);
                                    Debug.Log("리턴 발동");
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Building_BuildCost_:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], true, true, Active_Slot[i].Choice_Number);
                                    Debug.Log("리턴 발동");
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Building_Production:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], true, true, Active_Slot[i].Choice_Number);
                                    Debug.Log("리턴 발동");
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Building_Upkeep:
                                    Value_Change_Building_Production_Upkeep_Buildcost(Active_Slot[i], true, true, Active_Slot[i].Choice_Number);
                                    Debug.Log("리턴 발동");
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Data:
                                    //차후 배율이 나오면 교체
                                    Value_Change_Resource_Integer(Active_Slot[i], true, true, Active_Slot[i].Choice_Number);
                                    Side_Slot[i].gameObject.SetActive(false);
                                    Active_Slot.Remove(Active_Slot[i]);
                                    break;

                                case EventData.Choice.Event_Active_Area.Map:
                                    Value_Change_Map(Active_Slot[i], true, true, Active_Slot[i].Choice_Number);
                                    break;
                            }
                            break;

                        case EventData.Choice.Event_Active_Type.Active_End:
                            //이건 퀘스트 만들면서
                            break;

                        default:
                            Side_Slot[i].gameObject.SetActive(false);
                            Active_Slot.Remove(Active_Slot[i]);
                            break;
                    }
                 }
            }
        }
    }

    public void AddEvent(EventDataScriptableObject a)
    {
        //EventData b = a.even
        //Test_Slot.Add
    }


    [System.Serializable]
    public class EventData_ListWrapper
    {
        public List<EventData> EV_Type;
    }
    public List<EventData_ListWrapper> EV = new List<EventData_ListWrapper>();

    public IEnumerator ClickControll_Off()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        ClickControll.gameObject.SetActive(false);
    }

    public void Awake()
    {

        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
        a.GetComponent<Datamanager>().EM = this;
    }
}
[System.Serializable]
public class EventData
{
    [System.Serializable]
    public enum Event_Type
    {
        Resource_Notice,
        Resource_Choice,
        Building_Notice,
        Building_Choice,
        Exploration_Notice,
        exploration_Choice,
        Map_Notice,
        Map_Choice,
        MakeTile_Notice,
        MakeTile_Choice
    }
    public Event_Type event_Type;

    [System.Serializable]
    public enum Event_Active_Type
    {
        None,
        Each_Turn,
        End_Countdown,
        During_Countdown,
        Active_End
    }
    public Event_Active_Type Active_Type;

    [System.Serializable]
    public enum Event_Trigger_Type
    {
        Single,
        Series,
        Single_Story,
        Series_Story
    }
    public Event_Trigger_Type Trigger_Type;

    //타일 적용일 시 범위를 설정
    public enum Event_Active_Area
    {
        Map,
        Resource_All,
        Resource,
        Building_All,
        Building_Production,
        Building_Upkeep,
        Building_BuildCost_,
        Data
    }
    public Event_Active_Area Active_Area;

    //적용 범위의 리스트 넘버를 찾음
    public int Area_List_Number;

    public Sprite Event_Logo_Sprite;
    public Sprite Event_Active_Sprite;
    public string EventName;
    public string EventContents;
    public string ValueChange_Text;
    public int Event_ID;

    public bool IsPercent;
    public bool IsProportion;

    public List<int> ReType = new List<int>();
    public List<float> ReValue = new List<float>();

    public Sprite EventSprite;

    //정수형 이벤트일 경우의 랜덤값
    public int Integer_RandomRange;
    //배율 이벤트일 경우 배율 값
    public int TypeNumber;
    public int Next_Series_Number;

    public bool DestroyBuilding;

    public int Turn_Count;
    public int EventNumber_Goto;
    //선택지를 골랐을 경우 번호를 따짐
    public int Choice_Number;

    [System.Serializable]
    public class Choice //: ICloneable
    {
        [System.Serializable]
        public enum Event_Active_Type
        {
            None,
            Each_Turn,
            End_Countdown,
            During_Countdown,
            Active_End
        }
        public Event_Active_Type Active_Type;

        [System.Serializable]
        public enum Event_Trigger_Type
        {
            Single,
            Series,
            Single_Story,
            Series_Story
        }
        public Event_Trigger_Type Trigger_Type;

        //타일 적용일 시 범위를 설정
        public enum Event_Active_Area
        {
            Map,
            Resource_All,
            Resource,
            Building_All,
            Building_Production,
            Building_Upkeep,
            Building_BuildCost_,
            Data
        }
        public Event_Active_Area Active_Area;

        public string ChoiceName;
        public string ChoiceContents;

        public bool isPercent;
        public List<int> ReType = new List<int>();
        public List<float> ReValue = new List<float>();
        public Sprite Event_Active_Sprite;
        public int TypeNumber;

        public bool DestroyBuilding;

        public int Turn_Count;

        public int ExplorerNumber;

        public int Next_Series_Number;

        public int EventNumber_Goto;

        public int EndingPoint;

        public Choice Clone()
        {
            Choice newCopy = new Choice();
            newCopy.Active_Type = this.Active_Type;
            newCopy.Active_Area = this.Active_Area;
            newCopy.Trigger_Type = this.Trigger_Type;
            newCopy.ChoiceName = this.ChoiceName;
            newCopy.ChoiceContents = this.ChoiceContents;
            newCopy.isPercent = this.isPercent;
            newCopy.ReType = this.ReType;
            newCopy.ReValue = this.ReValue;
            newCopy.DestroyBuilding = this.DestroyBuilding;
            newCopy.Turn_Count = this.Turn_Count;
            newCopy.ExplorerNumber = this.ExplorerNumber;
            newCopy.Next_Series_Number = this.Next_Series_Number;
            newCopy.EndingPoint = this.EndingPoint;
            newCopy.EventNumber_Goto = this.EventNumber_Goto;
            newCopy.TypeNumber = this.TypeNumber;

            return newCopy;
        }
    }

    public List<Choice> choice = new List<Choice>();
}
