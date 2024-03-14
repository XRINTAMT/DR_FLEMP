using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ChecklistMechanic : MonoBehaviour
{
    [field: SerializeField] public bool Oncoming { set; get; }
    public int[] correctAnswers;
    [SerializeField] Toggle[] checkBoxes;
    [SerializeField] string scenarioName;
    [SerializeField] string scenarioTag;
    [SerializeField] GameObject ScorePanel;
    [SerializeField] Text ScoreText;
    [SerializeField] Text[] FormularyText;
    [SerializeField] private bool InitOnStartup = true;
    [SerializeField] RectTransform FormularyRoot;
    public NurseTabletRecord[] TabletRecords;
    public UnityEvent OnComplete;
    int[] givenAnswers;
    [HideInInspector]
    public bool indicate;
    private string[][] AnswerTexts;
    [SerializeField] private bool shuffleAnswers = true;
    Timer timer;
    public void Awake()
    {
        if(InitOnStartup)
            ParseTheScenario();
        timer = GetComponent<Timer>();
    }

    private void Start()
    {
        LayoutRebuilder.MarkLayoutForRebuild(FormularyRoot);
    }

    public void ParseTheScenario() //use with caution, erases all the answers gathered from the player
    {
        TabletRecords = GetComponentsInChildren<NurseTabletRecord>();
        AnswerTexts = new string[TabletRecords.Length][];
        var langTag = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0 ? "" : "German";
        CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/NursingTablets" +langTag);
        int i = -1;
        if (shuffleAnswers)
        {
            correctAnswers = new int[Scenario.rowData.Count];
        }
        givenAnswers = new int[Scenario.rowData.Count];
        for(int k = 0; k < givenAnswers.Length; k++)
        {
            givenAnswers[k] = -1;
        }
        if(Scenario.rowData[0].Length == 7) //normal scenario
        {
            Debug.Log("handling a normal scenario with hints and audiohints");
            foreach (string[] row in Scenario.rowData)
            {
                if (i != -1 && TabletRecords.Length > i)
                {
                    string[] _answers = new string[3];
                    int _correct = Random.Range(0, 3);
                    correctAnswers[i] = _correct;
                    for (int j = 0; j < _correct; j++)
                    {
                        _answers[j] = row[3 + j];
                    }
                    _answers[_correct] = row[2];
                    for (int j = _correct + 1; j <= 2; j++)
                    {
                        _answers[j] = row[2 + j];
                    }
                    TabletRecords[i].SetData(row[0], _answers, row[1]);
                    AnswerTexts[i] = _answers;
                }
                i++;
            }
        }
        else //no hint scenario
        {
            if (shuffleAnswers)
            {
                foreach (string[] row in Scenario.rowData)
                {
                    if (i != -1 && TabletRecords.Length > i)
                    {
                        string[] _answers = new string[3];
                        int _correct = Random.Range(0, 3);
                        correctAnswers[i] = _correct;
                        for (int j = 0; j < _correct; j++)
                        {
                            _answers[j] = row[2 + j];
                        }
                        _answers[_correct] = row[1];
                        for (int j = _correct + 1; j <= 2; j++)
                        {
                            _answers[j] = row[1 + j];
                        }
                        TabletRecords[i].SetData(row[0], _answers, null);
                    }
                    i++;
                }
            }
            else
            {
                foreach (string[] row in Scenario.rowData)
                {
                    if (i != -1 && TabletRecords.Length > i)
                    {
                        string[] _answers = new string[row.Length - 1];
                        for (int j = 1; j < row.Length; j++)
                        {
                            _answers[j - 1] = row[j];
                        }
                        TabletRecords[i].SetData(row[0], _answers, null);
                    }
                    i++;
                }
            }
        }
    }

    public void ParseTheScenarioRandomized(int[][] RandomizedMatrix) //use with caution, erases all the answers gathered from the player
    {
        TabletRecords = GetComponentsInChildren<NurseTabletRecord>();
        AnswerTexts = new string[TabletRecords.Length][];
        Debug.Log("Started parsing this shit");
        var langTag = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0 ? "" : "German";
        CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/NursingTablets" + langTag);
        int i = -1;
        if (shuffleAnswers)
        {
            correctAnswers = new int[Scenario.rowData.Count];
        }
        givenAnswers = new int[Scenario.rowData.Count];
        for (int k = 0; k < givenAnswers.Length; k++)
        {
            givenAnswers[k] = -1;
        }
        foreach (string[] row in Scenario.rowData)
        {
            if (i != -1 && TabletRecords.Length > i)
            {
                Debug.Log("Actually PROCESSING " + row[0]);
                string[] _answers = new string[RandomizedMatrix[i].Length];
                for (int j = 0; j < RandomizedMatrix[i].Length; j++)
                {
                    _answers[j] = row[RandomizedMatrix[i][j] + 1];
                }
                TabletRecords[i].SetData(row[0], _answers, null);
                AnswerTexts[i] = _answers;
            }
            i++;
        }
    }

    public void SaveAnswer(int _id, int _answer)
    {
        givenAnswers[_id] = _answer;
        if(_answer == correctAnswers[_id])
        {
            //run code if the correct answer is given
        }
        else
        {
            //run code if the wrong answer is given
        }
        if(FormularyText.Length > 0)
        {
            FormularyText[_id].text = AnswerTexts[_id][_answer];
            LayoutRebuilder.MarkLayoutForRebuild(FormularyRoot);
            //LayoutRebuilder.ForceRebuildLayoutImmediate(FormularyRoot);
        }
        CheckCompletion();

        timer.TimerStart();
    }

    public void CheckCompletion() {
        foreach (Toggle _checkbox in checkBoxes)
        {
            if (!_checkbox.isOn)
                return;
            //check in the answer is correct and probably save the score somewhere
        }
        //Everything is done
        ShiftChangeFinishScreen _scfs = FindObjectOfType<ShiftChangeFinishScreen>(true);
        if(_scfs != null)
        {
            FindObjectOfType<PauseManager>().ShowMultiplayerOutro();
        }
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + scenarioTag, 1);
        ScorePanel.SetActive(true);
        ScoreText.text = CalculateScore() + "/" + correctAnswers.Length;
        OnComplete.Invoke();
    }

    private int CalculateScore()
    {
        int _score = 0;
        for(int i = 0; i < correctAnswers.Length; i++)
        {
            if (correctAnswers[i] == givenAnswers[i])
                _score++;
        }
        return _score;
    }
    public void EnableIndicate()
    {
        for (int i = 0; i < TabletRecords.Length; i++)
        {
            //if (!TabletRecords[i].checkbox.isOn)
                //TabletRecords[i].MainText.color = Color.green;
        }
        indicate = true;
    }
    public void DisableIndicate() 
    {
        //for (int i = 0; i < TabletRecords.Length; i++)
            //TabletRecords[i].MainText.color = Color.white;

        indicate = false;
    }

    public void Reset()
    {
        foreach (Toggle _checkbox in checkBoxes)
        {
            _checkbox.isOn = false;
        }
        ParseTheScenario();
    }
}
