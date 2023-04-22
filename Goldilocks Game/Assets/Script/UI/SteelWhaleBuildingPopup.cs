using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;

public class SteelWhaleBuildingPopup : MonoBehaviour
{

    private WaitForSeconds wait = new WaitForSeconds(1f);
    public bool OnCheck;
    public StealWhalePanel SPM;

    public ResourceTooltipBox NameBox0;
    public ResourceTooltipBox NameBox1;

    private Coroutine c;
    
    public List<SelectedTilePanelSlot> Product_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> BuildCost_Info_Panel = new List<SelectedTilePanelSlot>();
    public List<SelectedTilePanelSlot> Upkeep_Info_Panel = new List<SelectedTilePanelSlot>();

    [SerializeField]
    public CanvasGroup canvasGroup;

    public void SetPopUp()
    {
        if (OnCheck == true)
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.DOFade(1, 1f);
        }
        else if (OnCheck == false)
        {
            canvasGroup.DOFade(0, 0f);
        }

        Vector2 localPoint = new Vector2(Input.mousePosition.x - 200f, Input.mousePosition.y - 120f);
        transform.position = localPoint;

    }

    public void Setting_PopUp( string Num_And_Ver)//List<SelectedTilePanelSlot> targetpopuplist,
    {
        int a = int.Parse(Num_And_Ver.Substring(0, 2));
        int b = int.Parse(Num_And_Ver.Substring(2, 2));

        NameBox0.TextBox.text = SPM.SWM.Swbd[a].BD_Version[b].Name;
        Vector2 TextSize = new Vector2(NameBox0.TextBox.preferredWidth + 60f, 50f);
        NameBox0.rectTransform.sizeDelta = TextSize;
        NameBox1.TextBox.text = SPM.SWM.Swbd[a].BD_Version[b].Name1;

        if (SPM.SWM.Swbd[a].BD_Version[b].BuildCost_Type.Count != 0)
        {
            for (int i = 0; i < SPM.SWM.Swbd[a].BD_Version[b].BuildCost_Type.Count; i++)
            {
                BuildCost_Info_Panel[i].Icon.SetActive(true);
                BuildCost_Info_Panel[i].image.sprite = SPM.SWM.DM.Resource_Sprites_Logo_NonBackGround[SPM.SWM.Swbd[a].BD_Version[b].BuildCost_Type[i]];
                BuildCost_Info_Panel[i].text.text = SPM.SWM.Swbd[a].BD_Version[b].BuildCost_Value[i].ToString();
            }
        }
        if (SPM.SWM.Swbd[a].BD_Version[b].Product_Type.Count != 0)
        {
            for (int i = 0; i < SPM.SWM.Swbd[a].BD_Version[b].Product_Type.Count; i++)
            {
                Product_Info_Panel[i].Icon.SetActive(true);
                Product_Info_Panel[i].image.sprite = SPM.SWM.DM.Resource_Sprites_Logo_NonBackGround[SPM.SWM.Swbd[a].BD_Version[b].Product_Type[i]];
                Product_Info_Panel[i].text.text = SPM.SWM.Swbd[a].BD_Version[b].Product_Value[i].ToString();
            }
        }
        if (SPM.SWM.Swbd[a].BD_Version[b].Upkeep_Type.Count != 0)
        {
            for (int i = 0; i < SPM.SWM.Swbd[a].BD_Version[b].Upkeep_Type.Count; i++)
            {
                Upkeep_Info_Panel[i].Icon.SetActive(true);
                Upkeep_Info_Panel[i].image.sprite = SPM.SWM.DM.Resource_Sprites_Logo_NonBackGround[SPM.SWM.Swbd[a].BD_Version[b].Upkeep_Type[i]];
                Upkeep_Info_Panel[i].text.text = SPM.SWM.Swbd[a].BD_Version[b].Upkeep_Value[i].ToString();
            }
        }
    }

    public void Reset_PopUP()
    {
        for(int i = 0; i < Product_Info_Panel.Count; i++)
        {
            Product_Info_Panel[i].text.text = "";
            Product_Info_Panel[i].image.sprite = null;
            Product_Info_Panel[i].Icon.SetActive(false);
        }
        for (int i = 0; i < Upkeep_Info_Panel.Count; i++)
        {
            Upkeep_Info_Panel[i].text.text = "";
            Upkeep_Info_Panel[i].image.sprite = null;
            Upkeep_Info_Panel[i].Icon.SetActive(false);
        }
        for (int i = 0; i < BuildCost_Info_Panel.Count; i++)
        {
            BuildCost_Info_Panel[i].text.text = "";
            BuildCost_Info_Panel[i].image.sprite = null;
            BuildCost_Info_Panel[i].Icon.SetActive(false);
        }
    }

    //여기서 인트는 선실 ID 정보
    public void On(int a)
    {
        OnCheck = true;
        StartCoroutine(PopUp_Setting(a));
    }

    public void Off()
    {
        Debug.Log("발동");
        OnCheck = false;
        gameObject.transform.position = new Vector2(2000, 2000);
    }
    
    //a는 버튼 번호, 버튼 번호는 SWBSM에서 체크
    public IEnumerator PopUp_Setting(int a)
    {
        Reset_PopUP();
        yield return wait;
        if(OnCheck == true)
        {
            switch (a)
            {
                case 0:
                    break;
                case 1:
                    Debug.Log(SPM.SWBSM.BuildingID_List1[0]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List1[0]);

                    break;
                case 2:
                    Debug.Log(SPM.SWBSM.BuildingID_List1[1]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List1[1]);

                    break;
                case 3:
                    Debug.Log(SPM.SWBSM.BuildingID_List1[2]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List1[2]);

                    break;
                case 4:
                    Debug.Log(SPM.SWBSM.BuildingID_List1[3]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List1[3]);

                    break;
                case 5:
                    Debug.Log(SPM.SWBSM.BuildingID_List1[4]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List1[4]);

                    break;
                case 6:
                    Debug.Log(SPM.SWBSM.BuildingID_List2[0]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List2[0]);

                    break;
                case 7:
                    Debug.Log(SPM.SWBSM.BuildingID_List2[1]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List2[1]);

                    break;
                case 8:
                    Debug.Log(SPM.SWBSM.BuildingID_List2[2]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List2[2]);

                    break;
                case 9:
                    Debug.Log(SPM.SWBSM.BuildingID_List2[3]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List2[3]);

                    break;
                case 10:
                    Debug.Log(SPM.SWBSM.BuildingID_List2[4]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List2[4]);

                    break;
                case 11:
                    Debug.Log(SPM.SWBSM.BuildingID_List3[0]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List3[0]);

                    break;
                case 12:
                    Debug.Log(SPM.SWBSM.BuildingID_List3[1]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List3[1]);

                    break;
                case 13:
                    Debug.Log(SPM.SWBSM.BuildingID_List3[2]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List3[2]);

                    break;
                case 14:
                    Debug.Log(SPM.SWBSM.BuildingID_List3[3]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List3[3]);

                    break;
                case 15:
                    Debug.Log(SPM.SWBSM.BuildingID_List3[4]);
                    Setting_PopUp(SPM.SWBSM.BuildingID_List3[4]);

                    break;
                    //25까지?
            }
            //SPM.SWBSM
            SetPopUp();
        }
    }
}
