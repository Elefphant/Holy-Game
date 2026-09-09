using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private string playScene;

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

    public void PlayGame()
    {
        SceneManager.LoadScene(playScene);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
