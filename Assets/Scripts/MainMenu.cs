using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "SampleScene";

    private void Start()
    {
        // Keep any menu initialization here (no SDK calls).
        if (Time.timeScale == 0f) Time.timeScale = 1f;
    }

    // Hook this to the Play button
    public void PlayGame()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    // Hook this to a Back button used when returning from gameplay to menu, if applicable
    public void OnReturnedToMenu()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;
    }

    // Hook this to the Quit button
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
