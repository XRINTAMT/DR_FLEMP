using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RecordedScenario
{
    [System.Serializable]
    public struct Phrase
    {
        public Phrase(string _s, bool _h, int[] _tc, string[] _txt)
        {
            Speaker = _s;
            Highlight = _h;
            Timecode = new int[_tc.Length];
            _tc.CopyTo(Timecode, 0);
            Text = new string[_txt.Length];
            _txt.CopyTo(Text, 0);
        }
        public string Speaker;
        public bool Highlight;
        public int[] Timecode;
        public string[] Text;
    }

    public class RecordedScenarioText : MonoBehaviour
    {
        [SerializeField] private string scenarioName;
        [SerializeField] private Text TranscriptTextbox;
        public bool PlayOnAwake;
        [SerializeField] private List<Phrase> Phrases;
        private int previousTimeElapsed;
        private float TimeElapsed;
        private bool running;
        
        // Start is called before the first frame update
        void Start()
        {
            CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/RecordedScenarioText");
            int _len = Scenario.rowData[0].Length;
            int _langNumber = (_len - 2) / 2;
            Phrases = new List<Phrase>();
            foreach (string[] _row in Scenario.rowData)
            {
                if (_row == Scenario.rowData[0])
                {
                    continue;
                }
                int[] _timecodes = new int[_langNumber];
                string[] _texts = new string[_langNumber];
                for(int i = 0; i < _langNumber; i++)
                {
                    _timecodes[i] = int.Parse(_row[2 + i * 2]);
                    _texts[i] = _row[3 + i * 2];
                }
                Phrases.Add(new Phrase(_row[0], _row[1] != "0", _timecodes, _texts));
            }
            running = PlayOnAwake;
        }

        private void ProcessTick(int _tickNumber)
        {
            foreach(Phrase _phrase in Phrases)
            {
                if(_phrase.Timecode[0] == _tickNumber)
                {
                    string textToAdd = "<b>" + _phrase.Speaker + ":</b> " + _phrase.Text[0];
                    if (_phrase.Highlight)
                    {
                        textToAdd = "<i><color=#68FF9A>" + textToAdd + "</color></i>";
                    }
                    textToAdd += "\n";
                    TranscriptTextbox.text += textToAdd;
                }
            }
        }

        public void Play()
        {
            running = true;
        }

        public void Pause()
        {
            running = false;
        }

        private void Tick()
        {
            TimeElapsed += Time.deltaTime;
            if(previousTimeElapsed < (int)TimeElapsed)
            {
                previousTimeElapsed = (int)TimeElapsed;
                ProcessTick((int)TimeElapsed);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(running)
                Tick();
        }
    }

}
