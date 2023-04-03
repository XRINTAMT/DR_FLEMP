using UnityEngine;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        //[SerializeField] private InputField inputField;
        [SerializeField] private Text playerUtterance;
        [SerializeField] private Text textArea;

        [SerializeField] private TTSSpeaker _speaker;
        [SerializeField] private string Instruction;

        private int secret = 0;
        string[] errorReplies = { "ChatGPT is down (or we didn't pay for it) so have this:",
                    "We're no strangers to love",
                    "You know the rules and so do I (do I)",
                    "A full commitment's what I'm thinking of",
                    "You wouldn't get this from any other guy" };

        private OpenAIApi openai = new OpenAIApi("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");

        private string userInput;
        //private string Instruction = "Act as a random stranger in a chat room and reply to the questions.\nQ: ";
        private string finalInstruction;

        void Start()
        {
            finalInstruction = $"{Instruction}\nQ:";
        }

        public async void SendReply()
        {
            userInput = playerUtterance.text;
            if(userInput == "Press activation to talk...")
            {
                Debug.Log("Heard nothing / did not have enough time to process the speech");
                return;
            }
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
                textArea.text = finalInstruction;
                Debug.Log("Instruction :: "+finalInstruction);
                _speaker.Speak(completionResponse.Choices[0].Text);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
                finalInstruction += $"{errorReplies[secret]}\nQ: ";
                textArea.text = finalInstruction;
                Debug.Log("Instruction :: " + finalInstruction);
                _speaker.Speak(errorReplies[secret]);
                secret++;
                if (secret >= errorReplies.Length)
                    secret = 0;
            }
            //inputField.enabled = true;
        }
    }
}
