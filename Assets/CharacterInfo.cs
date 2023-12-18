using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacter
{
    public class CharacterInfo : MonoBehaviour
    {
        public string name;
        public string description;
        public RandomPool<AudioClip> ThinkingPhrasesPool;
        [SerializeField] private AudioClip[] thinkingPhrases;
        [SerializeField] private AudioClip[] thinkingPhrasesGerman;

        private void Awake()
        {
            if(PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0)
            {
                ThinkingPhrasesPool = new RandomPool<AudioClip>(thinkingPhrases);
            }
            else
            {
                ThinkingPhrasesPool = new RandomPool<AudioClip>(thinkingPhrasesGerman);
            }
        }
    }
}
