using System.Collections;
using System.Collections.Generic;
using RecordedScenario;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WellcomeScreenController : MonoBehaviour
{
    [SerializeField] string [] blackListScenes;
    [SerializeField] GameObject panel;
    [SerializeField] Text text;
    [SerializeField] Button button;
    string sceneName;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < blackListScenes.Length; i++)
        {
            if (blackListScenes[i]== SceneManager.GetActiveScene().name)
                return;
        }

        button.onClick.AddListener(Continue);

        if (FindObjectOfType<RecordedScenarioText>())
            FindObjectOfType<RecordedScenarioText>().PlayOnAwake = false;

        panel.SetActive(true);
        //text.text = Parse(SceneManager.GetActiveScene().buildIndex);
    }

    string Parse(int rowIndex) 
    {
        CSVParser cSVParser = new CSVParser("Scenarios/" + "B12_Recorded" + "/B12_Sentence_Construction");
        string text = cSVParser.rowData[rowIndex][0];
        return text;
    }

    void Continue() 
    {
        if (FindObjectOfType<RecordedScenarioText>())
            FindObjectOfType<RecordedScenarioText>().Play();
      
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
