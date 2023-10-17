using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Meta.WitAi.TTS.Utilities;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS;
using Meta.WitAi.TTS.Data;
using System.IO;

namespace RecordedScenario
{
    [System.Serializable]
    public struct Phrase
    {
        public Phrase(string _s, bool _h, int[] _tc, string[] _txt, AudioClip[] _voiceAudio)
        {
            Speaker = _s;
            Highlight = _h;
            Timecode = new int[_tc.Length];
            _tc.CopyTo(Timecode, 0);
            Text = new string[_txt.Length];
            _txt.CopyTo(Text, 0);
            VoiceAudio = new AudioClip[_voiceAudio.Length];
            _voiceAudio.CopyTo(VoiceAudio, 0);
        }
        public string Speaker;
        public bool Highlight;
        public int[] Timecode;
        public string[] Text;
        public AudioClip[] VoiceAudio;
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
            Debug.Log("Speaker " + Name + " on gameobject " + Speaker.transform.parent.name + " -> " + Speaker.name);
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

    [ExecuteInEditMode]
    public class RecordedScenarioText : MonoBehaviour
    {
        [SerializeField] private string scenarioName;  
        public bool PlayOnAwake;
        public int PlayOnAwakeTimeout;
        public int ScenarioEndTimeout; //Timeout after the last phrase in the scenario is played
        [SerializeField] private UnityEvent OnScenarioEnd;
        [SerializeField] private List<SpeakerRef> Speakers;
        [SerializeField] private List<AnimationEntity> AnimationEntities;
        [SerializeField] private Text TranscriptTextbox;
        private int previousTimeElapsed;
        private float TimeElapsed;
        private bool running;
        [SerializeField] private float TestScenarioSpeed = 1;
        [SerializeField] private List<Phrase> Phrases;
        [SerializeField] private List<AnimationCall> AnimationCalls;
        private int DownloadProgressFlag = 0;

        // Start is called before the first frame update
        void Start()
        {
            if (PlayOnAwake)
                Invoke("Play", PlayOnAwakeTimeout);
        }

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
                for (int i = 0; i < _langNumber; i++)
                {
                    _timecodes[i] = int.Parse(_row[2 + i * 2]);
                    _texts[i] = _row[3 + i * 2];
                }
                if (_row[0] == "ANIMATION")
                {
                    AnimationCalls.Add(new AnimationCall(_row[3], _timecodes));
                }
                else
                {
                    string _name = _row[0] + _timecodes[0] + "English";
                    AudioClip[] _voiceAudio = new AudioClip[_langNumber];
                    SpeakerRef _speaker = Speakers.Find(a => a.Name == _row[0]);
                    _speaker.Speaker.runInEditMode = true;
                    _speaker.Speaker.Speak(_texts[0]);


                    //Wit.DownloadToDiskCache(_texts[0]);

                    /*
                    DownloadProgressFlag = 0;
                    float _timeElapsedLog = 0;
                    while (DownloadProgressFlag == 0)
                    {
                        _timeElapsedLog += Time.deltaTime;
                        Debug.Log("Waiting for " + _speaker.Name + " to start downloading: " + _name + ". Time elapsed: " + _timeElapsedLog);
                        yield return 0;
                    }
                    if(DownloadProgressFlag == 3)
                    {
                        Debug.LogError("Error starting to download a phrase");
                        yield break;
                    }
                    if (DownloadProgressFlag == 3)
                    {
                        Debug.LogError("Canceled starting to download a phrase");
                        yield break;
                    }
                    while (DownloadProgressFlag == 1)
                    {
                        _timeElapsedLog += Time.deltaTime;
                        Debug.Log("Waiting for " + _speaker.Name + " to finish downloading: " + _name + ". Time elapsed: " + _timeElapsedLog);
                        yield return 0;
                    }
                    if (DownloadProgressFlag == 3)
                    {
                        Debug.LogError("Error downloading a phrase");
                        yield break;
                    }
                    Debug.Log("downloaded successfully???");

                    */




                    /*
                    float _timeElapsedLog = 0;
                    AudioSource _source = _speaker.Speaker.AudioSource;
                    while (!_source.isPlaying)
                    {
                        _timeElapsedLog += Time.deltaTime;
                        Debug.Log("Waiting for " + _speaker.Name + " to say " + _texts[0] + ". Time elapsed: " + _timeElapsedLog);
                        yield return 0;
                    }
                    */
                    //_source.Stop();

                    float _timeElapsedLog = 0;
                    while (_speaker.Speaker.IsLoading)
                    {
                        _timeElapsedLog += Time.deltaTime;
                        Debug.Log("Waiting for " + _speaker.Name + " to load " + _texts[0] + ". Time elapsed: " + _timeElapsedLog);
                        yield return 0;
                    }
                    Debug.Log(_speaker.Name + " loaded the phrase. Trying to obtain the audio data.");


                    //AudioClip _streamedClip = _speaker.Speaker.SpeakingClip.clip;
                    //Debug.Log("Streamed clip: " + _streamedClip);
                    
                    //RequestDownload(string downloadPath, string textToSpeak, Dictionary< string, string> ttsData, RequestCompleteDelegate<bool> onComplete, RequestProgressDelegate onProgress = null)
                    // Create a new non-streamed AudioClip and copy the audio data
                    //SavWav.Save("Scenarios/" + scenarioName + "/Audios/" + _row[0] + _timecodes[0] + "English", _clip);

                    //TTSClipData _ttsData = TTSService.CreateClipData(_texts[0], _name, _speaker.Speaker.VoiceSettings, _diskSettings);

                    //WitTTSVRequest.RequestDownload("Scenarios/" + scenarioName + "/Audios/" + _name, _texts[0], _speaker.Speaker.

                    AudioClip _clip;
                    _clip = null;

                    /*
                    TTSDiskCacheSettings _diskSettings = new TTSDiskCacheSettings();
                    _diskSettings.DiskCacheLocation = TTSDiskCacheLocation.Persistent;
                    TTSClipData _ttsData = new TTSClipData();
                    _ttsData.audioType = AudioType.WAV;
                    _ttsData.clipID = _name;
                    _ttsData.textToSpeak = _texts[0];
                    _ttsData.voiceSettings = _speaker.Speaker.VoiceSettings;
                    _ttsData.diskCacheSettings = _diskSettings;
                    _speaker.Speaker._tts.WebHandler.RequestDownloadFromWeb(_ttsData, "Scenarios/" + scenarioName + "/Audios/" + _name);
                    
                    */
                    for (float i = 0; i < 5; i += Time.deltaTime)
                    {
                        yield return 0;
                    }
                    //string file = Directory.GetFiles("Assets/StreamingAssets/TempAudio/")[0];
                    //File.Copy(file, "Scenarios/" + scenarioName + "/Audios/" + _name, true);
                    /*
                    
                    */

                    //SavWav.Save("Scenarios/" + scenarioName + "/Audios/" + _name, _streamedClip);
                    //_clip = Resources.Load("Scenarios/" + scenarioName + "/Audios/" + _name) as AudioClip;
                    Debug.Log("Resulting clip: " + _clip);
                    _voiceAudio[0] = _clip;
                    //SavWav.Save("Scenarios/" + scenarioName + "/Audios/" + _name, _clip);
                    Phrases.Add(new Phrase(_row[0], _row[1] != "0", _timecodes, _texts, _voiceAudio));
                }
                    
            }
        }

        public void SetDownloadStatus(int _status) //0 - did not start yet, 1 - downloading, 2 - complete, 3 - error, 4 - cancelled
        {
            DownloadProgressFlag = _status;
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
