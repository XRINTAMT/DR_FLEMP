using System;
using System.Collections.Generic;
using System.Linq;
using OpenAI;
using UnityEngine;

namespace AICharacter
{

    
    public class ChatHistory
    {
        private struct HistoryEntry
        {
            internal ChatMessage message;
            internal ChatMessage response;
            internal List<float> embedding;
        }

        private List<HistoryEntry> history = new List<HistoryEntry>();

        public void NewEntry(ChatMessage message, ChatMessage response, List<float> embedding)
        {
            history.Add(new HistoryEntry
            {
                message = message,
                response = response,
                embedding = embedding
            });
        }

        public List<ChatMessage> GetHistory(int count)
        {
            for (int i = Math.Clamp(history.Count - count, 0, history.Count) ; i < history.Count; i++)
            {
                Debug.Log(history[i]);
            }

            return null;
        }
        
        public List<ChatMessage> GetDeepHistory(int cutoff, int count, List<float> embedding)
        {
            var temp = new List<HistoryEntry>();
            for (int i = 0 ; i < Math.Clamp(history.Count - cutoff, 0, history.Count); i++)
            {
                temp.Add(history[i]);
            }

            var searchResult = temp.OrderBy(e => -EmbeddingDB.vectorSimilarity(e.embedding, embedding)).ToList();
            
            var res = new List<ChatMessage>();
            for (int i = 0; i < count; ++i)
            {
                res.Add(searchResult[i].message);
                res.Add(searchResult[i].response);
            }

            return res;
        }
    }
}