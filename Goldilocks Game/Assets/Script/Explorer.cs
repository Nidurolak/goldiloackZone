using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using DG.Tweening;

[Serializable]
public class Explorer : UIMove
{
    public int Number;

    private string Notifi;

    private Sequence sequence;
    private bool Upcheck;
    private int StartManpower;

    public Text UnableText;
    public Button ReadyButton;

    public UnityEvent ExplorerAct;
    public Datamanager DM;
    public ExplorerManager EM;
    public Image Head;
    public Text Message_Text;
    //public int Explorer_Number;
    public float DeadTimer;
    public int FoundStack;
    public int FoundStack_Max;
    public int ReturnStack;
    public int ReturnStack_Max;
    public int Supply;
    public List<GameObject> Resource_Icon = new List<GameObject>();
    //public List<int> Resource_Type = new List<int>();
    public List<Image> Resource_Image = new List<Image>();
    public List<Text> Resource_Text = new List<Text>();
    public List<Message> MessageList = new List<Message>();
    public List<ImportantFound> FoundList = new List<ImportantFound>();
    public GameObject ReadyPannel;
    
    //작동 중인 중요발견은 따로 찾아둔다.
    public ImportantFound impo;
    public List<int> ReType = new List<int>();
    public List<float> ReValue = new List<float>();

    public GameObject TocheControll;

    public GameObject ReturnButton;
    public Text ReturnButton_Text;

    public GameObject EventPanel;
    public Image Event_Sprite;
    public Text Event_Name;
    public Text Event_Contents;
    public GameObject ClickControll;

    public Text Notification_Text;
    public GameObject CloseButton;
    public Text CloseText;
    public Image Logo;

    public GameObject Choose;
    public List<GameObject> Choice_Button = new List<GameObject>();
    public List<Event_Quest_Notice_Message> Side_Slot = new List<Event_Quest_Notice_Message>();

    public EventPanel2 EventPanel2;
    public Text Event_Name2;
    public Text Event_Contents2;

    public Text Notification_Text2;
    public GameObject CloseButton2;
    public Image Logo2;

    [Serializable]
    public enum NormalArea { None, Forest, Filed, SmallCity, BigCity, Mountain };
    public NormalArea normalArea;

    [Serializable]
    public enum EventArea { Idle, scenario1, scenario2, scenario3, scenario4, scenario5 };
    public EventArea eventArea;

    public override void Slot_Move()
    {

    }

    //0~11번 사이의 랜덤 탐사지역을 추가한다.
    public void AddListOfRandomEvent()
    {
        //현재는 들판, 숲, 협곡, 산맥만 추가
        int i = Random.Range(0, 2);
        for (int z = 0; z < EM.MessageListList[i].MessageList.Count; z++)
        {
            MessageList.Add(
                new Message(
                    (int)EM.MessageListList[i].MessageList[z].normalArea,
                    (int)EM.MessageListList[i].MessageList[z].eventArea,
                    (int)EM.MessageListList[i].MessageList[z].affair,
                    EM.MessageListList[i].MessageList[z].Report,
                    EM.MessageListList[i].MessageList[z].ReType1,
                    EM.MessageListList[i].MessageList[z].ReValue1,
                    EM.MessageListList[i].MessageList[z].ReType2,
                    EM.MessageListList[i].MessageList[z].ReValue2
                    )
                );
           // Debug.Log(EM.MessageListList[i].MessageList[z].Report);
        }
        for(int y = 0; y < EM.ImportantFoundListList[i].ImportantFoundList.Count; y++)
        {
            FoundList.Add(EM.ImportantFoundListList[i].ImportantFoundList[y]);
        }
    }

    public void AddListOfRandomImportantFound()
    {




    }

    //구역 발견시 추가, 데이터 매니저에 추가
    public void AddZone()
    {

    }

    public void AddInventory(int reType, float reValue)
    {
        if (reType != -1)
        {
            var i = ReType.IndexOf(reType);
        
            //인벤토리에 이미 있다면
            if (ReType.Contains(reType))
            {
                ReValue[i] += reValue;
                Resource_Text[i].text = ReValue[i].ToString();
            }
            //인벤에 없다면
            else
            {
                ReType.Add(reType);
                ReValue.Add(reValue);
                Resource_Icon[ReType.IndexOf(reType)].SetActive(true);
               // Resource_Image[ReType.IndexOf(reType)].sprite = DM.Resource_Sprites[reType];
                Resource_Text[ReType.IndexOf(reType)].text = reValue.ToString();
            }
        }
    }

    public void FoundStack_Up()
    {
        FoundStack += 1;
    }

    public void ChangeExplorerReport_Random()
    {
        FoundStack += 1;

        if (DeadTimer > 0)
        {
            Message_Text.text = "탐사 기지와의 연락이 끊긴 탐사대는 오래 버틸 수 없습니다! " + DeadTimer + "턴 내로 탐사 기지를 다시 가동하지 않으면 이 탐사대는 전멸합니다.";
            ReturnButton.GetComponent<Button>().interactable = false;
        }
        else if (FoundStack > FoundStack_Max)
        {
            ReturnButton.GetComponent<Button>().onClick.RemoveListener(ReturnToCity);
            DM.TurnEnd_Explorer.RemoveListener(ChangeExplorerReport_Random);

            Message_Text.text = "탐사대가 통신을 요청해 왔습니다. 무언가 중요한 것을 발견한 것이 틀림없습니다. 통신을 연결하기 전까지 그들은 기다릴 것입니다.";
            ReturnButton.GetComponent<Button>().onClick.AddListener(StartRandomImpo);
            ReturnButton_Text.text = "연결하기";
        }
        else
        {
            int i = Random.Range(0, MessageList.Count);
            Message_Text.text = MessageList[i].Report;
            switch(MessageList[i].affair)
            {
                case global::Message.Affair.gain :
                    AddInventory(MessageList[i].ReType1, MessageList[i].ReValue1);
                    AddInventory(MessageList[i].ReType2, MessageList[i].ReValue2);
                    break;

                case global::Message.Affair.FindZone:
                break;

                case global::Message.Affair.importantfind:
                break;
            }
}
    }

    public void StartRandomImpo()
    {
        ReturnButton.GetComponent<Button>().onClick.RemoveListener(StartRandomImpo);
        int i = Random.Range(0, FoundList.Count);
        impo = FoundList[i];
        ImportantFound_Popup(FoundList[i]);
        FoundStack = 0;
        FoundStack_Max = impo.FoundStack_Max;

        ReturnButton.GetComponent<Button>().onClick.AddListener(ReturnToCity);
        DM.TurnEnd_Explorer.AddListener(ChangeExplorerReport_Random);
    }

    //중요한 발견도 따로 매니저로 추가해야한다. 이벤트 매니저를 통해 이벤트 버튼도 애드 리스너로 관리하도록 해야할 것 같아.
    //여기서는 중요발견 팝업을 띄운다. 랜덤인지 스토리인지는 탐사대 성격에 따르자.
    public void ImportantFound_Popup(ImportantFound impo)
    {
        //DM.TurnEnd_Explorer.AddListener(ChangeExplorerReport_Random);
        ReturnButton_Text.text = "귀환";
        Message_Text.text = "이 탐사대는 중요한 발견을 보고한 뒤 잠깐 휴식하고 있습니다. 다음 턴에 곧장 탐사를 재개할 것입니다.";

        for (int z = 0; z < Choice_Button.Count; z++)
        {
            Choice_Button[z].transform.GetChild(0).GetComponent<Text>().text = "";
            Choice_Button[z].SetActive(false);
        }
        Notification_Text.text = "";
        Notification_Text.gameObject.SetActive(false);
        CloseButton.SetActive(false);

        //Json파일 하나 만들어서 받아놓은 다음 막 이것저것 건드려보자//여기서는 아냐. 매니저에서 할 일이야.
        EventPanel.SetActive(true);
        ClickControll.SetActive(true);

        //다지선다형 발견일 경우
        if(impo.event_Type == ImportantFound.Event_Type.Exploration_Choice)
        {
            for(int i = 0; i < impo.choice.Count; i++)
            {
                //여기서 뭔가 에러가 났어. 이벤트매니저의 원본과 비교해보자
                Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = "";
                Choice_Button[i].SetActive(true);
                Choice_Button[i].transform.GetChild(0).GetComponent<Text>().text = impo.choice[i].ChoiceName;
                Choice_Button[i].transform.GetChild(1).GetComponent<Text>().text = impo.choice[i].ChoiceContents;
                switch (i)
                {
                    case 0:
                        Choice_Button[i].GetComponent<Button>().onClick.AddListener(Button2);
                        break;
                    case 1:
                        Choice_Button[i].GetComponent<Button>().onClick.AddListener(Button3);
                        break;
                    case 2:
                        Choice_Button[i].GetComponent<Button>().onClick.AddListener(Button4);
                        break;
                }
            }
        }
        //단일형일 경우
        else if (impo.event_Type == ImportantFound.Event_Type.Exploration_Notice)
        {
            CloseButton.SetActive(true);
            //이벤트 추가
            CloseButton.GetComponent<Button>().onClick.AddListener(Button1);
            CloseText.text = impo.Close_Text;
        }

        //Event_Sprite.sprite = impo.EventSprite;
        Event_Name.text = impo.EventName;
        Event_Contents.text = impo.EventContents;
        //Logo.sprite = impo.Event_Logo_Sprite;
        
    }

    //리무브리스너를 자기자신을 대상으로 할 수 있는지도 확인을 해봐야겠어.성공했어
    public void ReturnToCity()
    {
        DM.TurnEnd_Explorer.RemoveListener(ChangeExplorerReport_Random);
        DM.TurnEnd_Explorer.AddListener(Returning);
        var a = ReturnStack_Max - ReturnStack;
        Message_Text.text = "탐사대가 도시로 귀환 중입니다. 귀환까지 앞으로 " + a + "턴";
        ReturnButton.SetActive(false);


    }

    public void Returning()
    {
        ReturnStack += 1;
        var a = ReturnStack_Max - ReturnStack;
        Message_Text.text = "탐사대가 도시로 귀환 중입니다. 귀환까지 앞으로 " + a + "턴";if (ReturnStack >= ReturnStack_Max)
        {
            
            ResetExplorer(false);
            DM.TurnEnd_Explorer.RemoveListener(Returning);
            ReturnButton.GetComponent<Button>().onClick.RemoveListener(ReturnToCity);
        }
    }

    public void ResetExplorer(bool isDead)
    {
        if (isDead == false)
        {
            ReturnButton.SetActive(true);
            Message_Text.text = "탐사대가 도시로 복귀했습니다. 지부장님의 확인을 기다리고 있습니다.";
            ReturnButton_Text.text = "확인하기";
            ReturnButton.GetComponent<Button>().onClick.AddListener(Report_Popup);
        }
    }

    public void ReadyExplorer()
    {
        ReType.Clear();
        ReValue.Clear();
        Message_Text.text = "탐험대가 이제 막 도시를 떠났습니다. 턴이 끝나면 보고를 받을 수 있습니다.";
        ReturnButton_Text.text = "귀환";
        ReadyPannel.SetActive(true);
        for(int i = 0; i < Resource_Icon.Count; i++)
        {
            Resource_Icon[i].SetActive(false);
        }
    }

    //귀환 완료하면 보고하기 버튼에 연동
    public void Report_Popup()
    {
        DM.All_Resource_List[38] += StartManpower;
        ReturnButton.GetComponent<Button>().onClick.RemoveListener(Report_Popup);
        for (int i = 0; i < ReType.Count; i++)
        {
            DM.All_Resource_List[ReType[i]] += (int)ReValue[i];
        }

        TocheControll.SetActive(true);

        ReturnButton.GetComponent<Button>().onClick.AddListener(ReturnToCity);
        //이건 다중 이프문으로 상황에 따라 연동되게 하자
        if (ReType.Count == 0)
        {
            NotifiCon();
            EventPanel2.GetComponent<EventPanel2>().Add_Message("탐사대 복귀", "죄송합니다 지부장님, 아무것도 찾아내지 못했습니다. 하지만 다음에는 반드시 성공하겠습니다.", null, "수고했다.", Notifi, true);
            
        }
        else if (ReType.Contains(38))
        {
            //EventPanel2.GetComponent<EventPanel2>().Add_Message();
            Event_Contents2.text = "외부에서 생존자들을 구출하는데 성공했습니다. 이들은 당장이라도 일할 준비가 되어있습니다. 아마도요.";
        }
        else if(Supply == 0)
        {
            //EventPanel2.GetComponent<EventPanel2>().Add_Message();
            Event_Contents2.text = "보급품이 떨어져 더 이상의 탐험이 불가능했습니다. 그래도 가져온 물건이 제법 있으니 이정도면 당분간은 안심일 것 같습니다.";
        }
        else if (Supply != 0)
        {
            //EventPanel2.GetComponent<EventPanel2>().Add_Message();
            Event_Contents2.text = "보급품이 남아있었지만 안전을 위해 복귀하였습니다. 그래도 가져온 물건이 제법 있으니 이정도면 당분간은 안심일 것 같습니다.";
        }
        EventPanel2.gameObject.SetActive(true);

        ReadyExplorer();
    }

    public void NotifiCon()
    {
            if (ReType.Count != 0)
            {
                for (int i = 0; i < ReType.Count; i ++)
                {
                    
                }
            }
            else if (ReType.Count == 0)
            {
                Notifi = "가져온 물건이 없습니다.";
            }
    }

    public void GetInventory()
    {
        Debug.Log("이게 된다!");
    }

    //탐사대 출발 버튼에 연계
    public void ExplorerStart()
    {
        StartManpower = DM.All_Resource_List[46];
        DM.TurnEnd_Explorer.AddListener(ChangeExplorerReport_Random);
        ReturnButton.GetComponent<Button>().onClick.AddListener(ReturnToCity);
        DM.All_Resource_List[38] -= StartManpower;
        DM.ManPower_Setting(); 
        DM.ManPower_Caculate();
    }

    public void DeepCopy(Message Ms, Message CopyTo)
    {
        Ms.normalArea = CopyTo.normalArea;
        Ms.eventArea = CopyTo.eventArea;
        Ms.affair = CopyTo.affair;
        Ms.Report = CopyTo.Report;
        Ms.ReType1 = CopyTo.ReType1;
        Ms.ReValue1 = CopyTo.ReValue1;
        Ms.ReType2 = CopyTo.ReType2;
        Ms.ReValue2 = CopyTo.ReValue2;
    }

    [Serializable]
    public struct a
    {
        public string A;
        public int ID;
    }

    public void Button1()
    {
        Active(impo, 0);
        if (Supply <= 0 || ReType.Contains(38))
        {
            ReturnToCity();
            Message_Text.text = "생존자를 구출하여 조속히 도시로 돌아오고 있습니다. 다음 턴에 복귀합니다.";
        }
        CloseButton.GetComponent<Button>().onClick.RemoveListener(Button1);
    }

    public void Button2()
    {
        Active(impo, 0);
        if (Supply <= 0 || ReType.Contains(38))
        {
            ReturnToCity();
            Message_Text.text = "생존자를 구출하여 조속히 도시로 돌아오고 있습니다. 다음 턴에 복귀합니다.";
        }
        Choice_Button[0].GetComponent<Button>().onClick.RemoveListener(Button2);
        Choice_Button[1].GetComponent<Button>().onClick.RemoveListener(Button3);
        Choice_Button[2].GetComponent<Button>().onClick.RemoveListener(Button4);
    }
    public void Button3()
    {
        Active(impo, 1);
        if (Supply <= 0 || ReType.Contains(38))
        {
            ReturnToCity();
            Message_Text.text = "생존자를 구출하여 조속히 도시로 돌아오고 있습니다. 다음 턴에 복귀합니다.";
        }
        Choice_Button[0].GetComponent<Button>().onClick.RemoveListener(Button2);
        Choice_Button[1].GetComponent<Button>().onClick.RemoveListener(Button3);
        Choice_Button[2].GetComponent<Button>().onClick.RemoveListener(Button4);
    }

    public void Button4()
    {
        Active(impo, 2);
        if (Supply <= 0 || ReType.Contains(38))
        {
            ReturnToCity();
            Message_Text.text = "생존자를 구출하여 조속히 도시로 돌아오고 있습니다. 다음 턴에 복귀합니다.";
        }
        Choice_Button[0].GetComponent<Button>().onClick.RemoveListener(Button2);
        Choice_Button[1].GetComponent<Button>().onClick.RemoveListener(Button3);
        Choice_Button[2].GetComponent<Button>().onClick.RemoveListener(Button4);
    }

    public void Active(ImportantFound impo, int choiceNumber)
    {
        //케이스에 따라 자원을 더하고 뺀다, 할당된 중요발견은 이전의 발동 트리거에서 이미 처리했다.
        switch (impo.event_Type)
        {
            case global::ImportantFound.Event_Type.Exploration_Notice:

                int[] ReType_Array = { impo.ReType1, impo.ReType2, impo.ReType3 };
                float[] ReValue_Array = { (float)impo.ReValue1, (float)impo.ReValue2, (float)impo.ReValue3 };

                if(impo.affair == ImportantFound.Affair.Gain)
                {
                    for (int i = 0; i < ReType_Array.Length; i++)
                    {
                        if(ReType_Array[i] != -1)
                        {
                            AddInventory(ReType_Array[i], ReValue_Array[i]);
                        }
                    }
                }
                else if(impo.affair == ImportantFound.Affair.FindZone)
                {
                    DM.FoundZone_List.Add(new FoundZone(ReType_Array[0], ReType_Array[1], ReType_Array[2]));
                }

                Supply -= 1;

                break;

            case global::ImportantFound.Event_Type.Exploration_Choice:

                int[] ReType_Array_Choice = { impo.choice[choiceNumber].ReType1, impo.choice[choiceNumber].ReType2, impo.choice[choiceNumber].ReType3 };
                float[] ReValue_Array_Choice = { (float)impo.choice[choiceNumber].ReValue1, (float)impo.choice[choiceNumber].ReValue2, (float)impo.choice[choiceNumber].ReValue3 };

                for (int i = 0; i < ReType_Array_Choice.Length; i++)
                {
                    AddInventory(ReType_Array_Choice[i], ReValue_Array_Choice[i]);
                }

                Supply -= 1;
                break;
        }
    }

    public IEnumerator CheckToStart()
    {
        WaitForSeconds a = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return a;
            if(Number <= DM.All_Resource_PerTurn_List_Total[45])
            {
                UnableText.gameObject.SetActive(false);
                ReadyButton.interactable = true;
            }
            if (Number > DM.All_Resource_PerTurn_List_Total[45])
            {
                if(ReadyPannel.activeSelf != true)
                {
                    if (DeadTimer > 0)
                    {
                        Message_Text.text = "탐사 기지와의 연락이 끊긴 탐사대는 오래 버틸 수 없습니다! " + DeadTimer + "턴 내로 탐사 기지를 다시 가동하지 않으면 이 탐사대는 전멸합니다.";
                        ReturnButton.GetComponent<Button>().interactable = false;
                    }
                }
                UnableText.gameObject.SetActive(true);
                ReadyButton.interactable = false;
            }
        }
    }
    public void OnEnable()
    {
        StartCoroutine(CheckToStart());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Awake()
    {
    }

    public void Start()
    {

    }
}
