using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using UnityEngine;

public class HintBehaviour : MonoBehaviour
{
    public static bool startFirstTime;
    public static bool playAudioFirstTime;
    public AudioClip[] audioClip;
    public GameObject hintSkip;
    public GameObject lineSkip;
    AudioSource audioSource;
    private void Awake()
    {
        if (!startFirstTime)
        {
            HintControllers[] hintControllers = FindObjectsOfType<HintControllers>();
            for (int i = 0; i < hintControllers.Length; i++)
            {
                hintControllers[i].EnableHint();
            }

            FindObjectOfType<XRMovementControls>().SwitchLocomotion(2);
            GetComponent<HandTriggerAreaEvents>().enabled = true;
            startFirstTime = true;
        }
        audioSource = GetComponent<AudioSource>();
        PlayAudio(0);
    }

    public void PlayAudio(int index)
    {
        if (!playAudioFirstTime)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audioClip[index]);
            if (index == 4)
            {
                Invoke("ChangeAudio", 24f);
            }
        }


    }
    void ChangeAudio() 
    {
        audioSource.PlayOneShot(audioClip[5]);
        lineSkip.SetActive(true);
        hintSkip.SetActive(true);

    }

    public void EnnableMove() 
    {
        FindObjectOfType<XRHandPlayerControllerLink>().moveAxis = Common2DAxis.primaryAxis;
    }
    // Start is called before the first frame update


}
