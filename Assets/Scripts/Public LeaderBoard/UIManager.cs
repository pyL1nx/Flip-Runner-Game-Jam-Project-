using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;      // Assign Main Menu UI GameObject
    [SerializeField] private GameObject leaderboardUI;   // Assign Leaderboard UI GameObject

    // Call this to show leaderboard and hide main menu (e.g., on Leaderboard button click)
    public void ShowLeaderboard()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (leaderboardUI != null) leaderboardUI.SetActive(true);
    }

    // Call this to return to main menu and hide leaderboard (e.g., on Back button click)
    public void ShowMainMenu()
    {
        if (leaderboardUI != null) leaderboardUI.SetActive(false);
        if (mainMenuUI != null) mainMenuUI.SetActive(true);
    }
}
