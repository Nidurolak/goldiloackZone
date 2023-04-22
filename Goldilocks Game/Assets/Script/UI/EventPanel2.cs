using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EventPanel2 : MonoBehaviour
{
    public Datamanager DM;
    public GameObject TouchControll;

    public Text Name;
    public Text Contents;
    public Image Logo;
    public Text CloseText;
    public Text NotificationText;

    public GameObject ResourcePanel;

    public List<GameObject> Resource_Icon = new List<GameObject>();
    public List<Image> Resource_Image = new List<Image>();
    public List<Text> Resource_Text = new List<Text>();

    public UnityEvent Show_Message;
    public List<EventPanelQue> EPL = new List<EventPanelQue>();


    public void Showing2(int reType, float reValue)
    {
        if (reType != -1)
        {
                    Resource_Icon[reType].SetActive(true);
                    //Resource_Image[reType].sprite = DM.Resource_Sprites[reType];
                    Resource_Text[reType].text = reValue.ToString();

        }
    }

    public void Clear_Showing()
    {
        EPL.RemoveAt(0);
        Debug.Log("클리어 쇼잉");
    }

    public void Showing()
    {
        Debug.Log("쇼잉");
        if (EPL.Count == 0)
        {
            StartCoroutine(CloseBackImage());
        }
        else
        {
            var a = EPL[0];
            Name.text = a.Name;
            Contents.text = a.Contents;
            Logo.sprite = a.Logo;
            CloseText.text = a.CloseText;
            NotificationText.text = a.NotificationText;
        }

        Clear_Showing();
    }

    //메시지를 추가하는 용도
    public void Add_Message(string name, string con,  Sprite logo, string closetext, string notifi, bool isExplorer)
    {
        EventPanelQue a = new EventPanelQue
        {
            Name = name,
            Contents = con,
            Logo = logo,
            CloseText = closetext,
            NotificationText = notifi
        };
        EPL.Add(a);
    }

    public IEnumerator CloseBackImage()
    {
        yield return new WaitForSeconds(0.01f);
        //yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
        TouchControll.SetActive(false);
    }

    public void OnEnable()
    {
        TouchControll.SetActive(true);
        TouchControll.GetComponent<Button>().onClick.AddListener(Showing);
        Showing();
    }

    public void OnDisable()
    {
        TouchControll.GetComponent<Button>().onClick.RemoveListener(Showing);
        //StartCoroutine(CloseBackImage());
    }


    [System.Serializable]
    public class EventPanelQue
    {
        public string Name;
        public string Contents;
        public Sprite Logo;
        public string CloseText;
        public string NotificationText;
        public bool isExplorer;
    }
}
