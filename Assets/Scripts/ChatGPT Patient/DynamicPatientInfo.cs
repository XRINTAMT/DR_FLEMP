using UnityEngine;
using System.Collections.Generic;

public class DynamicPatientInfo : MonoBehaviour
{
    [SerializeField] private string patientDescription;
    [SerializeField] private string patientName;
    [SerializeField] private string scenarioName;
    [SerializeField] private ChecklistMechanic Checklist;
    [SerializeField] private AICharacter.CharacterInfo Patient;
    [SerializeField] private int maxOptions;

    void Start()
    {
        string langTag = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0 ? "" : "German";
        string filePath = $"Scenarios/{scenarioName}/Character{patientName}" + langTag;
        Debug.Log(filePath); 
        CSVParser parser = new CSVParser(filePath);
        List<string[]> characterData = parser.rowData;
        int[][] RandomizedAnswerMatrix = new int[characterData.Count][];
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

            int[] PickedInfo = GenerateUniqueRandomNumbers(nonEmptyElements.Count - 1);
            Debug.Log(string.Join(", ", PickedInfo));


            // Randomly select a column index from non-empty elements
            int randomColumnIndex = Random.Range(0, PickedInfo.Length);
            patientDescription += nonEmptyElements[0] + " ";
            patientDescription += nonEmptyElements[PickedInfo[randomColumnIndex]+1] + "; ";
            int correctAnswerIndex = randomColumnIndex;
            Checklist.correctAnswers[i] = correctAnswerIndex;
            RandomizedAnswerMatrix[i] = PickedInfo;
        }
        Patient.description = patientDescription;
        Patient.germanDescription = patientDescription;
        Checklist.ParseTheScenarioRandomized(RandomizedAnswerMatrix);
    }

    int[] GenerateUniqueRandomNumbers(int n)
    {
        int[] numbers = new int[n];

        if (n < maxOptions)
        {
            // Fill an array with numbers from 0 to n-1
            for (int i = 0; i < n; i++)
            {
                numbers[i] = i;
            }

            return numbers;
        }

        // Fill an array with numbers from 0 to n-1
        for (int i = 0; i < n; i++)
        {
            numbers[i] = i;
        }


        for (int i = 0; i < n - 1; i++)
        {
            int randomIndex = Random.Range(i, n);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }


        int[] uniqueNumbers = new int[maxOptions];
        for (int i = 0; i < 4; i++)
        {
            uniqueNumbers[i] = numbers[i];
        }

        return uniqueNumbers;
    }
}
