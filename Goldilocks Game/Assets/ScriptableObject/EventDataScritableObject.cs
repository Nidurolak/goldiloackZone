using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//좀 더 큰 리스트를 만들고 집어넣어야한다.
[CreateAssetMenu(fileName = "Event", menuName = "SpawnEventData", order = 1)]
public class EventDataScriptableObject : ScriptableObject
{
        [System.Serializable]
        public enum Event_Type
        {
            Resource_Notice,
            Resource_Choice,
            Building_Notice,
            Building_Choice,
            Exploration_Notice,
            exploration_Choice,
            Map_Notice,
            Map_Choice,
            MakeTile_Notice,
            MakeTile_Choice
        }
        public Event_Type event_Type;

        [System.Serializable]
        public enum Event_Active_Type
        {
            None,
            Each_Turn,
            End_Countdown,
            During_Countdown,
            Active_End
        }
        public Event_Active_Type Active_Type;

        [System.Serializable]
        public enum Event_Trigger_Type
        {
            Single,
            Series,
            Single_Story,
            Series_Story
        }
        public Event_Trigger_Type Trigger_Type;

        //타일 적용일 시 범위를 설정
        public enum Event_Active_Area
        {
            Map,
            Resource_All,
            Resource,
            Building_All,
            Building_Production,
            Building_Upkeep,
            Building_BuildCost_,
            Data
        }
        public Event_Active_Area Active_Area;

        //적용 범위의 리스트 넘버를 찾음
        public int Area_List_Number;

        public Sprite Event_Logo_Sprite;
        public Sprite Event_Active_Sprite;
        public string EventName;
        public string EventContents;
        public string ValueChange_Text;
        public int Event_ID;

        public bool IsPercent;
        public bool IsProportion;
        public int ReType1;
        public int ReType2;
        public int ReType3;
        public float ReValue1;
        public float ReValue2;
        public float ReValue3;

        public Sprite EventSprite;

        //정수형 이벤트일 경우의 랜덤값
        public int Integer_RandomRange;
        //배율 이벤트일 경우 배율 값
        public int TypeNumber;
        public int Next_Series_Number;

        public bool DestroyBuilding;

        public int Turn_Count;
        public int EventNumber_Goto;
        //선택지를 골랐을 경우 번호를 따짐
        public int Choice_Number;

        [System.Serializable]
        public class Choice //: ICloneable
        {
            [System.Serializable]
            public enum Event_Active_Type
            {
                None,
                Each_Turn,
                End_Countdown,
                During_Countdown,
                Active_End
            }
            public Event_Active_Type Active_Type;

            [System.Serializable]
            public enum Event_Trigger_Type
            {
                Single,
                Series,
                Single_Story,
                Series_Story
            }
            public Event_Trigger_Type Trigger_Type;

            //타일 적용일 시 범위를 설정
            public enum Event_Active_Area
            {
                Map,
                Resource_All,
                Resource,
                Building_All,
                Building_Production,
                Building_Upkeep,
                Building_BuildCost_,
                Data
            }
            public Event_Active_Area Active_Area;

            public string ChoiceName;
            public string ChoiceContents;

            public bool isPercent;
            public int ReType1;
            public int ReType2;
            public int ReType3;
            public float ReValue1;
            public float ReValue2;
            public float ReValue3;
            public Sprite Event_Active_Sprite;
            public int TypeNumber;

            public bool DestroyBuilding;

            public int Turn_Count;

            public int ExplorerNumber;

            public int Next_Series_Number;

            public int EventNumber_Goto;

            public int EndingPoint;

            public Choice Clone()
            {
                Choice newCopy = new Choice();
                newCopy.Active_Type = this.Active_Type;
                newCopy.Active_Area = this.Active_Area;
                newCopy.Trigger_Type = this.Trigger_Type;
                newCopy.ChoiceName = this.ChoiceName;
                newCopy.ChoiceContents = this.ChoiceContents;
                newCopy.isPercent = this.isPercent;
                newCopy.ReType1 = this.ReType1;
                newCopy.ReType2 = this.ReType2;
                newCopy.ReType3 = this.ReType3;
                newCopy.ReValue1 = this.ReValue1;
                newCopy.ReValue2 = this.ReValue2;
                newCopy.ReValue3 = this.ReValue3;
                newCopy.DestroyBuilding = this.DestroyBuilding;
                newCopy.Turn_Count = this.Turn_Count;
                newCopy.ExplorerNumber = this.ExplorerNumber;
                newCopy.Next_Series_Number = this.Next_Series_Number;
                newCopy.EndingPoint = this.EndingPoint;
                newCopy.EventNumber_Goto = this.EventNumber_Goto;
                newCopy.TypeNumber = this.TypeNumber;

                return newCopy;
            }
        }

        public List<Choice> choice = new List<Choice>();
    }