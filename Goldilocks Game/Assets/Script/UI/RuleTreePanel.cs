using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class RuleTreePanel : MonoBehaviour
{
    public Text NameText;
    public Text NameText2;
    public Text ContentsText;
    public Text CannotText;
    public Text WaitinhText;

    public GameObject CannotPanel;

    public Button OkButton;

    public Image Seal;
    public AudioSource SealSound;
    public AudioClip SealSound_Clip;

    public GameObject TochControll;

    public void Seal_Active(bool isFirst)
    {
        if (Seal.gameObject.activeSelf == false && isFirst == true)
        {
            SealSound.PlayOneShot(SealSound_Clip);
            Seal.gameObject.SetActive(true);
            Seal.rectTransform.DORotate(new Vector3(0, 0, -0), 0f);
            Seal.rectTransform.DORotate(new Vector3(0, 0, -20), 0.7f, RotateMode.Fast);
            Seal.rectTransform.sizeDelta = new Vector2(256, 256);
            Seal.rectTransform.DOSizeDelta(new Vector2(128, 128), 0.7f, false).SetEase(Ease.InCirc);
        }
        if(isFirst == false)
        {
            Seal.gameObject.SetActive(true);
        }
    }

    public void OnEnable()
    {
        TochControll.SetActive(true);
    }
    public void OnDisable()
    {
        TochControll.SetActive(false);
    }
}

//public class 테크매니저 참고