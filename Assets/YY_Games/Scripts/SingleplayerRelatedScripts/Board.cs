using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class Board : MonoBehaviour
    {
        #region Variables and References
        [Header("Board Values")]
        [SerializeField] private BoxCollider2D border;
        [SerializeField] private static int rows = 20;
        [SerializeField] private static int columns = 10;
        [SerializeField] private int difficulty;
        [SerializeField] private int density;

        [Header("Board Grid Components")]
        [SerializeField] private GameObject gridCell;
        [SerializeField] private Transform gridStartPos;
        [SerializeField] private GameObject gridParent;
        [SerializeField] private GameObject[,] boardGrid = new GameObject[columns, rows];

        [Header("Block Variables")]
        [SerializeField] private List<GameObject> blocks = new List<GameObject>();
        [SerializeField] private int maxBlockCount;
        [SerializeField] private int blockDensity;
        [SerializeField] private int boxColorCount;

        [Header("Spawned Piece Variables")]
        [SerializeField] private Piece pieceToSpawn;
        [SerializeField] private Transform pieceSpawnPos;

        #endregion
        #region Functions to set up the board at start

        private void SetUpGameSettings()
        {
            difficulty = PlayerPrefs.GetInt("gameDifficulty");
            density = PlayerPrefs.GetInt("densityOfBoard");
            switch (density)
            {
                case 1:
                    blockDensity = 3;
                    maxBlockCount = 50;
                    break;
                case 2:
                    blockDensity = 4;
                    maxBlockCount = 75;
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
                        Debug.Log("Fixed" + i +""+ j);
                    }
                }
            }
        }

        private void SpawnPiece()
        {
            //Spawn the piece
            GameObject spawnedPiece = Instantiate(pieceToSpawn.gameObject, (Vector2) pieceSpawnPos.position, Quaternion.identity, gameObject.transform);

            //Set piece blocks randomly
            for(int i = 0; i < spawnedPiece.GetComponent<Piece>().blocksInPiece.Length; i++)
            {
                int rand = Random.Range(0, boxColorCount);
                spawnedPiece.GetComponent<Piece>().blocksInPiece[i] = blocks[rand];
            }
            Debug.Log(spawnedPiece.GetComponent<Piece>().blocksInPiece.Length);
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

