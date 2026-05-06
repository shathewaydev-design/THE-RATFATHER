using UnityEngine;
using System;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioSource AudioFootsteps;
        public AudioSource LandingAudio;
        public AudioSource AudioFoley;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded and DoubleJump logic")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        private int maxJumps = 1;
        [SerializeField] private int jumpCount = 0;
        //private bool _jumpPressedLastFrame;
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        [Header("Camera Settings")]
        [Tooltip("Mouse sensitivity multiplier")]
        public float MouseSensitivity = 1.0f;
        public float GamepadSensitivity = 100.0f;

        [Tooltip("Invert vertical camera axis")]
        public bool InvertY = false;

        [Tooltip("Invert horizontal camera axis")]
        public bool InvertX = false;
        [Header("Game mechanic Input")]
        public static ThirdPersonController Instance;
        public event Action OnStopInteract;
        public event Action OnTiltLeft;
        public event Action OnTiltRight;
        public event Action OnInteract; 
        public event Action OnSprinkle; 
        public event Action OnMouseClick; 
        //public event Action OnMousePositionChange; 
        public event Action OnOpenInventory; 
        private InputActionMap playerMap;
        private InputActionMap cookingMap;
        private InputActionMap mouseMap;

        public InputAction stopInteract;//press Q
        public InputAction tiltLeft;//press A
        public InputAction tiltRight;//press D
        public InputAction sprinkle;//press T
        public InputAction mouseClick;//press (Left) Mouse Button
        public InputAction mousePosition;//mouse position for dragging objects
        public InputAction openInventory;//press Tab
        public event Action<Vector2> OnMousePosition;
        public event Action<bool> OnMouseDrag;

        //[SerializeField] private UI_Inventory uiInventory;
        private void OnEnable()//used to subscribe to input events, make sure to unsubscribe in OnDisable to avoid bugs
        {
        #if ENABLE_INPUT_SYSTEM
            tiltLeft.performed += OnTiltLeftPerformed;
            tiltRight.performed += OnTiltRightPerformed;
            stopInteract.performed += OnStopInteractPerformed;
            sprinkle.performed += OnSprinklePerformed;
            openInventory.performed += OnOpenInventoryPerformed;

            mouseClick.performed += OnMouseClickPerformed;
            mouseClick.canceled += OnMouseClickCanceled;
            
        #endif
        }

        private void OnDisable()
        {
        #if ENABLE_INPUT_SYSTEM
            tiltLeft.performed -= OnTiltLeftPerformed;
            tiltRight.performed -= OnTiltRightPerformed;
            stopInteract.performed -= OnStopInteractPerformed;
            sprinkle.performed -= OnSprinklePerformed;
            openInventory.performed -= OnOpenInventoryPerformed;

            mouseClick.performed -= OnMouseClickPerformed;
            mouseClick.canceled -= OnMouseClickCanceled;
        #endif
        }

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            //game mechanic input
            Instance = this;
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();

            var cookingMap = _playerInput.actions.FindActionMap("Cooking");

            tiltLeft = cookingMap.FindAction("TiltLeft");
            tiltRight = cookingMap.FindAction("TiltRight");
            stopInteract = cookingMap.FindAction("StopInteract");
            sprinkle = cookingMap.FindAction("Sprinkle");
            var mouseMap = _playerInput.actions.FindActionMap("Mouse");
            mouseClick = mouseMap.FindAction("LeftClick");
            mousePosition = mouseMap.FindAction("MousePosition");


            // OPTIONAL: if you have interact in Player map
            var playerMap = _playerInput.actions.FindActionMap("Player");
            if (playerMap != null)
            {
                var interact = playerMap.FindAction("Interact");
                if (interact != null)
                    interact.performed += ctx => OnInteract?.Invoke();
                openInventory = playerMap.FindAction("OpenInventory");
                if (openInventory != null)                    
                    openInventory.performed += ctx => OnOpenInventory?.Invoke();
            }
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            //initialize inventory UI
            //uiInventory.SetInventory(inventorySystem);
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
            
            #if ENABLE_INPUT_SYSTEM
            if (mousePosition != null)
            {
                OnMousePosition?.Invoke(mousePosition.ReadValue<Vector2>());
            }

            if (mouseClick != null)
            {
                OnMouseDrag?.Invoke(mouseClick.IsPressed());
            }
        #endif

            // HandleGameMechanicInput(); // Check for game mechanic input each frame
            // tiltLeft.performed += ctx => OnTiltLeft?.Invoke();
            // tiltRight.performed += ctx => OnTiltRight?.Invoke();
            // stopInteract.performed += ctx => OnStopInteract?.Invoke();  
            // mouseClick.performed += ctx => OnMouseClick?.Invoke();
            // mousePosition.performed += ctx => OnMousePositionChange?.Invoke();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                
                //added mouse sensitivity and inversion options
                float sensitivity = IsCurrentDeviceMouse ? MouseSensitivity : GamepadSensitivity;

                float lookX = _input.look.x;
                float lookY = _input.look.y;

                // Apply inversion
                if (InvertX) lookX *= -1f;
                if (InvertY) lookY *= -1f;

                // Apply sensitivity
                lookX *= MouseSensitivity;
                lookY *= MouseSensitivity;
                _cinemachineTargetYaw += lookX * deltaTimeMultiplier;
                _cinemachineTargetPitch += lookY * deltaTimeMultiplier;
                //^original below
                //_cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                //_cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public void EnableDoubleJump()
        {
            maxJumps = 2;
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                jumpCount = 0;//reset jumps when touching ground

                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // // Jump
                // if (_input.jump)// && _jumpTimeoutDelta <= 0.0f
                // {
                //     if(jumpCount < maxJumps)
                //     {
                //         jumpCount++;
                //         // the square root of H * -2 * G = how much velocity needed to reach desired height
                //         _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                //         // update animator if using character
                //         if (_hasAnimator)
                //         {
                //             _animator.SetBool(_animIDJump, true);
                //         }
                //     }
                // }

                // jump timeout
                // if (_jumpTimeoutDelta >= 0.0f)
                // {
                //     _jumpTimeoutDelta -= Time.deltaTime;
                // }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                /// if we are not grounded, do not jump
                // if(jumpCount >= maxJumps)
                // {
                //     _input.jump = false;
                // }
            }
            // Jump
            if (_input.jump)// && _jumpTimeoutDelta <= 0.0f
            {
                if(jumpCount < maxJumps)
                {
                    jumpCount++;
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                    _input.jump = false;
                }
            }
            
            

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {

                if (AudioFootsteps != null)
                    AudioFootsteps.Play();
                if (AudioFoley != null)
                    AudioFoley.Play();
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (LandingAudio != null)
                    LandingAudio.Play();

            }
        }

        private void OnTiltLeftPerformed(InputAction.CallbackContext ctx)
        {
            OnTiltLeft?.Invoke();
        }

        private void OnTiltRightPerformed(InputAction.CallbackContext ctx)
        {
            OnTiltRight?.Invoke();
        }
        private void OnSprinklePerformed(InputAction.CallbackContext ctx)
        {
            OnSprinkle?.Invoke();
        }

        private void OnStopInteractPerformed(InputAction.CallbackContext ctx)
        {
            OnStopInteract?.Invoke();
        }
        private void OnOpenInventoryPerformed(InputAction.CallbackContext ctx)
        {
            OnOpenInventory?.Invoke();
        }

        private void OnMouseClickPerformed(InputAction.CallbackContext ctx)
        {
            OnMouseClick?.Invoke();
        }

        private void OnMouseClickCanceled(InputAction.CallbackContext ctx)
        {
            // optional: release event 
        }
    }
}