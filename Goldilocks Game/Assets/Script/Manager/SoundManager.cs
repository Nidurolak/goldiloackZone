using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public Datamanager DM;

    public AudioMixer Mixer;
    //public AudioMixer MixerGroup_BGM;
    //public AudioMixer MixerGroup_Effect;
    public AudioMixerGroup[] MixerGroup_Master = new AudioMixerGroup[0];
    public AudioMixerGroup[] MixerGroup_BGM = new AudioMixerGroup[0];
    public AudioMixerGroup[] MixerGroup_Effect = new AudioMixerGroup[0];
    
    public Slider BGMSlider;
    public Slider EffectSlider;
    public Slider MasterSlider;

    public void Awake()
    {
        Mixer = Resources.Load<AudioMixer>("Master");
        GameObject a = GameObject.Find("DataManager");
        DM = a.GetComponent<Datamanager>();
        a.GetComponent<Datamanager>().SM = this;
        GetBGMVolume();
        GetSFXVolume();
        GetMasterVolume();
    }

    public void GetBGMVolume()
    {
        float setvalue = PlayerPrefs.GetFloat("BGM");
        BGMSlider.value = setvalue;
    }
    public void GetSFXVolume()
    {
        float setvalue = PlayerPrefs.GetFloat("SFX");
        EffectSlider.value = setvalue;
    }
    public void GetMasterVolume()
    {
        float setvalue = PlayerPrefs.GetFloat("Master");
        MasterSlider.value = setvalue;
    }

    public void SetBGMVolume(float value)
    {
        Mixer.SetFloat("BGMVol", value);
        PlayerPrefs.SetFloat("BGM", value);
    }
    public void SetSFXVolume(float value)
    {
        Mixer.SetFloat("SFXVol", value);
        PlayerPrefs.SetFloat("SFX", value);
    }
    public void SetMasterVolume(float value)
    {
        Mixer.SetFloat("MasterVol", value);
        PlayerPrefs.SetFloat("Master", value);
    }
}
