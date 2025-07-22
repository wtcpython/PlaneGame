using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int bombCount = 3;
    private int score = 0;
    
    public static GameManager Instance {get; private set;}

    public GameState gameState = GameState.Game;

    public AudioSource useBombAudio;
    public AudioSource gameOverAudio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateBombCount(bombCount);
        ChangeGameState(true);
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.xButton.wasPressedThisFrame)
            {
                UseBomb();
                GamepadVibrator.Instance.Vibrate(0.5f, 1f, 0.8f);
            }
        }
        if (Keyboard.current.anyKey.isPressed)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                UseBomb();
            }
        }
    }

    public void UpdateBomb(int delta)
    {
        bombCount += delta;
        UIManager.Instance.UpdateBombCount(bombCount);
    }

    public void AddScore(int count)
    {
        this.score += count;
        UIManager.Instance.UpdateScore(this.score);
    }

    public void ChangeGameState(bool isActive)
    {
        if (isActive)
        {
            Time.timeScale = 1;
            gameState = GameState.Game;
        }
        else
        {
            Time.timeScale = 0;
            gameState = GameState.Pause;
        }
        AudioManager.Instance.SetAudioState(gameState);
    }

    private void UseBomb()
    {
        if (bombCount <= 0) return;
        UpdateBomb(-1);

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(enemy.hp);
        }
        useBombAudio.Play();
    }

    public void GameOver()
    {
        if (gameState == GameState.GameOver) return;
        ChangeGameState(false);
        gameState = GameState.GameOver;
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UIManager.Instance.ShowGameOverPanel(bestScore, score);

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
        gameOverAudio.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
