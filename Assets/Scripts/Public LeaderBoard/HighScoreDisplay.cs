using UnityEngine;
using TMPro;

public class HiScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hiScoreText;
    [SerializeField] private string hiScoreKey = "hiscore_time"; // Same key as in GameManager

    void Start()
    {
        int hiScore = PlayerPrefs.GetInt(hiScoreKey, 0);
        if (hiScoreText != null)
        {
            hiScoreText.text = $"Best Score: {hiScore}";
        }
    }
    public int GetHiScore()
    {
      return PlayerPrefs.GetInt("hiscore_time", 0);
    }

}
