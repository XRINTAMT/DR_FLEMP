using System.Collections;
using System.Collections.Generic;
using PersistentSaveSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PatientTabController : MonoBehaviour
{
    [SerializeField] Button[] buttonsPatients;
    [SerializeField] GameObject[] buttonsBackground;
    [SerializeField] Button buttonTab;
    [SerializeField] Button  buttonDone;
    [SerializeField] TMP_Text namePatient;
    [SerializeField] private CompleteRoom cr;
    [SerializeField] Animator animator;
    AudioSource audioSource;
    public AudioClip correct;
    public AudioClip uncorrect;
    int chooseIndex;

    private void Awake()
    {
        var characters = FindObjectsOfType<AICharacter.CharacterInfo>();
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].name == "Lizzy Parker")
            {
                Debug.Log(characters[i].name);
                foreach (var anim in characters[i].transform.parent.GetComponentsInChildren<Animator>())
                {
                    animator = anim;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //if (SceneManager.GetActiveScene().name != "B11" && SceneManager.GetActiveScene().name != "B21")
        //{
        //    gameObject.SetActive(false);
        //    buttonTab.gameObject.SetActive(false);
        //    return;
        //}
        if (SceneManager.GetActiveScene().name == "B21")
        {
            buttonsPatients[^1].gameObject.SetActive(false);
        }

        for (int i = 0; i < buttonsPatients.Length; i++)
        {
            int index = i;
            buttonsPatients[i].onClick.AddListener(()=> SelectPatient(index));
        }

        audioSource = GetComponent<AudioSource>();
        buttonTab.onClick.AddListener(OpenTab);
        buttonDone.onClick.AddListener(ButtonDone);

    }

    void OpenTab() 
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    void SelectPatient(int index) 
    {
        namePatient.text = buttonsPatients[index].transform.GetChild(1).GetComponent<TMP_Text>().text.Remove(0, 6);
        chooseIndex = index;
    }
    void ButtonDone() 
    {
        Debug.Log("ButtonChoose");

        if (namePatient.text != "Lizzy Parker") //if uncorrect
        {
            Debug.Log("Not lizzy chosen");
            if (SceneManager.GetActiveScene().name == "B11")
                SceneManager.LoadScene("B11");
            if (SceneManager.GetActiveScene().name == "B21")
                SceneManager.LoadScene("B21");
            buttonsPatients[chooseIndex].interactable = false;
            audioSource.PlayOneShot(uncorrect);
            buttonsBackground[chooseIndex].SetActive(true);
        }
        else //if correct
        {
            Debug.Log("lizzy chosen");
            cr.Complete();
            audioSource.PlayOneShot(correct);
            if (animator) 
            {
                animator.applyRootMotion = true;
                //animator.Rebind();
                animator.SetTrigger("StandUp2");
            }
            if (SceneManager.GetActiveScene().name == "B1_Pain_Assessment")
            {
                FindObjectOfType<WelcomeScreenController>(true).OpenEndPanel();
            }
            else
            {
                SceneManager.LoadScene("Lobby");
                Debug.Log("lizzy chosen 2");
            }
        }
    
    }
   
}
