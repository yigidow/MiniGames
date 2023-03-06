using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace YY_Games_Scripts
{
   public class LeaderBoardPrefabForScroll : MonoBehaviour
    {
        #region Referances
        [Header("UI Elements")]

        [SerializeField] private TextMeshProUGUI userNameTextRef;
        [SerializeField] private TextMeshProUGUI userRankTextRef;
        [SerializeField] private TextMeshProUGUI userScoreRef;
        #endregion

        #region LeaderBoard Functions
        public void SetValuesForLeaderBoard(string userName, string userRank, string userScore)
        {
            userNameTextRef.text = userName;
            userRankTextRef.text = userRank;
            userScoreRef.text = userScore;
        }
        #endregion

    }
}

