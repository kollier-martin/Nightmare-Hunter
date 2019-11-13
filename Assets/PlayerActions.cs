// GENERATED AUTOMATICALLY FROM 'Assets/PlayerActions.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerActions : IInputActionCollection
{
    private InputActionAsset asset;
    public PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""537bbf42-95a5-47a1-9adb-57c6604deb29"",
            ""actions"": [
                {
                    ""name"": ""Switch Weapons"",
                    ""type"": ""Button"",
                    ""id"": ""1baf3a89-66ac-46f5-8316-f9aa558fb66d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""a053b389-716f-4779-bb53-e57602c27f50"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""11aa1de1-4a7d-46b1-a697-91b2378ee7f1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""60ece161-26b4-47d2-8d1c-85fc8ba66dd3"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch Weapons"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fa6688c-060f-4ef3-a876-f4b7f038c78c"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch Weapons"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""281b609a-72d6-4b83-95c3-9baa62e4d0fc"",
                    ""path"": ""<DualShockGamepad>/dpad/left"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch Weapons"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f027fe41-4a69-4f42-b7a1-92320175a9f5"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch Weapons"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""682cb9c3-d5f6-4774-bf12-ff22bd048504"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d059817-42fe-4dd1-b4af-ff145c419968"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c27ea2fc-72ca-4cd6-a761-00415b4c65e2"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": ""Hold,Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_SwitchWeapons = m_Gameplay.FindAction("Switch Weapons", throwIfNotFound: true);
        m_Gameplay_Aim = m_Gameplay.FindAction("Aim", throwIfNotFound: true);
        m_Gameplay_Shoot = m_Gameplay.FindAction("Shoot", throwIfNotFound: true);
    }

    ~PlayerActions()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_SwitchWeapons;
    private readonly InputAction m_Gameplay_Aim;
    private readonly InputAction m_Gameplay_Shoot;
    public struct GameplayActions
    {
        private PlayerActions m_Wrapper;
        public GameplayActions(PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SwitchWeapons => m_Wrapper.m_Gameplay_SwitchWeapons;
        public InputAction @Aim => m_Wrapper.m_Gameplay_Aim;
        public InputAction @Shoot => m_Wrapper.m_Gameplay_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                SwitchWeapons.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchWeapons;
                SwitchWeapons.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchWeapons;
                SwitchWeapons.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchWeapons;
                Aim.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                Aim.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                Aim.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                Shoot.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                Shoot.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                Shoot.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                SwitchWeapons.started += instance.OnSwitchWeapons;
                SwitchWeapons.performed += instance.OnSwitchWeapons;
                SwitchWeapons.canceled += instance.OnSwitchWeapons;
                Aim.started += instance.OnAim;
                Aim.performed += instance.OnAim;
                Aim.canceled += instance.OnAim;
                Shoot.started += instance.OnShoot;
                Shoot.performed += instance.OnShoot;
                Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnSwitchWeapons(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
