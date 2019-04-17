using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.Input;
using Vector2 = UnityEngine.Vector2;

public class PlayerInputModel {
    public static PlayerInputModel instance = new PlayerInputModel();
    
    #region InputSettings
    [SerializeField] public bool useLegacyKeyboardInput { get; set; } = false;
    public enum KeyboardInputMapping {
        UseSpaceToJump,
        UseSpaceToInteract,
    }
    [SerializeField]
    public KeyboardInputMapping keyboardInputMapping { get; set; } = KeyboardInputMapping.UseSpaceToJump;
    private bool useSpaceToJump => keyboardInputMapping == KeyboardInputMapping.UseSpaceToJump;
    private bool useSpaceToInteract => keyboardInputMapping == KeyboardInputMapping.UseSpaceToInteract;
    
    #endregion
    #region PropertiesToEnableAndDisableInputActions
    public enum InputMode {
        Game,
        UI
    }
    [SerializeField] public InputMode inputMode { get; set; } = InputMode.Game;
    [SerializeField] public bool movementDisabled { get; set; } = false;
    [SerializeField] public bool jumpDisabled { get; set; } = false;
    [SerializeField] public bool interactDisabled { get; set; } = false;
    [SerializeField] public bool cameraActionsDisabled { get; set; } = false;

    void Reset(InputMode mode) {
        inputMode = mode;
        movementDisabled = jumpDisabled = interactDisabled = cameraActionsDisabled = false;
    }
    public bool uiInputActive => inputMode == InputMode.UI;
    public bool gameInputActive => inputMode == InputMode.Game;
    public bool movementActive => gameInputActive && !movementDisabled;
    public bool jumpActive => gameInputActive && !jumpDisabled;
    public bool interactActive => gameInputActive && !interactDisabled;
    public bool cameraActionsActive => gameInputActive && !cameraActionsDisabled;
    
    #endregion

    public void DebugLogInput() {
        Debug.Log($"movement: {movement} (gamepad: {gamepadInputVector}), jumpPressed: {jumpPressed}, interactPressed: {interactPressed}");
    }
    
    
    
    public Vector2 movement => movementActive ? inputVector : Vector2.zero;
    public Vector2 navigation => uiInputActive ? inputVector : Vector2.zero;
    public bool jumpPressed => jumpActive ? isJumpPressed : false;
    public bool interactPressed => jumpActive ? isInteractPressed : false;

    #region MovementAndNavigation
    private Vector2 inputVector => Vector2.ClampMagnitude(
        (useLegacyKeyboardInput ? legacyInputVector : newKeyboardInputVector) +
        gamepadInputVector,
        1.0f);
    
    private Vector2 legacyInputVector => new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));

    private Vector2 newKeyboardInputVector => newInputWASD + newInputKeyArrows;
    private Vector2 newInputWASD => new Vector2(
        Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
        Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
    );
    private Vector2 newInputKeyArrows => new Vector2(
        Keyboard.current.rightArrowKey.ReadValue() - Keyboard.current.leftArrowKey.ReadValue(),
        Keyboard.current.upArrowKey.ReadValue() - Keyboard.current.downArrowKey.ReadValue()
    );

    private Vector2 gamepadInputVector => Gamepad.current != null
        ? Gamepad.current.leftStick.ReadValue() + Gamepad.current.dpad.ReadValue()
        : Vector2.zero;
    
    #endregion 
    #region JumpInputBindings    
    private bool isJumpPressed => useLegacyKeyboardInput
        ? Input.GetButtonDown("Jump")
        : (keyboardJumpPressCount + gamepadJumpPressCount) > 0;
    private int keyboardJumpPressCount => 
        useSpaceToJump ? (Keyboard.current.spaceKey.isPressed ? 1 : 0) :
        useSpaceToInteract ? (Keyboard.current.upArrowKey.isPressed ? 1 : 0) : 0;
    private int gamepadJumpPressCount => 
        Gamepad.current != null && Gamepad.current.buttonSouth.isPressed ? 1 : 0;
    #endregion
    #region InteractInputBindings

    private bool isInteractPressed => useLegacyKeyboardInput
        ? Input.GetButtonDown("Interact")
        : (keyboardInteractPressCount + gamepadInteractPressCount) > 0;
    private int keyboardInteractPressCount =>
        useSpaceToJump ? (Keyboard.current.eKey.isPressed || Keyboard.current.enterKey.isPressed ? 1 : 0) :
        useSpaceToInteract ? (Keyboard.current.spaceKey.isPressed ? 1 : 0) : 0;
    private int gamepadInteractPressCount =>
        Gamepad.current != null && Gamepad.current.buttonEast.isPressed ? 1 : 0;

    #endregion
}