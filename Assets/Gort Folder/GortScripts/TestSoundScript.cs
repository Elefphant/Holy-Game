using UnityEngine;

public class TestSoundScript : MonoBehaviour
{
    public static TestSoundScript Instance;

    [SerializeField] private AudioSource soundEffectObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundEffect(AudioClip audioClip, float volume)
    {
        AudioSource audioSource = Instantiate(soundEffectObject);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

}
