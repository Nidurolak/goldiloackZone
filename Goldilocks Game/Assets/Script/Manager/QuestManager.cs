using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject Touchcontroll;
    public Datamanager DM;
    [SerializeField]
    public List<QuestDataScriptableObject_Resource> QuestList_All = new List<QuestDataScriptableObject_Resource>();

    public Button exitButton;

    //갯수에 따라 Y축 조절해야함
    public PanelOpener PO;
    public EventPanel2 EP2;

    [SerializeField]
    public List<Quest_Resource> Quest_On_Resource = new List<Quest_Resource>();

    [SerializeField]
    public List<QuestButton> Quest_Button_On = new List<QuestButton>();
    [SerializeField]
    public List<QuestButton> Quest_Button_Off = new List<QuestButton>();
    public List<Quest_Resource> Quest_End_Report = new List<Quest_Resource>();
    private int Quest_Button_Count;

    public List<QuestDataScriptableObject_Resource> QuestData_resource = new List<QuestDataScriptableObject_Resource>();

    //턴이 끝날 떄 퀘스트 성공 실패 여부를 결정
    public void Quest_End_Working()
    {
        for (int i = 0; i < Quest_End_Report.Count; i++)
        {
            EventPanel2.EventPanelQue a = new EventPanel2.EventPanelQue();
            List<string> rt = new List<string>();
            if (Quest_End_Report[i].condition() == true)
            {
                a.Name = "퀘스트 성공";
                a.Contents = Quest_End_Report[i].Quest_Clear_Contents;
                for (int j = 0; j < Quest_End_Report[i].Repay_Type.Count; j++)
                {
                    if (Quest_End_Report[i].Repay_Type[j] != -1)
                    {
                        if (Quest_End_Report[i].Repay_Value[j] < 0)
                        {
                            string aa = "";
                            switch (Quest_End_Report[i].Repay_Type[j])
                            {
                                case 30:
                                    aa = "를 잃었습니다.";
                                    break;
                                case 31:
                                    aa = "을 잃었습니다.";
                                    break;
                                case 32:
                                    aa = "이 줄었습니다.";
                                    break;
                                default:
                                    aa = Quest_End_Report[i].Repay_Value[j] + "만큼을 잃었습니다";
                                    break;
                            }
                            rt.Add(DM.resources(Quest_End_Report[i].Repay_Type[j], false) + aa);
                        }
                        else if (Quest_End_Report[i].Repay_Value[j] > 0)
                        {
                            string b = "";
                            switch (Quest_End_Report[i].Repay_Type[j])
                            {
                                case 30:
                                    b = "를 다졌습니다.";
                                    break;
                                case 31:
                                    b = "을 보았습니다.";
                                    break;
                                case 32:
                                    b = "이 늘었습니다.";
                                    break;
                                default:
                                    b = Quest_End_Report[i].Repay_Value[j] + "만큼을 얻었습니다";
                                    break;
                            }
                            rt.Add(DM.resources(Quest_End_Report[i].Repay_Type[j], false) + b);
                        }
                    }
                }
            }
            else if (Quest_End_Report[i].condition() == false)
            {
                a.Name = "퀘스트 실패";
                a.Contents = Quest_End_Report[i].Quest_Fail_Contents;
                for (int j = 0; j < Quest_End_Report[i].Penalty_Type.Count; j++)
                {
                    if (Quest_End_Report[i].Penalty_Type[j] != -1)
                    {
                        if (Quest_End_Report[i].Penalty_Value[j] < 0)
                        {
                            string aa = "";
                            switch (Quest_End_Report[i].Penalty_Type[j])
                            {
                                case 30:
                                    aa = "를 잃었습니다.";
                                    break;
                                case 31:
                                    aa = "을 잃었습니다.";
                                    break;
                                case 32:
                                    aa = "이 줄었습니다.";
                                    break;
                                default:
                                    aa = Quest_End_Report[i].Penalty_Value[j] + "만큼을 잃었습니다";
                                    break;
                            }
                            rt.Add(DM.resources(Quest_End_Report[i].Penalty_Type[j], false) + aa);
                        }
                        else if (Quest_End_Report[i].Penalty_Value[j] > 0)
                        {
                            string b = "";
                            switch (Quest_End_Report[i].Penalty_Type[j])
                            {
                                case 30:
                                    b = "를 다졌습니다.";
                                    break;
                                case 31:
                                    b = "을 보았습니다.";
                                    break;
                                case 32:
                                    b = "이 늘었습니다.";
                                    break;
                                default:
                                    b = Quest_End_Report[i].Penalty_Value[j] + "만큼을 얻었습니다";
                                    break;
                            }
                            rt.Add(DM.resources(Quest_End_Report[i].Penalty_Type[j], false) + b);
                        }
                    }
                }
            }

            a.Logo = Quest_End_Report[i].Thumbnail;
            a.CloseText = "알겠다.";
            for(int z = 0; z < rt.Count; z++)
            {
                a.NotificationText = a.NotificationText + " " + rt[z];
            }
            Debug.Log("우용아아");
            EP2.EPL.Add(a);
            //Quest_End_Invoke(Quest_End_Report[i]);
        }
        Quest_End_Report.Clear();
    }

    //끝난 이벤트 데이터매니저에 연동하면서 초기화
    public void Quest_End_Invoke(Quest_Resource QR)
    {
        if (QR.condition() == true)
        {
            for (int i = 0; i < QR.Repay_Type.Count; i++)
            {
                DM.All_Resource_List[QR.Repay_Type[i]] += QR.Repay_Value[i];
            }
        }
        else if (QR.condition() == false)
        {
            for (int i = 0; i < QR.Penalty_Type.Count; i++)
            {
                DM.All_Resource_List[QR.Penalty_Type[i]] += QR.Penalty_Value[i];
            }
        }
        //Quest_End_Report.Clear();
    }

    //퀘스트 할당하는 식
    public void Quest_Allocation(QuestDataScriptableObject_Resource c)
    {
        Debug.Log("퀘스트 할당됨");
        Quest_Resource a = new Quest_Resource();
        //a.QB = c.QB;
        a.Quest_Name = c.Quest_Name;
        a.Thumbnail = c.Thumbnail;
        Debug.Log(c.condition0.Count + "adasd");
        a.condition0 = c.condition0;
        a.Check_Clear_Before_End = c.Check_Clear_Before_End;
        a.Quest_Clear_Contents = c.Quest_Clear_Contents;
        a.Quest_Fail_Contents = c.Quest_Fail_Contents;
        for(int i = 0; i < c.Repay_Type.Count; i++)
        {
            a.Repay_Type.Add(c.Repay_Type[i]);
            a.Repay_Value.Add(c.Repay_Value[i]);
        }
        for (int i = 0; i < c.Penalty_Type.Count; i++)
        {
            a.Penalty_Type.Add(c.Penalty_Type[i]);
            a.Penalty_Value.Add(c.Penalty_Value[i]);
        }
        a.TurnCount = c.TurnCount;
        for(int i = 0; i < c.CD.Count; i++)
        {
            Quest_Resource.Condition d = new Quest_Resource.Condition();

            Quest_balancing(c.CD[i], d);

            d.Condition_Contents = c.CD[i].Condition_Contents;
            d.Condition_Type = c.CD[i].Condition_Type;
            d.Condition_Value_Target = c.CD[i].Condition_Value;
            d.Building_Type = c.CD[i].Building_Type;
            d.Building_Version = c.CD[i].Building_Version;

            Debug.Log(c.CD[i].Building_Type + "  " + d.Building_Type);

            switch (c.CD[i].Cc)
            {
                case QuestDataScriptableObject_Resource.Condition.Condition_category.building:
                    d.Cc = Quest_Resource.Condition.Condition_category.building;
                    break;
                case QuestDataScriptableObject_Resource.Condition.Condition_category.resource:
                    d.Cc = Quest_Resource.Condition.Condition_category.resource;
                    break;
                case QuestDataScriptableObject_Resource.Condition.Condition_category.rule:
                    d.Cc = Quest_Resource.Condition.Condition_category.rule;
                    break;
                case QuestDataScriptableObject_Resource.Condition.Condition_category.SteelWhale_Prograss:
                    d.Cc = Quest_Resource.Condition.Condition_category.SteelWhale_Prograss;
                    break;
                case QuestDataScriptableObject_Resource.Condition.Condition_category.tech:
                    d.Cc = Quest_Resource.Condition.Condition_category.tech;
                    break;
            }

            //이거 문제 터질지 몰라 고칠 준비해야해
            switch (c.CD[i].Ac)
            {
                case QuestDataScriptableObject_Resource.Condition.ActiveType_category.satisfaction_Down_End:
                    d.Ac = Quest_Resource.Condition.ActiveType_category.satisfaction_Down;
                    a.Condition_List.Add(d);
                    break;
                case QuestDataScriptableObject_Resource.Condition.ActiveType_category.satisfaction_Up_End:
                    d.Ac = Quest_Resource.Condition.ActiveType_category.satisfaction_Up;
                    a.Condition_List.Add(d);
                    break;
            }
        }
        Quest_On_Resource.Add(a);
        //이게 문제라는 군. 뭐가 문제인걸까
        //Show_QuestMark(a, Quest_Button_Off[0]);
        Quest_Button_Off[0].QR = a;
        Quest_Button_Off[0].QR.QB = Quest_Button_Off[0];
        Quest_Button_Off[0].gameObject.SetActive(true);
    }

    //맨 위 27번 줄에 있는 얼로케이션에 연동해야해. 순위는 아마 위쪽, 이건 BC를 스위치로 체크해야해
    public void Quest_balancing(QuestDataScriptableObject_Resource.Condition QSRC, Quest_Resource.Condition QRC)
    {
        switch (QSRC.BC)
        {
            case QuestDataScriptableObject_Resource.Condition.Balancing_category.Food:

                break;
            case QuestDataScriptableObject_Resource.Condition.Balancing_category.House:

                break;
            case QuestDataScriptableObject_Resource.Condition.Balancing_category.Mind:

                break;
            case QuestDataScriptableObject_Resource.Condition.Balancing_category.None:

                break;
        }


    }

    public void Show_QuestMark(Quest_Resource a, QuestButton b)
    {
        b.QR = a;
        b.Logo.sprite = a.Thumbnail;
        b.Name.text = a.Quest_Name;
        for(int i = 0; i < a.Condition_List.Count; i++)
        {
            QuestButton.Condition c = new QuestButton.Condition();
            a.QB = b;
            //퀘스트-컨디션 체크 항목 리스트에 퀘스트 버튼을 할당, 이걸로 퀘스트 버튼 스크립트에 할당된 텍스트, 이미지에 자동 연결
            a.Condition_List[i].QBC = b.QC.QCC[i];
            
            //이거 두 개는 스위치로 바꿔서
            switch (a.Condition_List[i].Ac)
            {
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Up:
                    c.AC = QuestButton.Condition.ActiveType_category.satisfaction_Down_End;
                    break;
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Down:
                    break;
            }
            //c.AC = a.Condition_List[i].Ac;
            //c.Cc = a.Condition_List[i].Cc;

            c.Condition_Type = a.Condition_List[i].Condition_Type;
            c.Condition_Value_Target = a.Condition_List[i].Condition_Value_Target;

            //콘텐츠는 목재 확보나 무슨무슨 건물 짓기 이런 것들을 적어둔다. 잠깐 그럼 타겟 벨류는 CC에 따라 바꿔야하는거 아냐?
            //생각해보니 이상한데? 퀘스트 조건을 다시 짜야겠어. 조건에 무조건 부합하게 하고 완료 조건을 체크하는거지, 카운트다운 끝나기 전에 만족하면 끝나는지 아니면 타이머 끝나고나서야 체크한다던지
            //퀘스트도 페이즈를 만들어야겠어. 프펑처럼 조건 만족하고 유지하는 것을 해당 퀘스트가 끝나면 바로 연계하는 것으로 말야. 괜찮은데?
            c.Condition_Contents = a.Condition_List[i].Condition_Contents;
            b.CD.Add(c);
            //여기서 퀘스트 버튼의 CD(컨디션)을 만들어서 할당해야해. 흠..........ㅁㅁ
            //a.Condition_List.Add(c);
            //b.QC.QCC[i].Icon = null;
            //a.Lines.Add(StartCoroutine(Check_QuestCondition_Text(a.Condition_List[i], b.QC.QCC[i])));
            //b.QC.QCC[i].Condition_Text.text = a.Condition_List[i].Condition_Contents;
            //Condition_Change_Image(b.QC.QCC[i].Icon, a.Condition_List[i].Condition_Type, b.QC.QCC[i].Condition_Bar, a.Condition_List[i]);
        }
    }

    public void Condition_Change_Image(Image a, int b, GameObject Panel, Quest_Resource.Condition c)

    {
        switch (c.Cc)
        {
            case Quest_Resource.Condition.Condition_category.building:
                a.color = new Color(255, 255, 255, 0);
                break;
            case Quest_Resource.Condition.Condition_category.resource:
                a.color = new Color(255, 255, 255, 1);
                //a.sprite = DM.Resource_Sprites[b];
                break;
            case Quest_Resource.Condition.Condition_category.rule:
                a.color = new Color(255, 255, 255, 0);
                break;
            case Quest_Resource.Condition.Condition_category.SteelWhale_Prograss:
                a.color = new Color(255, 255, 255, 0);
                break;
            case Quest_Resource.Condition.Condition_category.tech:
                a.color = new Color(255, 255, 255, 0);
                break;
        }
        Panel.SetActive(true);
    }
    

    //테스트용. 성공하면 퀘슽느-리소스를 스크립터블로 교체하는 작업을 해야할까 말아야할까
    public void QuestLine_Resource(Quest_Resource S)
    {
        for(int i = 0; i < S.Condition_List.Count; i++)
        {
            switch (S.Condition_List[i].Cc)
            {
                case Quest_Resource.Condition.Condition_category.resource:
                    break;
                case Quest_Resource.Condition.Condition_category.tech:
                    break;
                case Quest_Resource.Condition.Condition_category.rule:
                    break;
                case Quest_Resource.Condition.Condition_category.building:
                    break;
                case Quest_Resource.Condition.Condition_category.SteelWhale_Prograss:
                    break;
            }
        }
        //yield return null;
    }

    //이벤트 발동, 턴 엔드면 발동시키고 리메인이면 컨디션0값 체크
    public void Event_Quest_Resource()
    {
        for(int i = 0; i < Quest_On_Resource.Count; i++)
        {
            Quest_On_Resource[i].TurnCount -= 1;
            //컨디션 리스트 돌면서 체크체크체ㅐ크하면서 컨디션0 조절함
            for(int j = 0; j < Quest_On_Resource[i].Condition_List.Count; j++)
            {
                QuestActive_Resource(Quest_On_Resource[i], j);
            }

            //끝난 건 삭제함, 성공실패 여부는 앞에서 해결함
            if (Quest_On_Resource[i].TurnCount == 0)
            {
                //Quest_End_Report.Add(Quest_On_Resource[i]);
                Quest_On_Resource[i].QB.Button.SetActive(false);
            }
        }
    }

    //완료된 이벤트를 삭제 //
    public void Event_Quest_Clear()
    {
        for (int i = Quest_On_Resource.Count; i > 0; i--)
        {
            if (Quest_On_Resource[i -1].TurnCount <= 0 && Quest_On_Resource[i - 1].Have_Remain == false)
            {
                Quest_On_Resource[i - 1].QB.Button.SetActive(false);
                Quest_On_Resource.Remove(Quest_On_Resource[i -1]);
            }
        }
        if(EP2.EPL.Count > 0)
        {
            EP2.gameObject.SetActive(true);
        }
    }

    //자원 관련 항목일 경우 발동, 이거 말고 건물, 법안 등등 만들어야해
    public void QuestActive_Resource(Quest_Resource S, int i)
    {
        var a = S.Condition_List[i];
        if (S.Check_Clear_Before_End == true)
        {
            int z = 0;
            if(a.Cc == Quest_Resource.Condition.Condition_category.building)
            {
                for (int j = 0; j < DM.TM.TypeAll_Building_List[a.Building_Type - 1].Type_Building[a.Building_Version - 1].Buildings.Count; j++)
                {
                    if (DM.TM.TypeAll_Building_List[a.Building_Type - 1].Type_Building[a.Building_Version - 1].Buildings[j].IsWorking_OnTile == true
                       && DM.TM.TypeAll_Building_List[a.Building_Type - 1].Type_Building[a.Building_Version - 1].Buildings[j].IsDistory_OnTile == false)
                    {
                        z++;
                    }
                }
            }
            
            switch (S.Condition_List[i].Ac)
            {
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Up:

                    switch (S.Condition_List[i].Cc)
                    {
                        case Quest_Resource.Condition.Condition_category.building:
                                if ((int)a.Condition_Value_Target <= z)
                                {
                                    Debug.Log("애용3");
                                    S.condition0[i] = true;
                                }
                                else if ((int)a.Condition_Value_Target > z)
                                {
                                    Debug.Log("애용4");
                                    S.condition0[i] = false;
                                }
                            break;
                        case Quest_Resource.Condition.Condition_category.resource:

                            if ((int)a.Condition_Value_Target <= DM.All_Resource_List[a.Condition_Type])
                            {
                                Debug.Log("애용");
                                S.condition0[i] = true;
                            }
                            else if ((int)a.Condition_Value_Target > DM.All_Resource_List[a.Condition_Type])
                            {
                                Debug.Log("애용2");
                                S.condition0[i] = false;
                            }
                            break;
                    }
                    break;
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Down:

                    switch (S.Condition_List[i].Cc)
                    {
                        case Quest_Resource.Condition.Condition_category.building:
                                if ((int)a.Condition_Value_Target > z)
                                {
                                    Debug.Log("애용5");
                                    S.condition0[i] = true;
                                }
                                else if ((int)a.Condition_Value_Target <= z)
                                {
                                    Debug.Log("애용6");
                                    S.condition0[i] = false;
                                }
                            break;
                        case Quest_Resource.Condition.Condition_category.resource:

                            if ((int)a.Condition_Value_Target > DM.All_Resource_List[a.Condition_Type])
                            {
                                S.condition0[i] = true;
                            }
                            else if ((int)a.Condition_Value_Target <= DM.All_Resource_List[a.Condition_Type])
                            {
                                S.condition0[i] = false;
                            }

                            if (S.TurnCount <= 0 && S.condition() == true)
                            {
                                Debug.Log("퀘스트 성공");
                            }
                            else if (S.TurnCount == 0 && S.condition() == false)
                            {
                                Debug.Log("퀘스트 실패");
                            }

                            break;
                    }
                    break;
            }
            if(S.condition() == true)
            {
                S.TurnCount = 0;
            }
        }
        else if (S.Check_Clear_Before_End == false)
        {
            switch (S.Condition_List[i].Ac)
            {
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Up:

                    if ((int)a.Condition_Value_Target <= DM.All_Resource_List[a.Condition_Type])
                    {
                        S.condition0[i] = true;
                    }
                    else if ((int)a.Condition_Value_Target > DM.All_Resource_List[a.Condition_Type])
                    {
                        S.condition0[i] = false;
                    }

                    if (S.TurnCount <= 0)
                    {
                        if (S.condition0[i] == true)
                        {
                            Debug.Log("퀘스트 성공");
                        }
                        else if (S.condition0[i] == false)
                        {
                            Debug.Log("퀘스트 실패");
                        }
                    }
                    break;
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Down:



                    if ((int)a.Condition_Value_Target > DM.All_Resource_List[a.Condition_Type])
                    {
                        S.condition0[i] = true;
                    }
                    else if ((int)a.Condition_Value_Target <= DM.All_Resource_List[a.Condition_Type])
                    {
                        S.condition0[i] = false;
                    }

                    if (S.TurnCount <= 0)
                    {
                        if (S.condition() == true)
                        {
                            //여기다가 이벤트 패널2 띄워서 정보 얻는 함수가 들어와야한다.
                            Debug.Log("퀘스트 성공");
                        }
                        else if (S.condition() == false)
                        {
                            Debug.Log("퀘스트 실패");
                        }
                    }
                    break;
                }

        }
        
    }



    public IEnumerator Check_QuestPanelSize()
    {
        WaitForSeconds b = new WaitForSeconds(0.1f);
        while (true)
        {
            var a = -1;
            for(int i = 0; i < Quest_Button_On.Count; i++)
            {
                if(Quest_Button_On[i].On == true)
                {
                    a++;
                }
            }
            if (a != Quest_Button_Count)
            {
                PO.UpPosition.y = 54 + (a * 105);
            }
            Quest_Button_Count = a;
            yield return b;
        }
    }


    public void Awake()
    {
        int a = 0;
        //Quest_Allocation(QuestList_All[a]);
        StartCoroutine(Check_QuestPanelSize());


        GameObject b = GameObject.Find("DataManager");
        DM = b.GetComponent<Datamanager>();
        b.GetComponent<Datamanager>().QM = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitButton.onClick.Invoke();
        }
    }

}

//스크립터블 오브젝트든 이지세이브 저장이든 데이터를 불러올 때 배율을 찍는 함수가 있어야해
[System.Serializable]
public class Quest_Resource
{
    public QuestButton QB;
    public Sprite Thumbnail;
    public string Quest_Name;
    public string Quest_Clear_Contents;
    public string Quest_Fail_Contents;

    //끝나기 전에 조건 만족하면 클리어하는지 체크, 기존의 컨디션의 유지 여부를 체크하는 것임.
    public bool Check_Clear_Before_End;

    public bool Have_Remain;
    public bool Ready_To_Delete;

    public QuestDataScriptableObject_Resource Next_Quest_If_Clear;
    public QuestDataScriptableObject_Resource Next_Quest_If_False;

    public List<int> Repay_Type = new List<int>();
    public List<int> Repay_Value = new List<int>();
    public List<float> Repay_Multiply = new List<float>();
    public List<int> Penalty_Type = new List<int>();
    public List<int> Penalty_Value = new List<int>();
    public List<float> Penalty_Multiply = new List<float>();

    [SerializeField]
    public List<Coroutine> Lines = new List<Coroutine>();

    public int TurnCount;

    public List<bool> condition0 = new List<bool>();

    public bool condition()
    {
        bool a = true;
        if (condition0.Contains(false))
        {
            a = false;
        }
        return a;
    }

    [SerializeField]
    public List<Condition> Condition_List = new List<Condition>();
    

    [System.Serializable]
    public class Condition
    {
        public QuestButton.QuestContents_Condition QBC;
        //목표값 이상-달성, 목표값 이하-달성, 목표값 이상-유지, 목표값 이하-유지
        public enum ActiveType_category { satisfaction_Up, satisfaction_Down}
        public ActiveType_category Ac;

        public string Condition_Contents;

        public enum Condition_category { resource, tech, rule, building, SteelWhale_Prograss }
        public Condition_category Cc;

        public enum Balancing_category { House, Food, Mind, None }
        public Balancing_category BC;
        public int Building_Type;
        public int Building_Version;

        public int Condition_Type;
        public int Condition_Value_Current;
        public float Condition_Value_Target;
        public float Condition_Value_Target_Multiply;
    }

    //이건 그렇게 중요한게 아냐
    public enum Quest_category { main, sub }

    //이게 좀 중요한데.....
}
