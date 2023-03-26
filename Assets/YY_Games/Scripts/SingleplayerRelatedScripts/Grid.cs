using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YY_Games_Scripts
{
    public class Grid : MonoBehaviour
    {
        public bool hasBlock;
        public Vector3 positionOfGrid;
        public int colorCode = -1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Piece")
            {
                if (hasBlock)
                {
                    collision.gameObject.GetComponentInParent<Piece>().canMoveDown = false;
                    Debug.Log("Hey");
                    return;
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "PieceB")
            {
                collision.gameObject.GetComponentInParent<Piece>().canMoveDown = true;
                return;
            }
        }
    }
}


