using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YY_Games_Scripts
{
    public class CharacterSelector : MonoBehaviour
    {
        #region Character Key Handling
        public const string CharacterKey = "Character Key";

        private void SetKeyValueAtStart()
        {
            if (PlayerPrefs.HasKey(CharacterKey) == false)
            {
                PlayerPrefs.SetInt(CharacterKey, 0);
            }
            ChangeCharacterSprite();
        }
        private void SetNewKeyValue(int newKeyValue)
        {
            PlayerPrefs.SetInt(CharacterKey, newKeyValue);
        }
        #endregion
        #region Char Selection Functions
        [Header("Character Selector Variables")]
        [SerializeField] private List<Sprite> characterSprites;
        [SerializeField] private Image characterImage;
        private void ChangeCharacterSprite()
        {
            characterImage.sprite = characterSprites[PlayerPrefs.GetInt(CharacterKey)];
        }
        public void SelectNextCharacter()
        {
            int currentCharValue = PlayerPrefs.GetInt(CharacterKey);
            currentCharValue++;
            if(currentCharValue >= characterSprites.Count)
            {
                currentCharValue = 0;
            }
            SetNewKeyValue(currentCharValue);
            ChangeCharacterSprite();
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        public void SelectPreviousCharacter()
        {
            int currentCharValue = PlayerPrefs.GetInt(CharacterKey);
            currentCharValue--;
            if (currentCharValue < 0)
            {
                currentCharValue = characterSprites.Count -1;
            }
            SetNewKeyValue(currentCharValue);
            ChangeCharacterSprite();
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        #endregion
        #region Unity Functions
        void Start()
        {
            SetKeyValueAtStart();
        }
        #endregion
    }
}
