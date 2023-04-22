using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventList", menuName = "SpawnEventDataList", order = 1)]
public class EventDataList : ScriptableObject
{
    public List<EventDataScriptableObject> List = new List<EventDataScriptableObject>();
}
