using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void LoadScene(string sceneName, float fadeDuration = 1.5f)
    {
        StartCoroutine(TransitionRoutine(sceneName, fadeDuration));
    }

    private IEnumerator TransitionRoutine(string sceneName, float duration)
    {
        // 1. Fade to black
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 2. Load the new scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // 3. Fade back in
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
