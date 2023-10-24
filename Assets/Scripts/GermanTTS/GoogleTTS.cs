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
        
        public void Speak(string _text)
        {
            _errorReceived += ErrorReceived;
            _audioClipReceived += AudioClipReceived;
            textToSpeech.GetSpeechAudioFromGoogle(_text, voice, _audioClipReceived, _errorReceived);
        }

        private void ErrorReceived(BadRequestData badRequestData)
        {
            Debug.Log($"Error {badRequestData.error.code} : {badRequestData.error.message}");
        }

        private void AudioClipReceived(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
