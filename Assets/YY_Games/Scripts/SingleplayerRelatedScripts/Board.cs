using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class Board : MonoBehaviour
    {
        #region Variables and References
        [Header("Game Values")]
        [SerializeField] private static int rows = 20;
        [SerializeField] private static int columns = 10;
        private int difficulty;
        private int density;
        private float speed;

        [Header("Board Grid Components")]
        [SerializeField] private GameObject gridCell;
        [SerializeField] private Transform gridStartPos;
        [SerializeField] private GameObject gridParent;
        public GameObject[,] boardGrid = new GameObject[columns, rows];

        [Header("Block Variables")]
        [SerializeField] private List<GameObject> blocks = new List<GameObject>();
        private int maxBlockCount;
        private int blockDensity;
        private int boxColorCount;

        [Header("Spawned Piece Variables")]
        [SerializeField] private Piece pieceToSpawn;
        [SerializeField] private Transform pieceSpawnPos;
        [SerializeField] private GameObject spawnedPiece;
        private Vector2 spawnedPiecePos;
        private float pieceSpeed;

        [Header("Next and Holded Pieces")]
        [SerializeField] private GameObject nextPiece;
        [SerializeField] private GameObject holdedPiece;
        private bool isThereAPieceHolded = false;
        public bool canHoldPiece = true;

        [Header("Lists to hold match blocks")]
        private List<GameObject> matchColumns = new List<GameObject>();
        private List<GameObject> matchRows = new List<GameObject>();
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
                    blockDensity = 4;
                    maxBlockCount = 25;
                    break;
                case 2:
                    blockDensity = 5;
                    maxBlockCount = 35;
                    break;
                case 3:
                    blockDensity = 6;
                    maxBlockCount = 50;
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
                    grid.GetComponent<Grid>().positionOfGrid.Set(grid.transform.position.x, grid.transform.position.y);
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
                    if(randDensity == 0 || randDensity == 1 || randDensity == 2)
                    {
                        grid.GetComponent<Grid>().hasBlock = false;
                    }
                    else
                    {
                        if (counter < maxBlockCount && grid.GetComponent<Grid>().positionOfGrid.y <= 4)
                        {
                            grid.GetComponent<Grid>().hasBlock = true;
                            grid.GetComponent<Grid>().colorCode = randomColor;
                            temp = randomColor;
                            counter++;
                        }
                    }
                }
            }
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
        #region Functions to Spawn Playable Pieces and Control It
        private void SpawnPieceAtStart()
        {
            if(spawnedPiece == null)
            {
                canHoldPiece = true;
                //Spawn the piece
                spawnedPiece = Instantiate(pieceToSpawn.gameObject, (Vector2)pieceSpawnPos.position, Quaternion.identity, gameObject.transform);

                //Set piece blocks randomly
                for (int i = 0; i < spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab.Length; i++)
                {
                    int rand = Random.Range(0, boxColorCount);
                    spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab[i] = blocks[rand];
                }

                //Set the piece fall down speed
                spawnedPiece.GetComponent<Piece>().stepDelay = pieceSpeed;
            }
        }
        private void CheckPiecePos()
        {
            int x = 0;
            if(spawnedPiece != null)
            {
                spawnedPiecePos = spawnedPiece.transform.position;
                if(spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos0 || spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos2)
                {
                    x = 1;
                }
                else
                {
                    x = 0;
                }
                for (int i = 0; i < columns - x; i++) 
                {
                    for (int j = 0; j < rows - 1; j++)
                    {
                        //Horrizontal Pos
                        if (spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos0 || spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos2)
                        {
                            //Check Down 
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y - 1 && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x
                                || boardGrid[i + 1, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y - 1 && boardGrid[i + 1, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock || boardGrid[i + 1, j].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveDown = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveDown = true;
                                }
                            }
                            //Check left
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x - 1)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveLeft = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveLeft = true;
                                }
                            }
                            //Check Right 
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x + 2)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveRight = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveRight = true;
                                }
                            }
                            //Check Rotation
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y - 1 && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canRotateToVertical = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canRotateToVertical = true;
                                }
                            }
                        }
                        //Vertical Pos
                        else if (spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos1 || spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos3)
                        {
                            //Check Down 
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y - 2 && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveDown = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveDown = true;
                                }
                            }
                            //Check left
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x - 1)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock || boardGrid[i, j - 1].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveLeft = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveLeft = true;
                                }
                            }
                            //Check Right
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x + 1)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock || boardGrid[i, j - 1].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveRight = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canMoveRight = true;
                                }
                            }
                            //Check Rotation
                            if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid.y == spawnedPiecePos.y && boardGrid[i, j].GetComponent<Grid>().positionOfGrid.x == spawnedPiecePos.x + 1)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().hasBlock)
                                {
                                    spawnedPiece.GetComponent<Piece>().canRotateToHorizontal = false;
                                }
                                else
                                {
                                    spawnedPiece.GetComponent<Piece>().canRotateToHorizontal = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void LockPieceToBoard()
        {
            if(spawnedPiece != null)
            {
                if (spawnedPiece.GetComponent<Piece>().isPieceLocked)
                {
                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            //Horrizontal
                            if (spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos0)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid == spawnedPiecePos)
                                {
                                    for (int k = 0; k < boxColorCount; k++)
                                    {
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[0].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i, j].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i, j].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[0].transform.SetParent(boardGrid[i, j].transform);
                                        }
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[1].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i + 1, j].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i + 1, j].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[1].transform.SetParent(boardGrid[i+1, j].transform);
                                        }
                                    }
                                }
                            }
                            else if (spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos2)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid == spawnedPiecePos)
                                {
                                    for (int k = 0; k < boxColorCount; k++)
                                    {
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[1].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i, j].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i, j].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[1].transform.SetParent(boardGrid[i, j].transform);
                                        }
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[0].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i + 1, j].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i + 1, j].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[0].transform.SetParent(boardGrid[i+1, j].transform);
                                        }
                                    }
                                }
                            }
                            //Vertical
                            else if (spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos1)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid == spawnedPiecePos)
                                {
                                    for (int k = 0; k < boxColorCount; k++)
                                    {
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[0].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i, j].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i, j].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[0].transform.SetParent(boardGrid[i, j].transform);
                                        }
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[1].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i, j - 1].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i, j - 1].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[1].transform.SetParent(boardGrid[i, j-1].transform);
                                        }
                                    }
                                }
                            }
                            else if (spawnedPiece.GetComponent<Piece>().currentPosition == Piece.PiecePositions.pos3)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().positionOfGrid == spawnedPiecePos)
                                {
                                    for (int k = 0; k < boxColorCount; k++)
                                    {
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[1].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i, j].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i, j].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[1].transform.SetParent(boardGrid[i, j].transform);
                                        }
                                        if (spawnedPiece.GetComponent<Piece>().blocksInPiece[0].GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                                        {
                                            boardGrid[i, j - 1].GetComponent<Grid>().hasBlock = true;
                                            boardGrid[i, j - 1].GetComponent<Grid>().colorCode = k;
                                            spawnedPiece.GetComponent<Piece>().blocksInPiece[0].transform.SetParent(boardGrid[i, j-1].transform);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Destroy(spawnedPiece.gameObject);
                    spawnedPiece = null;
                }
            }
            else
            {
                FindMatchInColumns();
                FindMatchInRows();
                //StartCoroutine(LetPieceBlocksFell());
                StartCoroutine(SpawnPiece());
                StartCoroutine(ShowNextPiece());
            }
        }
        private IEnumerator SpawnPiece()
        {
            yield return new WaitForSeconds(0.5f);
            canHoldPiece = true;
            if (spawnedPiece == null)
            {
                //Spawn the piece
                spawnedPiece = Instantiate(pieceToSpawn.gameObject, (Vector2)pieceSpawnPos.position, Quaternion.identity, gameObject.transform);

                //Set piece according to next Piece
                for (int i = 0; i < spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab.Length; i++)
                {
                    int rand = Random.Range(0, boxColorCount);
                    spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab[i] = nextPiece.GetComponent<NextPiece>().blocksInNextPiece[i];
                }

                //Set the piece fall down speed
                spawnedPiece.GetComponent<Piece>().stepDelay = pieceSpeed;
            }
        }
        #endregion
        #region Functions to Shown next piece comming and holding a piece
        private IEnumerator ShowNextPiece()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < nextPiece.GetComponent<NextPiece>().blocksInNextPiece.Length; i++)
            {
                int rand = Random.Range(0, boxColorCount);
                nextPiece.GetComponent<NextPiece>().blocksInNextPiece[i] = blocks[rand];
            }
            nextPiece.GetComponent<NextPiece>().Init();
        }
        private void HoldSpawnedPiece()
        {
            if (spawnedPiece != null)
            {
                holdedPiece.SetActive(true);
                for (int i = 0; i < spawnedPiece.GetComponent<Piece>().blocksInPiece.Length; i++)
                {
                    holdedPiece.GetComponent<HoldedPiece>().blocksInHoldedPiece[i] = spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab[i];
                }
                holdedPiece.GetComponent<HoldedPiece>().Init();
                Destroy(spawnedPiece);
                isThereAPieceHolded = true;
            }
        }
        private void GetHoldedPiece()
        {
            if(spawnedPiece != null)
            {
                for (int i = 0; i < spawnedPiece.GetComponent<Piece>().blocksInPiece.Length; i++)
                {
                    spawnedPiece.GetComponent<Piece>().blocksInPiecePrefab[i] = holdedPiece.GetComponent<HoldedPiece>().blocksInHoldedPiece[i];
                    Destroy(spawnedPiece.GetComponent<Piece>().blocksInPiece[i]);
                }
                spawnedPiece.GetComponent<Piece>().Initialize();
                holdedPiece.SetActive(false);
                isThereAPieceHolded = false;
            }
        }
        #endregion
        #region Functions to Find Same 4
        public void FindMatchInColumns()
        {
            int countColumns = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows - 1; j++)
                {
                    for (int k = 1; k < rows; k++)
                    {
                        if ((j + 1) == k)
                        {
                            if (boardGrid[i, j].GetComponent<Grid>().hasBlock && boardGrid[i, k].GetComponent<Grid>().hasBlock)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().colorCode == boardGrid[i, k].GetComponent<Grid>().colorCode)
                                { 
                                    if (!matchColumns.Contains(boardGrid[i, j].transform.GetChild(0).gameObject))
                                    {
                                        matchColumns.Add(boardGrid[i, j].transform.GetChild(0).gameObject);
                                        countColumns++;
                                    }
                                    if (!matchColumns.Contains(boardGrid[i, k].transform.GetChild(0).gameObject))
                                    {
                                        matchColumns.Add(boardGrid[i, k].transform.GetChild(0).gameObject);
                                        countColumns++;
                                    }
                                }
                                else
                                {
                                    if(countColumns >= 4)
                                    {
                                        foreach (GameObject blck in matchColumns)
                                        {
                                            blck.transform.GetComponentInParent<Grid>().hasBlock = false;
                                            blck.transform.GetComponentInParent<Grid>().colorCode = -1;
                                            Destroy(blck);
                                            countColumns = 0;
                                            matchColumns = new List<GameObject>();
                                        }
                                    }
                                    else
                                    {
                                        countColumns = 0;
                                        matchColumns = new List<GameObject>();
                                    }
                                }
                            }
                            else
                            {
                                if (countColumns >= 4)
                                {
                                    foreach (GameObject blck in matchColumns)
                                    {
                                        blck.transform.GetComponentInParent<Grid>().hasBlock = false;
                                        blck.transform.GetComponentInParent<Grid>().colorCode = -1;
                                        Destroy(blck);
                                        countColumns = 0;
                                        matchColumns = new List<GameObject>();
                                    }
                                }
                                else
                                {
                                    countColumns = 0;
                                    matchColumns = new List<GameObject>();
                                } 
                            }
                        }
                    }
                }
            }
        }
        private void FindMatchInRows()
        {
            int countRows = 0;
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns-1; i++)
                {
                    for (int k = 1; k < columns; k++)
                    {
                        if ((i+ 1) == k)
                        {
                            if (boardGrid[i, j].GetComponent<Grid>().hasBlock && boardGrid[k, j].GetComponent<Grid>().hasBlock)
                            {
                                if (boardGrid[i, j].GetComponent<Grid>().colorCode == boardGrid[k, j].GetComponent<Grid>().colorCode)
                                {
                                    if (!matchRows.Contains(boardGrid[i, j].transform.GetChild(0).gameObject))
                                    {
                                        matchRows.Add(boardGrid[i, j].transform.GetChild(0).gameObject);
                                        countRows++;
                                    }
                                    if (!matchRows.Contains(boardGrid[k, j].transform.GetChild(0).gameObject))
                                    {
                                        matchRows.Add(boardGrid[k, j].transform.GetChild(0).gameObject);
                                        countRows++;
                                    }
                                }
                                else
                                {
                                    if (countRows >= 4)
                                    {
                                        foreach (GameObject blck in matchRows)
                                        {
                                            blck.transform.GetComponentInParent<Grid>().hasBlock = false;
                                            blck.transform.GetComponentInParent<Grid>().colorCode = -1;
                                            Destroy(blck);
                                            countRows = 0;
                                            matchRows = new List<GameObject>();
                                        }
                                    }
                                    else
                                    {
                                        countRows = 0;
                                        matchRows = new List<GameObject>();
                                    }
                                }
                            }
                            else
                            {
                                if (countRows >= 4)
                                {
                                    foreach (GameObject blck in matchRows)
                                    {
                                        blck.transform.GetComponentInParent<Grid>().hasBlock = false;
                                        blck.transform.GetComponentInParent<Grid>().colorCode = -1;
                                        Destroy(blck);
                                        countRows = 0;
                                        matchRows = new List<GameObject>();
                                    }
                                }
                                else
                                {
                                    countRows = 0;
                                    matchRows = new List<GameObject>();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region Functions after finding matches
        public void LetPieceBlocksFell()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (boardGrid[i, j].GetComponent<Grid>().hasBlock)
                    {
                        if (boardGrid[i, j].transform.GetChild(0).gameObject.tag == ("PieceBlock"))
                        {
                            var temp = boardGrid[i, j].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                            temp.a = 0.5f;
                            boardGrid[i, j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = temp;

                            if(i - 1 >= 0 && i + 1 < columns)
                            {
                                if (!boardGrid[i - 1, j].GetComponent<Grid>().hasBlock && !boardGrid[i + 1, j].GetComponent<Grid>().hasBlock
                                 && !boardGrid[i, j - 1].GetComponent<Grid>().hasBlock && !boardGrid[i, j + 1].GetComponent<Grid>().hasBlock)
                                {
                                    Debug.Log("Alone");
                                }
                            }else if(i - 1 < 0)
                            {
                                if (!boardGrid[i + 1, j].GetComponent<Grid>().hasBlock
                                && !boardGrid[i, j - 1].GetComponent<Grid>().hasBlock && !boardGrid[i, j + 1].GetComponent<Grid>().hasBlock)
                                {
                                    Debug.Log("Alone");
                                }
                            }else if (i + 1 >= columns) 
                            {
                                if (!boardGrid[i - 1, j].GetComponent<Grid>().hasBlock
                                && !boardGrid[i, j - 1].GetComponent<Grid>().hasBlock && !boardGrid[i, j + 1].GetComponent<Grid>().hasBlock)
                                {
                                    Debug.Log("Alone");
                                }
                            }
                            //int k = 0;
                            //if (k <= j)
                            //{
                            //    if (!boardGrid[i, j - k-1].GetComponent<Grid>().hasBlock)
                            //    {
                            //        boardGrid[i, j - k].transform.GetChild(0).gameObject.transform.SetParent(boardGrid[i, j - k - 1].GetComponent<Grid>().transform);
                            //        UpdateGrindCellInfos();
                            //        boardGrid[i, j - k - 1].transform.GetChild(0).gameObject.transform.localPosition = new Vector2(0, 0);
                            //        k++;
                            //    }
                            //}
                        }
                    }
                }
            }
        }
        public void UpdateGrindCellInfos()
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (boardGrid[i, j].GetComponent<Grid>().transform.childCount > 0)
                    {
                        boardGrid[i, j].GetComponent<Grid>().hasBlock = true;
                        for (int k = 0; k < boxColorCount; k++)
                        {
                            if (boardGrid[i, j].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == blocks[k].GetComponent<SpriteRenderer>().sprite)
                            {
                                boardGrid[i, j].GetComponent<Grid>().colorCode = k;
                            }
                        }
                    }
                    else
                    {
                        boardGrid[i, j].GetComponent<Grid>().hasBlock = false;
                        boardGrid[i, j].GetComponent<Grid>().colorCode = -1;
                    }
                }
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
            SpawnPieceAtStart();

            StartCoroutine(ShowNextPiece());
        }

        // Update is called once per frame
        void Update()
        {
            CheckPiecePos();
            LockPieceToBoard();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!isThereAPieceHolded && canHoldPiece)
                {
                    HoldSpawnedPiece();
                }
                else
                {
                    GetHoldedPiece();
                    canHoldPiece = false;
                }
            }
        }
        #endregion
    }
}

