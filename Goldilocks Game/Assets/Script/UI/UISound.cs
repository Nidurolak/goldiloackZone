using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public AudioSource Speaker;

    public AudioClip InSound;
    public AudioClip OutSound;
    public bool InCheck;

    public bool NoInOut;
    public AudioClip BasicSound;
    // Start is called before the first frame update


    public void PlaySound()
    {
        if (Speaker.isPlaying == false)
        {
            if (NoInOut == false)
            {
                //들어가는 사운드
                if (InCheck == false)
                {
                    InCheck = true;
                    Speaker.PlayOneShot(InSound);
                }
                //나오는 사운드
                else if (InCheck == true)
                {
                    InCheck = false;
                    Speaker.PlayOneShot(OutSound);
                }
            }
            else if (NoInOut == true)
            {
                //Speaker.clip = BasicSound;
                Speaker.PlayOneShot(BasicSound);
            }
        }
    }
}
