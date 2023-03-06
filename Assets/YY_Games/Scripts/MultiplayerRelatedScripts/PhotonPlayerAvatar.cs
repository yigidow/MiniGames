//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using Photon.Pun;
//using PlayFab.ClientModels;
//using PlayFab;
//using Newtonsoft.Json.Linq;

//namespace YY_Games_Scripts
//{
//    public class PhotonPlayerAvatar : MonoBehaviour, IPunObservable
//    {
//        #region References and objects
//        [Header("Photon Player Avatar Variables")]
//        [SerializeField] private SpriteRenderer mySpriteRenderer;
//        [SerializeField] private Rigidbody2D myRigidbody2D;
//        [SerializeField] private CapsuleCollider2D myCollider;
//        [SerializeField] private GameObject playerCanvas;

//        [Header("Photon Player Avatar Network Variables")]
//        [SerializeField] private PhotonView photonView;
//        public PhotonNetworkPlayer photonNetworkPlayer;

//        [Header("Photon Player Movement Variables")]
//        [SerializeField] private float movSpeed = 0.5f;
//        public PlayerStates playerStates = PlayerStates.IsGettingReady;
//        private Vector2 movementVector =new();
//        private Ray2D groundLeftRay;
//        private RaycastHit2D groundLeftRayHit;
//        private Ray2D groundRightRay;
//        private RaycastHit2D groundRightRayHit;
//        private Ray2D groundNormalRay;
//        private RaycastHit2D groundNormalRayHit;

//        [Header("Photon Player Jumping Variables")]
//        [SerializeField] private LayerMask groundRayLayerMask;
//        [SerializeField] private float jumpForce = 900f;
//        [SerializeField] private float downForce = 100f;
//        private Vector2 groundRayPos = new Vector2();
//        private RaycastHit2D groundRayHit;
//        private bool canJump = true;

//        [Header("Photon Player Animation Variables")]
//        [SerializeField] private Animator playerAnimator;
//        private static readonly int walkAnim = Animator.StringToHash("WalkAnim");
//        private static readonly int idlekAnim = Animator.StringToHash("IdleAnim");
//        private static readonly int velocity = Animator.StringToHash("velocity");
//        private static readonly int offground = Animator.StringToHash("offground");
//        private static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");

//        [Header("Camera")]
//        [SerializeField] private Camera mainCamera;

//        [Header("Game States and Timers")]
//        public bool isFinished = false;
//        public bool thisPlayerHasWon = false;
//        private int levelCompleteTime = 0;

//        [Header("Joy Stick Variables")]
//        [SerializeField] private GameObject JoystickCanvas;
//        [SerializeField] private FloatingJoystick floatingJoystick;
//        #endregion
//        #region Photon Functions
//        private void SettingUpPhotonRates()
//        {
//            PhotonNetwork.SendRate = 30;
//            PhotonNetwork.SerializationRate = 30;
//        }
//        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//        {
//            if (stream.IsWriting)
//            {
//                var flipXValue = mySpriteRenderer.flipX;

//                stream.SendNext(flipXValue);
//                stream.SendNext(transform.position);
//                stream.SendNext(myRigidbody2D.velocity);
//            }
//            if (stream.IsReading)
//            {
//                mySpriteRenderer.flipX = (bool)stream.ReceiveNext();
//                positionOnNetwork = (Vector3)stream.ReceiveNext();
//                velocityOnNetwork = (Vector2)stream.ReceiveNext();

//                //Getting network pos with lag
//                currentTime = 0f;
//                lastPacketReceivedTime = currentPacketTime;
//                currentPacketTime = (float) info.SentServerTime;
//                lastPositionOnNetwork = transform.position;

//                //float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
//                //positionOnNetwork += (Vector3) (velocityOnNetwork * lag);
//            }
//        }
//        private void SetUpPhysicsForForeignPlayer()
//        {
//            myRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
//            myRigidbody2D.isKinematic = true;
//            myRigidbody2D.simulated = false;
//            myRigidbody2D.sharedMaterial = null;
//        }
//        #endregion
//        #region Player States
//        public enum PlayerStates
//        {
//            Walk,
//            Idle,
//            OffGround,
//            WinLose,
//            IsGettingReady
//        }
//        private void HandlePlayerStates()
//        {
//            float horizontalInput = Input.GetAxis("Horizontal") + floatingJoystick.Horizontal;
//            horizontalInput = Mathf.Clamp(horizontalInput, 1, -1);

//            if (Mathf.Abs(horizontalInput) < 0.2f)
//            {
//                playerStates = PlayerStates.Idle;
//                return;
//            }

//            //if (Mathf.Abs(horizontalInput) > 0.2f)
//            //{
//            //    playerStates = PlayerStates.Walk;
//            //    return;
//            //}

//            if (GroundCheck() == false)
//            {
//                playerStates = PlayerStates.OffGround;
//                return;
//            }
            
//            if(isFinished == true)
//            {
//                playerStates = PlayerStates.WinLose;
//                return;
//            }

//            if (playerStates == PlayerStates.IsGettingReady)
//            {
//                return;
//            }

//            playerStates = PlayerStates.Idle;
//        }
//        private void HandlePlayerStatesInUpdate()
//        {
//            switch (playerStates)
//            {
//                case PlayerStates.Idle:
//                    HandleJumpingOfPlayer();
//                    break;
//                case PlayerStates.Walk:
//                    HandleJumpingOfPlayer();
//                    break;
//                case PlayerStates.OffGround:
//                    HandleVariableJumping();
//                    break;
//                case PlayerStates.WinLose:
//                    break;
//                case PlayerStates.IsGettingReady:
//                    break;
//                default:
//                    break;
//            }
//        }
//        private void HandlePlayerStatesInFixedUpdate()
//        {
//            switch (playerStates)
//            {
//                case PlayerStates.Idle:
//                    HandleMovementOfPlayer();
//                    HandlePlayerStates();
//                    HandleFlipingOfSprite();
//                    HandleAnimationTransitions();
//                    break;
//                case PlayerStates.Walk:
//                    HandleMovementOfPlayer();
//                    HandlePlayerStates();
//                    HandleFlipingOfSprite();
//                    HandleAnimationTransitions();
//                    break;
//                case PlayerStates.OffGround:
//                    HandleFlipingOfSprite();
//                    HandleMovementOfPlayer();
//                    HandlePlayerStates();
//                    HandleAnimationTransitions();
//                    AddDownwardForceToPlayer();
//                    break;
//                case PlayerStates.WinLose:
//                    HandleAnimationTransitions();
//                    RemoveHorizontalVelocityFromPlayer();
//                    break;
//                case PlayerStates.IsGettingReady:
//                    HandleAnimationTransitions();
//                    break;
//                default:
//                    break;
//            }
//        }   
//        #endregion
//        #region Movement Functions
//        private void HandleMovementOfPlayer()
//        {
           
//            float horizontal = Input.GetAxis("Horizontal") + floatingJoystick.Horizontal;
//            horizontal = Mathf.Clamp(horizontal, -1, 1);

 
//            if (playerStates == PlayerStates.Idle && Mathf.Abs(horizontal) < 0.2f)
//            {
//                horizontal = 0;

//                RemoveHorizontalVelocityFromPlayer();
//                return;
//            }

//            if (GroundCheck() == false || canJump == false)
//            {
//                movementVector.x = horizontal * movSpeed;
//                movementVector.y = myRigidbody2D.velocity.y;

//                if (Mathf.Abs(horizontal) > 0.2f)
//                {
//                    myRigidbody2D.velocity = movementVector;
//                }

//                return;
//            }

//            if (CurrentNormalFromRightRay() != Vector2.zero)
//            {
//                float angle = Vector2.Angle(CurrentNormalFromRightRay(), groundRightRay.direction);


//                if (angle < 35)
//                {
//                    movementVector.x = horizontal * movSpeed;
//                    movementVector.y = 0;

//                    Vector2 perpendicular = Vector2.Perpendicular(CurrentNormalFromRightRay());

//                    if (horizontal > 0)
//                    {
//                        movementVector = perpendicular * -movementVector.x;
//                    }

//                    if (horizontal < 0)
//                    {

//                        if (CurrentNormalFromGround() != Vector2.up)
//                        {
//                            movementVector = -perpendicular * movementVector.x;
//                        }
//                    }

//                    if (Mathf.Abs(horizontal) > 0.2f)
//                    {
//                        myRigidbody2D.velocity = movementVector;
//                    }

//                    return;
//                }
//            }

//            if (CurrentNormalFromLeftRay() != Vector2.zero)
//            {
//                float angle = Vector2.Angle(CurrentNormalFromLeftRay(), groundLeftRay.direction);

//                if (angle < 35)
//                {
//                    movementVector.x = horizontal * movSpeed;
//                    movementVector.y = 0;
//                    Vector2 perpendicular = Vector2.Perpendicular(CurrentNormalFromLeftRay());

//                    if (horizontal > 0)
//                    {
//                        if (CurrentNormalFromGround() != Vector2.up)
//                        {
//                            movementVector = perpendicular * -movementVector.x;
//                        }
//                    }

//                    if (horizontal < 0)
//                    {
//                        movementVector = -perpendicular * movementVector.x;
//                    }

//                    if (Mathf.Abs(horizontal) > 0.2f)
//                    {
//                        myRigidbody2D.velocity = movementVector;
//                    }

//                    return;
//                }
//            }

//            if (CurrentNormalFromGround() == Vector2.zero)
//            {
//                movementVector.x = horizontal * movSpeed;
//                movementVector.y = myRigidbody2D.velocity.y;

//                if (Mathf.Abs(horizontal) > 0.2f)
//                {
//                    myRigidbody2D.velocity = movementVector;
//                }
//            }
//            else
//            {
//                movementVector.x = horizontal * movSpeed;
//                movementVector.y = 0;

//                Vector2 perpendicular = Vector2.Perpendicular(CurrentNormalFromGround());

//                if (horizontal > 0)
//                {
//                    movementVector = perpendicular * -movementVector.x;
//                }

//                if (horizontal < 0)
//                {
//                    movementVector = -perpendicular * movementVector.x;
//                }

//                if (Mathf.Abs(horizontal) > 0.2f)
//                {
//                    myRigidbody2D.velocity = movementVector;
//                }
//            }
//        }
//        private void HandleFlipingOfSprite()
//        {
//            float horizontalInput = Input.GetAxis("Horizontal") + floatingJoystick.Horizontal;
//            horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);

//            if (horizontalInput > 0.1)
//            {
//                mySpriteRenderer.flipX = false;
//            }
//            else if (horizontalInput < -0.1)
//            {
//                mySpriteRenderer.flipX = true;
//            }
//        }
//        private void HandleJumpingOfPlayer()
//        {
//            if (canJump == false) return;
            
//            if(Input.GetButtonDown("Jump") && groundRayHit.collider != null)
//            {
//                myRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//                StartCoroutine(DisableJumping());
//            }
//        }
//        public void HandleJumpingOfPlayerForMobile()
//        {
//            if (canJump == false) return;

//            if (groundRayHit.collider != null)
//            {
//                myRigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//                StartCoroutine(DisableJumping());
//            }
//        }
//        private void AddDownwardForceToPlayer()
//        {
//            if(myRigidbody2D.velocity.y < 3.5f)
//            {
//                if (myRigidbody2D.velocity.y > 0)
//                {
//                    myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0f);
//                }
//                myRigidbody2D.AddForce(Vector2.down * downForce, ForceMode2D.Impulse);
//            }
//        }
//        private void HandleVariableJumping()
//        {
//            if (!(myRigidbody2D.velocity.y > 0)) return;
            
//            if (Input.GetButtonUp("Jump"))
//            {
//             myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0f);
//            }
            
//        }
//        private IEnumerator DisableJumping()
//        {
//            canJump = false;
//            yield return new WaitForSeconds(0.1f);
//            canJump = true;
//        }
//        private bool GroundCheck()
//        {
//            //Setting a ray to find groun 
//            groundRayPos.x = transform.position.x;
//            groundRayPos.y = transform.position.y - 1.15f;

//            //Debug.DrawRay(groundRayPos, Vector2.down * 0.2f, Color.red);

//            groundRayHit = Physics2D.CapsuleCast(
//                myCollider.bounds.center,
//                myCollider.bounds.size, 
//                CapsuleDirection2D.Vertical, 
//                0f,
//                Vector2.down,
//                0.25f,
//                groundRayLayerMask
//                );

//            if (groundRayHit)
//            {
//                if (groundRayHit.collider.CompareTag("Ground"))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//        //For Moving in slopes
//        private Vector2 CurrentNormalFromRightRay()
//        {
           
//            groundRightRay.origin = new Vector2(
//                transform.position.x,
//                transform.position.y - myCollider.bounds.extents.y
//            );
//            groundRightRay.direction = Vector2.right;

//            groundRightRayHit = Physics2D.Raycast(groundRightRay.origin,
//                groundRightRay.direction,
//                myCollider.bounds.extents.x + 0.15f,
//                groundRayLayerMask
//            );

//            //Returning the normal of the ray that was hit
//            if (groundRightRayHit.collider != null)
//            {
//                return groundRightRayHit.normal.normalized;
//            }

//            return Vector2.zero;
//        }
//        private Vector2 CurrentNormalFromLeftRay()
//        {
            
//            groundLeftRay.origin = new Vector2(
//                transform.position.x,
//                transform.position.y - myCollider.bounds.extents.y
//            );
//            groundLeftRay.direction = Vector2.left;

//            groundLeftRayHit = Physics2D.Raycast(groundLeftRay.origin,
//                groundLeftRay.direction,
//                myCollider.bounds.extents.x + 0.15f,
//                groundRayLayerMask
//            );

//            if (groundLeftRayHit.collider != null)
//            {
//                return groundLeftRayHit.normal.normalized;
//            }

//            return Vector2.zero;
//        }
//        private Vector2 CurrentNormalFromGround()
//        {
            
//            groundNormalRay.origin = new Vector2(
//                transform.position.x,
//                transform.position.y - myCollider.bounds.extents.y
//            );
//            groundNormalRay.direction = Vector2.down;

//            groundNormalRayHit = Physics2D.Raycast(
             
//                groundNormalRay.origin,
//                groundNormalRay.direction,
//                0.25f,
//                groundRayLayerMask
//            );
            
//            if (groundNormalRayHit.collider != null)
//            {
                
//                if (groundNormalRayHit.collider.CompareTag("Ground"))
//                {
//                    return groundNormalRayHit.normal.normalized;
//                }
//            }

//            return Vector2.zero;
//        }
//        private void RemoveHorizontalVelocityFromPlayer()
//        {
//            movementVector.x = 0;
//            movementVector.y = myRigidbody2D.velocity.y;

//            myRigidbody2D.velocity = movementVector;
//        }
//        #endregion
//        #region Functions For JoyStick
//        private void TurnOnJoyStickCanvasIfOnMMobile()
//        {
//            if(Application.platform == RuntimePlatform.Android)
//            {
//                JoystickCanvas.SetActive(true);
//            }
//            else
//            {
//                JoystickCanvas.SetActive(false);
//            }
//        }
//        #endregion
//        #region Animation Functions
//        private void HandleAnimationTransitions()
//        {
//            if (playerStates == PlayerStates.IsGettingReady)
//            {
//                playerAnimator.SetFloat(verticalVelocity, 0);
//                playerAnimator.SetBool(offground, false);
//                playerAnimator.SetFloat(velocity, 0);

//                return;
//            }

//            if (isFinished == true)
//            {
//                playerAnimator.SetFloat(verticalVelocity, 0);
//                playerAnimator.SetBool(offground, false);
//                playerAnimator.SetFloat(velocity, 0);

//                return;
//            }

//            float horizontalInput = Input.GetAxis("Horizontal") + floatingJoystick.Horizontal;

//            horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);

//            bool isOnGround = GroundCheck();

//            if (isOnGround == false)
//            {
//                playerAnimator.SetBool(offground, true);
//                playerAnimator.SetFloat(verticalVelocity, myRigidbody2D.velocity.y);
//                return;
//            }

//            //Setting vertical velocity back to false
//            playerAnimator.SetFloat(verticalVelocity, 0);
//            //Setting the OffGround to false so that we can run grounded animations
//            playerAnimator.SetBool(offground, false);
//            //Animation parameter velocity handling
//            playerAnimator.SetFloat(velocity, Mathf.Abs(horizontalInput));
//        }
//        #endregion
//        #region Win Lose Feature Functions
//        public void DisplayWinLoseScreen()
//        {
//            if (photonView.IsMine == false) return;

//            if(thisPlayerHasWon == true)
//            {
//                photonNetworkPlayer.EnableWinLoseScreen("You Won! Returning to Main Menu");
//                AddWinLossToPlayerRecordUsingCloud(true);
//                AddLevelCompleteTimesToLeaderBoardWithCloud();
//                return;
//            }
//            photonNetworkPlayer.EnableWinLoseScreen("You Lost! Returning to Main Menu");
//            AddWinLossToPlayerRecordUsingCloud(false);
//        }
//        private void AddWinLossToPlayerRecordUsingCloud(bool hasWon)
//        {
//            //Request to execute cloud script
//            if (hasWon)
//            {
//                var executeCloudScriptRequest = new ExecuteCloudScriptRequest
//                {
//                    FunctionName = "addOneWinToPlayerData",
//                    GeneratePlayStreamEvent = true
//                };
//                PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
//                    resultCallback: result =>
//                    {
//                    // Getting Result From Server
//                    var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

//                        print(serilizedResult["message"]);

//                    },
//                    errorCallback: error =>
//                    {
//                        print(error.ErrorMessage);
//                    }
//                    );
//                return;
//            }
//            else
//            {
//                var executeCloudScriptRequest = new ExecuteCloudScriptRequest
//                {
//                    FunctionName = "addOneLossToPlayerData",
//                    GeneratePlayStreamEvent = true
//                };
//                PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
//                    resultCallback: result =>
//                    {
//                        // Getting Result From Server
//                        var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

//                        print(serilizedResult["message"]);

//                    },
//                    errorCallback: error =>
//                    {
//                        print(error.ErrorMessage);
//                    }
//                    );
//                return;
//            }
//        }
//        private void AddLevelCompleteTimesToLeaderBoardWithCloud()
//        {
//            string funcNameToCall = "UpdateLevelOneTimeStat";

//            switch (PhotonRoom.instance.levelCreated)
//            {
//                case 1:
//                    funcNameToCall = "UpdateLevelOneTimeStat";
//                    break;
//                case 2:
//                    funcNameToCall = "UpdateLevelTwoTimeStat";
//                    break;
//                default:
//                    funcNameToCall = "UpdateLevelOneTimeStat";
//                    break;
//            }
//            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
//            {
//                FunctionName = funcNameToCall,
//                FunctionParameter = new
//                {
//                    timeInSeconds = levelCompleteTime
//                },
//                GeneratePlayStreamEvent = true
//            };
//            PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
//                resultCallback: result =>
//                {
//                  // Getting Result From Server
//                  var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

//                    print(serilizedResult["message"]);

//                },
//                errorCallback: error =>
//                {
//                    print(error.ErrorMessage);
//                }
//                );
//            return;
//        }
//        #endregion
//        #region Functions To Start The Race And Race Timer
//        public void SetToIdleStateAndStartLevelTimer()
//        {
//            playerStates = PlayerStates.Idle;
//            StartCoroutine(Routine_LevelCompleteTimeRoutine());
//        }
//        private IEnumerator Routine_LevelCompleteTimeRoutine()
//        {
//            while(isFinished == false)
//            {
//                levelCompleteTime++;
//                yield return new WaitForSeconds(0.01f);
//            }
//        }
//        #endregion
//        #region Camera & UI Functions
//        private IEnumerator FindTheCamera()
//        {
//            while(mainCamera == null)
//            {
//                mainCamera = Camera.main;
//                yield return new WaitForSeconds(0.1f);
//            }
//            SetPositionOfCameraForLocalPlayer();
//            yield break;
//        }
//        private void SetPositionOfCameraForLocalPlayer()
//        {
//            mainCamera.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
//            mainCamera.gameObject.transform.parent = transform;
//        }
//        private void ActivatePlayerCanvas()
//        {
//            playerCanvas.SetActive(true);
//        }
//        #endregion
//        #region Lag Compensation Functions
//        [Header("Lag Compensation Variables")]
//        private Vector3 positionOnNetwork;
//        private Vector2 velocityOnNetwork;
//        private Vector3 lastPositionOnNetwork;
//        private float lastPacketReceivedTime;
//        private float currentPacketTime;
//        private float currentTime;
//        private readonly float teleportIfDistanceGreaterThan = 3.0f;
//        private void MovePlayerWithLagCompensation()
//        {
//            double currentGoalTime = currentPacketTime - lastPacketReceivedTime;
//            currentTime += Time.deltaTime;

//            transform.position = Vector3.Lerp(lastPositionOnNetwork, positionOnNetwork, (float)(currentTime / currentGoalTime));

//            if(Vector3.Distance(transform.position, positionOnNetwork) > teleportIfDistanceGreaterThan)
//            {
//                transform.position = positionOnNetwork;
//            }
//        }
//        #endregion
//        #region Unity Functions
//        void Awake()
//        {
//            SettingUpPhotonRates();
//        }
//        void Start()
//        {
//            if (photonView.IsMine == false) 
//            {
//                SetUpPhysicsForForeignPlayer();
//                JoystickCanvas.SetActive(false);
//                return;
//            }
//            StartCoroutine(FindTheCamera());
//            ActivatePlayerCanvas();
//            TurnOnJoyStickCanvasIfOnMMobile();
//        }

//        void Update()
//        {
//            if (photonView.IsMine == false) return;
//            HandlePlayerStates();
//            HandlePlayerStatesInUpdate();
//        }
//        private void FixedUpdate()
//        {
//            if (photonView.IsMine == false)
//            {
//                MovePlayerWithLagCompensation();
//                return;
//            }
//            HandlePlayerStatesInFixedUpdate();
//        }
//        private void OnTriggerEnter2D(Collider2D collision)
//        {
//            if (collision.CompareTag("FinishLine"))
//            {
//                isFinished = true;
//                thisPlayerHasWon = true;

//                PhotonRoom.instance.gameObject.GetPhotonView().RPC(
//                  "FinishGame",
//                   RpcTarget.AllViaServer
//                   );
//            }
//        }
//        #endregion
//    }
//}

