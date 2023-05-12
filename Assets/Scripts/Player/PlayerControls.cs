//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Scripts/Player/PlayerControls.inputactions
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
                },
                {
                    ""name"": ""X"",
                    ""type"": ""Button"",
                    ""id"": ""d64bf6b5-0f51-4499-bd6d-ebe8dc21849a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Space"",
                    ""type"": ""Button"",
                    ""id"": ""9ee58b4b-c28f-4b4c-9f7b-1357e3e2b550"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CTRL"",
                    ""type"": ""Button"",
                    ""id"": ""239ab619-a2a1-4e36-b564-835eb6f40bd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""G"",
                    ""type"": ""Button"",
                    ""id"": ""e35fe902-0761-4838-9856-0569285fbcf2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""F"",
                    ""type"": ""Button"",
                    ""id"": ""367d0a8b-5238-44d6-9bbf-cb6742131cdc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""W"",
                    ""type"": ""Button"",
                    ""id"": ""6d81560c-918a-4a26-a8a7-06a1488f2a09"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ESC"",
                    ""type"": ""Button"",
                    ""id"": ""f7e238b6-cae0-4881-a7c2-201c9295fa66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BackSpace"",
                    ""type"": ""Button"",
                    ""id"": ""5ec26c65-9cd6-4999-a843-646c570586c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ZoomIn"",
                    ""type"": ""Value"",
                    ""id"": ""aec21b3d-e5ca-4ea5-89cc-5525f647ba10"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""S"",
                    ""type"": ""Button"",
                    ""id"": ""41973d00-0a11-4d27-8738-1c6322822725"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""28c46b70-e705-47eb-9084-3582c9151d11"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""576867f2-fc7a-43e0-9124-36ebca65635b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae45c601-7498-405e-aa6d-8e53fde267fb"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CTRL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef8673ec-58c0-4952-b45d-90ea80fc56b0"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""G"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95aee5ce-df95-4588-a55e-1ef2ebebf485"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomIn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efcc1ba6-4469-41d8-9093-faf03210c799"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""F"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f1bf4a0-9bbf-4cf6-bec1-af70723e2de3"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""W"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""869c9600-79e6-46ba-8569-5bd91df35728"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ESC"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c1ed68b-a702-4fa3-ab1e-59ddaac09a80"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BackSpace"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fa43b50-efcb-4c9f-b593-a4a5cd9c718b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""S"",
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
        m_Land_X = m_Land.FindAction("X", throwIfNotFound: true);
        m_Land_Space = m_Land.FindAction("Space", throwIfNotFound: true);
        m_Land_CTRL = m_Land.FindAction("CTRL", throwIfNotFound: true);
        m_Land_G = m_Land.FindAction("G", throwIfNotFound: true);
        m_Land_F = m_Land.FindAction("F", throwIfNotFound: true);
        m_Land_W = m_Land.FindAction("W", throwIfNotFound: true);
        m_Land_ESC = m_Land.FindAction("ESC", throwIfNotFound: true);
        m_Land_BackSpace = m_Land.FindAction("BackSpace", throwIfNotFound: true);
        m_Land_ZoomIn = m_Land.FindAction("ZoomIn", throwIfNotFound: true);
        m_Land_S = m_Land.FindAction("S", throwIfNotFound: true);
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
    private readonly InputAction m_Land_X;
    private readonly InputAction m_Land_Space;
    private readonly InputAction m_Land_CTRL;
    private readonly InputAction m_Land_G;
    private readonly InputAction m_Land_F;
    private readonly InputAction m_Land_W;
    private readonly InputAction m_Land_ESC;
    private readonly InputAction m_Land_BackSpace;
    private readonly InputAction m_Land_ZoomIn;
    private readonly InputAction m_Land_S;
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
        public InputAction @X => m_Wrapper.m_Land_X;
        public InputAction @Space => m_Wrapper.m_Land_Space;
        public InputAction @CTRL => m_Wrapper.m_Land_CTRL;
        public InputAction @G => m_Wrapper.m_Land_G;
        public InputAction @F => m_Wrapper.m_Land_F;
        public InputAction @W => m_Wrapper.m_Land_W;
        public InputAction @ESC => m_Wrapper.m_Land_ESC;
        public InputAction @BackSpace => m_Wrapper.m_Land_BackSpace;
        public InputAction @ZoomIn => m_Wrapper.m_Land_ZoomIn;
        public InputAction @S => m_Wrapper.m_Land_S;
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
                @X.started -= m_Wrapper.m_LandActionsCallbackInterface.OnX;
                @X.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnX;
                @X.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnX;
                @Space.started -= m_Wrapper.m_LandActionsCallbackInterface.OnSpace;
                @Space.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnSpace;
                @Space.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnSpace;
                @CTRL.started -= m_Wrapper.m_LandActionsCallbackInterface.OnCTRL;
                @CTRL.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnCTRL;
                @CTRL.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnCTRL;
                @G.started -= m_Wrapper.m_LandActionsCallbackInterface.OnG;
                @G.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnG;
                @G.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnG;
                @F.started -= m_Wrapper.m_LandActionsCallbackInterface.OnF;
                @F.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnF;
                @F.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnF;
                @W.started -= m_Wrapper.m_LandActionsCallbackInterface.OnW;
                @W.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnW;
                @W.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnW;
                @ESC.started -= m_Wrapper.m_LandActionsCallbackInterface.OnESC;
                @ESC.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnESC;
                @ESC.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnESC;
                @BackSpace.started -= m_Wrapper.m_LandActionsCallbackInterface.OnBackSpace;
                @BackSpace.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnBackSpace;
                @BackSpace.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnBackSpace;
                @ZoomIn.started -= m_Wrapper.m_LandActionsCallbackInterface.OnZoomIn;
                @ZoomIn.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnZoomIn;
                @ZoomIn.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnZoomIn;
                @S.started -= m_Wrapper.m_LandActionsCallbackInterface.OnS;
                @S.performed -= m_Wrapper.m_LandActionsCallbackInterface.OnS;
                @S.canceled -= m_Wrapper.m_LandActionsCallbackInterface.OnS;
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
                @X.started += instance.OnX;
                @X.performed += instance.OnX;
                @X.canceled += instance.OnX;
                @Space.started += instance.OnSpace;
                @Space.performed += instance.OnSpace;
                @Space.canceled += instance.OnSpace;
                @CTRL.started += instance.OnCTRL;
                @CTRL.performed += instance.OnCTRL;
                @CTRL.canceled += instance.OnCTRL;
                @G.started += instance.OnG;
                @G.performed += instance.OnG;
                @G.canceled += instance.OnG;
                @F.started += instance.OnF;
                @F.performed += instance.OnF;
                @F.canceled += instance.OnF;
                @W.started += instance.OnW;
                @W.performed += instance.OnW;
                @W.canceled += instance.OnW;
                @ESC.started += instance.OnESC;
                @ESC.performed += instance.OnESC;
                @ESC.canceled += instance.OnESC;
                @BackSpace.started += instance.OnBackSpace;
                @BackSpace.performed += instance.OnBackSpace;
                @BackSpace.canceled += instance.OnBackSpace;
                @ZoomIn.started += instance.OnZoomIn;
                @ZoomIn.performed += instance.OnZoomIn;
                @ZoomIn.canceled += instance.OnZoomIn;
                @S.started += instance.OnS;
                @S.performed += instance.OnS;
                @S.canceled += instance.OnS;
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
        void OnX(InputAction.CallbackContext context);
        void OnSpace(InputAction.CallbackContext context);
        void OnCTRL(InputAction.CallbackContext context);
        void OnG(InputAction.CallbackContext context);
        void OnF(InputAction.CallbackContext context);
        void OnW(InputAction.CallbackContext context);
        void OnESC(InputAction.CallbackContext context);
        void OnBackSpace(InputAction.CallbackContext context);
        void OnZoomIn(InputAction.CallbackContext context);
        void OnS(InputAction.CallbackContext context);
    }
}
