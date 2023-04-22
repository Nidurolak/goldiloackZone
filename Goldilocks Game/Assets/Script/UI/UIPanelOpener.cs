using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelOpener : MonoBehaviour
{
    public GameObject Panel;
    public List<GameObject> OtherPanel = new List<GameObject>();


    public void OpenPanel()
    {
        if(Panel != null)
        {
            Animator animator = Panel.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("OpenIt");

                animator.SetBool("OpenIt", !isOpen);
            }
        }
    }

    public void OnOffPanel()
    {
        if(Panel != null)
        {
            if(Panel.activeSelf == false)
            {
                Panel.SetActive(true);
            }
            else if(Panel.activeSelf == true)
            {
                Panel.SetActive(false);
            }
        }
    }

    public void OffPanel()
    {
        if (Panel != null)
        {
            if (Panel.activeSelf == true)
            {
                Panel.SetActive(false);
            }
        }
    }
}
