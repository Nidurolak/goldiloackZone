using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AgeColor : MonoBehaviour
{
    public Datamanager DM;
    public List<Sprite> AgeList = new List<Sprite>();
    public List<string> AgeString = new List<string>();
    public List<string> AgeContents = new List<string>();
    [TextArea]
    public List<string> AgeEffect = new List<string>();

    public GameObject Panel;

    public Image Age_Sprite;
    public Image Age_Announce_Sprite;
    public Image Age_Announce_Sprite_Panel;
    public Text Age_Name_Text;
    public Text Age_Contents_Text;
    public Text Age_Effect_Text;

    public Slider slider;
    public Image slider_Back;
    public Image slider_Fillgage;
    public Image slider_Fill;
    
    public Coroutine coroutine;
    public WaitForSeconds wait = new WaitForSeconds(20f);

    public void Start()
    {
        Debug.Log("aas");
        //Age_Announce_Sprite.DOFade(1, 1f);
        StartCoroutine(first());
    }

    public IEnumerator first()
    {
        yield return new WaitForSeconds(14f);
        Announce_Age();
    }

    public void Announce_Age()
    {
        StartCoroutine(PanelOnOff());
        StartCoroutine(AlphaCurve_Image(Age_Announce_Sprite, 1f));
        StartCoroutine(AlphaCurve_Image(slider_Back, 1f));
        StartCoroutine(AlphaCurve_Image(slider_Fillgage, 1f));
        StartCoroutine(AlphaCurve_Image(slider_Fill, 1f));
        StartCoroutine(AlphaCurve_Image(Age_Announce_Sprite_Panel, 1f));

        StartCoroutine(AlphaCurve_Text(Age_Name_Text));
        StartCoroutine(AlphaCurve_Text(Age_Contents_Text));
        StartCoroutine(AlphaCurve_Text(Age_Effect_Text));
        switch (DM.stage_Escalation)
        {
            case Datamanager.Stage_Escalation.dawn:
                Age_Name_Text.text = AgeString[0];
                Age_Contents_Text.text = AgeContents[0];
                Age_Effect_Text.text = AgeEffect[0];
                slider.DOValue(1f, 4f, false);
                break;
            case Datamanager.Stage_Escalation.adaptive:
                Age_Name_Text.text = AgeString[1];
                Age_Contents_Text.text = AgeContents[1];
                Age_Effect_Text.text = AgeEffect[1];
                slider.DOValue(0.75f, 2f, false);
                break;
            case Datamanager.Stage_Escalation.twilight:
                Age_Name_Text.text = AgeString[2];
                Age_Contents_Text.text = AgeContents[2];
                Age_Effect_Text.text = AgeEffect[2];
                slider.DOValue(0.3f, 2f, false);
                break;
            case Datamanager.Stage_Escalation.exodus:
                Age_Name_Text.text = AgeString[3];
                Age_Contents_Text.text = AgeContents[3];
                Age_Effect_Text.text = AgeEffect[3];
                slider.DOValue(0f, 2f, false);
                break;
        }
    }

    public IEnumerator PanelOnOff()
    {
        //억지로 20초로 늘린 상태임 뿌잉뿌잉
        Panel.SetActive(true);
        Age_Announce_Sprite.gameObject.SetActive(true);
        Age_Announce_Sprite.transform.DORotate(new Vector3(0, 0, -360), 20f, RotateMode.FastBeyond360);
        yield return new WaitForSeconds(20f);
        Panel.SetActive(false);
        Age_Announce_Sprite.gameObject.SetActive(false);
    }

    public IEnumerator AlphaCurve_Image(Image target, float alpha)
    {
        target.DOFade(0, 0f);
        target.DOFade(alpha, 1f);
        yield return wait;
        target.DOFade(0, 1f);
    }
    public IEnumerator AlphaCurve_Text(Text a)
    {
        a.DOFade(0, 0f);
        a.DOFade(1, 1f);
        yield return wait;
        a.DOFade(0, 1f);
    }




}
