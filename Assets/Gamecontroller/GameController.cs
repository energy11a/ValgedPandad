using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum WinConditionType
{
    Score,
    SurviveTime
}

public class GameController : MonoBehaviour
{
    [Header("References")]
    public SimpleSpawner spawner;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float startDelay = 1f;

    [Header("Score")]
    public float score = 0f;
    public float crashes = 0f;

    [Header("Win Condition")]
    public WinConditionType winCondition = WinConditionType.Score;
    [Tooltip("Score needed to win (when Win Condition = Score)")]
    public float scoreToWin = 10f;
    [Tooltip("Seconds to survive to win (when Win Condition = SurviveTime)")]
    public float surviveTimeToWin = 60f;

    [Header("Lose Condition")]
    [Tooltip("How many crashes before you lose (0 = one crash = instant lose)")]
    public int maxCrashes = 0;

    [Header("UI")]
    [Tooltip("Näitab mängijale, mitu autot on veel vaja maha ajada või kui kaua ellu jääda")]
    public TextMeshProUGUI objectiveText;
    [Tooltip("Lühike juhis mängijale, mida teha tuleb")]
    public TextMeshProUGUI instructionText;
    public GameObject startScreen;
    public GameObject winScreen;
    public GameObject loseScreen;

    public string[] music;

    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onWin;
    public UnityEvent onLose;

    public static GameController Instance;

    bool gameStarted = false;
    bool gameOver = false;
    float surviveTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (startScreen != null) startScreen.SetActive(true);
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        
        score = 0f;
        crashes = 0f;
        gameOver = false;
        surviveTimer = 0f;
        UpdateUI();

        if (startScreen != null) startScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);

        InvokeRepeating(nameof(SpawnObject), startDelay, spawnInterval);
        onGameStart?.Invoke();
    }

    void Update()
    {
        AudioManager.instance.PlayRandomSounds(music);

        if (!gameStarted || gameOver) return;

        if (winCondition == WinConditionType.SurviveTime)
        {
            surviveTimer += Time.deltaTime;
            UpdateUI();
            if (surviveTimer >= surviveTimeToWin)
            {
                Win();
            }
        }
    }

    void SpawnObject()
    {
        if (gameOver) return;

        if (spawner != null)
        {
            spawner.Spawn();
        }
    }

    public void IncrementScore(float amount = 1)
    {
        if (gameOver) return;

        score += amount;
        UpdateUI();

        if (winCondition == WinConditionType.Score && score >= scoreToWin)
        {
            Win();
        }
    }

    public void Incrementcrashes(float amount = 1)
    {
        if (gameOver) return;

        crashes += amount;
        UpdateUI();

        if (crashes > maxCrashes)
        {
            Lose();
        }
    }

    public void DecrementScore(float amount = 1)
    {
        score -= amount;
        UpdateUI();
    }

    public void SetScore(float value)
    {
        score = value;
        UpdateUI();
    }

    void Win()
    {
        if (gameOver) return;
        gameOver = true;

        CancelInvoke(nameof(SpawnObject));
        if (winScreen != null) winScreen.SetActive(true);
        onWin?.Invoke();
    }

    void Lose()
    {
        if (gameOver) return;
        gameOver = true;

        CancelInvoke(nameof(SpawnObject));
        if (loseScreen != null) loseScreen.SetActive(true);
        onLose?.Invoke();
    }

    public bool IsGameOver => gameOver;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {

        if (objectiveText != null)
        {
            if (winCondition == WinConditionType.Score)
            {
                float remaining = Mathf.Max(0, scoreToWin - score);
                objectiveText.text = $"Autosid veel maha ajada: {remaining:0}";
            }
            else
            {
                float remaining = Mathf.Max(0, surviveTimeToWin - surviveTimer);
                objectiveText.text = $"Jää ellu veel: {remaining:0} sek";
            }
        }

        if (instructionText != null)
        {
            if (winCondition == WinConditionType.Score)
                instructionText.text = $"Aja {scoreToWin:0} autot teelt maha, et võita!";
            else
                instructionText.text = $"Jää {surviveTimeToWin:0} sekundit ellu!";
        }
    }
}
