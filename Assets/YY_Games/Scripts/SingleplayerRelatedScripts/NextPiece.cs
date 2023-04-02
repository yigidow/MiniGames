using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class NextPiece : MonoBehaviour
    {
        public GameObject[] blocksInNextPiece;
        public GameObject[] blocksInNextPieceShown;
        public void Init()
        {
            for (int i = 0; i < blocksInNextPiece.Length; i++)
            {
                blocksInNextPieceShown[i].GetComponent<SpriteRenderer>().sprite = blocksInNextPiece[i].GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
