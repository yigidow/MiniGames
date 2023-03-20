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

        [Header("Movement Variables")]
        public float stepDelay = 5f;
        public float moveDelay = 0.1f;
        public float lockDelay = 1f;
        private float stepTime;
        private float moveTime;
        private bool isPieceLocked = false;

        [Header("Rotation Variables")]
        public float rotateDelay = 0.1f;
        private float rotateTime;
        public enum PiecePositions
        {
            pos0,
            pos1,
            pos2,
            pos3,
        }
        public PiecePositions currentPosition = PiecePositions.pos0;

        #endregion
        #region Functions To Initilaze the spawned piece
        private void Initialize()
        {
            for(int i = 0; i < blocksInPiecePrefab.Length; i++)
            {
                blocksInPiece[i] = Instantiate(blocksInPiecePrefab[i], (Vector3)blockPositions[i].position, Quaternion.identity, this.gameObject.transform);
            }
            gameBoard = FindObjectOfType<Board>();
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
                    transform.position += new Vector3(-1, 0, 0);
                }
                else if (transform.localPosition.x < 3.5f && Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(1, 0, 0);
                }
            }
            else
            {
                if (transform.localPosition.x > -4.5f && Input.GetKey(KeyCode.A))
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
                else if (transform.localPosition.x < 4.5f && Input.GetKey(KeyCode.D))
                {
                    transform.position += new Vector3(1, 0, 0);
                }
            }
        }
        private void MoveVerticalFreeFall()
        {
            stepTime = Time.time + stepDelay;
            transform.localPosition += new Vector3(0, -1, 0);
            if (transform.position.y == -10)
            {
                isPieceLocked = true;
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
                    isPieceLocked = true;
                }
            }
            else
            {
                if (transform.position.y == -10)
                {
                    isPieceLocked = true;
                }
            }
        }
        private void RotateRight()
        {
            rotateTime = Time.time + rotateDelay;

            if (currentPosition == PiecePositions.pos0)
            {
                currentPosition = PiecePositions.pos1;
            }
            else if (currentPosition == PiecePositions.pos1)
            {
                currentPosition = PiecePositions.pos2;

                //Wall kick
                if (transform.localPosition.x == 4.5)
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos2)
            {
                currentPosition = PiecePositions.pos3;
            }
            else if (currentPosition == PiecePositions.pos3)
            {
                currentPosition = PiecePositions.pos0;

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
                currentPosition = PiecePositions.pos3;
            }
            else if (currentPosition == PiecePositions.pos1)
            {
                currentPosition = PiecePositions.pos0;

                //Wall kick
                if (transform.localPosition.x == 4.5) 
                {
                    transform.localPosition += new Vector3(-1, 0, 0);
                }
            }
            else if (currentPosition == PiecePositions.pos2)
            {
                currentPosition = PiecePositions.pos1;
            }
            else if (currentPosition == PiecePositions.pos3)
            {
                currentPosition = PiecePositions.pos2;

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

        public bool CanMoveThere()
        {
            if(currentPosition == PiecePositions.pos0 || currentPosition == PiecePositions.pos2)
            {
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if ((blockPositions[0].transform.position.x - 1) == gameBoard.boardGrid[i, j].transform.position.x &&
                            gameBoard.boardGrid[i, j].GetComponent<Grid>().hasBlock)
                        {
                            return false;
                            Debug.Log("nope");
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            //else if(currentPosition == PiecePositions.pos1 || currentPosition == PiecePositions.pos3)
            //{

            //}
            return true;
        }
        public IEnumerator LockPiece()
        {
            yield return new WaitForSeconds(lockDelay);
            isPieceLocked = true;
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
                    MoveVertical();
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
                    MoveVerticalFreeFall();
                }
            }
        }
        #endregion
    }
}

