using UnityEngine;
using System.Collections.Generic;

public class DynamicPatientInfo : MonoBehaviour
{
    [SerializeField] private string patientDescription;
    [SerializeField] private string patientName;
    [SerializeField] private string scenarioName;
    [SerializeField] private ChecklistMechanic Checklist;
    [SerializeField] private AICharacter.CharacterInfo Patient;

    void Start()
    {
        string langTag = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0 ? "" : "German";
        string filePath = $"Scenarios/{scenarioName}/Character{patientName}" + langTag;
        Debug.Log(filePath); 
        CSVParser parser = new CSVParser(filePath);
        List<string[]> characterData = parser.rowData;

        for (int i = 0; i < characterData.Count; i++)
        {
            string[] row = characterData[i];

            // Filter out empty strings
            List<string> nonEmptyElements = new List<string>();
            foreach (string element in row)
            {
                string trimmedElement = element.Trim();
                if (!string.IsNullOrEmpty(trimmedElement))
                {
                    nonEmptyElements.Add(trimmedElement);
                }
            }

            // Randomly select a column index from non-empty elements
            int randomColumnIndex = Random.Range(0, nonEmptyElements.Count);
            patientDescription += nonEmptyElements[randomColumnIndex] + " ";
            int correctAnswerIndex = randomColumnIndex;
            Checklist.correctAnswers[i] = correctAnswerIndex;
        }

        Patient.description = patientDescription;
        Patient.germanDescription = patientDescription;
    }
}
