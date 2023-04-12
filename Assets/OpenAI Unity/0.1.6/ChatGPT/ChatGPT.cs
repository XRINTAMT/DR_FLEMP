using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        //[SerializeField] private InputField inputField;
        [SerializeField] private Text playerUtterance;
        [SerializeField] private Text textArea;
        [SerializeField] private Toggle pauseOnCommas;

        [SerializeField] private TTSSpeaker _speaker;
        [SerializeField] private string Instruction;

        [SerializeField] private WitAutoReactivation WitReact;

        [SerializeField] private string[] sentences;
        int instrLen;

        private int secret = 0;
        string[] errorReplies = { "Chat GPT is down (or we didn't pay for it) so have this:",
                    "We're no strangers to love",
                    "You know the rules and so do I (do I)",
                    "A full commitment's what I'm thinking of",
                    "You wouldn't get this from any other guy" };
        List<string> forbiddenPhrases = new List<string>
        {
            "Press activation to talk...",
            "Processing..."
        };

        private OpenAIApi openai = new OpenAIApi("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");

        private string userInput;
        //private string Instruction = "Act as a random stranger in a chat room and reply to the questions.\nQ: ";
        private string finalInstruction;

        void Start()
        {
            finalInstruction = $"{Instruction}\nQ: ";
            instrLen = finalInstruction.Length - 4;
        }

        public async void SendReply()
        {
            userInput = playerUtterance.text;
            if(forbiddenPhrases.Contains(userInput))
            {
                Debug.Log("Heard nothing / did not have enough time to process the speech");
                return;
            }

            WitReact.temporarilyIgnore = true;

            Debug.Log("userInput:: "+userInput);
            finalInstruction += $"{userInput}\nA: ";
            
            textArea.text = "...";
            playerUtterance.text = "";
            
            // Complete the instruction
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = finalInstruction,
                Model = "text-davinci-003",
                MaxTokens = 128
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                finalInstruction += $"{completionResponse.Choices[0].Text}\nQ: ";
                textArea.text = finalInstruction.Substring(instrLen);
                Debug.Log("Instruction :: "+finalInstruction);
                if(pauseOnCommas.isOn)
                    sentences = completionResponse.Choices[0].Text.Split(new char[] { ',', '\n', '.', '?', ';', '!' });
                else
                    sentences = completionResponse.Choices[0].Text.Split(new char[] { '\n', '.', '?', ';', '!' });
                StartCoroutine(PlayAndWait());
                //WitReact.temporarilyIgnore = false;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
                finalInstruction += $"{errorReplies[secret]}\nQ: ";
                textArea.text = finalInstruction.Substring(instrLen);
                _speaker.Speak(errorReplies[secret]);
                secret++;
                if (secret >= errorReplies.Length)
                    secret = 0;
                //WitReact.temporarilyIgnore = false;
            }
            //inputField.enabled = true;
        }
        IEnumerator PlayAndWait()
        {
            foreach (string sentence in sentences)
            {
                if (sentence == string.Empty)
                    continue;
                _speaker.Speak(sentence);
                
                while (!_speaker.IsSpeaking)
                {
                    yield return 0;
                }
                while (_speaker.IsSpeaking)
                {
                    yield return 0;
                }
                WitReact.temporarilyIgnore = true;
            }
            WitReact.temporarilyIgnore = false;
            Debug.Log("Giving control back to the stt");
        }
    }
}
