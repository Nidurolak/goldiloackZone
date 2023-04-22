using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class SteelWhaleBuildingSlotManager : MonoBehaviour
{
    public SteelWhaleManager SWM;
    public Datamanager DM;
    public StealWhalePanel SWP;

    //담당하는 버튼들을 한 곳에 모아서 처리한다.
    public List<ButtonInfo> ButtonList1 = new List<ButtonInfo>();
    public List<ButtonInfo> ButtonList2 = new List<ButtonInfo>();
    public List<ButtonInfo> ButtonList3 = new List<ButtonInfo>();
    public List<ButtonInfo> ButtonList4 = new List<ButtonInfo>();
    public List<ButtonInfo> ButtonList5 = new List<ButtonInfo>();

    public List<string> BuildingID_List1 = new List<string>();
    public List<string> BuildingID_List2 = new List<string>();
    public List<string> BuildingID_List3 = new List<string>();
    public List<string> BuildingID_List4 = new List<string>();
    public List<string> BuildingID_List5 = new List<string>();

    [System.Serializable]
    public class ButtonInfo
    {
        public GameObject button;
        public Image logo;

        public int type;
        public int version;
    }

    public UISound uiSound;
    
    public void Slot_Change_Category(string Building_And_ListNum)
    {
        int a = int.Parse(Building_And_ListNum.Substring(0, 2));
        int b = int.Parse(Building_And_ListNum.Substring(2, 2));
        int d = int.Parse(Building_And_ListNum.Substring(4, 2));
        int c = 0;

        for (int i = 0; i < SWM.Swbd[b].BD_Version.Count; i++)
        {
            if(SWM.Swbd[b].BD_Version[i].Unlock == true)
            {
                Slot_Change(Building_And_ListNum, c);
            }
            c++;
        }
    }

    //버튼리스트 번호, 할당할 건물 버전타입, 버튼 숫자
    public void Slot_Change(string Num_And_Ver, int i)
    {
        int a = int.Parse(Num_And_Ver.Substring(0, 2));
        int b = int.Parse(Num_And_Ver.Substring(2, 2));
        switch (a)
        {
            case 0:
                Slot_Initialise(a, b);
                //BuildingID_List1[i] = int.Parse(Num_And_Ver);
                break;
            case 1:
                Slot_Initialise(a, b);
                //BuildingID_List2[i] = int.Parse(Num_And_Ver);
                break;
            case 2:
                Slot_Initialise(a, b);
                //BuildingID_List3[i] = int.Parse(Num_And_Ver);
                break;
            case 3:
                Slot_Initialise(a, b);
                //BuildingID_List4[i] = int.Parse(Num_And_Ver);
                break;
            case 4:
                Slot_Initialise(a, b);
                //BuildingID_List5[i] = int.Parse(Num_And_Ver);
                break;
        }
    }

    public void Slot_All_Reset()
    {
        Slot_Reset(ButtonList1);
        Slot_Reset(ButtonList2);
        Slot_Reset(ButtonList3);
        Slot_Reset(ButtonList4);
        Slot_Reset(ButtonList5);
    }

    public void Slot_Reset(List<ButtonInfo> a)
    {
        for (int i = 0; i < a.Count; i++)
        {
            a[i].button.GetComponent<Button>().interactable = true;
            a[i].logo.sprite = null;
            a[i].type = 99;
            a[i].version = 99;
            a[i].button.SetActive(false);
        }
    }

    //a는 버튼리스트 넘버, b는 버튼리스트 리스트 인덱스
    public void Slot_Initialise(int a, int b)
    {
        for (int i = 0; i < SWM.Swbd[b].BD_Version.Count; i++)
        {
            if (SWM.Swbd[b].BD_Version[i].Unlock == true)
            {
                switch (a)
                {
                    case 0:
                        ButtonList1[i].button.SetActive(true);
                        ButtonList1[i].button.GetComponent<Button>().interactable = SWM.Check_Construct(b, i);
                        ButtonList1[i].logo.sprite = SWM.Swbd[b].BD_Version[i].Room_Logo;
                        BuildingID_List1[i] = SWM.Swbd[b].BD_Version[i].ID;
                        break;
                    case 1:
                        ButtonList2[i].button.SetActive(true);
                        ButtonList1[i].button.GetComponent<Button>().interactable = SWM.Check_Construct(b, i);
                        ButtonList2[i].logo.sprite = SWM.Swbd[b].BD_Version[i].Room_Logo;
                        BuildingID_List2[i] = SWM.Swbd[b].BD_Version[i].ID;
                        break;
                    case 2:
                        ButtonList3[i].button.SetActive(true);
                        ButtonList1[i].button.GetComponent<Button>().interactable = SWM.Check_Construct(b, i);
                        ButtonList3[i].logo.sprite = SWM.Swbd[b].BD_Version[i].Room_Logo;
                        BuildingID_List3[i] = SWM.Swbd[b].BD_Version[i].ID;
                        break;
                    case 3:
                        ButtonList4[i].button.SetActive(true);
                        ButtonList1[i].button.GetComponent<Button>().interactable = SWM.Check_Construct(b, i);
                        ButtonList4[i].logo.sprite = SWM.Swbd[b].BD_Version[i].Room_Logo;
                        BuildingID_List4[i] = SWM.Swbd[b].BD_Version[i].ID;
                        break;
                    case 4:
                        ButtonList5[i].button.SetActive(true);
                        ButtonList1[i].button.GetComponent<Button>().interactable = SWM.Check_Construct(b, i);
                        ButtonList5[i].logo.sprite = SWM.Swbd[b].BD_Version[i].Room_Logo;
                        BuildingID_List5[i] = SWM.Swbd[b].BD_Version[i].ID;
                        break;
                }
                Debug.Log(SWM.Swbd[b].BD_Version[i].Name + " 불러오기 성공");
            }
        }
        
    }

    //선실버튼 눌렀을 때 건설가능한지 체크하고 가능하면 시작
    public void Building_Start(string Num_And_Ver)
    {
        int a = int.Parse(Num_And_Ver.Substring(0, 2));
        int b = int.Parse(Num_And_Ver.Substring(2, 2));

        Debug.Log(SWM.Check_Construct(a, b));
        
    }


}
