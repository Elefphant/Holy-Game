using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject firstSelectedOptionsButton;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private ScreenFader fader;
    [SerializeField] private AudioClip playGameSound;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayGame(string sceneName)
    {
        SoundManager.Instance.PlaySFX(playGameSound, 0.5f);
        fader.LoadScene(sceneName);
    }

    public void Options()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedOptionsButton);
    }


    public void Back()
    {
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
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
