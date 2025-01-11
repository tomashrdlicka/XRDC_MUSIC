//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/XRDC_HIT_005/InputActions.inputactions
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

public partial class @InputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""e859b8f2-9b67-49d7-9a68-102083953e31"",
            ""actions"": [
                {
                    ""name"": ""PlayComposition"",
                    ""type"": ""Button"",
                    ""id"": ""b84d6b8e-8d86-45d2-9d83-13c07f549282"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PreviousInstrument"",
                    ""type"": ""Button"",
                    ""id"": ""7002627f-f3a4-427e-8033-50e86e5af6a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""NextInstrument"",
                    ""type"": ""Button"",
                    ""id"": ""7b03d9b5-7667-4023-aa13-c98c010218ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""f7437fe7-9c64-41bc-b854-1a34e4df9345"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightMouseDown"",
                    ""type"": ""Button"",
                    ""id"": ""defce7d3-e8e4-45cd-a959-17f566a47ec1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightMouseUp"",
                    ""type"": ""Button"",
                    ""id"": ""de5baf55-0227-4cd6-9f7a-1f783c9ce710"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OnStop"",
                    ""type"": ""Button"",
                    ""id"": ""d3db95b1-3240-45ee-b49b-5b722db848e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Value"",
                    ""id"": ""dc5ebda3-bc4c-4795-8dc5-bf93db336cc2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""431a3d34-17df-4dd5-aa50-ffb20a87938a"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayComposition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5727f078-6ccb-4be1-87b5-0a75054f939c"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousInstrument"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a362752-ac6f-4b2c-884d-26748fa73ac6"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextInstrument"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d372842-4115-4428-a1cf-b603706a057c"",
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
                    ""id"": ""95fd267f-3ad5-4a6b-accf-2c2878da6c4f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightMouseDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41423f0b-84ee-43c2-9603-e420810e2fb7"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightMouseUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9197779d-367a-442c-b4cc-178a1b29cd0d"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OnStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cffeb4ee-b70b-4452-a5be-82825bbdb1dd"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=-1)"",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e00ef5ed-1f1c-4aec-b6aa-cc4e4eb72fb2"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
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
        m_Gameplay_PlayComposition = m_Gameplay.FindAction("PlayComposition", throwIfNotFound: true);
        m_Gameplay_PreviousInstrument = m_Gameplay.FindAction("PreviousInstrument", throwIfNotFound: true);
        m_Gameplay_NextInstrument = m_Gameplay.FindAction("NextInstrument", throwIfNotFound: true);
        m_Gameplay_LeftClick = m_Gameplay.FindAction("LeftClick", throwIfNotFound: true);
        m_Gameplay_RightMouseDown = m_Gameplay.FindAction("RightMouseDown", throwIfNotFound: true);
        m_Gameplay_RightMouseUp = m_Gameplay.FindAction("RightMouseUp", throwIfNotFound: true);
        m_Gameplay_OnStop = m_Gameplay.FindAction("OnStop", throwIfNotFound: true);
        m_Gameplay_RotateCamera = m_Gameplay.FindAction("RotateCamera", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_PlayComposition;
    private readonly InputAction m_Gameplay_PreviousInstrument;
    private readonly InputAction m_Gameplay_NextInstrument;
    private readonly InputAction m_Gameplay_LeftClick;
    private readonly InputAction m_Gameplay_RightMouseDown;
    private readonly InputAction m_Gameplay_RightMouseUp;
    private readonly InputAction m_Gameplay_OnStop;
    private readonly InputAction m_Gameplay_RotateCamera;
    public struct GameplayActions
    {
        private @InputActions m_Wrapper;
        public GameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayComposition => m_Wrapper.m_Gameplay_PlayComposition;
        public InputAction @PreviousInstrument => m_Wrapper.m_Gameplay_PreviousInstrument;
        public InputAction @NextInstrument => m_Wrapper.m_Gameplay_NextInstrument;
        public InputAction @LeftClick => m_Wrapper.m_Gameplay_LeftClick;
        public InputAction @RightMouseDown => m_Wrapper.m_Gameplay_RightMouseDown;
        public InputAction @RightMouseUp => m_Wrapper.m_Gameplay_RightMouseUp;
        public InputAction @OnStop => m_Wrapper.m_Gameplay_OnStop;
        public InputAction @RotateCamera => m_Wrapper.m_Gameplay_RotateCamera;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @PlayComposition.started += instance.OnPlayComposition;
            @PlayComposition.performed += instance.OnPlayComposition;
            @PlayComposition.canceled += instance.OnPlayComposition;
            @PreviousInstrument.started += instance.OnPreviousInstrument;
            @PreviousInstrument.performed += instance.OnPreviousInstrument;
            @PreviousInstrument.canceled += instance.OnPreviousInstrument;
            @NextInstrument.started += instance.OnNextInstrument;
            @NextInstrument.performed += instance.OnNextInstrument;
            @NextInstrument.canceled += instance.OnNextInstrument;
            @LeftClick.started += instance.OnLeftClick;
            @LeftClick.performed += instance.OnLeftClick;
            @LeftClick.canceled += instance.OnLeftClick;
            @RightMouseDown.started += instance.OnRightMouseDown;
            @RightMouseDown.performed += instance.OnRightMouseDown;
            @RightMouseDown.canceled += instance.OnRightMouseDown;
            @RightMouseUp.started += instance.OnRightMouseUp;
            @RightMouseUp.performed += instance.OnRightMouseUp;
            @RightMouseUp.canceled += instance.OnRightMouseUp;
            @OnStop.started += instance.OnOnStop;
            @OnStop.performed += instance.OnOnStop;
            @OnStop.canceled += instance.OnOnStop;
            @RotateCamera.started += instance.OnRotateCamera;
            @RotateCamera.performed += instance.OnRotateCamera;
            @RotateCamera.canceled += instance.OnRotateCamera;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @PlayComposition.started -= instance.OnPlayComposition;
            @PlayComposition.performed -= instance.OnPlayComposition;
            @PlayComposition.canceled -= instance.OnPlayComposition;
            @PreviousInstrument.started -= instance.OnPreviousInstrument;
            @PreviousInstrument.performed -= instance.OnPreviousInstrument;
            @PreviousInstrument.canceled -= instance.OnPreviousInstrument;
            @NextInstrument.started -= instance.OnNextInstrument;
            @NextInstrument.performed -= instance.OnNextInstrument;
            @NextInstrument.canceled -= instance.OnNextInstrument;
            @LeftClick.started -= instance.OnLeftClick;
            @LeftClick.performed -= instance.OnLeftClick;
            @LeftClick.canceled -= instance.OnLeftClick;
            @RightMouseDown.started -= instance.OnRightMouseDown;
            @RightMouseDown.performed -= instance.OnRightMouseDown;
            @RightMouseDown.canceled -= instance.OnRightMouseDown;
            @RightMouseUp.started -= instance.OnRightMouseUp;
            @RightMouseUp.performed -= instance.OnRightMouseUp;
            @RightMouseUp.canceled -= instance.OnRightMouseUp;
            @OnStop.started -= instance.OnOnStop;
            @OnStop.performed -= instance.OnOnStop;
            @OnStop.canceled -= instance.OnOnStop;
            @RotateCamera.started -= instance.OnRotateCamera;
            @RotateCamera.performed -= instance.OnRotateCamera;
            @RotateCamera.canceled -= instance.OnRotateCamera;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnPlayComposition(InputAction.CallbackContext context);
        void OnPreviousInstrument(InputAction.CallbackContext context);
        void OnNextInstrument(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightMouseDown(InputAction.CallbackContext context);
        void OnRightMouseUp(InputAction.CallbackContext context);
        void OnOnStop(InputAction.CallbackContext context);
        void OnRotateCamera(InputAction.CallbackContext context);
    }
}
