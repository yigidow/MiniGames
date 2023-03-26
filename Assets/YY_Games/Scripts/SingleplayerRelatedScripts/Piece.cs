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
        public float lockDelay = 1f;
        private float stepTime;
        private float moveTime;
        private bool isPieceLocked = false;

        [Header("Rotation Variables")]
        public float rotateDelay = 0.1f;
        private float rotateTime;

        [Header("Box Checks")]
        public BoxCollider2D currentCollider;
        public BoxCollider2D horrizontalCollider;
        public BoxCollider2D verticalCollider;
        public bool canMoveDown = true;

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
            currentCollider = horrizontalCollider;
            verticalCollider.gameObject.SetActive(false);
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
                    currentCollider = horrizontalCollider;
                    horrizontalCollider.gameObject.SetActive(true);
                    verticalCollider.gameObject.SetActive(false);
                    break;
                case PiecePositions.pos1:
                    blocksInPiece[0].transform.position = blockPositions[0].position;
                    blocksInPiece[1].transform.position = blockPositions[2].position;
                    currentCollider = verticalCollider;
                    horrizontalCollider.gameObject.SetActive(false);
                    verticalCollider.gameObject.SetActive(true);
                    break;
                case PiecePositions.pos2:
                    blocksInPiece[0].transform.position = blockPositions[1].position;
                    blocksInPiece[1].transform.position = blockPositions[0].position;
                    currentCollider = horrizontalCollider;
                    horrizontalCollider.gameObject.SetActive(true);
                    verticalCollider.gameObject.SetActive(false);
                    break;
                case PiecePositions.pos3:
                    blocksInPiece[0].transform.position = blockPositions[2].position;
                    blocksInPiece[1].transform.position = blockPositions[0].position;
                    currentCollider = verticalCollider;
                    horrizontalCollider.gameObject.SetActive(false);
                    verticalCollider.gameObject.SetActive(true);
                    break;
            }
        }
        #endregion

        #region Functions to Interact with board
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
                //if (Time.time > stepTime)
                //{
                //    if (canMoveDown)
                //    {
                //        MoveVerticalFreeFall();
                //    }
                //}
            }
        }
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(currentCollider.bounds.center - new Vector3(0f, 1f, 0f), currentCollider.bounds.size);
        }
        #endregion
    }
}

