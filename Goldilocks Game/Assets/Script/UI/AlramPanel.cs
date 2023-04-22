using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class AlramPanel : MonoBehaviour
{
    public List<AlarmIcon> CautionList_Off = new List<AlarmIcon>();
    public List<AlarmIcon> CautionList_On = new List<AlarmIcon>();
    public List<string> Alarm_Contents = new List<string>();
    public List<bool> BellRinging = new List<bool>();
    public Datamanager DM;
    public AudioSource Bell;

    public void Add_Alarm(string a, bool b)
    {
        Alarm_Contents.Add(a);
        BellRinging.Add(b);
    }

    public IEnumerator Alarm_Call()
    {
        WaitForSeconds a = new WaitForSeconds(0.3f);
        if (Alarm_Contents.Count >= 1)
        {
            int z = Alarm_Contents.Count;
            for (int i = 0; i < z; i++)
            {
                yield return a;
                StartCoroutine(Alarm_Active(CautionList_Off[i], Alarm_Contents[0], i, BellRinging[0]));
                Alarm_Contents.RemoveAt(0);
                BellRinging.RemoveAt(0);
            }
        }
    }

    public IEnumerator Alarm_Active(AlarmIcon a, string c, int i, bool isBell)
    {
        while (a.Icon.activeSelf == true)
        {
            i = i + 1;
            a = CautionList_Off[i];
        }
        a = CautionList_Off[i];
        a.Bell_Bool = isBell;
        //알람 울리기
        if(Bell == true)
        {
            Bell.Play();
        }
        a.Icon.SetActive(true);
        a.Icon_Background.DOFade(1f, 1f);
        a.Contents.text = c;
        a.Contents.DOFade(1f, 1f);
        CautionList_On.Add(a);
        yield return new WaitForSeconds(5f);
        a.Icon_Background.DOFade(0f, 1f);
        a.Contents.text = "";
        CautionList_On.Remove(a);
        a.Contents.DOFade(0f, 1f);
        yield return new WaitForSeconds(1.2f);
        a.Bell_Bool = false;
        a.Icon.SetActive(false);
        //BellRinging.RemoveAt(0);
    }

    [System.Serializable]
    public class AlarmIcon
    {
        public GameObject Icon;
        public Image Icon_Background;
        public Text Contents;
        public bool Bell_Bool;
    }
}
