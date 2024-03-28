using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] latinSpawnPositions;
    [SerializeField] private Transform[] translationSpawnPositions;

    [SerializeField] private GameObject latinBubblePrefab;
    [SerializeField] private GameObject translationBubblePrefab;

    [SerializeField] private GameObject VFXCorrect;
    [SerializeField] private GameObject VFXWrong;

    [SerializeField] private string scenarioName;

    private Dictionary<int, WordPair> wordPairs = new Dictionary<int, WordPair>();
    private Dictionary<int, WordPair> allWordPairs = new Dictionary<int, WordPair>();

    private List<BubbleWord> spawnedBubbles = new List<BubbleWord>();

    [SerializeField] UnityEvent OnScenarioCompleted;

    public void OnBubbleMatch(BubbleWord latinBubble, BubbleWord translationBubble)
    {
        if (translationBubble.latin)
            return;
        GameObject vfx;
        if (latinBubble.id == translationBubble.id)
        {
            vfx = Instantiate(VFXCorrect);
            vfx.transform.position = (latinBubble.transform.position + translationBubble.transform.position) / 2;
            RemoveBubbles(latinBubble, translationBubble);
        }
        else
        {
            vfx = Instantiate(VFXWrong);
            vfx.transform.position = (latinBubble.transform.position + translationBubble.transform.position) / 2;
            wordPairs.Add(latinBubble.id, allWordPairs[latinBubble.id]);
            wordPairs.Add(translationBubble.id, allWordPairs[translationBubble.id]);
            SpawnNewBubbles();
        }
        vfx.transform.localScale = Vector3.one * 0.2f;
        CheckForLastTwoBubbles();
    }

    void Start()
    {
        CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/VocabBubbles");
        int lang = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);

        for (int i = 1; i < Scenario.rowData.Count; i++)
        {
            wordPairs.Add(i, new WordPair(Scenario.rowData[i][0], Scenario.rowData[i][1+lang]));
            allWordPairs.Add(i, new WordPair(Scenario.rowData[i][0], Scenario.rowData[i][1 + lang]));
        }

        SpawnNewBubbles(); 
    }

    private void RemoveBubbles(BubbleWord latinBubble, BubbleWord translationBubble)
    {
        spawnedBubbles.Remove(latinBubble);
        spawnedBubbles.Remove(translationBubble);
        Destroy(latinBubble.gameObject);
        Destroy(translationBubble.gameObject);
    }

    private void CheckForLastTwoBubbles()
    {
        if (spawnedBubbles.Count == 0)
        {
            if (wordPairs.Count > 0) 
            {
                SpawnNewBubbles();
            }
            else 
            {
                OnScenarioCompleted.Invoke();
            }
        }
    }

    private void SpawnNewBubbles()
    {
        foreach (var bubble in spawnedBubbles)
        {
            Destroy(bubble.gameObject);
        }
        spawnedBubbles.Clear();

        List<int> shuffledIds = new List<int>(wordPairs.Keys);
        ShuffleList(shuffledIds);

        int n = translationSpawnPositions.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Transform temp = translationSpawnPositions[i];
            translationSpawnPositions[i] = translationSpawnPositions[j];
            translationSpawnPositions[j] = temp;
        }

        for (int i = 0; i < Mathf.Min(latinSpawnPositions.Length, shuffledIds.Count); i++)
        {
            int id = shuffledIds[i];
            WordPair pair = wordPairs[id];

            GameObject latinBubbleGO = Instantiate(latinBubblePrefab, latinSpawnPositions[i]);
            BubbleWord latinBubble = latinBubbleGO.GetComponent<BubbleWord>();
            latinBubble.id = id;
            latinBubble.Init(pair.latinWord);
            spawnedBubbles.Add(latinBubble);

            GameObject translationBubbleGO = Instantiate(translationBubblePrefab, translationSpawnPositions[i]);
            BubbleWord translationBubble = translationBubbleGO.GetComponent<BubbleWord>();
            translationBubble.id = id;
            translationBubble.Init(pair.translation);
            spawnedBubbles.Add(translationBubble);

            wordPairs.Remove(id);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private struct WordPair
    {
        public string latinWord;
        public string translation;

        public WordPair(string latin, string trans)
        {
            latinWord = latin;
            translation = trans;
        }
    }
}