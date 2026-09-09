using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private AudioSource audioSource;

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
    }
    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
