using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTheNextTask : MonoBehaviour
{
    [SerializeField] ScenarioRelations Scenarios;
    [SerializeField] string StartNode;
    [SerializeField] string EndNode;

    public void ToNext()
    {
        string currentScenario = StartNode;

        while (currentScenario != null)
        {
            bool completion = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + currentScenario, 0) == 1;

            if (!completion)
            {
                SceneManager.LoadScene(currentScenario);
                return;
            }
            currentScenario = Scenarios.GetNext(currentScenario);
        }
        SceneManager.LoadScene(EndNode);
    }
}