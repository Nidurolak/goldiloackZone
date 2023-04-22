using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControll : MonoBehaviour
{
    public List<GameObject> TagetUI;
    public MouseControll MS;

    public IEnumerator Touch_Cor()
    {
        yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < TagetUI.Count; i++)
        {
            if (TagetUI[i].activeSelf == true)
            {
                TagetUI[i].SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }

    public void Touch()
    {
        StartCoroutine(Touch_Cor());
    }

    public void OnEnable()
    {
        MS.onUI = true;
    }

    public void OnDisable()
    {
        MS.onUI = false;
    }
}
