using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;


namespace AICharacter
{
    [System.Serializable]
    public class Character
    {
        public string Name;
        public int Age;
        public string Description;
        public string Gender;

        public List<FunctionDescription> functionDescriptions;

        private ChatHistory ChatHistory {get; set;}

        private int lenOfThinkingPhrases = 3;

        public Character() {}

        public void InitCharacter()
        {
            Debug.Log("Init Character: "+Name);
        }
    }

}
