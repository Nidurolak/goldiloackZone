using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.IO;
using System;

public class ExplorerManagerasdasdadasd : MonoBehaviour
{
    public List<Explorer> ExplorerList = new List<Explorer>();
    public List<Message> baddddd = new List<Message>();

    public void Awake()
    {
        /*for (int i = 0; i < ExplorerList.Count; i++)
        {
            b.Add(new ExplorerEvent((int)ExplorerList[i].normalArea, (int)ExplorerList[i].eventArea, ExplorerList[i].Message.text));
            Debug.Log((int)ExplorerList[i].normalArea);
            Debug.Log(b[i].eventArea);
            Debug.Log(b[i].Report);
        }*/
    }

    public void Start()
    {
        Save();
    }

    public void Save()
    {
        JsonData ToJoson = JsonMapper.ToJson(baddddd);
        File.WriteAllText(Application.dataPath + "/Dialouge/ExplorerReport6.json", ToJoson.ToString());
        //ES3.Save<List<Message>>("ReportMessage", baddddd, Application.dataPath + "/Dialouge/ExplorerReport7");
    }
}

[Serializable]
public class Message1
{
    public enum NormalArea { None, Forest, Filed, SmallCity, BigCity, Mountain };
    public NormalArea normalArea;
    
    public enum EventArea { Idle, scenario1, scenario2, scenario3, scenario4, scenario5 };
    public EventArea eventArea;
    
    public string Report;

    public Message1(int NormalArea, int EventArea, string report)
    {
        normalArea = (Message1.NormalArea)NormalArea;
        eventArea = (Message1.EventArea)EventArea;
        Report = report;
    }
}

