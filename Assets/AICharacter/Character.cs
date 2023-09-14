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


        private ChatHistory ChatHistory {get; set;}

        private int lenOfThinkingPhrases = 3;

        // !!! Change this into something more Dynamic.. !!!
        public string contentPain =  "{\"pain\":\"9\"}";
        public List<FunctionDescription> functionDescriptions = new List<FunctionDescription>
            {
                new FunctionDescription
                {
                    Name = "report_pain_level",
                    Description = "Patient's pain as a number between 1 and 10.",
                    Parameters = new Parameters
                    {
                        Type = "object",
                        Properties = new Dictionary<string, Property>
                        {
                            {
                                "Pain", new Property
                                {
                                    Type = "number",
                                    Description = "A number from one to ten"
                                }
                            },
                            {
                                "Reponse", new Property
                                {
                                    Type = "string",
                                    Description = "The in-character response. How the patient responds to the question."
                                }
                            }
                        },
                        Required = new List<string> { "pain", "reponse" }
                    }
                }
            };

        public Character() {}

        public void InitCharacter()
        {
            Debug.Log("Init Character :: "+Name);
        }
    }

}
