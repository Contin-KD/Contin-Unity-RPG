using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputSystem : MonoBehaviour
{
    private InputController _inputController;

    public Vector2 playerMovement => _inputController.PlayerInput.Movement.ReadValue<Vector2>();
    public Vector2 cameraLook => _inputController.PlayerInput.CameraLook.ReadValue<Vector2>();

    public bool playerLAtk => _inputController.PlayerInput.LAtk.triggered;
    public bool playerRAtk => _inputController.PlayerInput.RAtk.triggered;
    public bool playerJump => _inputController.PlayerInput.Jump.triggered;
    public bool playerDefen => _inputController.PlayerInput.Defen.phase == InputActionPhase.Performed;
    public bool playerCrouch => _inputController.PlayerInput.Crouch.triggered;

    private bool isRunButtonPressed;
    private float runButtonPressedTime;
    private float runButtonThreshold = 0.5f; 

    public bool playerRun { get; set; }
    public bool playerDodge { get; set; }

    private void Awake()
    {
        if (_inputController == null)
            _inputController = new InputController();

        _inputController.PlayerInput.Run.started += OnRunStarted;
        _inputController.PlayerInput.Run.canceled += OnRunCanceled;
    }

    private void OnEnable()
    {
        _inputController.Enable();
    }

    private void OnDisable()
    {
        _inputController.Disable();
    }

    private void OnRunStarted(InputAction.CallbackContext context)
    {
        isRunButtonPressed = true;
        runButtonPressedTime = Time.time;
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        isRunButtonPressed = false;
        float pressedDuration = Time.time - runButtonPressedTime;
        if (pressedDuration < runButtonThreshold)
        {
            playerDodge = true;
        }
        else
        {
            playerDodge = false;
        }
    }

    private void Update()
    {
        // 如果在按下时长超过阈值，则进入奔跑状态
        if (isRunButtonPressed && (Time.time - runButtonPressedTime >= runButtonThreshold))
        {
            playerRun = true;
        }
        else
        {
            playerRun = false;
        }
    }

    public void ResetDodge()
    {
        playerDodge = false;
    }
}
