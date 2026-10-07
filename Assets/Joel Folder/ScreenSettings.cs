using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle; // Reference to your UI Toggle

    private const string FULLSCREEN_PREF_KEY = "IsFullscreen";

    private void Start()
    {
        // Load saved state (default to 1 / true if it hasn't been saved yet)
        bool isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_PREF_KEY, 1) == 1;

        // Set screen mode
        Screen.fullScreen = isFullscreen;

        // Sync the UI Toggle checkbox without re-triggering events during Start
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
        }
    }

    // Call this from the UI Toggle's OnValueChanged event (Dynamic bool)
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        // Save state: 1 = true, 0 = false
        PlayerPrefs.SetInt(FULLSCREEN_PREF_KEY, isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
