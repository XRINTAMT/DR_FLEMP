using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ViewerUIController : MonoBehaviour
{
    [SerializeField] private Slider setDialogueVolumeStatus;
    [SerializeField] private Slider setSoundVolumeStatus;
    [SerializeField] private Slider setMusicVolumeStatus;
    [SerializeField] private AudioMixer AppMixer;

    public static float dialogueVolume;
    public static float soundVolume;
    public static float musicVolume;
    // Start is called before the first frame update
    void Start()
    {
        LoadSettingsIntoUI();
    }

    public void LoadSettingsIntoUI()
    {
        setDialogueVolumeStatus.value = PlayerPrefs.GetFloat("dialogueVolume", 0.5f);
        setSoundVolumeStatus.value = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        setMusicVolumeStatus.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
    }

    public void SetDialogueVolume()
    {
        dialogueVolume = setDialogueVolumeStatus.value;
        PlayerPrefs.SetFloat("dialogueVolume", dialogueVolume);
        if (dialogueVolume == 0)
            AppMixer.SetFloat("Dialogues", -80);
        else
        {
            AppMixer.SetFloat("Dialogues", Mathf.Log(dialogueVolume) * 20);
        }
    }

    public void SetSoundVolume()
    {
        soundVolume = setSoundVolumeStatus.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
        if (soundVolume == 0)
            AppMixer.SetFloat("Sounds", -80);
        else
        {
            AppMixer.SetFloat("Sounds", Mathf.Log(soundVolume) * 20);
        }
    }

    public void SetMusicVolume()
    {
        musicVolume = setMusicVolumeStatus.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        if (musicVolume == 0)
            AppMixer.SetFloat("Music", -80);
        else
        {
            AppMixer.SetFloat("Music", Mathf.Log(musicVolume) * 20);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
