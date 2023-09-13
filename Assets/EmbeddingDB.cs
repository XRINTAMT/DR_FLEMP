using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AICharacter;
using Unity.Mathematics;
using NaughtyAttributes;
using UnityEngine;
using OpenAI;
using UnityEditor.Search;

public class EmbeddingDB : MonoBehaviour
{
    [SerializeField] private string prompt;
    [SerializeField] private string key;
    [SerializeField] private int chunkLength;
    [SerializeField] private Dictionary<string, List<float>> _embeddings;
    public string searchtext;
    public int returnTop;
    public List<string> pwins;



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

    public static double dot(List<float> a, List<float> b)
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
    
    public static double vectorSimilarity(List<float> a, List<float> b)
    {
        return Math.Abs(dot(a, b));
    }

    [Button("Generate DB")]
    void generateDB()
    {
        var wins = window(prompt, chunkLength);
        pwins = wins;
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

    [Button("Search")]
    void searchEditor()
    {
        search(searchtext, returnTop);
    }
    
    void search(string text, int topN)
    {
        if(_embeddings == null) return;
        if (openai == null) openai = new OpenAIApi(key);
        Debug.Log("Searching for: \""+text+"\"");
        openai.CreateEmbeddings(new CreateEmbeddingsRequest()
        {
            Input = text,
            Model = model
        }).ContinueWith(r =>
        {
            var e = r.Result.Data[0].Embedding;
            var searchResult = _embeddings.OrderBy(kv => -vectorSimilarity(kv.Value, e)).GetEnumerator();
            searchResult.MoveNext();
            for (int i = 0; i < topN; ++i)
            {
                Debug.Log(searchResult.Current.Key);
                searchResult.MoveNext();
            }
        });
    }
}
