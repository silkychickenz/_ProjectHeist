// GENERATED AUTOMATICALLY FROM 'Assets/default/input/DefaultControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DefaultControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""371f8e53-5796-4d07-95a5-a1fba9a5562b"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""22ee022c-0509-4b1f-935b-717ca7669a55"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookAround"",
                    ""type"": ""PassThrough"",
                    ""id"": ""899fc690-572a-4d1a-947e-a4f2e9be3f6d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TEMPcameratoggle"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5bc9d66b-c5f2-4754-bc88-9e059ac489c8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""flipGravityPlayer"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d6263a4d-f14e-45f1-ac5f-3152c6aae01a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""movementXbox"",
                    ""id"": ""dd2e5475-3901-4b74-89fe-093a048e9875"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""896470ce-3426-41b1-838b-c670b2e87c28"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bd68a134-8747-49bf-9233-36125ef47cbc"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""82c4afb6-faba-4776-825a-8d7eec7b87a5"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""96db0dfa-a766-4641-99ed-d4cefa70bc65"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""movementkeyboard"",
                    ""id"": ""a333ab86-5353-41ba-b236-b38388cc351d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d7c24f77-b667-4f73-b6ab-581bfdd8fbcb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f83283e2-f666-45c4-a24f-1521f33451fb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a1efde1d-3334-41e2-a438-519f65e25568"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b4ae31af-4bad-46f9-9227-662c614f7163"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""xBox"",
                    ""id"": ""a28e4b2b-8559-4801-932c-916abbfe942a"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=2,y=2),InvertVector2(invertX=false)"",
                    ""groups"": """",
                    ""action"": ""LookAround"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""255a53bf-9e3a-402d-a57e-22425988a4b0"",
                    ""path"": ""<XInputController>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fd0129f0-f6fe-48c6-9f6e-349c16a9843a"",
                    ""path"": ""<XInputController>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1f82551c-fe33-4ba0-b0e5-477cdad82b67"",
                    ""path"": ""<XInputController>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f1b49333-b160-4ece-9ba1-3a86fb5e32e2"",
                    ""path"": ""<XInputController>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""57ba64b6-87ff-45eb-804e-3d960dcbc553"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=0.1,y=0.1)"",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de93c3ba-0dad-4443-aade-e1afad549271"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController;keyboard&mouse"",
                    ""action"": ""TEMPcameratoggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32882064-5632-453d-a641-4b84f04647bc"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController;keyboard&mouse"",
                    ""action"": ""TEMPcameratoggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""xBox"",
                    ""id"": ""a79c5777-b7b3-4df6-8b2e-45d9e4825c5d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""21f0b501-5298-433a-9727-7c9b5a3baa12"",
                    ""path"": ""<XInputController>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ff6dc948-8332-418f-a92b-d7c3d9a3071a"",
                    ""path"": ""<XInputController>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""62cf7d4a-bbf5-4f37-9754-805c3a454b52"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d50d972f-7737-4684-aa92-35764178ec6e"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""keyboard"",
                    ""id"": ""2ca76b9d-6f3f-45f5-ae43-f4f27185acdb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7d15b11d-d122-4608-ae5a-62a069baa293"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3fe8fe8f-8837-4b57-927a-6f4d05619746"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""88c64fbc-16da-40fa-91cf-9b8831e5dca5"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2bdcd988-4471-4526-980d-36db753579de"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard&mouse"",
                    ""action"": ""flipGravityPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""xBoxController"",
            ""bindingGroup"": ""xBoxController"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""keyboard&mouse"",
            ""bindingGroup"": ""keyboard&mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_move = m_Player.FindAction("move", throwIfNotFound: true);
        m_Player_LookAround = m_Player.FindAction("LookAround", throwIfNotFound: true);
        m_Player_TEMPcameratoggle = m_Player.FindAction("TEMPcameratoggle", throwIfNotFound: true);
        m_Player_flipGravityPlayer = m_Player.FindAction("flipGravityPlayer", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_move;
    private readonly InputAction m_Player_LookAround;
    private readonly InputAction m_Player_TEMPcameratoggle;
    private readonly InputAction m_Player_flipGravityPlayer;
    public struct PlayerActions
    {
        private @DefaultControls m_Wrapper;
        public PlayerActions(@DefaultControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @move => m_Wrapper.m_Player_move;
        public InputAction @LookAround => m_Wrapper.m_Player_LookAround;
        public InputAction @TEMPcameratoggle => m_Wrapper.m_Player_TEMPcameratoggle;
        public InputAction @flipGravityPlayer => m_Wrapper.m_Player_flipGravityPlayer;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @LookAround.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLookAround;
                @LookAround.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLookAround;
                @LookAround.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLookAround;
                @TEMPcameratoggle.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTEMPcameratoggle;
                @TEMPcameratoggle.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTEMPcameratoggle;
                @TEMPcameratoggle.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTEMPcameratoggle;
                @flipGravityPlayer.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFlipGravityPlayer;
                @flipGravityPlayer.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFlipGravityPlayer;
                @flipGravityPlayer.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFlipGravityPlayer;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @move.started += instance.OnMove;
                @move.performed += instance.OnMove;
                @move.canceled += instance.OnMove;
                @LookAround.started += instance.OnLookAround;
                @LookAround.performed += instance.OnLookAround;
                @LookAround.canceled += instance.OnLookAround;
                @TEMPcameratoggle.started += instance.OnTEMPcameratoggle;
                @TEMPcameratoggle.performed += instance.OnTEMPcameratoggle;
                @TEMPcameratoggle.canceled += instance.OnTEMPcameratoggle;
                @flipGravityPlayer.started += instance.OnFlipGravityPlayer;
                @flipGravityPlayer.performed += instance.OnFlipGravityPlayer;
                @flipGravityPlayer.canceled += instance.OnFlipGravityPlayer;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_xBoxControllerSchemeIndex = -1;
    public InputControlScheme xBoxControllerScheme
    {
        get
        {
            if (m_xBoxControllerSchemeIndex == -1) m_xBoxControllerSchemeIndex = asset.FindControlSchemeIndex("xBoxController");
            return asset.controlSchemes[m_xBoxControllerSchemeIndex];
        }
    }
    private int m_keyboardmouseSchemeIndex = -1;
    public InputControlScheme keyboardmouseScheme
    {
        get
        {
            if (m_keyboardmouseSchemeIndex == -1) m_keyboardmouseSchemeIndex = asset.FindControlSchemeIndex("keyboard&mouse");
            return asset.controlSchemes[m_keyboardmouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLookAround(InputAction.CallbackContext context);
        void OnTEMPcameratoggle(InputAction.CallbackContext context);
        void OnFlipGravityPlayer(InputAction.CallbackContext context);
    }
}
