using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Text;
using UnityEngine.Events;

public class StealWhalePanel : MonoBehaviour
{
    public SteelWhaleManager SWM;
    public SteelWhaleBuildingSlotManager SWBSM;
    public GameObject TochControll;
    public List<Room_Panel> PanelList = new List<Room_Panel>();
    
    [SerializeField]
    private AnimationCurve AnimationCurve;
    
    private bool Wheel_IsUp = false;
    private bool RoomInfo_IsUp = false;
    private bool BuildInfo_IsUp = false;
    private Sequence sequence0 = null;
    private Sequence sequence1 = null;
    private Sequence sequence2 = null;

    public Image Background;
    public Image Logo;

    public Text Name0;
    public Text Name1;
    public Text Contents;

    public GameObject Room_Info_Panel;
    public GameObject Room_Info_Panel_Wheel;
    public UISound WheelSound;
    public GameObject Room_Info_Panel_Wheel_RoomInfo;
    public GameObject Room_Info_Panel_Wheel_BuildInfo;

    public bool Wheel_IsMoving;

    public GameObject Product_Info;
    public GameObject Battle_Info;
    public GameObject Living_Info;
    public GameObject Upkeep_Info;
    public GameObject OtherBuff_Info;
    public GameObject Addon_Info;
     

    public List<SelectedTilePanelSlot> Product_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> Battle_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> Living_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> Upkeep_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> OtherBuff_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> Addon_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<ResourceTooltip> Addon_Info_Panel_ToolTip = new List<ResourceTooltip>();

    public SteelWhaleManager.Room_Info Selected_Room;

    public static StealWhalePanel instance = null;

    public void Awake()
    {

        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);


        DontDestroyOnLoad(gameObject);

    }

    [System.Serializable]
    public class Room_Panel
    {
        public List<GameObject> PanelList = new List<GameObject>();
    }

    [System.Serializable]
    public class Room_Vertical
    {
        public List<RoomObject> Rooms_Vertical = new List<RoomObject>();
    }
    public List<Room_Vertical> Rooms_Horizontal = new List<Room_Vertical>();

    [System.Serializable]
    public class RoomObject
    {
        public GameObject This;
        public SteelWhaleManager.Room_Info Room_Infos;
        public Image RoomPanel_Image;
        public Image RoomPanel_Logo;
        //public Text Name;
        //public Text Contents;
    }
    
    public void Check_Product(SteelWhaleManager.Room_Info a)
    {
        a.Product_Type_Current = new List<int>(a.Product_Type);
        a.Product_Value_Current = new List<float>(a.Product_Value);
        a.Upkeep_Type_Current = new List<int>(a.Upkeep_Type);
        a.Upkeep_Value_Current = new List<float>(a.Upkeep_Value);
        if (a.AddOns.Count > 0)
        {
            for (int i = 0; i < a.AddOns.Count; i++)
            {

                //생산 체크하는 부분

                //선실이 생산하는 것이 없고, 갱신한 자원생산 목록도 없고, 애드온 자원생산이 존재할 때
                if (a.Product_Type.Count == 0 && a.Product_Type_Current.Count == 0 && a.AddOns[i].Product_Value != 0)
                {
                    a.Product_Type_Current.Add(a.AddOns[i].Product_Type);
                    a.Product_Value_Current.Add(a.AddOns[i].Product_Value);
                }
                //갱신한 자원생산 목록이 있고, 애드온 자원생산이 존재할 때
                else if (a.Product_Type_Current.Count != 0 && a.AddOns[i].Product_Value != 0)
                {
                    //갱신한 자원생산 목록에 애드온 자원생산과 같은 종류가 없으면
                    if (a.Product_Type_Current.Contains(a.AddOns[i].Product_Type) == false)
                    {
                        a.Product_Type_Current.Add(a.AddOns[i].Product_Type);
                        a.Product_Value_Current.Add(a.AddOns[i].Product_Value);
                    }
                    //갱신한 자원생산 목록에 애드온 자원생산과 같은 종류가 있으면
                    else
                    {
                        a.Product_Value_Current[a.Product_Type_Current.FindIndex(x => x == a.AddOns[i].Product_Type)] += a.AddOns[i].Product_Value;
                    }
                }

                //유지비 체크하는 부분

                //선실이 소비하는 것이 없고, 갱신한 자원소비 목록도 없고, 애드온 자원소비가 존재할 때
                if (a.Upkeep_Type.Count == 0 && a.Upkeep_Type_Current.Count == 0 && a.AddOns[i].Upkeep_Value != 0)
                {
                    a.Upkeep_Type_Current.Add(a.AddOns[i].Upkeep_Type);
                    a.Upkeep_Value_Current.Add(a.AddOns[i].Upkeep_Value);
                }
                else if (a.Upkeep_Type_Current.Count != 0 && a.AddOns[i].Upkeep_Value != 0)
                {
                    if (a.Upkeep_Type_Current.Contains(a.AddOns[i].Upkeep_Type) == false)
                    {
                        a.Upkeep_Type_Current.Add(a.AddOns[i].Upkeep_Type);
                        a.Upkeep_Value_Current.Add(a.AddOns[i].Upkeep_Value);
                    }
                
                    else
                    {
                        a.Upkeep_Value_Current[a.Upkeep_Type_Current.FindIndex(x => x == a.AddOns[i].Upkeep_Type)] += a.AddOns[i].Upkeep_Value;
                    }
                }
            }
        }
    }

    public void Room_Insitate() 
    {
        for (int i = 0; i < SWM.Line_Horizontal.Count; i++)
        {
            for(int z = 0; z < SWM.Line_Horizontal[i].ID.Count; z++)
            {
                SWM.Room_ID_Setting(Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos, SWM.Line_Horizontal[i].ID[z]);

                if (Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos.ID != "9999")
                {
                    Rooms_Horizontal[i].Rooms_Vertical[z].This.SetActive(true);
                    Rooms_Horizontal[i].Rooms_Vertical[z].RoomPanel_Logo.sprite = Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos.Room_Logo;
                    Rooms_Horizontal[i].Rooms_Vertical[z].RoomPanel_Image.sprite = Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos.Room_Image;
                    Check_Product(Rooms_Horizontal[i].Rooms_Vertical[z].Room_Infos);
                }
            }
        }
    }

    public void Room_Info_Panel_Move()
    {
        if(Wheel_IsMoving == false)
        {
            if(Wheel_IsUp == false || Selected_Room == null)
            {
                StartCoroutine(Wheel_Panel_Up());
            }
            StartCoroutine(Room_Info_Panel_Up());
            if (BuildInfo_IsUp == true)
            {
                StartCoroutine(Building_Info_Panel_Up());
            }
        }
       // int z = Selected_Room.BuildCost_Type.IndexOf(2);
        //Debug.Log(z);
    }

    public void Build_Info_Panel_Move()
    {
        if (Wheel_IsMoving == false)
        {
            WheelSound.PlaySound();
            StartCoroutine(Wheel_Panel_Up());
            StartCoroutine(Building_Info_Panel_Up());
            if (RoomInfo_IsUp == true)
            {
                StartCoroutine(Room_Info_Panel_Up());
            }
        }
    }

    public void Room_Building_Info_Panel_Move()
    {

    }

    public void Room_Info_Panel_Reset()
    {
        Selected_Room = null;
    }

    public void Room_Info_ReadingID(string a)
    {
        int x = int.Parse(a.Substring(0, 2));

        int y = int.Parse(a.Substring(2, 2));
        Selected_Room = Rooms_Horizontal[x].Rooms_Vertical[y].Room_Infos;

        StringBuilder checktext = new StringBuilder();
        float checkfloat = new float(); 

        Name0.text = Selected_Room.Name;
        Name1.text = Selected_Room.Name1;
        Contents.text = Selected_Room.Contents;

        Background.sprite = Selected_Room.Room_Image;
        Logo.sprite = Selected_Room.Room_Logo;

        for(int i = 0; i< 3; i++)
        {
            Upkeep_Info_Panel[i].Icon.SetActive(false);
            Product_Info_Panel[i].Icon.SetActive(false);
        }

        //유지비 체크
        if (Selected_Room.Upkeep_Type_Current.Count > 0)
        {
            Upkeep_Info.SetActive(true);
            for (int i = 0; i < Selected_Room.Upkeep_Type_Current.Count; i++)
            {
                checkfloat = new float();
                checktext = new StringBuilder();

                if (Selected_Room.Upkeep_Type.Count > i)
                {
                    checkfloat = Selected_Room.Upkeep_Value_Current[i] - Selected_Room.Upkeep_Value[i];
                }
                else if(Selected_Room.Upkeep_Type.Count <= i)
                {
                    checkfloat = Selected_Room.Upkeep_Value_Current[i];
                }
                Upkeep_Info_Panel[i].Icon.SetActive(true);
                Upkeep_Info_Panel[i].image.sprite = SWM.DM.Resource_Sprites_Logo_NonBackGround[Selected_Room.Upkeep_Type_Current[i]];

                checktext.Append("-").Append(Selected_Room.Upkeep_Value_Current[i]).Append("(-").Append(checkfloat).Append(")");
                Upkeep_Info_Panel[i].text.text = checktext.ToString();
            }
        }
        else
        {
            Upkeep_Info.SetActive(false);
            for(int i = 0; i < Upkeep_Info_Panel.Count; i++)
            {
                Upkeep_Info_Panel[i].Icon.SetActive(false);
            }
        }
        //생산비 체크
        if (Selected_Room.Product_Type_Current.Count > 0)
        {
            Product_Info.SetActive(true);
            for (int i = 0; i < Selected_Room.Product_Type_Current.Count; i++)
            {
                checkfloat = new float();
                checktext = new StringBuilder();

                if (Selected_Room.Product_Type.Count > i)
                {
                    checkfloat = Selected_Room.Product_Value_Current[i] - Selected_Room.Product_Value[i];
                }
                else if (Selected_Room.Product_Type.Count <= i)
                {
                    checkfloat = Selected_Room.Product_Value_Current[i];
                }
                Product_Info_Panel[i].Icon.SetActive(true);
                Product_Info_Panel[i].image.sprite = SWM.DM.Resource_Sprites_Logo_NonBackGround[Selected_Room.Product_Type_Current[i]];

                checktext.Append("+").Append(Selected_Room.Product_Value_Current[i]).Append("(+").Append(checkfloat).Append(")");
                Product_Info_Panel[i].text.text = checktext.ToString();
            }
        }
        else
        {
            Product_Info.SetActive(false);
            for (int i = 0; i < Product_Info_Panel.Count; i++)
            {
                Product_Info_Panel[i].Icon.SetActive(false);
            }
        }

        //주거옵션 체크
        Living_Info_Panel[0].text.text = Selected_Room.People_Capacity.ToString();
        Living_Info_Panel[1].text.text = Selected_Room.Default_Anti_Disaster.ToString();
        Living_Info_Panel[2].text.text = Selected_Room.Cog_Needed.ToString();
        Living_Info_Panel[3].text.text = Selected_Room.Operation_Needed.ToString();

        Battle_Info_Panel[0].text.text = Selected_Room.Battler_Capacity.ToString();
        Battle_Info_Panel[1].text.text = Selected_Room.Room_HP.ToString();
        Battle_Info_Panel[2].text.text = Selected_Room.Attack_Bonus.ToString();
        Battle_Info_Panel[3].text.text = Selected_Room.Deffense_Bonus.ToString();

        for (int i = 0; i < Addon_Info_Panel.Count; i++)
        {
            Addon_Info_Panel[i].Icon.SetActive(false);
        }

        //애드온 체크
        if(Selected_Room.AddOn_Capacity > 0)
        {
            for(int i = 0; i < Selected_Room.AddOn_Capacity; i++)
            {
                if (Selected_Room.AddOns.Count > i)// && Selected_Room.AddOns.Count > 0)
                {
                    Addon_Info_Panel[i].Icon.SetActive(true);
                    Addon_Info_Panel[i].image.DOFade(1f, 0f);
                    Addon_Info_Panel[i].image.sprite = Selected_Room.AddOns[i].Logo;
                    Addon_Info_Panel[i].text.text = Selected_Room.AddOns[i].Name;
                    Addon_Info_Panel_ToolTip[i].Name = Selected_Room.AddOns[i].Contents;
                    string b = "";
                    if (Selected_Room.AddOns[i].Product_Value != 0)
                    {
                        b += "생산품 : " + SWM.DM.resources(Selected_Room.AddOns[i].Product_Type, false) + " +" + Selected_Room.AddOns[i].Product_Value;
                    }
                    if(Selected_Room.AddOns[i].Upkeep_Value != 0)
                    {
                        if (Selected_Room.AddOns[i].Product_Value == 0)
                        {
                            b += "유지비 : " + SWM.DM.resources(Selected_Room.AddOns[i].Upkeep_Type, false) + " -" + Selected_Room.AddOns[i].Upkeep_Value;
                        }
                        else if(Selected_Room.AddOns[i].Product_Value != 0)
                        {
                            b += "\n" + "유지비 : " + SWM.DM.resources(Selected_Room.AddOns[i].Upkeep_Type, false) + " -" + Selected_Room.AddOns[i].Upkeep_Value;

                        }

                    }
                    Addon_Info_Panel_ToolTip[i].Contents = b;
                }
                else if (Selected_Room.AddOns.Count <= i)
                {
                    Addon_Info_Panel[i].Icon.SetActive(true);
                    Addon_Info_Panel[i].image.DOFade(0f, 0f);
                    Addon_Info_Panel[i].image.sprite = null;
                    Addon_Info_Panel[i].text.text = "애드온 없음";
                }
            }
        }
    }

    private IEnumerator Wheel_Panel_Up()
    {
        sequence0.Kill();
        Debug.Log("???");
        if (Wheel_IsMoving == false)
        {
            Wheel_IsMoving = true;
            if (Wheel_IsUp == false)
            {
                Wheel_IsUp = true;
                sequence0.Append(Room_Info_Panel_Wheel.transform.DORotate(new Vector3(0, 0, Room_Info_Panel_Wheel.transform.localEulerAngles.z +1), 0f, RotateMode.Fast).SetEase(AnimationCurve));
                sequence0.Append(Room_Info_Panel_Wheel.transform.DORotate(new Vector3(0, 0, 0), 1.2f, RotateMode.Fast).SetEase(AnimationCurve));
                sequence0.Play();
                yield return null;
            }
            else if (Wheel_IsUp == true)
            {
                Wheel_IsUp = false;
                sequence0.Append(Room_Info_Panel_Wheel.transform.DORotate(new Vector3(0, 0, 180), 1.2f, RotateMode.FastBeyond360).SetEase(AnimationCurve));
                sequence0.Play();
                yield return null;
            }
            yield return new WaitForSeconds(1.3f);
            Wheel_IsMoving = false;
        }
    }

    private IEnumerator Room_Info_Panel_Up()
    {
        sequence1.Kill();
            if (RoomInfo_IsUp == false)
            {
            RoomInfo_IsUp = true;
                sequence1.Append(Room_Info_Panel_Wheel_RoomInfo.transform.DORotate(new Vector3(0, 0, Room_Info_Panel_Wheel_RoomInfo.transform.localEulerAngles.z + 1), 0f, RotateMode.Fast).SetEase(AnimationCurve));
                sequence1.Append(Room_Info_Panel_Wheel_RoomInfo.transform.DORotate(new Vector3(0, 0, 0), 1.5f, RotateMode.Fast).SetEase(AnimationCurve));
                sequence1.Play();
                yield return null;
            }
            else if (RoomInfo_IsUp == true && Selected_Room == null)
            {
            RoomInfo_IsUp = false;
                sequence1.Append(Room_Info_Panel_Wheel_RoomInfo.transform.DORotate(new Vector3(0, 0, 180), 1.5f, RotateMode.FastBeyond360).SetEase(AnimationCurve));
                sequence1.Play();
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator Building_Info_Panel_Up()
    {
        Room_Info_Panel_Reset();
        sequence2.Kill();
            if (BuildInfo_IsUp == false)
            {
                BuildInfo_IsUp = true;
                sequence2.Append(Room_Info_Panel_Wheel_BuildInfo.transform.DORotate(new Vector3(0, 0, Room_Info_Panel_Wheel_BuildInfo.transform.localEulerAngles.z + 1), 0f, RotateMode.Fast).SetEase(AnimationCurve));
                sequence2.Append(Room_Info_Panel_Wheel_BuildInfo.transform.DORotate(new Vector3(0, 0, 0), 1.5f, RotateMode.Fast).SetEase(AnimationCurve));
                sequence2.Play();
                yield return null;
            }
            else if (BuildInfo_IsUp == true)
            {
                BuildInfo_IsUp = false;
                sequence2.Append(Room_Info_Panel_Wheel_BuildInfo.transform.DORotate(new Vector3(0, 0, 180), 1.5f, RotateMode.FastBeyond360).SetEase(AnimationCurve));
                sequence2.Play();
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);
    }
    public void Start()
    {
           sequence0 = DOTween.Sequence();
           sequence1 = DOTween.Sequence();
           sequence2 = DOTween.Sequence();
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonUp(1) && Wheel_IsUp == true)
        {
            Room_Info_Panel_Reset();
            if (RoomInfo_IsUp == true)
            {
                Room_Info_Panel_Move();
            }
            if (BuildInfo_IsUp == true)
            {
                Build_Info_Panel_Move();
            }
        }
    }

    public void OnEnable()
    {
        Room_Insitate();
        TochControll.SetActive(true);
    }
    public void OnDisable()
    {
        TochControll.SetActive(false);


        Room_Info_Panel_Reset();
        if (RoomInfo_IsUp == true)
        {
            Room_Info_Panel_Move();
        }
        if (BuildInfo_IsUp == true)
        {
            Build_Info_Panel_Move();
        }
    }
}
