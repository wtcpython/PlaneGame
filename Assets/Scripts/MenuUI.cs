using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    private void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return;
        if (gamepad.aButton.wasPressedThisFrame)
        {
            OnStartButtonClick();
        }
        else if (gamepad.bButton.wasPressedThisFrame)
        {
            OnQuitButtonClick();
        }
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("PlaneGameScene");
    }

    public void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
