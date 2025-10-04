using UnityEngine;

public class BestScoreService : MonoBehaviour
{
    public static BestScoreService Instance { get; private set; }
    [SerializeField] private string hiScoreKey = "hiscore_time";
    public int BestScore { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BestScore = PlayerPrefs.GetInt(hiScoreKey, 0);
    }

    public void TrySetBest(int score)
    {
        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt(hiScoreKey, BestScore);
            PlayerPrefs.Save();
        }
    }
}
