using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider SFXSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();

        LoadVolumeSettings();
    }
    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume * (masterSlider.value * 2) * (SFXSlider.value * 2));
        }
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }
    public void LoadVolumeSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
