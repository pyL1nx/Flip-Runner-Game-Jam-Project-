using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dan.Main; // Namespace for LeaderboardCreator package
using UnityEngine.UI;

public class Top10Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> nameTexts;          // Assign 10 TMP_Text UI elements for player names
    [SerializeField] private List<TMP_Text> scoreTexts;         // Assign 10 TMP_Text UI elements for scores
    [SerializeField] private HiScoreDisplay hiScoreDisplay;     // Reference to your HiScoreDisplay script
    [SerializeField] private TMP_Text displayNameText;          // UI Text to display the player name
    [SerializeField] private TMP_InputField nameInputField;     // Input field for player to enter name
    [SerializeField] private Button submitNameButton;            // Button to submit player name

    private string publicLeaderboardKey = "Enter Your Key"; // Your leaderboard key
    private string playerName;
    private const string PlayerNameKey = "SavedPlayerName";
    private const float refreshInterval = 30f; // Refresh interval in seconds

    void Awake()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            playerName = PlayerPrefs.GetString(PlayerNameKey);
            if (displayNameText != null)
                displayNameText.text = playerName;

            if (nameInputField != null)
                nameInputField.text = playerName;
        }
        else
        {
            playerName = PlayerNameGenerator.GenerateRandomName();
            SavePlayerName(playerName);
            if (displayNameText != null)
                displayNameText.text = playerName;
            if (nameInputField != null)
                nameInputField.text = playerName;
        }

        if (submitNameButton != null)
            submitNameButton.onClick.AddListener(OnSubmitName);
    }

    void Start()
    {
        SubmitHiScoreAndRefresh();
        StartCoroutine(RefreshLeaderboardRoutine());
    }

    private IEnumerator RefreshLeaderboardRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            GetTop10Leaderboard();
        }
    }

    public void SubmitHiScoreAndRefresh()
    {
        if (hiScoreDisplay == null)
        {
            Debug.LogWarning("HiScoreDisplay reference not set.");
            GetTop10Leaderboard();
            return;
        }

        int hiScore = hiScoreDisplay.GetHiScore();

        SubmitScore(playerName, hiScore);
    }

    public void GetTop10Leaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (entries) =>
        {
            int maxEntries = Mathf.Min(entries.Length, 10);

            for (int i = 0; i < maxEntries; i++)
            {
                nameTexts[i].text = entries[i].Username;
                scoreTexts[i].text = entries[i].Score.ToString();
            }

            for (int i = maxEntries; i < 10; i++)
            {
                nameTexts[i].text = "";
                scoreTexts[i].text = "";
            }
        });
    }

    public void SubmitScore(string username, int score)
    {
        if (string.IsNullOrEmpty(username)) username = "Player";

        if (username.Length > 10)
            username = username.Substring(0, 10);

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, (message) =>
        {
            GetTop10Leaderboard();
        });
    }

    private void OnSubmitName()
    {
        if (nameInputField == null) return;

        string inputName = nameInputField.text.Trim();

        if (IsValidName(inputName))
        {
            playerName = inputName;
            SavePlayerName(playerName);

            if (displayNameText != null)
                displayNameText.text = playerName;

            Debug.Log("Player name updated to: " + playerName);

            SubmitHiScoreAndRefresh();
        }
        else
        {
            Debug.LogWarning("Invalid player name. Please use 1-10 alphanumeric characters.");
            // Optionally show UI warning here
        }
    }

    private bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (name.Length > 10) return false;

        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c))
                return false;
        }

        return true;
    }

    private void SavePlayerName(string name)
    {
        PlayerPrefs.SetString(PlayerNameKey, name);
        PlayerPrefs.Save();
    }
}

// PlayerNameGenerator class to generate random player names
public static class PlayerNameGenerator
{
    public static string GenerateRandomName()
    {
        int randomNumber = Random.Range(1000, 10000);
        return $"#player{randomNumber}";
    }
}
