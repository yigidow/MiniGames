using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class Piece : MonoBehaviour
    {
        [Header("Piece Block Components")]
        public GameObject[] blocksInPiece;
        public Transform[] blockPositions;

        [Header("Movement Variables")]
        public float stepDelay = 1f;
        public float moveDelay = 0.1f;
        public float lockDelay = 0.5f;
        private float stepTime;
        private float moveTime;
        private float lockTime;

        public Board board { get; private set; }
        public Vector3Int position { get; private set; }


        private void Initialize()
        {
            for(int i = 0; i < blocksInPiece.Length; i++)
            {
                Instantiate(blocksInPiece[i], (Vector3)blockPositions[i].position, Quaternion.identity, this.gameObject.transform);
            }
        }

        private void Move()
        {
            moveTime = Time.time + moveDelay;

            if (transform.position.x > -4.5f && Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else if (transform.position.x < 4.5f && Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        private void Rotate()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(0, 0, -90);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(0, 0, 90);
            }
        }
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > moveTime)
            {
               Move();
            }

            Rotate();
            //Move();
        }
    }
}

