using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class Piece : MonoBehaviour
    {
        #region Variables and References
        [Header("Board")]
        [SerializeField] private Board gameBoard;

        [Header("Piece Block Components")]
        public GameObject[] blocksInPiecePrefab;
        public GameObject[] blocksInPiece;
        public Transform[] blockPositions;
        public GameObject blocksInPieceParent;

        [Header("Movement Variables")]
        public float stepDelay = 5f;
        public float moveDelay = 0.1f;
        public float lockDelay = 2.5f;
        private float stepTime;
        private float moveTime;
        private float lockTime;

        [Header("Rotation Variables")]
        public float rotateDelay = 0.1f;
        private float rotateTime;

        [Header("Movement Checks")]
        public bool canMoveDown = true;
        public bool canMoveLeft = true;
        public bool canMoveRight = true;

        public bool canRotateToHorizontal = true;
        public bool canRotateToVertical = true;

        public bool isPieceLocked = false;

        public enum PiecePositions
        {
            pos0,
            pos1,
            pos2,
            pos3,
        }
        [Header("Piece Pos")]
        public PiecePositions currentPosition = PiecePositions.pos0;

        #endregion
        #region Functions To Initilaze the spawned piece
        private void Initialize()
        {
            gameBoard = FindObjectOfType<Board>();
            for (int i = 0; i < blocksInPiecePrefab.Length; i++)
            {
                blocksInPiece[i] = Instantiate(blocksInPiecePrefab[i], (Vector3)blockPositions[i].position, Quaternion.identity, blocksInPieceParent.transform);
                blocksInPiece[i].gameObject.tag = ("PieceBlock");
            }
        }
        #endregion
        #region Function to Handle Movement and Rotation of Piece
        private void MoveHorrizontal()
        {
            moveTime = Time.time + moveDelay;

            if (currentPosition == PiecePositions.pos0 || currentPosition == PiecePositions.pos2) 
            {
                if (transform.localPosition.x > -4.5f && Input.GetKey(KeyCode.A))
                {
                    if (canMoveLeft)
                    {
                        transform.position += new Vector3(-1, 0, 0);
                    }
                }
                else if (transform.localPosition.x < 3.5f && Input.GetKey(KeyCode.D))
                {
                    if (canMoveRight)
                    {
                        transform.position += new Vector3(1, 0, 0);
                    }
                }
            }
            else
            {
                if (transform.localPosition.x > -4.5f && Input.GetKey(KeyCode.A))
                {
                    if (canMoveLeft)
                    {
                        transform.position += new Vector3(-1, 0, 0);
                    }
                }
                else if (transform.localPosition.x < 4.5f && Input.GetKey(KeyCode.D))
                {
                    if (canMoveRight)
                    {
                        transform.position += new Vector3(1, 0, 0);
                    }
                }
            }
        }
        private void MoveVerticalFreeFall()
        {
            stepTime = Time.time + stepDelay;
            transform.localPosition += new Vector3(0, -1, 0);
            if (currentPosition == PiecePositions.pos1 || currentPosition == PiecePositions.pos3)
            {
                if (transform.position.y == -9)
                {
                    canMoveDown = false;
                }
            }
            else
            {
                if (transform.position.y == -10)
                {
                    canMoveDown = false;
                }
            }
        }
        private void MoveVertical()
        {
            moveTime = Time.time + moveDelay;
            if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition += new Vector3(0, -1, 0);
            }
            if(currentPosition == PiecePositions.pos1 || currentPosition == PiecePositions.pos3)
            {
                if (transform.position.y == -9)
                {
                    canMoveDown = false;
                }
            }
            else
            {
                if (transform.position.y == -10)
                {
                    canMoveDown = false;
                }
            }
        }
        private void RotateRight()
        {
            rotateTime = Time.time + rotateDelay;

            if (currentPosition == PiecePositions.pos0)
            {
                if (canRotateToVertical)
                {
                    currentPosition = PiecePositions.pos1;
                }
                //Bot Wall Kick
                if (transform.localPosition.y == -10)
                {
                    transform.position += new Vector3(0, 1, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos1)
            {
                if (canRotateToHorizontal)
                {
                    currentPosition = PiecePositions.pos2;
                }

                //Wall kick
                if (transform.localPosition.x == 4.5)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos2)
            {
                if (canRotateToVertical)
                {
                    currentPosition = PiecePositions.pos3;
                }
                //Bot Wall Kick
                if (transform.localPosition.y == -10)
                {
                    transform.position += new Vector3(0, 1, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos3)
            {
                if (canRotateToHorizontal)
                {
                    currentPosition = PiecePositions.pos0;
                }

                //Wall kick
                if (transform.localPosition.x == 4.5)
                {
                    transform.localPosition += new Vector3(-1, 0, 0);
                }
            }
        }
        private void RotateLeft()
        {
            rotateTime = Time.time + rotateDelay;

            if (currentPosition == PiecePositions.pos0)
            {
                if (canRotateToVertical)
                {
                    currentPosition = PiecePositions.pos3;
                }
                //Bot Wall Kick
                if (transform.localPosition.y == -10)
                {
                    transform.position += new Vector3(0, 1, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos1)
            {
                if (canRotateToHorizontal)
                {
                    currentPosition = PiecePositions.pos0;
                }

                //Wall kick
                if (transform.localPosition.x == 4.5) 
                {
                    transform.localPosition += new Vector3(-1, 0, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos2)
            {
                if (canRotateToVertical)
                {
                    currentPosition = PiecePositions.pos1;
                }
                //Bot Wall Kick
                if (transform.localPosition.y == -10)
                {
                    transform.position += new Vector3(0, 1, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos3)
            {
                if (canRotateToHorizontal) 
                {
                    currentPosition = PiecePositions.pos2;
                }

                //Wall kick
                if (transform.localPosition.x == 4.5)
                {
                    transform.localPosition += new Vector3(-1, 0, 0);
                }

            }
        }

        //Piece Positions
        private void HandleRotation()
        {
            switch (currentPosition)
            {
                case PiecePositions.pos0:
                    blocksInPiece[0].transform.position = blockPositions[0].position;
                    blocksInPiece[1].transform.position = blockPositions[1].position;
                    break;
                case PiecePositions.pos1:
                    blocksInPiece[0].transform.position = blockPositions[0].position;
                    blocksInPiece[1].transform.position = blockPositions[2].position;
                    break;
                case PiecePositions.pos2:
                    blocksInPiece[0].transform.position = blockPositions[1].position;
                    blocksInPiece[1].transform.position = blockPositions[0].position;
                    break;
                case PiecePositions.pos3:
                    blocksInPiece[0].transform.position = blockPositions[2].position;
                    blocksInPiece[1].transform.position = blockPositions[0].position;
                    break;
            }
        }
        #endregion

        #region Functions to Interact with board
        public void LockPiece()
        {
            if (!canMoveDown)
            {
                if (lockDelay >= 0)
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        lockDelay -= Time.time;
                    }
                    else
                    {
                        lockDelay -= Time.deltaTime;
                    }
                }
                else
                {
                    lockDelay = 0;
                }
            }
            else
            {
                lockDelay = 2.5f;
            }
            if(lockDelay <= 0f)
            {
                lockDelay = 0;
                isPieceLocked = true;
            }      
        }

        #endregion
        #region Unity Functions
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPieceLocked)
            {
                // Hande movement with horizontal Inputs
                if (Time.time > moveTime)
                {
                    MoveHorrizontal();
                    if(canMoveDown)
                    {
                        MoveVertical();
                    }
                }

                //Handling Rotation Movements
                if (Time.time > rotateTime)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        RotateRight();
                        HandleRotation();
                    }

                    if (Input.GetKey(KeyCode.Q))
                    {
                        RotateLeft();
                        HandleRotation();
                    }
                }

                //Handling free fall
                if (Time.time > stepTime)
                {
                    if (canMoveDown)
                    {
                        MoveVerticalFreeFall();
                    }
                }
            }
            LockPiece();
        }
        #endregion
    }
}

