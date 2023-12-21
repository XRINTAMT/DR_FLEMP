using System.Collections;
using System.Collections.Generic;
using RecordedScenario;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WellcomeScreenController : MonoBehaviour
{
    [SerializeField] string[] blackListScenes;
    [SerializeField] GameObject panelStart;
    [SerializeField] GameObject panelEnd;
    [SerializeField] Text textStart;
    [SerializeField] Text textEnd;
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonEnd;
    string sceneName;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < blackListScenes.Length; i++)
        {
            if (blackListScenes[i] == SceneManager.GetActiveScene().name)
                return;
        }

        buttonStart.onClick.AddListener(ContinueStart);
        buttonEnd.onClick.AddListener(ContinueEnd);

        if (FindObjectOfType<RecordedScenarioText>())
            FindObjectOfType<RecordedScenarioText>().PlayOnAwake = false;

        panelStart.SetActive(true);
        //textStart.text = Parse(SceneManager.GetActiveScene().buildIndex,1);
        //textEnd.text = Parse(SceneManager.GetActiveScene().buildIndex,2);
    }

    string Parse(int rowIndex, int column)
    {
        CSVParser cSVParser = new CSVParser("Scenarios/" + "B12_Recorded" + "/B12_Sentence_Construction");
        string text = cSVParser.rowData[rowIndex][column];
        return text;
    }
    public void OpenEndPanel()
    {
        panelEnd.SetActive(true);
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
