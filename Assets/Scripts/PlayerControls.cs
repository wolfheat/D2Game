//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Scripts/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Land"",
            ""id"": ""38e9262b-f9e6-43f2-80fb-b076a3b04c12"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""884b019a-0b00-4703-8ecc-2b651829f3f8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""3a66ae39-81da-4769-83cc-89d1867dca18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""6250050f-26e0-4a15-b1e4-ca11c45038d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""811f0b5f-df6e-4e8d-96aa-3889464903ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""2ead9527-745e-48c0-91cd-2d56464d2505"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""70f1b0c1-57e4-4516-a65b-0434e4e0a092"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateL"",
                    ""type"": ""Button"",
                    ""id"": ""7da6b8f2-4a0b-4760-8955-e9b3e628bf9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateR"",
                    ""type"": ""Button"",
                    ""id"": ""9e7acdcb-249a-4e29-83d4-816063f4bec3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WayPointType"",
                    ""type"": ""Button"",
                    ""id"": ""8830004a-fa0a-4162-bc72-f100713bc2e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MusicToggle"",
                    ""type"": ""Button"",
                    ""id"": ""5d0672f0-a3c1-4501-8fd7-5852a6dec282"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c157ad52-b4a9-4b41-815d-2b8bf188501e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f93753f-447f-4305-913c-00afaf7d27d5"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18e6b845-cf18-4350-ace3-7fe18e25037f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9328d609-6661-4cfc-9f14-fe138d079bdb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e19ebf4-7dc4-456b-8e2a-2ce2f8df7af9"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fccce780-67cf-4da9-9fca-dcc4db447d7e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb18e604-e98c-4458-8c9b-33440c8f75ec"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd5a7d48-54af-47f1-bb8b-f5475cbac9c4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33cc247d-72f9-461e-a5e0-ce20277ac2ee"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WayPointType"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c781dce-9ab8-46bc-9540-4e3f7416aa1d"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MusicToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Land
        m_Land = asset.FindActionMap("Land", throwIfNotFound: true);
        m_Land_Move = m_Land.FindAction("Move", throwIfNotFound: true);
        m_Land_Shoot = m_Land.FindAction("Shoot", throwIfNotFound: true);
        m_Land_RightClick = m_Land.FindAction("RightClick", throwIfNotFound: true);
        m_Land_LeftClick = m_Land.FindAction("LeftClick", throwIfNotFound: true);
        m_Land_Attack = m_Land.FindAction("Attack", throwIfNotFound: true);
        m_Land_Shift = m_Land.FindAction("Shift", throwIfNotFound: true);
        m_Land_RotateL = m_Land.FindAction("RotateL", throwIfNotFound: true);
        m_Land_RotateR = m_Land.FindAction("RotateR", throwIfNotFound: true);
        m_Land_WayPointType = m_Land.FindAction("WayPointType", throwIfNotFound: true);
        m_Land_MusicToggle = m_Land.FindAction("MusicToggle", throwIfNotFound: true);
    }

    public void Dispose()
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Land
    private readonly InputActionMap m_Land;
    private ILandActions m_LandActionsCallbackInterface;
    private readonly InputAction m_Land_Move;
    private readonly InputAction m_Land_Shoot;
    private readonly InputAction m_Land_RightClick;
    private readonly InputAction m_Land_LeftClick;
    private readonly InputAction m_Land_Attack;
    private readonly InputAction m_Land_Shift;
    private readonly InputAction m_Land_RotateL;
    private readonly InputAction m_Land_RotateR;
    private readonly InputAction m_Land_WayPointType;
    private readonly InputAction m_Land_MusicToggle;
    public struct LandActions
    {
        private @PlayerControls m_Wrapper;
        public LandActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Land_Move;
        public InputAction @Shoot => m_Wrapper.m_Land_Shoot;
        public InputAction @RightClick => m_Wrapper.m_Land_RightClick;
        public InputAction @LeftClick => m_Wrapper.m_Land_LeftClick;
        public InputAction @Attack => m_Wrapper.m_Land_Attack;
        public InputAction @Shift => m_Wrapper.m_Land_Shift;
        public InputAction @RotateL => m_Wrapper.m_Land_RotateL;
        public InputAction @RotateR => m_Wrapper.m_Land_RotateR;
        public InputAction @WayPointType => m_Wrapper.m_Land_WayPointType;
        public InputAction @MusicToggle => m_Wrapper.m_Land_MusicToggle;
        public InputActionMap Get() { return m_Wrapper.m_Land; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LandActions set) { return set.Get(); }
        public void SetCallbacks(ILandActions instance)
        {
            if (m_Wrapper.m_LandActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMove;
                @Shoot.started -= m_Wrapper.m_LandActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnShoot;
                @RightClick.started -= m_Wrapper.m_LandActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnRightClick;
                @LeftClick.started -= m_Wrapper.m_LandActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnLeftClick;
                @Attack.started -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnAttack;
                @Shift.started -= m_Wrapper.m_LandActionsCallbackInterface.OnShift;
                @Shift.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnShift;
                @Shift.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnShift;
                @RotateL.started -= m_Wrapper.m_LandActionsCallbackInterface.OnRotateL;
                @RotateL.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnRotateL;
                @RotateL.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnRotateL;
                @RotateR.started -= m_Wrapper.m_LandActionsCallbackInterface.OnRotateR;
                @RotateR.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnRotateR;
                @RotateR.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnRotateR;
                @WayPointType.started -= m_Wrapper.m_LandActionsCallbackInterface.OnWayPointType;
                @WayPointType.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnWayPointType;
                @WayPointType.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnWayPointType;
                @MusicToggle.started -= m_Wrapper.m_LandActionsCallbackInterface.OnMusicToggle;
                @MusicToggle.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnMusicToggle;
                @MusicToggle.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnMusicToggle;
            }
            m_Wrapper.m_LandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Shift.started += instance.OnShift;
                @Shift.performed += instance.OnShift;
                @Shift.canceled += instance.OnShift;
                @RotateL.started += instance.OnRotateL;
                @RotateL.performed += instance.OnRotateL;
                @RotateL.canceled += instance.OnRotateL;
                @RotateR.started += instance.OnRotateR;
                @RotateR.performed += instance.OnRotateR;
                @RotateR.canceled += instance.OnRotateR;
                @WayPointType.started += instance.OnWayPointType;
                @WayPointType.performed += instance.OnWayPointType;
                @WayPointType.canceled += instance.OnWayPointType;
                @MusicToggle.started += instance.OnMusicToggle;
                @MusicToggle.performed += instance.OnMusicToggle;
                @MusicToggle.canceled += instance.OnMusicToggle;
            }
        }
    }
    public LandActions @Land => new LandActions(this);
    public interface ILandActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
        void OnRotateL(InputAction.CallbackContext context);
        void OnRotateR(InputAction.CallbackContext context);
        void OnWayPointType(InputAction.CallbackContext context);
        void OnMusicToggle(InputAction.CallbackContext context);
    }
}
