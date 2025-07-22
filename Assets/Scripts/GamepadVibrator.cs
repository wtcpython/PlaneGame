using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibrator : MonoBehaviour
{
    public static GamepadVibrator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Vibrate(float lowFrequency, float highFrequency, float duration)
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(StopVibration(duration));
        }
    }

    private IEnumerator StopVibration(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        Gamepad gamepad = Gamepad.current;
        gamepad?.SetMotorSpeeds(0f, 0f);
    }
}
