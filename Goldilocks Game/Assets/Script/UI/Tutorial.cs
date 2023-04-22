using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject Touchcontroll;
    public Image CutImage;
    public Text Flavor;

    public List<TutorialInfo> tutorialInfos = new List<TutorialInfo>();

    public void Setting0()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[0].CutImage;
        Flavor.text = tutorialInfos[0].Flavor;
    }
    public void Setting1()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[1].CutImage;
        Flavor.text = tutorialInfos[1].Flavor;
    }
    public void Setting2()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[2].CutImage;
        Flavor.text = tutorialInfos[2].Flavor;
    }
    public void Setting3()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[03].CutImage;
        Flavor.text = tutorialInfos[3].Flavor;
    }
    public void Setting4()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[4].CutImage;
        Flavor.text = tutorialInfos[4].Flavor;
    }
    public void Setting5()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[5].CutImage;
        Flavor.text = tutorialInfos[5].Flavor;
    }
    public void Setting6()
    {
        CutImage.SetNativeSize();
        CutImage.sprite = tutorialInfos[6].CutImage;
        Flavor.text = tutorialInfos[6].Flavor;
    }
    public void Setting7()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[7].CutImage;
        Flavor.text = tutorialInfos[7].Flavor;
    }
    public void Setting8()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[8].CutImage;
        Flavor.text = tutorialInfos[8].Flavor;
    }
    public void Setting9()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[9].CutImage;
        Flavor.text = tutorialInfos[9].Flavor;
    }
    public void Setting11()
    {
        CutImage.rectTransform.sizeDelta = new Vector2(900, 900);
        CutImage.sprite = tutorialInfos[10].CutImage;
        Flavor.text = tutorialInfos[10].Flavor;
    }

    public void OnEnable()
    {
        Setting0();
    }
}


[System.Serializable]
public class TutorialInfo
{
    public Sprite CutImage;
    public string Flavor;

}
