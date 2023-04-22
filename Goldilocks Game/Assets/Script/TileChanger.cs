using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class TileChanger : MonoBehaviour
{

    public Sprite newSprite;
    public Vector2[] Vector2points;
    private PolygonCollider2D polygonCollider;

    public List<Sprite> floorSprites = new List<Sprite>();
    
    public string a;
    public int b;
    public enum What_To_Change_Type { Floor, Resource, Building, Event, Middle, Large, Huge};
    public What_To_Change_Type WTC;
    public int What_To_Change_Version;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == a)
        {
            SpriteRenderer Sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            Sprite.sprite = newSprite;

            //바닥타일의 버전을 바꾸도록 했지만 이후 이벤트 등의 변환을 줄 때에 대비해 바리에이션을 늘릴 수 있도록 할거야. 스위치는 어떨까.
            var VersionChange = collision.gameObject.GetComponent<TileData>();
            var a = Random.Range(0, 5);
            var b = Random.Range(0, 3);
            VersionChange.FloorVersion = b;
            VersionChange.Floor.sprite = floorSprites[b];
            if (VersionChange.ReType == 0)
            {
                if (a == 1 || a == 2 || a == 3)
                {
                    VersionChange.TM.OnEnableTileList_ResourceLayoutOnly.Remove(collision.gameObject);
                    var c = Random.Range(0, 4);
                    VersionChange.ReType = 2;
                    VersionChange.ReVersion = c;
                    VersionChange.Turn_Count = 1;
                    VersionChange.Distrucktive = true;
                    VersionChange.Needed_ManPower = 1;
                    VersionChange.Resource.sprite = VersionChange.TM.Resource_Sprites_Wood[c];
                    Debug.Log("타일 체인저- 나무 심기 성공");
                }
            }
        }
    }

    public void OnEnable()
    {
        Vector2points = gameObject.GetComponent<PolygonCollider2D>().points;
        Vector2points[0] = new Vector2(0, Random.Range(0, 3f));
        Vector2points[1] = new Vector2(Random.Range(0, -6f), 0.05f);
        Vector2points[2] = new Vector2(0, Random.Range(0, -3f));
        Vector2points[3] = new Vector2(Random.Range(0, 6f), -0.05f);

        gameObject.GetComponent<PolygonCollider2D>().points = Vector2points;

        b = 0;
        StartCoroutine(A());
    }

    public IEnumerator A()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        gameObject.SetActive(false);
    }
}
