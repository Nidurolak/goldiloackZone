using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SteelWhaleTileCleaner : MonoBehaviour
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
        Debug.Log("포지션세팅 발동");
        //점 위치들이 이상해. 나중에 고쳐보자.
        switch (TA)
        {
            case Tilealignment.UpperLeft:

                switch (grade)
                {
                    case Grade.Tier1:
                        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                        Vector2points[0] = new Vector2((TM.columns - 3) * -2.4f - 0.9f, TM.rows * 1.3f);
                        Vector2points[1] = new Vector2((TM.columns - 1) * -2.4f, TM.rows * 1.3f);
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

                //해당 타일 정보(자원)을 다른 타일로 옮긴다. 밸런스 조절 차원
                if (collision.gameObject.GetComponent<TileData>().ReType != 0)
                {
                    var a = collision.gameObject.GetComponent<TileData>(); ;

                    a.ReType = 0;
                    a.ReVersion = 0;
                    a.Turn_Count = 0;
                    a.Distrucktive = false;
                    a.Needed_ManPower = 0;
                    a.Resource.sprite = null;
                }
                Debug.Log("타일클리너 발동");
            }
        }
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
    }

    public IEnumerator A()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        polygonCollider.enabled = false;
        yield return new WaitForSecondsRealtime(0.2f);
        polygonCollider.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        gameObject.SetActive(false);
    }
}
