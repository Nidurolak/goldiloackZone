using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;

public class ResourceTooltip : MonoBehaviour
{
    private WaitForSeconds wait = new WaitForSeconds(1f);
    public bool OnCheck;

    public ResourceTooltipBox ToolTipBox;
    public string Name;
    [TextArea]
    public string Contents;

    public Vector2 position;

    public void On()
    {
        OnCheck = true;
        StartCoroutine(TextCheck());
    }

    public void Off()
    {
        OnCheck = false;
        ToolTipBox.transform.position = new Vector2(2000, 2000);
    }

    public IEnumerator TextCheck()
    {
        yield return wait;
        if (OnCheck == true)
        {
            StringBuilder a = new StringBuilder();
            a.Append("<b>").Append(Name).Append("</b>").Append("\n").Append(Contents);
            string c = a.ToString();
            ToolTipBox.SetText(c);
        }
    }

}
