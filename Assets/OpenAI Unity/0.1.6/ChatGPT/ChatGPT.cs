using UnityEngine;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        //[SerializeField] private InputField inputField;
        [SerializeField] private Text playerUtterance;
        [SerializeField] private Button button;
        [SerializeField] private Text textArea;

        [SerializeField] private TTSSpeaker _speaker;
        

        private OpenAIApi openai = new OpenAIApi("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");

        private string userInput;
        //private string Instruction = "Act as a random stranger in a chat room and reply to the questions.\nQ: ";
        private string Instruction = "Act as an angry patient in a hospital and reply to the questions.\nQ:";

        private void Start()
        {
            button.onClick.AddListener(SendReply);
        }

        private async void SendReply()
        {
            userInput = playerUtterance.text;
            Debug.Log("userInput:: "+userInput);
            Instruction += $"{userInput}\nA: ";
            
            textArea.text = "...";
            //inputField.text = "";
            playerUtterance.text = "";

            button.enabled = false;
            //inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = Instruction,
                Model = "text-davinci-003",
                MaxTokens = 128
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                Instruction += $"{completionResponse.Choices[0].Text}\nQ: ";
                textArea.text = Instruction;
                Debug.Log("Instruction :: "+Instruction);
                _speaker.Speak(completionResponse.Choices[0].Text);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            //inputField.enabled = true;
        }
    }
}
