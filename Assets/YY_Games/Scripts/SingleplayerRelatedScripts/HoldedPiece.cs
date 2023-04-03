using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class HoldedPiece : MonoBehaviour
    {
        public GameObject[] blocksInHoldedPiece;
        public GameObject[] blocksInHoldedPieceShown;
        public void Init()
        {
            for (int i = 0; i < blocksInHoldedPiece.Length; i++)
            {
                blocksInHoldedPieceShown[i].GetComponent<SpriteRenderer>().sprite = blocksInHoldedPiece[i].GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
