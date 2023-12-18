using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Meta.WitAi.TTS.Utilities;
using System;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS;
using Meta.WitAi.TTS.Data;
using System.IO;
using GoogleTextToSpeech.Scripts;
using System.Security.Cryptography;

namespace RecordedScenario
{
    [System.Serializable]
    public struct PhraseOnDemand
    {
        public PhraseOnDemand(string _s, string _tg, string[] _txt, AudioClip[] _voiceAudio)
        {
            Speaker = _s;
            Tag = _tg;
            Text = new string[_txt.Length];
            _txt.CopyTo(Text, 0);
            VoiceAudio = new AudioClip[_voiceAudio.Length];
            _voiceAudio.CopyTo(VoiceAudio, 0);
        }
        public string Speaker;
        public string Tag;
        public string[] Text;
        public AudioClip[] VoiceAudio;
    }

    [ExecuteInEditMode]
    public class PrerecordedAudioOnDemand : MonoBehaviour
    {   
        [SerializeField] private string scenarioName;  
        public Action <string> speakAction;
        [SerializeField] private UnityEvent OnScenarioEnd;
        [SerializeField] private List<SpeakerRef> Speakers;
        [SerializeField] private List<PhraseOnDemand> Phrases;
        [SerializeField] private int language;

        // Start is called before the first frame update
        void Start()
        {
            language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);
        }

        #if UNITY_EDITOR
        public void InitiateSetup()
        {
            StopAllCoroutines();
            StartCoroutine("Setup");
        }

        public void BreakSetup()
        {
            StopAllCoroutines();
        }

        private IEnumerator Setup()
        {
            CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/RecordedScenarioText");
            int _len = Scenario.rowData[0].Length;
            int _langNumber = _len - 2;
            Phrases = new List<PhraseOnDemand>();
            foreach (string[] _row in Scenario.rowData)
            {
                if (_row == Scenario.rowData[0])
                {
                    continue;
                }
                string _tag = _row[1];
                string[] _texts = new string[_langNumber];
                for (int i = 0; i < _langNumber; i++)
                {
                    _texts[i] = _row[2 + i];
                }
                if (_row[0] == "ANIMATION")
                {
                    Debug.LogWarning("Animations are not implemented for audio on demand type of scenarios.");
                }
                else
                {
                    //preprocess English TTS audio
                    string _name = _row[1];
                    AudioClip[] _voiceAudio = new AudioClip[_langNumber];
                    SpeakerRef _speaker = Speakers.Find(a => a.Name == _row[0]);

                    _speaker.Speaker.runInEditMode = true;
                    _speaker.Speak(_texts[0]);
                    float _timeElapsedLog = 0;
                    while (_speaker.Speaker.IsLoading)
                    {
                        _timeElapsedLog += Time.deltaTime;
                        Debug.Log("Waiting for " + _speaker.Name + " to load " + _texts[0] + ". Time elapsed: " + _timeElapsedLog);
                        yield return 0;
                    }
                    _speaker.Speaker.AudioSource.Stop();

                    //preprocess the german stuff here;
                    string _resourcesName = "Scenarios/" + scenarioName + "/TTS_AudioRecordings/German/" + _name;

                    _voiceAudio[1] = Resources.Load<AudioClip>(_resourcesName);
                    if(_voiceAudio[1] == null)
                    {
                        string _fullName = Application.dataPath + "/Resources/" + _resourcesName;
                        for (int i = 0; i < 3; i++)
                        {
                            _speaker.GoogleSpeaker.SaveToFile(_texts[1], _fullName);
                            while (_speaker.GoogleSpeaker.SaveWIP == 0)
                            {
                                Debug.Log("Waiting for " + _speaker.Name + " to load " + _texts[1] + ". Time elapsed: " + _timeElapsedLog);
                                yield return 0;
                            }
                            if (_speaker.GoogleSpeaker.SaveWIP == 1)
                                break;
                            else
                            {
                                if (i < 2)
                                    Debug.LogWarning("Couldn't load " + _name + ", trying " + (2 - i) + "more times");
                                else
                                    Debug.LogError("Did not manage to load " + _name + "after 3 attempts");
                            }
                        }
                        if (_speaker.GoogleSpeaker.SaveWIP == 1)
                            _voiceAudio[1] = Resources.Load<AudioClip>(_resourcesName);
                    }
                    Phrases.Add(new PhraseOnDemand(_row[0], _tag, _texts, _voiceAudio));
                }
                    
            }
        }
        #endif

        static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void PlayTag(string tag)
        {
            Debug.Log("Got request to play the phrase: " + tag);
            foreach (PhraseOnDemand _phrase in Phrases)
            {
                if (_phrase.Tag == tag)
                {
                    SpeakerRef _speaker = Speakers.Find(a => a.Name == _phrase.Speaker);
                    //_phrase.Speaker
                    if (_speaker != null)
                    {
                        if(language == 0)
                        {
                            _speaker.Speak(_phrase.Text[language]);
                            speakAction?.Invoke(_speaker.Name);
                        }
                        else
                        {
                            string _name = ComputeSHA256Hash(_phrase.Speaker + _phrase.Text[1]);
                            string _resourcesName = "Scenarios/" + scenarioName + "/TTS_AudioRecordings/German/" + _name;
                            AudioClip _toPlay = Resources.Load<AudioClip>(_resourcesName);
                            if (_toPlay != null)
                                _speaker.GoogleSpeaker.PlaySaved(_toPlay);
                            else
                                Debug.LogError("Phrase " + _phrase.Text[1] + " for " + _phrase.Speaker + " hasn't been prerecorded.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No speaker found with name " + _phrase.Speaker + "!");
                    }
                    if (_phrase.Equals(Phrases[Phrases.Count - 1]))
                    {
                        Debug.Log("Scenario ended...");
                        Invoke("End",0);
                    }
                }
            }
        }

        private void End()
        {
            OnScenarioEnd.Invoke();
        }
    }

}
