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
        [SerializeField] private int rows = 20;
        [SerializeField] private int columns = 10;
        [SerializeField] private int difficulty;
        [SerializeField] private int density;

        [Header("Board Grid Components")]
        [SerializeField] private GameObject gridCell;
        [SerializeField] private Transform gridStartPos;
        [SerializeField] private GameObject gridParent;
        [SerializeField] private List<GameObject> boardGrid = new List<GameObject>();

        [Header("Block Variables")]
        [SerializeField] private List<GameObject> blocks = new List<GameObject>();
        [SerializeField] private int blockCount = 25;
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
                    blockCount = 50;
                    break;
                case 2:
                    blockCount = 75;
                    break;
                case 3:
                    blockCount = 100;
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
                    //for randomly fill
                    int rand = Random.Range(0, 3);

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

                    //Setting the position of grid
                    Vector2 tempPos = new Vector2(i, j);
                    GameObject grid = Instantiate(gridCell, (Vector2)gridStartPos.position + tempPos, Quaternion.identity, gridParent.transform);

                    grid.GetComponent<Grid>().positionOfGrid.Set(grid.transform.position.x, grid.transform.position.y, grid.transform.position.z);
                    grid.gameObject.name = "("+ i + "," + j +")";
                    boardGrid.Add(grid);

                    //Setting the grid if it has a block or not
                    if(rand == 0)
                    {
                        grid.GetComponent<Grid>().hasBlock = false;
                    }
                    else if(rand == 1 || rand == 2)
                    {
                        if (counter < blockCount && grid.GetComponent<Grid>().positionOfGrid.y <= 5)
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
            for (int i = 0; i < boardGrid.Count; i++)
            {
                if (boardGrid[i].GetComponent<Grid>().hasBlock)
                {
                    Instantiate(blocks[boardGrid[i].GetComponent<Grid>().colorCode], 
                        boardGrid[i].GetComponent<Grid>().positionOfGrid, Quaternion.identity, boardGrid[i].gameObject.transform);
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

