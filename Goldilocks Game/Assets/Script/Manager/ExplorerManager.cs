using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;

public class ExplorerManager : MonoBehaviour
{
    public List<Explorer> ExplorerList = new List<Explorer>();
    public List<ImportantFound> FoundList = new List<ImportantFound>();
    public List<Message> MessageList = new List<Message>();
    public List<ScriptableObject> a = new List<ScriptableObject>();

    public int MaxExplorerCount;

    public ExplorerPanel exPanel;
    public Datamanager DM;

    public Button ExplorerButton;
    public GameObject ExplorerPanel;
    public bool ExplorerReturn;
    public GameObject EventPanel;
    public Image Event_Sprite;
    public Text Event_Name;
    public Text Event_Contents;
    public GameObject ClickControll;

    public Text Notification_Text;
    public GameObject CloseButton;
    public Image Logo;

    public GameObject Choose;
    public List<GameObject> Choice_Button = new List<GameObject>();
    public List<Event_Quest_Notice_Message> Side_Slot = new List<Event_Quest_Notice_Message>();

    [Serializable]
    public class MessageListWraaper
    {
        public List<Message> MessageList;
    }
    public List<MessageListWraaper> MessageListList = new List<MessageListWraaper>();
    
    [Serializable]
    public class FoundListWraaper
    {
        public List<ImportantFound> ImportantFoundList;
    }
    public List<FoundListWraaper> ImportantFoundListList = new List<FoundListWraaper>();

    public void OnEnable()
    {
        StartCoroutine(CheckExplorerCount());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Awake()
    {
        /*for (int i = 0; i < ExplorerList.Count; i++)
        {
            b.Add(new ExplorerEvent((int)ExplorerList[i].normalArea, (int)ExplorerList[i].eventArea, ExplorerList[i].Message.text));
            Debug.Log((int)ExplorerList[i].normalArea);
            Debug.Log(b[i].eventArea);
            Debug.Log(b[i].Report);
        }*/
        //Save();
        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
        a.GetComponent<Datamanager>().EXM = this;
    }

    public void Start()
    {
        //Save();
        //Save2();
        Load();
    }

    public IEnumerator CheckExplorerCount()
    {
        WaitForSeconds a = new WaitForSeconds(1f);
        while (true)
        {
            yield return a;
            if (MaxExplorerCount < DM.All_Resource_List[45])
            {
                AddExplorerCount();
                MaxExplorerCount += 1;
            }
            else if (MaxExplorerCount > DM.All_Resource_List[45])
            {
                MinusExplorerCount();
                MaxExplorerCount -= 1;
            }
            //MaxExplorerCount = DM.All_Resource_List[45];
        }
    }

    //탐사대 숫자를 늘린다. 온에이블 처리
    public void AddExplorerCount()
    {
        Debug.Log("탐사대 더함");
        /*switch (DM.All_Resource_List[45])
        {
            case 1:
                ExplorerList[0].gameObject.SetActive(true);
                break;
            case 2:
                ExplorerList[1].gameObject.SetActive(true);
                break;
            case 3:
                ExplorerList[2].gameObject.SetActive(true);
                break;
            case 4:
                ExplorerList[3].gameObject.SetActive(true);
                break;
            case 5:
                ExplorerList[4].gameObject.SetActive(true);
                break;
        }*/
        ExplorerList[MaxExplorerCount].gameObject.SetActive(true);
    }

    //탐사대 숫자를 줄인다. 디스에이블 처리
    public void MinusExplorerCount()
    {
        Debug.Log("탐사대 뺌");
        ExplorerList[MaxExplorerCount].gameObject.SetActive(false);
    }

    public void Save()
    {
        JsonData ToJoson = JsonMapper.ToJson(MessageListList);
        File.WriteAllText(Application.dataPath + "/Dialouge/ExplorerReport.json", ToJoson.ToString());
        //ES3.Save<List<Message>>("ReportMessage", baddddd, Application.dataPath + "/Dialouge/ExplorerReport");
    }
    public void Save2()
    {
        JsonData ToJoson2 = JsonMapper.ToJson(ImportantFoundListList);
        File.WriteAllText(Application.dataPath + "/Dialouge/ExplorerFound.json", ToJoson2.ToString());
    }

    public void Load()
    {
        string a = File.ReadAllText(Application.dataPath + "/Dialouge/ExplorerReport.json");
        JsonData b = JsonMapper.ToObject(a);
        ParsingJsonItemToList(b);
        //Debug.Log(a);
        Debug.Log(MessageList[2].affair);
        Debug.Log(Application.dataPath);
    }

    //Json 데이터를 파싱한다
    private void ParsingJsonItemToList(JsonData JD)
    {
        for(int i = 0; i < JD.Count; i++)
        {
            for(int x = 0; x < JD[i][0].Count; x++)
            {
                //if(i == 3)
                //{
                    MessageList.Add(
                        new Message(
                            (int)JD[i][0][x]["normalArea"],
                            (int)JD[i][0][x]["eventArea"],
                            (int)JD[i][0][x]["affair"],
                            JD[i][0][x]["Report"].ToString(),
                            (int)JD[i][0][x]["ReType1"],
                            (int)JD[i][0][x]["ReValue1"],
                            (int)JD[i][0][x]["ReType2"],
                            (int)JD[i][0][x]["ReValue2"]));
                //}
                //Debug.Log(JD[i][0][x]["normalArea"]);
            }
        }
    }

}






[Serializable]
public class Message
{
    public enum NormalArea { Idle, Filed, Forest, Velly, Mountain, Ruins, SmallCity, BigCity, WarFiled, BaseCamp };
    public NormalArea normalArea;

    public enum EventArea { Idle, scenario1, scenario2, scenario3, scenario4, scenario5 };
    public EventArea eventArea;

    public enum Affair { Idle, Move, gain, FindZone, importantfind };
    public Affair affair;

    public string Report;

    public int ReType1;
    public int ReValue1;
    public int ReType2;
    public int ReValue2;

    public Message(int NormalArea, int EventArea, int Affair, string report, int retype1, int revalue1, int retype2, int revalue2)
    {
        normalArea = (Message.NormalArea)NormalArea;
        eventArea = (Message.EventArea)EventArea;
        affair = (Message.Affair)Affair;
        Report = report;
        ReType1 = retype1;
        ReValue1 = revalue1;
        ReType2 = retype2;
        ReValue2 = revalue2;
    }
}



    [Serializable]
    public class ImportantFound
    {
        public enum NormalArea { Idle, Filed, Forest, Velly, Mountain, Ruins, SmallCity, BigCity, WarFiled, BaseCamp };
        public NormalArea normalArea;

        public enum EventArea { Idle, scenario1, scenario2, scenario3, scenario4, scenario5 };
        public EventArea eventArea;

        public enum Affair { Idle, Gain, FindZone };
        public Affair affair;

    [System.Serializable]
        public enum Event_Type
        {
            Exploration_Notice,
            Exploration_Choice
        }
        public Event_Type event_Type;

        //이건 직렬화 문제로 Json저장이 불가능해. 가능하면 이벤트 ID로 받아와서 따로 스프라이트 할당을 해야겠어.
        //public Sprite Event_Logo_Sprite;
        //public Sprite EventSprite;
        public int Event_Logo_Sprite_Number;
        public int EvenSprite_Number;

        public string EventName;
        public string EventContents;
        public string ValueChange_Text;
        public string Close_Text;

        public int ReType1;
        public int ReType2;
        public int ReType3;
        public double ReValue1;
        public double ReValue2;
        public double ReValue3;

        public int FoundStack_Max;

        //선택지를 골랐을 경우 번호를 따짐
        public int Choice_Number;

        [System.Serializable]
        public class Choice //: ICloneable
        {
            public string ChoiceName;
            public string ChoiceContents;

            public int ReType1;
            public int ReType2;
            public int ReType3;
            public double ReValue1;
            public double ReValue2;
            public double ReValue3;
            //public Sprite Event_Active_Sprite;
            public int TypeNumber;

            public bool DeathSentence;

            public int Next_Series_Number;

            public int EventNumber_Goto;

            public int EndingPoint;

            public Choice Clone()
            {
                Choice newCopy = new Choice();
                newCopy.ChoiceName = this.ChoiceName;
                newCopy.ChoiceContents = this.ChoiceContents;
                newCopy.ReType1 = this.ReType1;
                newCopy.ReType2 = this.ReType2;
                newCopy.ReType3 = this.ReType3;
                newCopy.ReValue1 = this.ReValue1;
                newCopy.ReValue2 = this.ReValue2;
                newCopy.ReValue3 = this.ReValue3;
                newCopy.DeathSentence = this.DeathSentence;
                newCopy.Next_Series_Number = this.Next_Series_Number;
                newCopy.EndingPoint = this.EndingPoint;
                newCopy.EventNumber_Goto = this.EventNumber_Goto;
                newCopy.TypeNumber = this.TypeNumber;

                return newCopy;
            }
        }

    public List<Choice> choice = new List<Choice>();
    }

