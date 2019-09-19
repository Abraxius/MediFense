using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer audiomixer;
    private float volumeSave;
    public Slider optionSlider;

    private void Start()
    {
        volumeSave = PlayerPrefs.GetFloat("volumen");
        audiomixer.SetFloat("volume", volumeSave);
        optionSlider.value = volumeSave;
    }

    public void SetVolume (float volume)
    {
        audiomixer.SetFloat("volume", volume);
        volumeSave = volume;
    }

    public void ResetUpgrades()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OptionBackButton()
    {
        PlayerPrefs.SetFloat("volumen", volumeSave);
    }
}
