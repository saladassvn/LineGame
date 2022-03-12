using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    public Slider slider;
    public Slider musicSlider;

    private void Start()
    {
        SetUISliderVolumn();
    }
    public void SetUISliderVolumn()
    {
        if (PlayerPrefs.HasKey("volumn") == false)
        {
            slider.value = 0f;
        }
        else
        {
            slider.value = PlayerPrefs.GetFloat("volumn");
        }
        if (PlayerPrefs.HasKey("musicVolumn") == false)
        {
            musicSlider.value = 0f;
        }
        else
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolumn");
        }
        

    }
    public AudioMixer audioMixer;
    public AudioMixer musicAudioMixer;
    public void SetVolume(float volumn)
    {
        PlayerPrefs.SetFloat("volumn", volumn);
        audioMixer.SetFloat("Volumn", volumn);
       
    }

    public void SetMusicVolumn(float volumn)
    {
        PlayerPrefs.SetFloat("musicVolumn", volumn);
        musicAudioMixer.SetFloat("Volumn", volumn);
    }
}
