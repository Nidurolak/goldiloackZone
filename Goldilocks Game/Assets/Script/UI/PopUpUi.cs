using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PopUpUi : MonoBehaviour
{
    public GameObject This;

    public SpriteRenderer This_Logo;

    public SpriteRenderer Logo;

    public SpriteRenderer PlusLogo;

    public void OnEnable()
    {
        This_Logo.DOFade(1f,0f);
        Logo.DOFade(1f,0f);
        PlusLogo.DOFade(1f,0f);
        StartCoroutine("Play");
    }

    public void OnDisable()
    {
        gameObject.transform.position = new Vector3(0, 0.3f, 0.08f);
        Logo.sprite = null;
    }

    public IEnumerator Play()
    {
        gameObject.transform.DOLocalMoveY(gameObject.transform.position.y + 0.8f, 0.8f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(2f);
        This_Logo.DOFade(0f, 1f);
        Logo.DOFade(0f, 1f);
        PlusLogo.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
