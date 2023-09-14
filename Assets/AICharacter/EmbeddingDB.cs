using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaughtyAttributes;
using OpenAI;
using UnityEngine;

namespace AICharacter
{
    public class EmbeddingDB : MonoBehaviour
    {
        [SerializeField] private string prompt;
        [SerializeField] private string key;
        [SerializeField] private int chunkLength;
        [SerializeField] private Dictionary<string, List<float>> _embeddings;
        public int returnTop;



        private String model = "text-embedding-ada-002";
        private OpenAIApi openai;
        void Start()
        {
            openai = new OpenAIApi(key);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public static List<string> window(string s, int winsize)
        {
            var res = new List<string>();
            int halfWin = winsize/2;
            for (var remaining = s.Length; remaining > 0; remaining -= halfWin)
            {
                if (remaining > winsize)
                {
                    res.Add(s.Substring(s.Length - remaining, winsize));
                }
                else
                {
                    res.Add(s.Substring(s.Length - remaining, remaining));
                }
                
            }
            return res;
        }

        public static double Dot(List<float> a, List<float> b)
        {
            if (a.Count != b.Count)
            {
                Debug.Log("Dot product can only be calculated for vector of the same length");
                return float.NaN;
            }
            double sum = 0;
            for (var i = 0; i < a.Count; ++i)
            {
                sum += a[i] * b[i];
            }
            return sum;
        }
    
        public static double VectorSimilarity(List<float> a, List<float> b)
        {
            return Math.Abs(Dot(a, b));
        }

        [Button("Generate DB")]
        void GenerateDB()
        {
            var wins = window(prompt, chunkLength);
            if (_embeddings == null) _embeddings = new Dictionary<string, List<float>>();
            if (openai == null) openai = new OpenAIApi(key);
            _embeddings.Clear();
            int rescount = 0;
            Debug.Log(wins.Count +" embeddings creations scheduled");
            foreach (string s in wins)
            {
                openai.CreateEmbeddings(new CreateEmbeddingsRequest()
                {
                    Input = s,
                    Model = model
                }).ContinueWith(r =>
                {
                    _embeddings.Add(s, r.Result.Data[0].Embedding);
                    Debug.Log("Window "+(rescount++)+": Embeddings creation done");
                });
            }
        

        }

        public Task<List<float>> GetEmbedding(string message)
        {
            if (openai == null) openai = new OpenAIApi(key);

            return openai.CreateEmbeddings(new CreateEmbeddingsRequest()
            {
                Input = message,
                Model = model
            }).ContinueWith(r => r.Result.Data[0].Embedding);
        }
    
        List<string> Search(List<float> embedding, int topN)
        {
            if (_embeddings == null) return null;

            var searchResult = _embeddings.OrderBy(kv => -VectorSimilarity(kv.Value, embedding)).ToList();
            var res = new List<string>();
            for (int i = 0; i < topN; ++i)
            {
                res.Add(searchResult[i].Key);
            }

            return res;
        }
    }
}
