using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;

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

    [System.Serializable]
    public class SpeakerRef
    {
        public string Name;
        public TTSSpeaker Speaker;
        public void Speak(string _s)
        {
            Debug.Log("Speaker " + Name + "on gameobject " + Speaker.transform.parent.name + ">" + Speaker.name);
            Speaker.Speak(_s);
        }
    }

    public class RecordedScenarioText : MonoBehaviour
    {
        [SerializeField] private string scenarioName;
        [SerializeField] private Text TranscriptTextbox;
        public bool PlayOnAwake;
        public int PlayOnAwakeTimeout;
        [SerializeField] private List<Phrase> Phrases;
        [SerializeField] private List<SpeakerRef> Speakers;
        private int previousTimeElapsed;
        private float TimeElapsed;
        private bool running;
        public float TestScenarioSpeed = 1;
        
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
            if (PlayOnAwake)
                Invoke("Play", PlayOnAwakeTimeout);
        }

        private void ProcessTick(int _tickNumber)
        {
            foreach(Phrase _phrase in Phrases)
            {
                int _lang = 0;
                if (_phrase.Timecode[_lang] == _tickNumber)
                {
                    string textToAdd = "<b>" + _phrase.Speaker + ":</b> " + _phrase.Text[_lang];
                    if (_phrase.Highlight)
                    {
                        textToAdd = "<i><color=#68FF9A>" + textToAdd + "</color></i>";
                    }
                    textToAdd += "\n";
                    TranscriptTextbox.text += textToAdd;
                    //_phrase.Speaker
                    SpeakerRef _speaker = Speakers.Find(a => a.Name == _phrase.Speaker);
                    if (_speaker != null)
                    {
                        _speaker.Speak(_phrase.Text[_lang]);
                    }
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
            TimeElapsed += Time.deltaTime * TestScenarioSpeed;
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
