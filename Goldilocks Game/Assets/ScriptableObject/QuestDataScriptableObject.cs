using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "SpawnQuestData", order = 2)]
public class QuestDataScriptableObject_Resource : ScriptableObject
{
    //[System.Serializable]
    //public class Quest_Resource
    //{
    public Sprite Thumbnail;
    public bool Check_Clear_Before_End;
    public string Quest_Name;
    public string Quest_Start_Contents;
    public string Quest_Clear_Contents;
    public string Quest_Fail_Contents;
    public int TurnCount;

    public QuestDataScriptableObject_Resource Next_Quest_If_Clear;
    public QuestDataScriptableObject_Resource Next_Quest_If_False;

    public List<int> Repay_Type = new List<int>();
    public List<int> Repay_Value = new List<int>();
    public List<float> Repay_Multiply = new List<float>();
    public List<int> Penalty_Type = new List<int>();
    public List<int> Penalty_Value = new List<int>();
    public List<float> Penalty_Multiply = new List<float>();

    public List<Coroutine> Lines = new List<Coroutine>();
    
        public List<bool> condition0 = new List<bool>();

        public IEnumerator QuestLine()
        {
            yield return null;
        }

        [SerializeField]
        public List<Condition> CD;

        [System.Serializable]
        public class Condition 
        {
        //목표값 이상-달성, 목표값 이하-달성, 목표값 이상-유지, 목표값 이하-유지

        public string Condition_Contents;

        public enum ActiveType_category { satisfaction_Up_End, satisfaction_Down_End}
        public ActiveType_category Ac;
        public enum Condition_category { resource, tech, rule, building, SteelWhale_Prograss }
        public Condition_category Cc;
 
        public enum Balancing_category { House, Food, Mind, Rule, None }
        public Balancing_category BC;
        public int Building_Type;
        public int Building_Version;

        public int Condition_Type;

        public int Condition_Value;
        }

    //이건 그렇게 중요한게 아냐
    [SerializeField]
    public enum Quest_category { main, sub }

        //이게 좀 중요한데.....
    //}
}