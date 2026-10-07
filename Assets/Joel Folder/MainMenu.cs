using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject firstSelectedOptionsButton;

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject optionsCanvas;

    [SerializeField] private ScreenFader fader;
    [SerializeField] private AudioClip playGameSound;

    public GameObject soundManager;

    private void OnEnable()
    {
        StartCoroutine(SelectFirstButtonNextFrame());
    }

    private IEnumerator SelectFirstButtonNextFrame()
    {
        // Waiting 1 frame ensures the EventSystem is fully initialized
        yield return null;

        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Clear focus first
            EventSystem.current.SetSelectedGameObject(firstSelectedButton); // Assign focus
        }
    }

    private void Awake()
    {
        mainMenuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayGame(string sceneName)
    {
        SoundManager.Instance.PlaySFX(playGameSound, 0.5f);
        fader.LoadScene(sceneName);
    }

    public void Options()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedOptionsButton);
    }


    public void Back()
    {
        if (optionsCanvas.activeSelf)
        {
            soundManager.GetComponent<SoundManager>().SaveVolumeSettings();

            optionsCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
