using System.Collections;
using System.Collections.Generic;
using RecordedScenario;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class WelcomeScreenController : MonoBehaviour
{
    [SerializeField] ScenarioRelations Scenarios;
    [SerializeField] string[] blackListScenes;
    [SerializeField] GameObject panelStart;
    [SerializeField] GameObject panelEnd;
    [SerializeField] Text textStart;
    [SerializeField] Text textEnd;
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonEnd;
    [SerializeField] Button buttonContinue;
    string sceneName;
    string nextScene;
    // Start is called before the first frame update
    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        
        for (int i = 0; i < blackListScenes.Length; i++)
        {
            if (blackListScenes[i] == sceneName)
                return;
        }

        buttonStart.onClick.AddListener(ContinueStart);
        buttonEnd.onClick.AddListener(ContinueEnd);

        if (FindObjectOfType<RecordedScenarioText>())
            FindObjectOfType<RecordedScenarioText>().PlayOnAwake = false;

        panelStart.SetActive(true);
        textStart.text = GetWelcomeText();
    }

    string Parse(int rowIndex, int column)
    {
        CSVParser cSVParser = new CSVParser("Scenarios/" + "B12_Recorded" + "/B12_Sentence_Construction");
        string text = cSVParser.rowData[rowIndex][column];
        return text;
    }

    private string GetWelcomeText()
    {
        List<string[]> csvData = (new CSVParser("Scenarios/Intros")).rowData;

        int language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);

        // Find the row that matches the specified sceneName
        string[] matchingRow = csvData.Find(row => row[0].Equals(sceneName));

        if (matchingRow != null)
        {
            return matchingRow[language + 1];
        }
        else
        {
            Debug.LogWarning($"Scene Name '{sceneName}' not found in the CSV data.");
            return $"Welcome text for '{sceneName}' (not implemented yet)";
        }
    }

    public void OpenEndPanel()
    {
        panelEnd.SetActive(true);
        nextScene = Scenarios.GetNext(sceneName);
        buttonContinue.gameObject.SetActive(nextScene != null);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    void ContinueStart() 
    {
        if (FindObjectOfType<RecordedScenarioText>())
            FindObjectOfType<RecordedScenarioText>().Play();
      
        panelStart.SetActive(false);
    }

    void ContinueEnd()
    {
        SceneManager.LoadScene("Lobby");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
