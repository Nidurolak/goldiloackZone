using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChecker : MonoBehaviour
{
    public PolygonCollider2D PolCol;
    int b;
    public Vector2 Up;
    public Vector2 Down;
    public Vector2 Left;
    public Vector2 Right;
    public bool Check_Building;
    public bool Check_Electricity;
    public bool Check_Steam;
    public bool Check_Idea;
    public bool Check_Public;
    public bool Check_House;
    public bool Is_Off_Working;

    public TileData TD;

    public Vector2[] Vector2points;

    //옌 쓸모가 있는건지 모르겠어
    public void Add_Near_Resource()
    {
       switch(TD.ReType)
        {
            case 2:
                Vector2points[0] = new Vector2(0, 1.1f);
                Vector2points[1] = new Vector2(-1.9f, 0.16f);
                Vector2points[2] = new Vector2(0, -0.8f);
                Vector2points[3] = new Vector2(1.9f, 0.16f);
                break;
            case 3:
                Vector2points[0] = new Vector2(0, 1.1f);
                Vector2points[1] = new Vector2(-1.9f, 0.16f);
                Vector2points[2] = new Vector2(0, -0.8f);
                Vector2points[3] = new Vector2(1.9f, 0.16f);
                break;
            default:
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
           var a = collision.gameObject.GetComponent<TileData>();

        //지금 켜지는 이 에리어체크의 타일에 건물이 깔려있을 때
        if (Check_Building == true)
        {
            //StartCoroutine(collision.gameObject.GetComponent<TileData>().Tile_Jump());
            switch (TD.BuildingType)
            {
                case 4:
                    if (a.ReType == 3)
                    {
                        a.Resource_using = true;
                        a.Distrucktive = false;
                        TD.TM.On_Ore.Remove(a);
                        //StartCoroutine(collision.gameObject.GetComponent<TileData>().Tile_Jump());
                        Debug.Log("제외 발동");
                    }
                    break;
                case 9:
                    if (a.ReType == 2)
                    {
                        a.Resource_using = true;
                        a.Distrucktive = false;
                        TD.TM.On_Wood.Remove(a);
                        //StartCoroutine(collision.gameObject.GetComponent<TileData>().Tile_Jump());
                        Debug.Log("제외 발동");
                    }
                    break;
                default:
                    break;
            }
        }

        //전력망 체크일 때
        if (Check_Electricity == true)
        {
            if (TD.TM.NearBy_Elec_Grid.Contains(a) == false && TD.TM.On_Electricity.Contains(a) == false)
            {
                //collision.gameObject.GetComponent<TileData>().Side.color = new Color(255, 255, 255, 255);
                if (TD.TM.NearBy_Elec_Grid.Contains(a) == false)
                {
                    TD.TM.NearBy_Elec_Grid.Add(a);
                    collision.gameObject.GetComponent<TileData>().ElecOn_side();
                    collision.gameObject.GetComponent<TileData>().Electricity_Online = true;
                }
            }
        }
        //증기망 체크일 때
        if (Check_Steam == true)
        {
            if(TD.TM.NearBy_Steam_Grid.Contains(a) == false && TD.TM.On_Steam.Contains(a) == false)
            {
                TD.TM.NearBy_Steam_Grid.Add(a);
                collision.gameObject.GetComponent<TileData>().SteamOn_side();
                collision.gameObject.GetComponent<TileData>().Steam_Online = true;
            }
        }
        if (Check_House == true)
        {
            if (TD.TM.NearBy_House.Contains(a) == false)
            {
                TD.TM.NearBy_House.Add(a);
                collision.gameObject.GetComponent<TileData>().House_Online = true;
            }
        }
        if (Check_Idea == true)
        {
            if (TD.TM.NearBy_Steam_Grid.Contains(a) == false)
            {
                TD.TM.NearBy_Steam_Grid.Add(a);
                collision.gameObject.GetComponent<TileData>().Idea_Online = true;
            }
        }
        if (Check_Public == true)
        {
            if (TD.TM.NearBy_Steam_Grid.Contains(a) == false)
            {
                TD.TM.NearBy_Steam_Grid.Add(a);
                collision.gameObject.GetComponent<TileData>().Public_Online = true;
            }
        }
        //부모 타일의 자원 값을 받아와 체크한다.
        else if (Check_Building == false && Check_Electricity == false && Check_Steam == false)
        {
            if (TD.Resource_using == false)
            {
                switch (TD.ReType)
                {
                    case 2:
                        if (a.ReType == 0 && a.BuildingType == 0)
                        {
                            if (TD.TM.NearBy_Wood.Contains(a) == false)
                            {
                                TD.TM.NearBy_Wood.Add(a);
                            }
                        }
                        break;
                    case 3:
                        if (a.ReType == 0 && a.BuildingType == 0)
                        {
                            if (TD.TM.NearBy_Ore.Contains(a) == false)
                            {
                                TD.TM.NearBy_Ore.Add(a);
                            }
                        }
                        break;
                }
            }

            else if (TD.Resource_using == true)
            {
                switch (TD.ReType)
                 {
                     case 2:
                             if (TD.TM.NearBy_Wood.Contains(a))
                             {
                             //StartCoroutine(collision.gameObject.GetComponent<TileData>().Tile_Jump());
                                 TD.TM.NearBy_Wood.Remove(a);
                             }
                         break;
                     case 3:
                             if (TD.TM.NearBy_Ore.Contains(a))
                             {
                            //StartCoroutine(collision.gameObject.GetComponent<TileData>().Tile_Jump());
                             TD.TM.NearBy_Ore.Remove(a);
                             }
                         break;
                     default:
                         break;
                 }

             }
             
        }
    
    }

    public void Set_to_Default()
    {
        Vector2points[0] = Up;
        Vector2points[1] = Left;
        Vector2points[2] = Down;
        Vector2points[3] = Right;
    }

    public void OnEnable()
    {
        if(TD.UpCheck == true)
        {
            gameObject.transform.localPosition = new Vector2(0, -0.15f);
        }
        if(Check_Steam == true)
        {
            Debug.Log("으아아 증기!" + TD.TM.DM.All_Resource_List[9] + "추가 반경!");
            Vector2[] a = new Vector2[4];

            a[0] = new Vector2(Vector2points[0].x, Vector2points[0].y + (TD.TM.DM.All_Resource_List[9] * 1.06f));
            a[1] = new Vector2(Vector2points[1].x - (TD.TM.DM.All_Resource_List[9] * 2.25f), Vector2points[1].y);
            a[2] = new Vector2(Vector2points[2].x, Vector2points[2].y - (TD.TM.DM.All_Resource_List[9] * 1.29f));
            a[3] = new Vector2(Vector2points[3].x + (TD.TM.DM.All_Resource_List[9] * 2.25f), Vector2points[3].y);

            Debug.Log(a[0] + " " + a[1] + " " + a[2] + " " + a[3]);

            PolCol.points = a;
        }
        if (Check_Electricity == true)
        {
            Debug.Log("으아아 전기!" + TD.TM.DM.All_Resource_List[10] + "배!");
            Vector2[] a = new Vector2[4];

            a[0] = new Vector2(Vector2points[0].x, Vector2points[0].y + (TD.TM.DM.All_Resource_List[10] * 1.06f));
            a[1] = new Vector2(Vector2points[1].x - (TD.TM.DM.All_Resource_List[10] * 2.25f), Vector2points[1].y);
            a[2] = new Vector2(Vector2points[2].x, Vector2points[2].y - (TD.TM.DM.All_Resource_List[10] * 1.29f));
            a[3] = new Vector2(Vector2points[3].x + (TD.TM.DM.All_Resource_List[10] * 2.25f), Vector2points[3].y);

            Debug.Log(a[0] + " " + a[1] + " " + a[2] + " " + a[3]);

            PolCol.points = a;
        }
        if (Check_House == true)
        {
            Vector2[] a = new Vector2[4];

            a[0] = new Vector2(Vector2points[0].x, Vector2points[0].y + (TD.TM.DM.All_Resource_List[20] * 1.06f));
            a[1] = new Vector2(Vector2points[1].x - (TD.TM.DM.All_Resource_List[20] * 2.25f), Vector2points[1].y);
            a[2] = new Vector2(Vector2points[2].x, Vector2points[2].y - (TD.TM.DM.All_Resource_List[20] * 1.29f));
            a[3] = new Vector2(Vector2points[3].x + (TD.TM.DM.All_Resource_List[20] * 2.25f), Vector2points[3].y);

            PolCol.points = a;
        }
        if (Check_Public == true)
        {
            Vector2[] a = new Vector2[4];

            a[0] = new Vector2(Vector2points[0].x, Vector2points[0].y + (TD.TM.DM.All_Resource_List[18] * 1.06f));
            a[1] = new Vector2(Vector2points[1].x - (TD.TM.DM.All_Resource_List[18] * 2.25f), Vector2points[1].y);
            a[2] = new Vector2(Vector2points[2].x, Vector2points[2].y - (TD.TM.DM.All_Resource_List[18] * 1.29f));
            a[3] = new Vector2(Vector2points[3].x + (TD.TM.DM.All_Resource_List[18] * 2.25f), Vector2points[3].y);

            Debug.Log(a[0] + " " + a[1] + " " + a[2] + " " + a[3]);

            PolCol.points = a;
        }
        if (Check_Idea == true)
        {
            Debug.Log("으아아 사상!" + TD.TM.DM.All_Resource_List[16] + "배!");
            Vector2[] a = new Vector2[4];

            a[0] = new Vector2(Vector2points[0].x, Vector2points[0].y + (TD.TM.DM.All_Resource_List[16] * 1.06f));
            a[1] = new Vector2(Vector2points[1].x - (TD.TM.DM.All_Resource_List[16] * 2.25f), Vector2points[1].y);
            a[2] = new Vector2(Vector2points[2].x, Vector2points[2].y - (TD.TM.DM.All_Resource_List[16] * 1.29f));
            a[3] = new Vector2(Vector2points[3].x + (TD.TM.DM.All_Resource_List[16] * 2.25f), Vector2points[3].y);

            Debug.Log(a[0] + " " + a[1] + " " + a[2] + " " + a[3]);

            PolCol.points = a;
        }
        StartCoroutine(wait());
}

public IEnumerator wait()
{
//yield return new WaitForSecondsRealtime(0.5f);
yield return new WaitForFixedUpdate();
gameObject.SetActive(false);
}


    public void OnDisable()
    {
    Set_to_Default();
    if (TD.UpCheck == true)
    {
    gameObject.transform.localPosition = new Vector2(0, +0.15f);
    }
    if (TD.Resource_using == true)
    {
    switch (TD.ReType)
    {
        case 2:
        TD.TM.Check_NearBy_Resource(TD.TM.On_Wood);
        break;
        case 3:
        TD.TM.Check_NearBy_Resource(TD.TM.On_Ore);
        break;
    }

    }
Check_Building = false;
//타일 오브젝트가 비활성화할때 없애야겠어. 
Check_Electricity = false;
Check_Steam = false;
gameObject.transform.localPosition = new Vector2(0, 0f);
}
}
