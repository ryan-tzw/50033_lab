using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private const string MasterVolumeParameter = "MasterVolume";
    private const string BgmVolumeParameter = "BGMVolume";
    private const string SfxVolumeParameter = "SFXVolume";

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        InitialiseSlider(masterVolumeSlider, MasterVolumeParameter);
        InitialiseSlider(bgmVolumeSlider, BgmVolumeParameter);
        InitialiseSlider(sfxVolumeSlider, SfxVolumeParameter);
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(MasterVolumeParameter, volume);
    }

    public void SetBgmVolume(float volume)
    {
        SetVolume(BgmVolumeParameter, volume);
    }

    public void SetSfxVolume(float volume)
    {
        SetVolume(SfxVolumeParameter, volume);
    }

    private void InitialiseSlider(Slider slider, string parameter)
    {
        float volume;

        if (PlayerPrefs.HasKey(parameter))
        {
            volume = PlayerPrefs.GetFloat(parameter);
        }
        else if (audioMixer.GetFloat(parameter, out float decibels))
        {
            volume = Mathf.Pow(10f, decibels / 20f);
        }
        else
        {
            volume = 1f;
        }

        slider.SetValueWithoutNotify(volume);
        audioMixer.SetFloat(parameter, LinearToDecibels(volume));
    }

    private void SetVolume(string parameter, float volume)
    {
        audioMixer.SetFloat(parameter, LinearToDecibels(volume));
        PlayerPrefs.SetFloat(parameter, volume);
    }

    private static float LinearToDecibels(float volume)
    {
        return volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f;
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}