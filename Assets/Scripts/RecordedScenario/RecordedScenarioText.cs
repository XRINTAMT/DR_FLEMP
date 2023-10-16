using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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
    public struct AnimationCall
    {
        public AnimationCall(string _t, int[] _tc)
        {
            AnimationTag = _t;
            Timecode = new int[_tc.Length];
            _tc.CopyTo(Timecode, 0);
        }
        public string AnimationTag;
        public int[] Timecode;
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

    [System.Serializable]
    public class AnimationEntity
    {
        public string Name;
        public UnityEvent AnimationEvent;
        public void Activate()
        {
            AnimationEvent.Invoke();
        }
    }

    public class RecordedScenarioText : MonoBehaviour
    {
        [SerializeField] private string scenarioName;
        [SerializeField] private Text TranscriptTextbox;   
        public bool PlayOnAwake;
        public int PlayOnAwakeTimeout;
        public int ScenarioEndTimeout; //Timeout after the last phrase in the scenario is played
        [SerializeField] private UnityEvent OnScenarioEnd;
        [SerializeField] private List<Phrase> Phrases;
        [SerializeField] private List<SpeakerRef> Speakers;
        [SerializeField] private List<AnimationCall> AnimationCalls;
        [SerializeField] private List<AnimationEntity> AnimationEntities;
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
            AnimationCalls = new List<AnimationCall>();
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
                if (_row[0] == "ANIMATION")
                    AnimationCalls.Add(new AnimationCall(_row[3], _timecodes));
                else
                    Phrases.Add(new Phrase(_row[0], _row[1] != "0", _timecodes, _texts));
            }
            if (PlayOnAwake)
                Invoke("Play", PlayOnAwakeTimeout);
        }

        private void ProcessTick(int _tickNumber)
        {
            int _lang = 0;
            foreach (Phrase _phrase in Phrases)
            {
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
                    else
                    {
                        Debug.LogWarning("No speaker found with name " + _phrase.Speaker + "!");
                    }
                    if (_phrase.Equals(Phrases[Phrases.Count - 1]))
                    {
                        Debug.Log("Scenario ended...");
                        Invoke("End", ScenarioEndTimeout);
                    }
                }
            }
            foreach (AnimationCall _call in AnimationCalls)
            {
                if (_call.Timecode[_lang] == _tickNumber)
                {
                    AnimationEntity _animation = AnimationEntities.Find(a => a.Name == _call.AnimationTag);
                    if (_animation != null)
                    {
                        _animation.Activate();
                    }
                    else
                    {
                        Debug.LogWarning("No animation found with tag " + _call.AnimationTag + "!");
                    }
                }
            }
        }

        private void End()
        {
            OnScenarioEnd.Invoke();
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
