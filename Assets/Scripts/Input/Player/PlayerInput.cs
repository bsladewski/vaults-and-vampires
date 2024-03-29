//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Input/Player/PlayerInput.inputactions
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

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Third-Person Camera"",
            ""id"": ""64f516d1-e79d-4cb3-98a0-e223ce1f41a7"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""8b552038-de4c-4ac2-871a-29a1a21d4797"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fixed Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""06005355-e32d-4427-bf8f-8a2b2e05b21e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""Normalize(min=-1,max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Button"",
                    ""id"": ""04883bfe-e3c1-4807-9ab6-27a7b710afde"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""f4c3fd22-d4f1-40ac-adf2-767e7414a77f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""QE"",
                    ""id"": ""58c24f0f-b60e-4ffc-abda-467590b8b8ff"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5b2e1faa-da10-4ff5-969f-5984cd86076f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c26c82d8-0fd6-4ba5-9e62-4de864ac02cc"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""88d1849f-10e7-44f9-acda-52d308e8a56d"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f30db01e-5eee-4466-b85e-b68d844e8b8a"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1297098-2841-4108-8773-f62bf8473afb"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0933fcba-8d7a-4e95-881d-e759f9933439"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e593c599-05c5-4806-8af5-a1f0375d4cca"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ZC"",
                    ""id"": ""2f5cc349-6cef-4208-ba43-7908f54ad0ea"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fixed Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f8c9fbdb-16f3-4da1-90b4-e9ac1ba635f2"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fixed Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ec0f5887-06da-4b4e-af90-ae96a2183de0"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fixed Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""d0fb853f-b8ce-4628-b732-c82225bb94d3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fixed Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""cbd0ccdd-ca3d-4a50-a299-9536df644b2e"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fixed Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ca30ae38-75c4-4f28-b073-29c346ac642f"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fixed Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Third-Person Movement"",
            ""id"": ""b2531885-ab54-4a2a-bef9-1ff58e8d19e7"",
            ""actions"": [
                {
                    ""name"": ""Run"",
                    ""type"": ""Value"",
                    ""id"": ""ff6cf091-bef2-4db8-9c08-f98ce96e963d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1d7a0392-e791-4393-8322-48a720d9f7a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""2445616b-fe43-4844-8cb2-01e6b73a8835"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""20fae1c7-999a-4592-b863-68c11c8db3a7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""4a09e4de-50c8-4d67-980f-5cb27f8ffc19"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""562096c6-3765-4aad-bcb0-d01e6583f82a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""25897c7c-b904-48b3-824d-2d4e7f5fac5e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c40ad7df-8b98-414d-a65b-c3935769175a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3f6817f0-4703-429c-bd7e-86af302070e8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d62ea534-8fb1-4878-8d6f-a5a2856440b2"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""392f19b2-dd08-4738-9266-598e5beaff73"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""QE"",
                    ""id"": ""1d59e789-42ce-4265-9133-a0c0c770520e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c655cdff-402a-40d8-a61f-b7e3accaf4fe"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""11ff256b-22de-4842-b1a7-36a670a3cbc4"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e15dfe60-a43f-44f4-90d8-99642f2c5efb"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Third-Person Combat"",
            ""id"": ""d0c82b91-1b49-4ec2-96c5-ac39f5b298a4"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""fa63d170-cfe1-471a-9438-b86dc1957eef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""deac31c4-8e9e-4db3-8eb3-48c45b82ace1"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b9e8e77-ae5a-4aea-bfc8-9fcee8e62b4c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Third-Person Camera
        m_ThirdPersonCamera = asset.FindActionMap("Third-Person Camera", throwIfNotFound: true);
        m_ThirdPersonCamera_Rotate = m_ThirdPersonCamera.FindAction("Rotate", throwIfNotFound: true);
        m_ThirdPersonCamera_FixedRotate = m_ThirdPersonCamera.FindAction("Fixed Rotate", throwIfNotFound: true);
        m_ThirdPersonCamera_Zoom = m_ThirdPersonCamera.FindAction("Zoom", throwIfNotFound: true);
        m_ThirdPersonCamera_Aim = m_ThirdPersonCamera.FindAction("Aim", throwIfNotFound: true);
        // Third-Person Movement
        m_ThirdPersonMovement = asset.FindActionMap("Third-Person Movement", throwIfNotFound: true);
        m_ThirdPersonMovement_Run = m_ThirdPersonMovement.FindAction("Run", throwIfNotFound: true);
        m_ThirdPersonMovement_Jump = m_ThirdPersonMovement.FindAction("Jump", throwIfNotFound: true);
        m_ThirdPersonMovement_Rotate = m_ThirdPersonMovement.FindAction("Rotate", throwIfNotFound: true);
        // Third-Person Combat
        m_ThirdPersonCombat = asset.FindActionMap("Third-Person Combat", throwIfNotFound: true);
        m_ThirdPersonCombat_Attack = m_ThirdPersonCombat.FindAction("Attack", throwIfNotFound: true);
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

    // Third-Person Camera
    private readonly InputActionMap m_ThirdPersonCamera;
    private List<IThirdPersonCameraActions> m_ThirdPersonCameraActionsCallbackInterfaces = new List<IThirdPersonCameraActions>();
    private readonly InputAction m_ThirdPersonCamera_Rotate;
    private readonly InputAction m_ThirdPersonCamera_FixedRotate;
    private readonly InputAction m_ThirdPersonCamera_Zoom;
    private readonly InputAction m_ThirdPersonCamera_Aim;
    public struct ThirdPersonCameraActions
    {
        private @PlayerInput m_Wrapper;
        public ThirdPersonCameraActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_ThirdPersonCamera_Rotate;
        public InputAction @FixedRotate => m_Wrapper.m_ThirdPersonCamera_FixedRotate;
        public InputAction @Zoom => m_Wrapper.m_ThirdPersonCamera_Zoom;
        public InputAction @Aim => m_Wrapper.m_ThirdPersonCamera_Aim;
        public InputActionMap Get() { return m_Wrapper.m_ThirdPersonCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ThirdPersonCameraActions set) { return set.Get(); }
        public void AddCallbacks(IThirdPersonCameraActions instance)
        {
            if (instance == null || m_Wrapper.m_ThirdPersonCameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ThirdPersonCameraActionsCallbackInterfaces.Add(instance);
            @Rotate.started += instance.OnRotate;
            @Rotate.performed += instance.OnRotate;
            @Rotate.canceled += instance.OnRotate;
            @FixedRotate.started += instance.OnFixedRotate;
            @FixedRotate.performed += instance.OnFixedRotate;
            @FixedRotate.canceled += instance.OnFixedRotate;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
        }

        private void UnregisterCallbacks(IThirdPersonCameraActions instance)
        {
            @Rotate.started -= instance.OnRotate;
            @Rotate.performed -= instance.OnRotate;
            @Rotate.canceled -= instance.OnRotate;
            @FixedRotate.started -= instance.OnFixedRotate;
            @FixedRotate.performed -= instance.OnFixedRotate;
            @FixedRotate.canceled -= instance.OnFixedRotate;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
        }

        public void RemoveCallbacks(IThirdPersonCameraActions instance)
        {
            if (m_Wrapper.m_ThirdPersonCameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IThirdPersonCameraActions instance)
        {
            foreach (var item in m_Wrapper.m_ThirdPersonCameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ThirdPersonCameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ThirdPersonCameraActions @ThirdPersonCamera => new ThirdPersonCameraActions(this);

    // Third-Person Movement
    private readonly InputActionMap m_ThirdPersonMovement;
    private List<IThirdPersonMovementActions> m_ThirdPersonMovementActionsCallbackInterfaces = new List<IThirdPersonMovementActions>();
    private readonly InputAction m_ThirdPersonMovement_Run;
    private readonly InputAction m_ThirdPersonMovement_Jump;
    private readonly InputAction m_ThirdPersonMovement_Rotate;
    public struct ThirdPersonMovementActions
    {
        private @PlayerInput m_Wrapper;
        public ThirdPersonMovementActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Run => m_Wrapper.m_ThirdPersonMovement_Run;
        public InputAction @Jump => m_Wrapper.m_ThirdPersonMovement_Jump;
        public InputAction @Rotate => m_Wrapper.m_ThirdPersonMovement_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_ThirdPersonMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ThirdPersonMovementActions set) { return set.Get(); }
        public void AddCallbacks(IThirdPersonMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_ThirdPersonMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ThirdPersonMovementActionsCallbackInterfaces.Add(instance);
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Rotate.started += instance.OnRotate;
            @Rotate.performed += instance.OnRotate;
            @Rotate.canceled += instance.OnRotate;
        }

        private void UnregisterCallbacks(IThirdPersonMovementActions instance)
        {
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Rotate.started -= instance.OnRotate;
            @Rotate.performed -= instance.OnRotate;
            @Rotate.canceled -= instance.OnRotate;
        }

        public void RemoveCallbacks(IThirdPersonMovementActions instance)
        {
            if (m_Wrapper.m_ThirdPersonMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IThirdPersonMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_ThirdPersonMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ThirdPersonMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ThirdPersonMovementActions @ThirdPersonMovement => new ThirdPersonMovementActions(this);

    // Third-Person Combat
    private readonly InputActionMap m_ThirdPersonCombat;
    private List<IThirdPersonCombatActions> m_ThirdPersonCombatActionsCallbackInterfaces = new List<IThirdPersonCombatActions>();
    private readonly InputAction m_ThirdPersonCombat_Attack;
    public struct ThirdPersonCombatActions
    {
        private @PlayerInput m_Wrapper;
        public ThirdPersonCombatActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_ThirdPersonCombat_Attack;
        public InputActionMap Get() { return m_Wrapper.m_ThirdPersonCombat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ThirdPersonCombatActions set) { return set.Get(); }
        public void AddCallbacks(IThirdPersonCombatActions instance)
        {
            if (instance == null || m_Wrapper.m_ThirdPersonCombatActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ThirdPersonCombatActionsCallbackInterfaces.Add(instance);
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
        }

        private void UnregisterCallbacks(IThirdPersonCombatActions instance)
        {
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
        }

        public void RemoveCallbacks(IThirdPersonCombatActions instance)
        {
            if (m_Wrapper.m_ThirdPersonCombatActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IThirdPersonCombatActions instance)
        {
            foreach (var item in m_Wrapper.m_ThirdPersonCombatActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ThirdPersonCombatActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ThirdPersonCombatActions @ThirdPersonCombat => new ThirdPersonCombatActions(this);
    public interface IThirdPersonCameraActions
    {
        void OnRotate(InputAction.CallbackContext context);
        void OnFixedRotate(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
    public interface IThirdPersonMovementActions
    {
        void OnRun(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
    public interface IThirdPersonCombatActions
    {
        void OnAttack(InputAction.CallbackContext context);
    }
}
