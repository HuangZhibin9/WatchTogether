using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private InputController inputController;

    public event Action PauseOrPlayButtonPressed;

    private void Awake()
    {
        Instance = this;
        inputController = new InputController();

        inputController.Input.PauseOrPlay.performed += (context) =>
        {
            PauseOrPlayButtonPressed?.Invoke();
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