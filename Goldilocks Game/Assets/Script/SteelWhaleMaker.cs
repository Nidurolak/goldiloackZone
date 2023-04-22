using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SteelWhaleMaker : MonoBehaviour
{
    public GameObject SteelWhaleTile;
    public GameObject SteelWhalePositonSetting;
    public Vector2[] Vector2points;
    [SerializeField]
    private PolygonCollider2D polygonCollider;
    public Datamanager DM;
    public TileManager TM;
    public List<Sprite> Floor_T1 = new List<Sprite>();
    public List<Sprite> Floor_T2 = new List<Sprite>();
    public List<Sprite> Floor_T3 = new List<Sprite>();
    public enum Grade { Tier1, Tier2, Tier3, Tier4 }
    public Grade grade;
    public enum Tilealignment { UpperLeft, UpperRight, LowerLeft, LowerRight }
    public Tilealignment TA;

    public void PositionSetting()
    {
        gameObject.transform.position = SteelWhalePositonSetting.transform.position;
        //점 위치들이 이상해. 나중에 고쳐보자.
        switch (TA)
        {
            case Tilealignment.UpperLeft:

                switch (grade) 
                {
                    case Grade.Tier1:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0, 1.45f);
                        Vector2points[1] = new Vector2(-2.5f, 0.2f);
                        Vector2points[2] = new Vector2(0, -1.05f);
                        Vector2points[3] = new Vector2(2.5f, 0.2f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;

                    case Grade.Tier2:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(-0.62f, 1.40f);
                        Vector2points[1] = new Vector2(-1.89f, 0.78f);
                        Vector2points[2] = new Vector2(0, -0.15f);
                        Vector2points[3] = new Vector2(1.26f, 0.47f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;
                }
                break;
            case Tilealignment.UpperRight:
                switch (grade)
                {
                    case Grade.Tier1:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0, 1.45f);
                        Vector2points[1] = new Vector2(-2.5f, 0.2f);
                        Vector2points[2] = new Vector2(0, -1.05f);
                        Vector2points[3] = new Vector2(2.5f, 0.2f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;

                    case Grade.Tier2:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0.63f, 1.40f);
                        Vector2points[1] = new Vector2(-1.22f, 0.47f);
                        Vector2points[2] = new Vector2(0, -0.1f);
                        Vector2points[3] = new Vector2(1.89f, 0.78f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;
                }
                break;
            case Tilealignment.LowerLeft:
                switch (grade)
                {
                    case Grade.Tier1:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0, 1.45f);
                        Vector2points[1] = new Vector2(-2.5f, 0.2f);
                        Vector2points[2] = new Vector2(0, -1.05f);
                        Vector2points[3] = new Vector2(2.5f, 0.2f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;

                    case Grade.Tier2:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0, 0.47f);
                        Vector2points[1] = new Vector2(-1.89f, -0.46f);
                        Vector2points[2] = new Vector2(-0.63f, -1.08f);
                        Vector2points[3] = new Vector2(1.22f, 0.78f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;
                }
                break;
            case Tilealignment.LowerRight:
                switch (grade)
                {
                    case Grade.Tier1:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0, 1.45f);
                        Vector2points[1] = new Vector2(-2.5f, 0.2f);
                        Vector2points[2] = new Vector2(0, -1.05f);
                        Vector2points[3] = new Vector2(2.5f, 0.2f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;

                    case Grade.Tier2:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2(0, 0.47f);
                        Vector2points[1] = new Vector2(-1.26f, -0.15f);
                        Vector2points[2] = new Vector2(0.63f, -1.08f);
                        Vector2points[3] = new Vector2(1.89f, 0.80f);
                        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                        break;
                }
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Floor")
        {
            //해당 타일 정보(자원)을 다른 타일로 옮긴다. 밸런스 조절 차원
            if (collision.gameObject.GetComponent<TileData>().ReType != 0)
            {
                var a = collision.gameObject.GetComponent<TileData>();
                var b = Random.Range(0, TM.OnEnableTileList_ResourceLayoutOnly.Count);
                var c = TM.OnEnableTileList_ResourceLayoutOnly[b].GetComponent<TileData>();

                c.ReType = a.ReType;
                c.ReVersion = a.ReVersion;
                c.Turn_Count = a.Turn_Count;
                c.Distrucktive = a.Distrucktive;
                c.Needed_ManPower = a.Needed_ManPower;
                c.Resource.sprite = a.Resource.sprite;
                TM.OnEnableTileList_ResourceLayoutOnly.RemoveAt(b);
            }
            
            collision.gameObject.SetActive(false);
        }
    }

    //스틸웨일 복수 버전이 나오면 수정해야해. 지금은 1버전에만 한정되어 있어
    public void Setting_SteelWhale()
    {
        SteelWhaleTile.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.65f, SteelWhalePositonSetting.GetComponent<BigTileMaker>().RandomNumRow  * 0.01f + 0.005f); ;
        TileData a = SteelWhaleTile.GetComponent<TileData>();
        a.OriginalPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y +0.65f, (TM.columns + 1) * 0.005f);
        a.CurrentTilePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.65f, (TM.columns + 1) * 0.005f);
        a.FloorType = 99;
        a.BuildingType = 21;
        switch (TM.mapCondition)
        {
            case TileManager.MapCondition.red:
                a.Event_Desaster = -6;
                break;
            case TileManager.MapCondition.orange:
                a.Event_Desaster = -5;
                break;
            case TileManager.MapCondition.yellow:
                a.Event_Desaster = -4;
                break;
            case TileManager.MapCondition.green:
                a.Event_Desaster = -3;
                break;
            case TileManager.MapCondition.gold:
                a.Event_Desaster = 0;
                break;
        }
        a.Anti_Disaster = 1 + DM.All_Resource_List[53];
        a.Allow_Disaster = -15;
        a.BuildingVersion = (int)grade + 1;
        SteelWhaleTile.SetActive(true);
    }

    public void OnEnable()
    {
        PositionSetting();
        StartCoroutine(A());
    }
    public void OnDisable()
    {
        polygonCollider.enabled = true;
    }

    public void Awake()
    {

        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
    }

    public void Start()
    {
        SteelWhaleTile = Instantiate(SteelWhaleTile) as GameObject;
    }

    public IEnumerator A()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        polygonCollider.enabled = false;
        yield return new WaitForSecondsRealtime(2.1f);
        Setting_SteelWhale();
        TM.Add_NearAndOn_All();
        gameObject.SetActive(false);
    }
}
