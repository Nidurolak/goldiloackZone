using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class MouseControll : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float targetZoom;
    [SerializeField]
    private float zoomFactor;
    [SerializeField]
    private float zoomLerpSpeed;
    [SerializeField]
    private float xLimit;
    [SerializeField]
    private float yLimit;
    [SerializeField]
    private float xLimitCurrent;
    [SerializeField]
    private float yLimitCurrent;
    [SerializeField]
    private GameObject SkySound;

    public bool onUI;
    public void OnUI()
    {
        onUI = true;
    }
    public void OffUI()
    {
        onUI = false;
    }

    public bool CamMoving;
    public List<AudioSource> Construction_Sound = new List<AudioSource>();
    public List<AudioSource> Ground_Click_Sound = new List<AudioSource>();

    public GameObject TouchControll_0;
    public GameObject TouchControll_1;

    public float dragSpeed;

    private Vector3 dragOrigin;

    public GameObject AudioListener;
    public GameObject Tile_Selected;
    public GameObject Tile_Checker;
    public SelectedTile selectedTileScript;
    public GameObject BuildingAndSkill;
    public bool BuildingAndSkill_bool;

    public enum BuildingMod {None, Build, Destroy, Skill}
    public BuildingMod BM;

    public enum SteelWhaleRoomMod { None, Build, Destroy, Skill }
    public SteelWhaleRoomMod SWRM;

    public GameObject BuildingAndSkill_Selected;
    public GameObject UpsideMessage;
    public BuildingButtonData BBM;
    public SlotManager SM;
    public PanelOpener QPO;
    public ExplorerManager EM;
    public List<PanelOpener> ResourcePanel = new List<PanelOpener>();

    public delegate void UIMoveMent();
    public UIMoveMent UIMove;
    
    public UnityEvent MouseClick_Left_UP;
    public UnityEvent MouseClick_Right_UP;

    public delegate void assddd();
    assddd ass;

    public Button DestroyButton;

    private void Start()
    {
        UIMove += selectedTileScript.Slot_Move;
    }

    public void SWRM_Out(int i)
    {
        switch (i)
        {
            case 0:
                SWRM = SteelWhaleRoomMod.None;
                break;
            case 1:
                SWRM = SteelWhaleRoomMod.Build;
                break;
            case 2:
                SWRM = SteelWhaleRoomMod.Destroy;
                break;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown("x"))
        {
                EM.exPanel.Slot_Move();
        }
        if (Input.GetKeyDown("c"))
        {
            DestroyButton.onClick.Invoke();
        }
        if (Input.GetKeyDown("space"))
        {
            if (Tile_Selected != null &&
                Tile_Selected.GetComponent<TileData>().Workable_OnTile() == true &&
                Tile_Selected.GetComponent<TileData>().IsDistory_OnTile == false)
            {
                selectedTileScript.Toggle_On();
                Debug.Log("스페이스 발동");
            }
        }

        Tile_Checker.transform.position = Input.mousePosition;
        //여러개가 클릭되는 게 안되게 해야한다. 이전에 만든 스크립트를 써야겠어
        
        //좌클릭 업
        if (Input.GetMouseButtonUp(0))
        {
            MouseClick_Left_UP.Invoke();
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (Tile_Selected != null && !IsPointerOverUiObject() && BuildingAndSkill_bool == false)
            {
                if //(Tile_Selected.GetComponent<TileData>().CR_Running == false &
                    (Tile_Selected.GetComponent<TileData>().UpCheck == true)
                {
                    Tile_Selected.GetComponent<TileData>().Tile_Click();
                    UIMove();
                }
                Tile_Selected = null;
            }

            if (!IsPointerOverUiObject())
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Floor") &
                        Tile_Selected != hit.collider.gameObject &
                        BuildingAndSkill_bool == false)
                    {
                        Tile_Selected = hit.collider.gameObject;
                        var a = hit.collider.GetComponent<TileData>();

                        if (a.CR_Running == false)
                        {
                            a.Tile_Click();
                            UIMove();
                            Ground_Click_Sound[0].Play();
                        }
                    }
                }
            }
            // 건물이나 스킬 건설 스크립트를 발동한다.
            if (//hit.collider != null &
                hit.collider.gameObject == BuildingAndSkill_Selected &
                BuildingAndSkill_bool == true &
                BBM.Check_Constructable_List.Contains(hit.collider.gameObject.GetComponent<TileData>()) == true &
                BBM.OnlyOne == false &
                onUI == false)
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    var a = hit.collider.gameObject;
                    //BBM.Change_TileData(hit.collider.gameObject.GetComponent<TileData>());

                    if (BM == BuildingMod.Build)
                    {
                        StartCoroutine(ChangeTile(a));
                        StartCoroutine(a.GetComponent<TileData>().Tile_Clicked());
                    }
                    if(BM == BuildingMod.Destroy &&
                       a.GetComponent<TileData>().Distrucktive == true &&
                       a.GetComponent<TileData>().Resource_using == false)
                    {
                        StartCoroutine(DistroyTile(a));
                        StartCoroutine(a.GetComponent<TileData>().Tile_Clicked());
                    }
                }
            }

        }

        //좌클릭 다운
        if (Input.GetMouseButtonDown(0))
        {
            //마우스 위치에서 광선을 발사한다.
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);


            // 건물이나 스킬 건설 스크립트를 발동한다.
            if (//hit.collider != null &
                BuildingAndSkill.CompareTag("building") &
                BuildingAndSkill_bool == true)
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    GameObject a = hit.collider.gameObject;
                    BuildingAndSkill_Selected = a;
                }
            }
            if (BM == BuildingMod.Destroy)
            {
                GameObject a = hit.collider.gameObject;
                BuildingAndSkill_Selected = a;
            }
        }

        //우클릭 다운
        if (Input.GetMouseButtonDown(1))
        {
        }

        //우클릭 업
        if (Input.GetMouseButtonUp(1))
        {
            MouseClick_Right_UP.Invoke();
            ClearTileSide();
            
        }
        SkySound.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, SkySound.transform.position.z);

        //카메라 줌인을 결정
        if (onUI == false && CamMoving == false)
        {
            float scrollData;
            scrollData = Input.GetAxis("Mouse ScrollWheel");
            targetZoom -= scrollData * zoomFactor;
            targetZoom = Mathf.Clamp(targetZoom, 2.5f, 7f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
            AudioListener.transform.position = new Vector3(gameObject.transform.position.x,
                                                           gameObject.transform.position.y,
                                                           (targetZoom - 1.5f) * 2f);
        }

        dragSpeed = cam.orthographicSize * 7f;

        //마우스 휠 클릭으로 화면 드래그
        if (Input.GetMouseButton(2) && onUI == false)
        {
            transform.position -= new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed,
                                              Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed,
                                              0.0f);
        }

        xLimitCurrent = xLimit * ((yLimit - Mathf.Abs(transform.position.y)) / yLimit);
        yLimitCurrent = yLimit * ((xLimit - Mathf.Abs(transform.position.x)) / xLimit);

        /*transform.position = new Vector3(Mathf.Clamp(transform.position.x,
                                                    -xLimit * ((yLimit - Mathf.Abs(transform.position.y)) / yLimit),
                                                    xLimit * ((yLimit - Mathf.Abs(transform.position.y)) / yLimit)
                                                    ),
                                        Mathf.Clamp(transform.position.y,
                                                    -yLimit * ((xLimit - Mathf.Abs(transform.position.x)) / xLimit),
                                                    yLimit * ((xLimit - Mathf.Abs(transform.position.x)) / xLimit)
                                                    ),
                                        -10f);*/

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, Mathf.Clamp(transform.position.x,
                                                    -xLimit * ((yLimit - Mathf.Abs(transform.position.y)) / yLimit),
                                                    xLimit * ((yLimit - Mathf.Abs(transform.position.y)) / yLimit)),
                                                    (yLimit - Mathf.Abs(transform.position.y)) / yLimit),
                                        Mathf.Lerp(transform.position.y, Mathf.Clamp(transform.position.y,
                                                    0,
                                                    yLimit * ((xLimit - Mathf.Abs(transform.position.x)) / xLimit)
                                                    ), (xLimit - Mathf.Abs(transform.position.x)) / xLimit),
                                        -10f);

    }

    public void ClearTileSide()
    {
        for (int i = 0; i < SM.TM.OnEnableTileList.Count; i++)
        {
            SM.TM.OnEnableTileList[i].GetComponent<TileData>().Side.color = new Color32(0, 0, 0, 0);
        }
    }

    public void OutBuildingMod()
    {
        if (SM.UpCheck == true)
        {
            SM.Slot_Move();
            for (int i = 0; i < 5; i++)
            {
                ResourcePanel[i].Slot_Move();
            }
        }

        if (QPO.DownCheck == true)
        {
            QPO.Slot_Move();
        }

        if (Tile_Selected != null & BuildingAndSkill_bool == false)
        {
            if (Tile_Selected.GetComponent<TileData>().CR_Running == false &
                Tile_Selected.GetComponent<TileData>().UpCheck == true)
            {
                UIMove();
                Tile_Selected.GetComponent<TileData>().Tile_Click();
            }
            Tile_Selected = null;
        }


        if (UpsideMessage.GetComponent<UpsideMessage>().MoveCheck == true)
        {
            UpsideMessage.GetComponent<UpsideMessage>().Slot_Move();
        }
        if(BuildingAndSkill != null)
        {
            var a = BuildingAndSkill.GetComponent<BuildingButtonData>();
            a.ConstructMode_Building_Out();
        }

    }

    public float calculating(float a, float b)
    {
        var c = 0f;
        if (b != 0)
        {
            c = Mathf.Floor(a) / Mathf.Floor(b);
        }
        return c;
    }

    public void SettingLimit()
    {
        xLimit = (SM.TM.columns) * 3.6f;
        yLimit = (SM.TM.rows+5) * 1.8f; 
    }

    //건물 짓는 스크립트
    public IEnumerator ChangeTile(GameObject a)
    {
        int i = Random.Range(0, 2);
        Construction_Sound[i].Play();
        BBM.Change_TileData(a.GetComponent<TileData>());
        yield return new WaitForSeconds(0.02f);
        SM.TM.Check_NearBy_Part(a);
        BBM.List_Initialize(BBM.BuildingType, a.GetComponent<TileData>());
        yield return new WaitForSeconds(0.02f);
        BBM.Check_Constructable();
        BBM.check_OnlyOne();
        BBM.Show_Constructable_tile();
        a.GetComponent<TileData>().Check_Disaster();
        a.GetComponent<TileData>().Play_DistructionOrBuild_Particle_On(true);

        //즉시 건설이면 파티클 발동
        if (a.GetComponent<TileData>().Turn_Count == 0)
        {
            a.GetComponent<TileData>().Play_DistructionOrBuild_Particle_On(false);
            //var ac = a.GetComponent<TileData>().BuildOrCrash_Particle.GetComponent<ParticleSystem>().main;
            //ac.loop = false;
        }
    }

    public IEnumerator DistroyTile(GameObject a)
    { 
        int i = Random.Range(0, 2);
        Construction_Sound[i].Play();

        //건물인지 자원인지 체크해서 파괴
        if (a.GetComponent<TileData>().BuildingType != 0)
        {
            Debug.Log("건물파괴시작");
            if(SM.DM.All_Resource_List[28] >= 1)
            {
                a.GetComponent<TileData>().Distroy_Building();
            }
        }
        else if (a.GetComponent<TileData>().ReType != 0)
        {
            Debug.Log("자원파괴시작");
            a.GetComponent<TileData>().Toggle_On();
        }
        yield return new WaitForSeconds(0.02f);
        SM.TM.Check_NearBy_Part(a);
        yield return new WaitForSeconds(0.02f);
        BBM.Check_Distructable();
        BBM.Show_Distructable_Tile();
    }

    //건물 버튼 누를 시 발동
    public void Slot_ShutDown()
    {
        //선택된 타일 정보 패널이 튀어나왔으면 안으로 밀어넣기
        if (selectedTileScript.MoveCheck == true)
        {
            UIMove();
            Tile_Selected.GetComponent<TileData>().Tile_Click();
        }
    }

    //카메라를 움직이는 함수, 목표 위치, 움직이는 시간, 안쓰는 플롯, 시작 전 시간
    public IEnumerator CamControll(float zoom, float timer, float rate, float delay)
    {
        CamMoving = true;
        yield return new WaitForSeconds(delay);
        WaitForEndOfFrame c = new WaitForEndOfFrame();
        DOTween.To(() => targetZoom, x => targetZoom = x, zoom, timer);
        yield return new WaitForSeconds(timer);
        CamMoving = false;
    }

    public void Slot_ShutDown_Turn_End()
    {
        ClearTileSide();
        if (SM.UpCheck == true)
        {
            SM.Slot_Move();
            for (int i = 0; i < 4; i++)
            {
                ResourcePanel[i].Slot_Move();
            }
        }

        if (UpsideMessage.GetComponent<UpsideMessage>().MoveCheck == true)
        {
            UpsideMessage.GetComponent<UpsideMessage>().Slot_Move();
        }

        if (Tile_Selected != null & BuildingAndSkill_bool == false)
        {
            if (Tile_Selected.GetComponent<TileData>().CR_Running == false &
                Tile_Selected.GetComponent<TileData>().UpCheck == true)
            {
                UIMove();
                Tile_Selected.GetComponent<TileData>().Tile_Click();
            }
            Tile_Selected = null;
        }
        if (BuildingAndSkill != null)
        {
            var a = BuildingAndSkill.GetComponent<BuildingButtonData>();
            if(a.BuildingType != 99)
            {
                a.ConstructMode_Building_Out();
            }
            else
            {
                a.DistructiveMode_Out();
            }
        }
        OutBuildingMod();
    }

        public bool IsPointerOverUiObject()//UI에 레이가 막히게 해준다.
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

    public void Awake()
    {
        StartCoroutine(CamControll(2.5f, 1f, 0.01f, 0.1f));
        StartCoroutine(CamControll(7, 4f, 0.01f, 12f));
        GameObject a = GameObject.Find("DataManager");
        a.GetComponent<Datamanager>().MS = this;
    }


}

