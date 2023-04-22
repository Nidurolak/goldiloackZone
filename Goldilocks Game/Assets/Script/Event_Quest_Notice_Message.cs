using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Event_Quest_Notice_Message : UIMove, IPointerEnterHandler, IPointerExitHandler
{
    //11-03일 : 액티브 에리어(빌드코스트, 업킵 등등)에 따로 스프라이트 대조군을 찾도록 해야겠어. 이걸 생각 못했네

    public Vector2 DownPosition;
    public Vector2 UpPosition;

    public Datamanager DM;
    public GameObject ToolTip;
    public EventData Connected_EventData;

    public List<GameObject> ValueList = new List<GameObject>();

    public bool IsRefresh;
    public Image ReType1;
    public Text ReValue1;

    public Image ReType2;
    public Text ReValue2;

    public Image ReType3;
    public Text ReValue3;

    public Text Time;

    public Text EventName_SlotOnly;
    public string EventContents;

    public Image EventSprite;

    public int Turn_Count;
    
    public void Connectd_Event(EventData ev)
    {
        EventSprite.sprite = ev.Event_Logo_Sprite;
        //이건 차후 이벤트를 자세히 짤 때 슬롯 전용 이름을 넣을 것임
        EventName_SlotOnly.text = ev.EventName;

        if(ev.event_Type == EventData.Event_Type.Building_Notice ||
           ev.event_Type == EventData.Event_Type.Map_Notice ||
           ev.event_Type == EventData.Event_Type.Resource_Notice)
        {
            Debug.Log("발동");
            Debug.Log(ev.Turn_Count.ToString());
            Time.text = ev.Turn_Count.ToString();
            if(ev.ReType[0] != -1)
            {
                if(ev.Active_Area == EventData.Event_Active_Area.Building_All)
                {
                    //건물 파손된 이미지를 59번에 고정함. 일단은
                    //ReType1.sprite = DM.Resource_Sprites[59];
                    ReValue1.text = "건물 파손";
                }
                else
                {
                    //ReType1.sprite = DM.Resource_Sprites[ev.ReType[0]];
                    ReValue1.text = ev.ReValue[0].ToString();
                    ValueList[0].SetActive(true);
                }
            }
            if (ev.ReType[1] != -1)
            {
                //ReType2.sprite = DM.Resource_Sprites[ev.ReType[1]];
                ReValue2.text = ev.ReValue[1].ToString();
                ValueList[1].SetActive(true);
            }
            if (ev.ReType[2] != -1)
            {
                //ReType3.sprite = DM.Resource_Sprites[ev.ReType[2]];
                ReValue3.text = ev.ReValue[2].ToString();
                ValueList[2].SetActive(true);
            }
        }
        else if (ev.event_Type == EventData.Event_Type.Building_Choice ||
                 ev.event_Type == EventData.Event_Type.Map_Choice ||
                 ev.event_Type == EventData.Event_Type.Resource_Choice)
        {
             if (ev.choice[ev.Choice_Number].ReType[0] != -1)
             {
                //ReType1.sprite = DM.Resource_Sprites[ev.choice[ev.Choice_Number].ReType[0]];
                ReValue1.text = ev.choice[ev.Choice_Number].ReValue[0].ToString();
                ValueList[0].SetActive(true);
             }
             if (ev.choice[ev.Choice_Number].ReType[1] != -1)
             {
                //ReType2.sprite = DM.Resource_Sprites[ev.choice[ev.Choice_Number].ReType[1]];
                ReValue2.text = ev.choice[ev.Choice_Number].ReValue[1].ToString();
                ValueList[1].SetActive(true);
             }
             if (ev.choice[ev.Choice_Number].ReType[2] != -1)
             {
                //ReType3.sprite = DM.Resource_Sprites[ev.choice[ev.Choice_Number].ReType[2]];
                ReValue3.text = ev.choice[ev.Choice_Number].ReValue[2].ToString();
                ValueList[2].SetActive(true);
             }
        }
    }

    public override void Slot_Move()
    {
        //StartCoroutine(Slot_Move_Coroutine(ToolTip, , false, 1, 200, 0));
    }

    public IEnumerator Refresh_Event()
    {
        if(IsRefresh == false)
        {
            IsRefresh = true;
            Debug.Log("발동");
            StartCoroutine(Slot_Move_Coroutine(ToolTip, UpPosition, true, 0.4f));
            yield return new WaitForSeconds(3f);
            StartCoroutine(Slot_Move_Coroutine(ToolTip, DownPosition, false, 0.4f));
            IsRefresh = false;
            yield return null;
        }
    }

    public void OnEnable()
    {
        Connectd_Event(Connected_EventData);
        StartCoroutine(Refresh_Event());
    }

    public void OnDisable()
    {
        ToolTip.transform.localPosition = new Vector2(400,0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(IsRefresh == false)
        {
            StartCoroutine(Slot_Move_Coroutine(ToolTip, UpPosition, true, 0.4f));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(IsRefresh == false)
        {
            StartCoroutine(Slot_Move_Coroutine(ToolTip, DownPosition, false, 0.4f));
        }
    }
}
