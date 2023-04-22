using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    public Image Main_Image;
    public Image Tile_Image;
    public Text Start_text;
    public Text Over_text;
    public Text Over_text_Conetents;
    public Datamanager DM;
    public GameObject ResetButton;

    public enum Casues {AllDead, CantEscape, MaxCompliment, MinHope }
    public List<Casues> CS = new List<Casues>();

    public List<string> Cause = new List<string>();
    Coroutine a;

    public string CauseList()
    {
        string a = "";
        
        for(int i = 0; i < CS.Count; i++)
        {
            switch (CS[i])
            {
                case Casues.AllDead:
                    a = a + "\n" + "모든 시민이 죽었습니다. 당신을 포함해서요.";
                    break;
                case Casues.CantEscape:
                    a = a + "\n" + "탈출하는데 충분한 준비를 갖추지 못했습니다.";
                    break;
                case Casues.MaxCompliment:
                    a = a + "\n" + "불만이 최대치에 달해 당신은 탄핵당했습니다.";
                    break;
                case Casues.MinHope:
                    a = a + "\n" + "모든 시민이 죽었습니다.";
                    break;
            }
        }
        return a;
    }

    public IEnumerator StartText()
    {
        //text.DOText("본 게임은 2020-03-14일에 빌드한 미완성 테크 데모입니다. 게임 시스템이 얼마나 구현되었는지 보여주기 위한 목적으로 빌드되었으며 미확인된 버그가 다수 있을 수 있습니다.", 7f);

        Tile_Image.DOFade(1, 2);
        Start_text.DOFade(1, 4);
        yield return new WaitForSeconds(9f);
        Start_text.DOFade(0, 4);
        Tile_Image.DOFade(0, 4);
        //b.DOFade(0, 3);
        yield return new WaitForSecondsRealtime(3f);
        gameObject.SetActive(false);
    }

    public IEnumerator GameOver()
    {
        Over_text.text = CauseList();
        Main_Image.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(2f);
        Main_Image.DOFade(1, 4f);
        yield return new WaitForSeconds(4f);
        Over_text_Conetents.DOFade(1, 2f);
        yield return new WaitForSeconds(3f);
        Over_text.DOFade(1, 2f);
        yield return new WaitForSeconds(3f);
        ResetButton.SetActive(true);
        
    }

    public void Start()
    {
        a = StartCoroutine(StartText());
        DM.IsCinematic = true;
    }

    public void OnDisable()
    {
        DM.IsCinematic = false;
    }
}
