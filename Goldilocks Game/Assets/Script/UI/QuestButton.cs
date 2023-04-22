using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public QuestManager QM;
    public GameObject Button;
    public Image Logo;
    public Text Name;
    public GameObject ContentsPanel;
    public bool On;

    public Quest_Resource QR;

    public QuestContents QC;

    public List<SelectedTilePanelSlot> Slots = new List<SelectedTilePanelSlot>();
    public SelectedTilePanelSlot Timer;

    public List<Coroutine> Lines = new List<Coroutine>();
    

    public int TurnCount;

    public List<bool> condition0 = new List<bool>();

    public void Show_QuestMark()
    {
        Logo.sprite = QR.Thumbnail;
        Name.text = QR.Quest_Name;
        TurnCount = QR.TurnCount;
        for(int i = 0; i < QR.Condition_List.Count; i++)
        {
            QuestButton.Condition c = new QuestButton.Condition();
            QR.Condition_List[i].QBC = QC.QCC[i];
            switch (QR.Condition_List[i].Ac)
            {
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Up:
                    c.AC = QuestButton.Condition.ActiveType_category.satisfaction_Up_End;
                    break;
                case Quest_Resource.Condition.ActiveType_category.satisfaction_Down:
                    c.AC = QuestButton.Condition.ActiveType_category.satisfaction_Down_End;
                    break;
            }
            switch (QR.Condition_List[i].Cc)
            {
                case Quest_Resource.Condition.Condition_category.building:
                    c.Cc = Condition.Condition_category.building;
                    break;
                case Quest_Resource.Condition.Condition_category.resource:
                    c.Cc = Condition.Condition_category.resource;

                    break;
                case Quest_Resource.Condition.Condition_category.rule:
                    c.Cc = Condition.Condition_category.rule;

                    break;
                case Quest_Resource.Condition.Condition_category.tech:
                    c.Cc = Condition.Condition_category.tech;

                    break;
            }

            c.Building_Type = QR.Condition_List[i].Building_Type;
            c.Building_Version = QR.Condition_List[i].Building_Version;
            c.Condition_Type = QR.Condition_List[i].Condition_Type;
            c.Condition_Value_Target = QR.Condition_List[i].Condition_Value_Target;
            c.Condition_Contents = QR.Condition_List[i].Condition_Contents;
            
            CD.Add(c);
        }
    }

    public IEnumerator QuestLine()
    {
        yield return null;
    }

    public IEnumerator Check_QuestCondition_Text_AllTime(Quest_Resource.Condition a, QuestContents_Condition b, Condition d)
    {
        //이것도 스위치 통해서 타입 정해야해. 빌딩 건물 자원 등인지 확인해서 말이지 
        switch (a.Cc)
        {
            case Quest_Resource.Condition.Condition_category.building:
                b.Icon.color = new Color(255, 255, 255, 0);
                break;
            case Quest_Resource.Condition.Condition_category.resource:
                b.Icon.color = new Color(255, 255, 255, 1);
               // b.Icon.sprite = QM.DM.Resource_Sprites[a.Condition_Type];

                break;
            case Quest_Resource.Condition.Condition_category.rule:
                b.Icon.color = new Color(255, 255, 255, 0);
                break;
        }
        //Debug.Log(QM.DM.Resource_Sprites[a.Condition_Type].name);
        WaitForSeconds c = new WaitForSeconds(0.3f);
        while (true)
        {
            CheckIcons();
            b.Condition_Text.text = Contents_Front(a.Condition_Type, d);
            yield return c;
        }
    }

    public void CheckIcons()
    {
            for(int i = 0; i < CD.Count; i++)
            {
                switch (CD[i].AC)
                {
                    case Condition.ActiveType_category.satisfaction_Down_End:
                    switch (CD[i].Cc)
                    {
                        case Condition.Condition_category.resource:
                            if (CD[i].Condition_Value_Current > CD[i].Condition_Value_Target)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.No;
                            }
                            else if (CD[i].Condition_Value_Current <= CD[i].Condition_Value_Target)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.OK;
                            }
                            break;
                        case Condition.Condition_category.building:
                            Debug.Log("작동");
                            if (CheckBuildingForCondition(false, CD[i].Building_Type, CD[i].Building_Version, (int)CD[i].Condition_Value_Target, false) == false)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.OK;
                            }
                            else if (CheckBuildingForCondition(false, CD[i].Building_Type, CD[i].Building_Version, (int)CD[i].Condition_Value_Target, false) == true)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.No;
                            }
                            
                            break;
                    }
                        break;
                    case Condition.ActiveType_category.satisfaction_Up_End:

                    switch (CD[i].Cc)
                    {
                        case Condition.Condition_category.resource:
                            if (CD[i].Condition_Value_Current < CD[i].Condition_Value_Target)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.No;
                            }
                            else if (CD[i].Condition_Value_Current >= CD[i].Condition_Value_Target)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.OK;
                            }
                            break;
                        case Condition.Condition_category.building:
                            //Debug.Log("작동");
                            if (CheckBuildingForCondition(false, CD[i].Building_Type, CD[i].Building_Version, (int)CD[i].Condition_Value_Target, true) == true)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.OK;
                            }
                            else if (CheckBuildingForCondition(false, CD[i].Building_Type, CD[i].Building_Version, (int)CD[i].Condition_Value_Target, true) == false)
                            {
                                QC.QCC[i].CheckIcon.sprite = QC.No;
                            }
                            break;
                    }
                        break;
                }
            }
        QC.Timer.text = TurnCount.ToString();
    }

    public bool CheckBuildingForCondition(bool returnValue, int type, int version, int count, bool isUp)
    {
        int a = 0;
        for(int i = 0; i < QM.DM.TM.TypeAll_Building_List[type].Type_Building[version].Buildings.Count; i++)
        {
            if (QM.DM.TM.TypeAll_Building_List[type].Type_Building[version].Buildings[i].IsWorking_OnTile == true
                && QM.DM.TM.TypeAll_Building_List[type].Type_Building[version].Buildings[i].IsDistory_OnTile == false)
            {
                a++;
            }
        }
        //건물을 채워야 한다면
        if(isUp == true)
        {
            if (a >= count)
            {
                returnValue = true;
            }
        }
        else if(isUp == false)
        {
            if (a <= count)
            {
                returnValue = true;
            }
        }
        return returnValue;
    }

    //퀘스트 컨디션 리스트의 현황을 나타냄
    public string Contents_Front(int i, Condition a)
    {
        string b = null;
        switch (a.Cc)
        {
            case Condition.Condition_category.resource:
                a.Condition_Value_Current = QM.DM.All_Resource_List[i];
                b = a.Condition_Contents + "  " + a.Condition_Value_Current + " / " + a.Condition_Value_Target;
                
                break;
            case Condition.Condition_category.building:
                int c = 0;
                for(int z = 0; z < QM.DM.TM.TypeAll_Building_List[a.Building_Type - 1].Type_Building[a.Building_Version - 1].Buildings.Count; z++)
                {
                    if(QM.DM.TM.TypeAll_Building_List[a.Building_Type - 1].Type_Building[a.Building_Version - 1].Buildings[z].IsWorking_OnTile == true
                        && QM.DM.TM.TypeAll_Building_List[a.Building_Type - 1].Type_Building[a.Building_Version - 1].Buildings[z].IsDistory_OnTile == false)
                    {
                        c++;
                    }
                }
                a.Condition_Value_Current = c;
                b = a.Condition_Contents + " " + a.Condition_Value_Current + " / " + a.Condition_Value_Target;
                break;
        }
        //b = i.ToString();
        return b;
    }

    [SerializeField]
    public List<Condition> CD;

    [System.Serializable]
    public class Condition
    {
        //목표값 이상-달성, 목표값 이하-달성, 목표값 이상-유지, 목표값 이하-유지
        public enum ActiveType_category { satisfaction_Up_End, satisfaction_Down_End, satisfaction_Up_Remain, satisfaction_Down_Remain }
        public ActiveType_category AC;
        public enum Condition_category { resource, tech, rule, building, SteelWhale_Prograss }
        public Condition_category Cc;
        public enum Balancing_category {House, Food, Mind, None }
        public Balancing_category BC;
        public int Building_Type;
        public int Building_Version;
        public int Condition_Type;
        public int Condition_Value_Current;
        public float Condition_Value_Target;
        public string Condition_Contents;
    }

    //이건 그렇게 중요한게 아냐
    [SerializeField]
    public enum Quest_category { main, sub }


    public void OnEnable()
    {
        On = true;
        Show_QuestMark();
        QM.Quest_Button_On.Add(this);
        QM.Quest_Button_Off.Remove(this);
        Debug.Log(condition0.Count + "zzzzzzzz");
        Debug.Log(CD.Count + "안휘 이게 왜 안돼?");
        //할당된 컨디션만큼 반복함
        for (int i = 0; i < CD.Count; i++)
        {
            QC.QCC[i].Condition_Bar.SetActive(true);
            QC.QCC[i].CheckIcon.sprite = QC.No;
            Lines.Add(StartCoroutine(Check_QuestCondition_Text_AllTime(QR.Condition_List[i], QC.QCC[i], CD[i])));
            //QC.QCC[i].Condition_Text.text = "";
            //QC.QCC[i].CheckIcon.sprite = null;
        }
    }
    public void OnDisable()
    {
        On = false;
        QM.DM.Check_Mind();
        QM.Quest_End_Report.Add(QR);
        QM.Quest_End_Invoke(QR);

        QM.Quest_Button_On.Remove(this);
        QM.Quest_Button_Off.Add(this);

        for (int i = 0; i < QC.QCC.Count; i++)
        {
            QC.QCC[i].Condition_Bar.SetActive(false);
            QC.QCC[i].CheckIcon.sprite = QC.No;
            QC.QCC[i].Condition_Text.text = "";
            QC.QCC[i].CheckIcon.sprite = null;
        }
        QR = new Quest_Resource();
        CD = new List<Condition>();
        Logo.sprite = null;
        Name.text = "";
        QC.Timer.text = "";
    }

    [System.Serializable]
    public class QuestContents
    {
        public GameObject QCObject;

        public List<QuestContents_Condition> QCC = new List<QuestContents_Condition>();

        public Sprite OK;
        public Sprite No;
        public Text Timer;
    }

    [System.Serializable]
    public class QuestContents_Condition
    {
        public GameObject Condition_Bar;
        public Image Icon;
        public Text Condition_Text;
        public Image CheckIcon;
    }
}
