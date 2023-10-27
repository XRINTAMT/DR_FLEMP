using System;
using GoogleTextToSpeech.Scripts.Data;
using UnityEngine;

namespace GoogleTextToSpeech.Scripts
{
    public class GoogleTTS : MonoBehaviour
    {
        [SerializeField] private VoiceScriptableObject voice;
        [SerializeField] private TextToSpeech textToSpeech;
        [SerializeField] private AudioSource audioSource;

        private Action<AudioClip> _audioClipReceived;
        private Action<BadRequestData> _errorReceived;
        public int SaveWIP { private set; get; } = 1; // 0 for loading, 1 for free, 2 for error
        private string SaveLocation;

        public void SaveToFile(string _text, string _filename)
        {
            if (SaveWIP == 0)
            {
                Debug.LogError("Download is still in progress, but there was a call to make another request anyway.");
                return;
            }
            SaveWIP = 0;
            SaveLocation = _filename;
            _errorReceived += ErrorReceived;
            _audioClipReceived += SaveClip;
            textToSpeech.GetSpeechAudioFromGoogle(_text, voice, _audioClipReceived, _errorReceived);
        }

        public void Speak(string _text)
        {
            _errorReceived += ErrorReceived;
            _audioClipReceived += AudioClipReceived;
            textToSpeech.GetSpeechAudioFromGoogle(_text, voice, _audioClipReceived, _errorReceived);
        }

        private void ErrorReceived(BadRequestData badRequestData)
        {
            Debug.Log($"Error {badRequestData.error.code} : {badRequestData.error.message}");
            SaveWIP = 2;
        }

        private void AudioClipReceived(AudioClip _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.Play();
        }

        private void SaveClip(AudioClip _clip)
        {
            SavWav.Save(SaveLocation, _clip);
            SaveWIP = 1;
        }

        public void PlaySaved(AudioClip _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.Play();
        }
    }
}
