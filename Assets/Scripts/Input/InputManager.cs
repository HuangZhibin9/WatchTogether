using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public InputController inputController;

    public event Action PauseOrPlayButtonPressed;

    public event Action PauseOrPlayButtonReleased;

    private void Awake()
    {
        Instance = this;
        inputController = new InputController();

        inputController.Input.PauseOrPlay.started += (context) =>
        {
            if (context.control.path == "/Mouse/leftButton" && InBottomUI.isInBottomUIZone)
            {
                return;
            }
            PauseOrPlayButtonPressed?.Invoke();
        };
        inputController.Input.PauseOrPlay.canceled += (context) =>
        {
            if (context.control.path == context.action.bindings[0].path && InBottomUI.isInBottomUIZone)
            {
                return;
            }
            PauseOrPlayButtonReleased?.Invoke();
        };
    }

    private void OnEnable()
    {
        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
}