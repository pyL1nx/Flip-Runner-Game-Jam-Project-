using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Speed settings")]
    [SerializeField] private float baseObstacleSpeed = 2f;
    [SerializeField] private float speedIncreasePerSecond = 0.02f;

    [Header("State")]
    public bool IsGameActive { get; private set; } = true;
    public float CurrentObstacleSpeed { get; private set; }

    [Header("Score")]
    [SerializeField] private string hiScoreKey = "hiscore_time";
    [SerializeField] private bool autoStartScoring = true;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiScoreText;

    public int Score { get; private set; }
    public int HiScore { get; private set; }

    private float scoreTimer = 0f; // To accumulate delta time to increment score by 1

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject inGameScoreRoot;

    private bool cursorLocked = true;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        CurrentObstacleSpeed = baseObstacleSpeed;

        // Load persisted hi-score on boot
        HiScore = PlayerPrefs.GetInt(hiScoreKey, 0);
        UpdateHiScoreUI();
        UpdateScoreUI();

        // Initial UI states
        if (gameOverUI) gameOverUI.SetActive(false);
        if (inGameScoreRoot) inGameScoreRoot.SetActive(true);

        // Lock cursor at start
        SetCursorLock(true);
    }

    private void OnEnable()
    {
        if (autoStartScoring) StartScoring();
    }

    private void Update()
    {
        if (!IsGameActive) return;

        // Toggle cursor lock with ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorLock(!cursorLocked);
        }

        // Difficulty ramp
        CurrentObstacleSpeed += speedIncreasePerSecond * Time.deltaTime;

        // Increase Score by 1 per each full second or based on time accumulation
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            Score += Mathf.FloorToInt(scoreTimer);
            scoreTimer -= Mathf.Floor(scoreTimer);

            UpdateScoreUI();

            // Live update highest score if surpassed
            if (Score > HiScore)
            {
                HiScore = Score;
                PlayerPrefs.SetInt(hiScoreKey, HiScore);
                PlayerPrefs.Save();
                UpdateHiScoreUI();
            }
        }
    }

    private void SetCursorLock(bool shouldLock)
    {
        cursorLocked = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }

    public void StartScoring()
    {
        IsGameActive = true;
        if (Time.timeScale == 0f) Time.timeScale = 1f;

        if (inGameScoreRoot) inGameScoreRoot.SetActive(true);
        if (gameOverUI) gameOverUI.SetActive(false);

        UpdateScoreUI();
        UpdateHiScoreUI();
    }

    public void StopScoring()
    {
        IsGameActive = false;
        TrySaveHiScoreNow();
    }

    public void GameOver()
    {
        if (!IsGameActive) return;
        IsGameActive = false;

        // Unlock cursor automatically on game over
        SetCursorLock(false);

        TrySaveHiScoreNow();

        if (inGameScoreRoot) inGameScoreRoot.SetActive(false);
        if (finalScoreText)  finalScoreText.text = Score.ToString();

        Time.timeScale = 0f;
        if (gameOverUI) gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex, LoadSceneMode.Single);
    }

    public void GoToMenu()
    {
        if (Time.timeScale == 0f) Time.timeScale = 1f;
        TrySaveHiScoreNow();
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    private void TrySaveHiScoreNow()
    {
        if (Score > HiScore)
        {
            HiScore = Score;
            PlayerPrefs.SetInt(hiScoreKey, HiScore);
            PlayerPrefs.Save();
            UpdateHiScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText) scoreText.text = Score.ToString();
    }

    private void UpdateHiScoreUI()
    {
        if (hiScoreText) hiScoreText.text = HiScore.ToString();
    }
}
