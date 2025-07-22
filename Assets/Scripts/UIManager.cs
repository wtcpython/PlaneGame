using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bombCountText;
    public TextMeshProUGUI lifeNumText;
    
    public Button pauseButton;
    public Button resumeButton;
    
    public GameObject gameOverPanel;
    public GameObject settingsPanel;
    
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI finalScoreText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        pauseButton.onClick.AddListener(OnPauseButtonClick);
        resumeButton.onClick.AddListener(OnResumeButtonClick);
    }

    private void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return;
        if (gamepad.bButton.wasPressedThisFrame)
        {
            if (settingsPanel.activeSelf)
            {
                OnResumeButtonClick();
            }
            else
            {
                OnPauseButtonClick();
            }
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateBombCount(int count)
    {
        bombCountText.text = $"x {count}";
    }

    public void UpdateLiftNum(int num)
    {
        lifeNumText.text = $"x {num}";
    }

    private void OnPauseButtonClick()
    {
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        settingsPanel.SetActive(true);
        GameManager.Instance.ChangeGameState(false);
        AudioManager.Instance.PlayButtonClickAudio();
    }

    private void OnResumeButtonClick()
    {
        resumeButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        settingsPanel.SetActive(false);
        GameManager.Instance.ChangeGameState(true);
        AudioManager.Instance.PlayButtonClickAudio();
    }

    public void ShowGameOverPanel(int bestScore, int finalScore)
    {
        gameOverPanel.SetActive(true);
        bestScoreText.text = $"Best Score: {bestScore}";
        finalScoreText.text = $"Final Score: {finalScore}";
    }

    public void OnRestartButtonClick()
    {
        GameManager.Instance.RestartGame();
        AudioManager.Instance.PlayButtonClickAudio();
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.ExitGame();
        AudioManager.Instance.PlayButtonClickAudio();
    }
}
