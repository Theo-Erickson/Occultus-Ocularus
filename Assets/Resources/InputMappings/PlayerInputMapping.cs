// GENERATED AUTOMATICALLY FROM 'Assets/Resources/InputMappings/PlayerInputMapping.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class PlayerInputMapping : InputActionAssetReference
{
    public PlayerInputMapping()
    {
    }
    public PlayerInputMapping(InputActionAsset asset)
        : base(asset)
    {
    }
    [NonSerialized] private bool m_Initialized;
    private void Initialize()
    {
        // Player
        m_Player = asset.GetActionMap("Player");
        m_Player_Move = m_Player.GetAction("Move");
        m_Player_Look = m_Player.GetAction("Look");
        m_Player_Interact = m_Player.GetAction("Interact");
        m_Player_Jump = m_Player.GetAction("Jump");
        m_Player_ToggleCamera = m_Player.GetAction("ToggleCamera");
        m_Player_ToggleLayerSwitching = m_Player.GetAction("ToggleLayerSwitching");
        m_Player_SetCameraMode1 = m_Player.GetAction("SetCameraMode1");
        m_Player_SetCameraMode2 = m_Player.GetAction("SetCameraMode2");
        m_Player_SetCameraMode3 = m_Player.GetAction("SetCameraMode3");
        m_Player_SetCameraMode4 = m_Player.GetAction("SetCameraMode4");
        // UI
        m_UI = asset.GetActionMap("UI");
        m_UI_Navigate = m_UI.GetAction("Navigate");
        m_UI_Submit = m_UI.GetAction("Submit");
        m_UI_Cancel = m_UI.GetAction("Cancel");
        m_UI_Point = m_UI.GetAction("Point");
        m_UI_Click = m_UI.GetAction("Click");
        m_UI_OpenMenu = m_UI.GetAction("OpenMenu");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        if (m_PlayerActionsCallbackInterface != null)
        {
            Player.SetCallbacks(null);
        }
        m_Player = null;
        m_Player_Move = null;
        m_Player_Look = null;
        m_Player_Interact = null;
        m_Player_Jump = null;
        m_Player_ToggleCamera = null;
        m_Player_ToggleLayerSwitching = null;
        m_Player_SetCameraMode1 = null;
        m_Player_SetCameraMode2 = null;
        m_Player_SetCameraMode3 = null;
        m_Player_SetCameraMode4 = null;
        if (m_UIActionsCallbackInterface != null)
        {
            UI.SetCallbacks(null);
        }
        m_UI = null;
        m_UI_Navigate = null;
        m_UI_Submit = null;
        m_UI_Cancel = null;
        m_UI_Point = null;
        m_UI_Click = null;
        m_UI_OpenMenu = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        var PlayerCallbacks = m_PlayerActionsCallbackInterface;
        var UICallbacks = m_UIActionsCallbackInterface;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
        Player.SetCallbacks(PlayerCallbacks);
        UI.SetCallbacks(UICallbacks);
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Player
    private InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private InputAction m_Player_Move;
    private InputAction m_Player_Look;
    private InputAction m_Player_Interact;
    private InputAction m_Player_Jump;
    private InputAction m_Player_ToggleCamera;
    private InputAction m_Player_ToggleLayerSwitching;
    private InputAction m_Player_SetCameraMode1;
    private InputAction m_Player_SetCameraMode2;
    private InputAction m_Player_SetCameraMode3;
    private InputAction m_Player_SetCameraMode4;
    public struct PlayerActions
    {
        private PlayerInputMapping m_Wrapper;
        public PlayerActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move { get { return m_Wrapper.m_Player_Move; } }
        public InputAction @Look { get { return m_Wrapper.m_Player_Look; } }
        public InputAction @Interact { get { return m_Wrapper.m_Player_Interact; } }
        public InputAction @Jump { get { return m_Wrapper.m_Player_Jump; } }
        public InputAction @ToggleCamera { get { return m_Wrapper.m_Player_ToggleCamera; } }
        public InputAction @ToggleLayerSwitching { get { return m_Wrapper.m_Player_ToggleLayerSwitching; } }
        public InputAction @SetCameraMode1 { get { return m_Wrapper.m_Player_SetCameraMode1; } }
        public InputAction @SetCameraMode2 { get { return m_Wrapper.m_Player_SetCameraMode2; } }
        public InputAction @SetCameraMode3 { get { return m_Wrapper.m_Player_SetCameraMode3; } }
        public InputAction @SetCameraMode4 { get { return m_Wrapper.m_Player_SetCameraMode4; } }
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                Look.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                Interact.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Jump.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                ToggleCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleCamera;
                ToggleCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleCamera;
                ToggleCamera.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleCamera;
                ToggleLayerSwitching.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleLayerSwitching;
                ToggleLayerSwitching.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleLayerSwitching;
                ToggleLayerSwitching.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleLayerSwitching;
                SetCameraMode1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode1;
                SetCameraMode1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode1;
                SetCameraMode1.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode1;
                SetCameraMode2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode2;
                SetCameraMode2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode2;
                SetCameraMode2.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode2;
                SetCameraMode3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode3;
                SetCameraMode3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode3;
                SetCameraMode3.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode3;
                SetCameraMode4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode4;
                SetCameraMode4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode4;
                SetCameraMode4.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSetCameraMode4;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.cancelled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.cancelled += instance.OnLook;
                Interact.started += instance.OnInteract;
                Interact.performed += instance.OnInteract;
                Interact.cancelled += instance.OnInteract;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.cancelled += instance.OnJump;
                ToggleCamera.started += instance.OnToggleCamera;
                ToggleCamera.performed += instance.OnToggleCamera;
                ToggleCamera.cancelled += instance.OnToggleCamera;
                ToggleLayerSwitching.started += instance.OnToggleLayerSwitching;
                ToggleLayerSwitching.performed += instance.OnToggleLayerSwitching;
                ToggleLayerSwitching.cancelled += instance.OnToggleLayerSwitching;
                SetCameraMode1.started += instance.OnSetCameraMode1;
                SetCameraMode1.performed += instance.OnSetCameraMode1;
                SetCameraMode1.cancelled += instance.OnSetCameraMode1;
                SetCameraMode2.started += instance.OnSetCameraMode2;
                SetCameraMode2.performed += instance.OnSetCameraMode2;
                SetCameraMode2.cancelled += instance.OnSetCameraMode2;
                SetCameraMode3.started += instance.OnSetCameraMode3;
                SetCameraMode3.performed += instance.OnSetCameraMode3;
                SetCameraMode3.cancelled += instance.OnSetCameraMode3;
                SetCameraMode4.started += instance.OnSetCameraMode4;
                SetCameraMode4.performed += instance.OnSetCameraMode4;
                SetCameraMode4.cancelled += instance.OnSetCameraMode4;
            }
        }
    }
    public PlayerActions @Player
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new PlayerActions(this);
        }
    }
    // UI
    private InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private InputAction m_UI_Navigate;
    private InputAction m_UI_Submit;
    private InputAction m_UI_Cancel;
    private InputAction m_UI_Point;
    private InputAction m_UI_Click;
    private InputAction m_UI_OpenMenu;
    public struct UIActions
    {
        private PlayerInputMapping m_Wrapper;
        public UIActions(PlayerInputMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate { get { return m_Wrapper.m_UI_Navigate; } }
        public InputAction @Submit { get { return m_Wrapper.m_UI_Submit; } }
        public InputAction @Cancel { get { return m_Wrapper.m_UI_Cancel; } }
        public InputAction @Point { get { return m_Wrapper.m_UI_Point; } }
        public InputAction @Click { get { return m_Wrapper.m_UI_Click; } }
        public InputAction @OpenMenu { get { return m_Wrapper.m_UI_OpenMenu; } }
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                Navigate.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                Submit.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Cancel.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                Point.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                Click.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                OpenMenu.started -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                OpenMenu.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
                OpenMenu.cancelled -= m_Wrapper.m_UIActionsCallbackInterface.OnOpenMenu;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                Navigate.started += instance.OnNavigate;
                Navigate.performed += instance.OnNavigate;
                Navigate.cancelled += instance.OnNavigate;
                Submit.started += instance.OnSubmit;
                Submit.performed += instance.OnSubmit;
                Submit.cancelled += instance.OnSubmit;
                Cancel.started += instance.OnCancel;
                Cancel.performed += instance.OnCancel;
                Cancel.cancelled += instance.OnCancel;
                Point.started += instance.OnPoint;
                Point.performed += instance.OnPoint;
                Point.cancelled += instance.OnPoint;
                Click.started += instance.OnClick;
                Click.performed += instance.OnClick;
                Click.cancelled += instance.OnClick;
                OpenMenu.started += instance.OnOpenMenu;
                OpenMenu.performed += instance.OnOpenMenu;
                OpenMenu.cancelled += instance.OnOpenMenu;
            }
        }
    }
    public UIActions @UI
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new UIActions(this);
        }
    }
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get

        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.GetControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get

        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.GetControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
}
public interface IPlayerActions
{
    void OnMove(InputAction.CallbackContext context);
    void OnLook(InputAction.CallbackContext context);
    void OnInteract(InputAction.CallbackContext context);
    void OnJump(InputAction.CallbackContext context);
    void OnToggleCamera(InputAction.CallbackContext context);
    void OnToggleLayerSwitching(InputAction.CallbackContext context);
    void OnSetCameraMode1(InputAction.CallbackContext context);
    void OnSetCameraMode2(InputAction.CallbackContext context);
    void OnSetCameraMode3(InputAction.CallbackContext context);
    void OnSetCameraMode4(InputAction.CallbackContext context);
}
public interface IUIActions
{
    void OnNavigate(InputAction.CallbackContext context);
    void OnSubmit(InputAction.CallbackContext context);
    void OnCancel(InputAction.CallbackContext context);
    void OnPoint(InputAction.CallbackContext context);
    void OnClick(InputAction.CallbackContext context);
    void OnOpenMenu(InputAction.CallbackContext context);
}
