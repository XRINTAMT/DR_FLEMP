using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.SimpleLocalization;

public class DoorChoiceMenu : MonoBehaviour
{
    [SerializeField] private LocalizedText infoText;
    private ToggleGroup toggleGroup;

    private void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    UpdateInfoText(toggle);
                }
            });
        }
    }

    public void SubmitSelection()
    {
        Toggle selectedToggle = GetSelectedToggle();

        if (selectedToggle != null)
        {
            string sceneName = selectedToggle.gameObject.name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No scene selected");
        }
    }

    private Toggle GetSelectedToggle()
    {
        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn && toggle.interactable)
            {
                return toggle;
            }
        }

        return null;
    }

    private void UpdateInfoText(Toggle toggle)
    {
        if (toggle != null)
        {
            string scenarioName = toggle.gameObject.name;
            infoText.ChangeTag("Task" + scenarioName);
        }
    }
}