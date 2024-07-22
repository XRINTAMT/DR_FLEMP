using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AICharacter
{
    public class ChatGPTConversationController : MonoBehaviour
    {
        private OpenAIApi openai;

        [SerializeField] private CharacterConversation conversation;

        private void Awake()
        {
            TextAsset apiKeyTextAsset = Resources.Load<TextAsset>("OpenAI_API_Key");
            if (apiKeyTextAsset == null)
            {
                Debug.LogError("OpenAI API Key file not found in Resources folder!");
                return;
            }
            string apiKey = apiKeyTextAsset.text.Trim();
            openai = new OpenAIApi(apiKey);
        }

        public async void SendReply()
        {
            Debug.Log("Send Reply!!");
            string userInput = conversation.GetUserInput();
            conversation.SetWitReactTemporarilyIgnore(true);
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = userInput
            };

            if (conversation.HasChatHistory()) 
            {
                newMessage.Content = conversation.GetCharacterContent() + "\n" + userInput; 
            }
            AddToChatHistory(newMessage);

            Debug.Log("Full chat history: " + conversation.GetFullPromt());

            // Step 1: send the conversation and available functions to GPT
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = conversation.GetChathistory(),
                Functions = conversation.GetCharacterFunctionDescriptions()
            });
            // Step 2: check if GPT wanted to call a function
            bool MessageHasFunctionCalling = (completionResponse.Choices[0].Message.FunctionCall != null);
            if (MessageHasFunctionCalling)
            {
                // Step 3: call the function
                Debug.Log("Message Has FunctionCalling!!");
                FunctionCall? functionCall = completionResponse.Choices[0].Message.FunctionCall;
                ChatMessage funcMsg = HandleFunctionCalling(functionCall);
                AddToChatHistory(funcMsg);
            }
            bool responseHasChoices = (completionResponse.Choices != null && completionResponse.Choices.Count > 0);
            if (responseHasChoices)
            {
                Debug.Log("responseHasChoices!!");
                if (!MessageHasFunctionCalling)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    
                    AddToChatHistory(message);
                    string m = JsonConvert.SerializeObject(GetChathistory());
                    Debug.Log("::::  message  :::::::");
                    Debug.Log(m);
                    Debug.Log(":::: _message_ :::::::");
                }
            }
        }

        private void AddToChatHistory(ChatMessage message) 
        {
            conversation.AddToChatHistory(message);
        }

        private List<ChatMessage> GetChathistory() 
        {
            return conversation.GetChathistory();
        }

        private ChatMessage HandleFunctionCalling(FunctionCall? functionCall)
        {
            string functionName = functionCall?.Name;
            string msgContent = "";
            Debug.Log("functionName :: " + functionName);
            if (functionName == "report_pain_level") {
                Debug.Log("Add pain level to message");
                msgContent = conversation.GetCharacterFunctionContent();
            }
            Debug.Log("=================================");
            return new ChatMessage()
            {
                Role = "function",
                Name = functionName,
                Content = msgContent
            };
        }

    }
}
