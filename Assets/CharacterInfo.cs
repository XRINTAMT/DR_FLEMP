using UnityEngine;

namespace AICharacter
{
    public class CharacterInfo : MonoBehaviour
    {
        public string name;
        public string germanName;
        public string description;
        public string germanDescription;
        public RandomPool<AudioClip> ThinkingPhrasesPool;
        [SerializeField] private AudioClip[] thinkingPhrases;
        [SerializeField] private AudioClip[] thinkingPhrasesGerman;
        

        private void Awake()
        {
            if(string.IsNullOrEmpty(germanName))
            {
                germanName = name;
            }
            if(string.IsNullOrEmpty(germanDescription))
            {
                germanDescription = description;
            }
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
