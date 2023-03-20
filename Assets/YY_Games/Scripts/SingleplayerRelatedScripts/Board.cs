using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class Board : MonoBehaviour
    {
        #region Variables and References
        [Header("Game Values")]
        [SerializeField] private BoxCollider2D border;
        [SerializeField] private static int rows = 20;
        [SerializeField] private static int columns = 10;
        [SerializeField] private int difficulty;
        [SerializeField] private int density;
        [SerializeField] private float speed;

        [Header("Board Grid Components")]
        [SerializeField] private GameObject gridCell;
        [SerializeField] private Transform gridStartPos;
        [SerializeField] private GameObject gridParent;
        [SerializeField] public GameObject[,] boardGrid = new GameObject[columns, rows];

        [Header("Block Variables")]
        [SerializeField] private List<GameObject> blocks = new List<GameObject>();
        [SerializeField] private int maxBlockCount;
        [SerializeField] private int blockDensity;
        [SerializeField] private int boxColorCount;

        [Header("Spawned Piece Variables")]
        [SerializeField] private Piece pieceToSpawn;
        [SerializeField] private GameObject spawnedPiece;
        [SerializeField] private Transform pieceSpawnPos;
        [SerializeField] private float pieceSpeed;

        #endregion
        #region Functions to set up the board at start

        private void SetUpGameSettings()
        {
            difficulty = PlayerPrefs.GetInt("gameDifficulty");
            density = PlayerPrefs.GetInt("densityOfBoard");
            speed = PlayerPrefs.GetFloat("gameSpeed");
            switch (density)
            {
                case 1:
                    blockDensity = 3;
                    maxBlockCount = 25;
                    break;
                case 2:
                    blockDensity = 4;
                    maxBlockCount = 50;
                    break;
                case 3:
                    blockDensity = 5;
                    maxBlockCount = 100;
                    break;
            }
            switch (difficulty)
            {
                //3 colours
                case 1:
                    boxColorCount = 3;
                    break;
                //4 colours
                case 2:
                    boxColorCount = 4;
                    break;
                //4 colours
                case 3:
                    boxColorCount = 5;
                    break;
            }
            switch (speed)
            {
                case 1:
                    pieceSpeed = 1;
                    break;
                case 2:
                    pieceSpeed = 0.5f;
                    break;
                case 3:
                    pieceSpeed = 0.25f;
                    break;
            }

        }
        private void SetUpBoardGrind()
        {
            //holding box counter
            int counter = 0;
            //holding random counter to avoid 4 same color in a colomn
            int randomCounter = 0;
            int temp = -1;
            for (int i = 0; i < columns; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    //Setting the position of grid
                    Vector2 tempPos = new Vector2(i, j);
                    GameObject grid = Instantiate(gridCell, (Vector2)gridStartPos.position + tempPos, Quaternion.identity, gridParent.transform);
                    grid.GetComponent<Grid>().positionOfGrid.Set(grid.transform.position.x, grid.transform.position.y, grid.transform.position.z);
                    grid.gameObject.name = "("+ i + "," + j +")";
                    boardGrid[i, j] = grid;

                    //for randomly fill
                    int randDensity = Random.Range(0, blockDensity);

                    //for picking random color
                    int randomColor = Random.Range(0, boxColorCount);
                    if(temp == randomColor)
                    {
                        randomCounter++;
                    }
                    else
                    {
                        randomCounter = 0;
                    }
                    if(randomCounter == 2)
                    {
                        randomColor++;
                        if (randomColor >= boxColorCount)
                        {
                            randomColor = 0;
                        }
                    }

                    //Setting the grid if it has a block or not
                    if(randDensity == 0 || randDensity == 1)
                    {
                        grid.GetComponent<Grid>().hasBlock = false;
                    }
                    else
                    {
                        if (counter < maxBlockCount && grid.GetComponent<Grid>().positionOfGrid.y <= 5)
                        {
                            grid.GetComponent<Grid>().hasBlock = true;
                            grid.GetComponent<Grid>().colorCode = randomColor;
                            temp = randomColor;
                            counter++;
                        }
                    }
                }
            }
            Debug.Log(counter);
        }
        private void FillUpBoardRandomly()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j< rows; j++)
                {
                    if (boardGrid[i,j].GetComponent<Grid>().hasBlock)
                    {
                        Instantiate(blocks[boardGrid[i,j].GetComponent<Grid>().colorCode],
                            boardGrid[i,j].GetComponent<Grid>().positionOfGrid, Quaternion.identity, boardGrid[i,j].gameObject.transform);
                    }
                }
            }
        }
        private void FixRowBoxColour()
        {
            for (int i = 0; i < columns - 3; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (boardGrid[i, j].GetComponent<Grid>().colorCode == boardGrid[i + 1, j].GetComponent<Grid>().colorCode && 
                        boardGrid[i+1, j].GetComponent<Grid>().colorCode == boardGrid[i + 2, j].GetComponent<Grid>().colorCode &&
                        boardGrid[i + 2, j].GetComponent<Grid>().colorCode == boardGrid[i + 3, j].GetComponent<Grid>().colorCode)
                    {
                        boardGrid[i + 2, j].GetComponent<Grid>().colorCode++;
                        if(boardGrid[i + 2, j].GetComponent<Grid>().colorCode >= boxColorCount)
                        {
                            boardGrid[i + 2, j].GetComponent<Grid>().colorCode = 0;
                        }
                    }
                }
            }
        }
        #endregion
        #region Functions to Spawn Playable Pieces
        private void SpawnPiece()
        {
            //Spawn the piece
            spawnedPiece = Instantiate(pieceToSpawn.gameObject, (Vector2) pieceSpawnPos.position, Quaternion.identity, gameObject.transform);

            //Set piece blocks randomly
            for(int i = 0; i < spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab.Length; i++)
            {
                int rand = Random.Range(0, boxColorCount);
                spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab[i] = blocks[rand];
            }

            //Set the piece fall down speed
            spawnedPiece.GetComponent<Piece>().stepDelay = pieceSpeed;
        }

        private void LockTheSpawnedPiece()
        {
            foreach(GameObject block in spawnedPiece.GetComponent<Piece>().blocksInPiece)
            {
                
            }
        }
        #endregion
        #region Unity Functions
        private void Awake()
        {
            SetUpGameSettings();
        }
        void Start()
        {
            SetUpBoardGrind();
            FixRowBoxColour();
            FillUpBoardRandomly();
            SpawnPiece();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion
    }
}

