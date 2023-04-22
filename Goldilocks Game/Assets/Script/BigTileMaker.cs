using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTileMaker : MonoBehaviour
{
    public List<Sprite> Sprite = new List<Sprite>();
    public Vector2[] Vector2points;
    private PolygonCollider2D polygonCollider;
    public TileManager TM;
    public GameObject TilesCleaner;
    public GameObject TilesMaker;
    public List<GameObject> CanterTiles = new List<GameObject>();
    public enum TileSize {Middle, Large, Huge }
    public TileSize TS;
    public enum Tilealignment {UpperLeft, RightLeft, LowerLeft, LowerRight}
    public Tilealignment TA;

    public int RandomNumCol;
    public int RandomNumRow;

    public void PositionSetting2()
    {
        CanterTiles.Clear();
        RandomNumCol = Random.Range(-(int)(TM.columns * 0.5f) + 4, (int)(TM.columns * 0.5f) - 4);
        RandomNumRow = Random.Range(4, (int)(TM.rows * 0.5f) - 2);
        Debug.Log(RandomNumCol + " asdsad  " + RandomNumRow);
        gameObject.transform.position = new Vector3(RandomNumCol * 2.55f, RandomNumRow * 1.3f, 0);
       
        switch (TM.mapStyle)
        {
            case TileManager.MapSize.small:
                Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                Vector2points[0] = new Vector2(0, 2.1f);
                Vector2points[1] = new Vector2(-3.7f, 0.22f);
                Vector2points[2] = new Vector2(0, -1.7f);
                Vector2points[3] = new Vector2(3.7f, 0.22f);
                gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                break;

            case TileManager.MapSize.middle:
                Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                Vector2points[0] = new Vector2(0, 3.4f);
                Vector2points[1] = new Vector2(-6.3f, 0.22f);
                Vector2points[2] = new Vector2(0, -2.9f);
                Vector2points[3] = new Vector2(6.3f, 0.22f);
                gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                break;

            case TileManager.MapSize.Large:
                Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
                Vector2points[0] = new Vector2(0, 4.68f);
                Vector2points[1] = new Vector2(-8.7f, 0.22f);
                Vector2points[2] = new Vector2(0, -4.25f);
                Vector2points[3] = new Vector2(8.7f, 0.22f);
                gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

                break;
        }
    }

    public void PositionSetting()
    {
        Debug.Log("포지션세팅 발동");
        CanterTiles.Clear();
        switch (TM.mapStyle)
        {
            case TileManager.MapSize.small:
                var a = TM.OnEnableTileList.Count * 0.25f;
                Debug.Log(a);
                for(int i = 0; i < TM.rows*0.5f - 1; i++)
                {
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) + 1 - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) + (TM.rows + TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) - (TM.rows + TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) + TM.rows + 1 - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) + (TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) - (TM.columns - 3) - i]);
                }
                for (int i = 0; i < CanterTiles.Count; i++)
                {
                    Debug.Log(CanterTiles[i].GetComponent<TileData>().Tile_Number);
                    CanterTiles[i].SetActive(false);
                }

                break;
            case TileManager.MapSize.middle:
                var b = TM.OnEnableTileList.Count * 0.25f;
                Debug.Log(b);
                for (int i = 0; i < TM.rows * 0.5f - 1; i++)
                {
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) + 1 - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) + (TM.rows + TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) - (TM.rows + TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) + TM.rows + 1 - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) + (TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) - (TM.columns - 3) - i]);
                }
                for(int i = 0; i < CanterTiles.Count - 1; i++)
                {
                    Debug.Log(CanterTiles[i].GetComponent<TileData>().Tile_Number);
                    CanterTiles[i].SetActive(false);
                }

                break;
            case TileManager.MapSize.Large:
                var c = TM.OnEnableTileList.Count * 0.25f;
                Debug.Log(c);
                for (int i = 0; i < TM.rows * 0.5f - 1; i++)
                {
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) + 1 - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) + (TM.rows + TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.25f) - (TM.rows + TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) + TM.rows + 1 - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) + (TM.columns - 3) - i]);
                    CanterTiles.Add(TM.OnEnableTileList[(int)(TM.OnEnableTileList.Count * 0.75f) - (TM.columns - 3) - i]);
                }
                for (int i = 0; i < CanterTiles.Count; i++)
                {
                    Debug.Log(CanterTiles[i].GetComponent<TileData>().Tile_Number);
                    CanterTiles[i].SetActive(false);
                }

                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
                CanterTiles.Add(collision.gameObject);
            
            //여기서 얻은 제거한 타일 정보를 타일 매니저에 전송해서 다시 요로코롬조로코롬
        }
    }

    public void OnEnable()
    {
        var a = 0f;
        var b = 0f;
        var c = 0f;
        var d = 0f;
        //타일 정렬을 결정
        switch (TA)
        {
            case Tilealignment.LowerLeft:
                break;
        }
        PositionSetting2();
        
        
        StartCoroutine(A());
    }

    public IEnumerator A()
    {
        Debug.Log("A발동");
        yield return new WaitForSecondsRealtime(0.6f);
        //TilesMaker.transform.position = CanterTiles[Random.Range(0, CanterTiles.Count)].GetComponent<TileData>().OriginalPosition;
        //TilesCleaner.transform.position = TilesMaker.transform.position;

        //스틸웨일 메이커일 경우
        if (TilesMaker.gameObject.name == "SteelWhaleMaker")
        {
            var a = TilesMaker.GetComponent<SteelWhaleMaker>().TA;
            var b = TilesMaker.GetComponent<SteelWhaleMaker>().grade;
            switch (a)
            {
                case SteelWhaleMaker.Tilealignment.UpperLeft:
                    switch (b)
                    {
                        case SteelWhaleMaker.Grade.Tier1:
                            break;
                        case SteelWhaleMaker.Grade.Tier2:
                            break;
                        case SteelWhaleMaker.Grade.Tier3:
                            break;
                        case SteelWhaleMaker.Grade.Tier4:
                            break;
                    }
                    break;
                case SteelWhaleMaker.Tilealignment.UpperRight:
                    switch (b)
                    {
                        case SteelWhaleMaker.Grade.Tier1:
                            break;
                        case SteelWhaleMaker.Grade.Tier2:
                            break;
                        case SteelWhaleMaker.Grade.Tier3:
                            break;
                        case SteelWhaleMaker.Grade.Tier4:
                            break;
                    }
                    break;
                case SteelWhaleMaker.Tilealignment.LowerLeft:
                    switch (b)
                    {
                        case SteelWhaleMaker.Grade.Tier1:
                            break;
                        case SteelWhaleMaker.Grade.Tier2:
                            break;
                        case SteelWhaleMaker.Grade.Tier3:
                            break;
                        case SteelWhaleMaker.Grade.Tier4:
                            break;
                    }
                    break;
                case SteelWhaleMaker.Tilealignment.LowerRight :
                    switch (b)
                    {
                        case SteelWhaleMaker.Grade.Tier1:
                            break;
                        case SteelWhaleMaker.Grade.Tier2:
                            break;
                        case SteelWhaleMaker.Grade.Tier3:
                            break;
                        case SteelWhaleMaker.Grade.Tier4:
                            break;
                    }
                    break;
            }
            yield return new WaitForSecondsRealtime(0.2f);
            TilesCleaner.SetActive(true);
            yield return new WaitForSecondsRealtime(0.2f);
            TilesMaker.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        gameObject.SetActive(false);
    }
}
