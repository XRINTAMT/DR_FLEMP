using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RecordedScenario;
using Autohand;
using UnityEngine.Events;
using System.Linq;

public class FindMeXQuiz : MonoBehaviour
{
    [SerializeField] private PrerecordedAudioOnDemand PhrasesPlayer;
    [SerializeField] private List<Grabbable> Items;
    [SerializeField] private Transform ItemsParent;
    [SerializeField] private UnityEvent OnQuizCompleted;
    [SerializeField] private List<int> ItemIndices;
    [SerializeField] private int CorrectItemIndex;
    [SerializeField] private bool DestroyOnGuess;
    private bool sleep = true;
    

    private void Start()
    {
        InitializeItemIndices();
    }

    public void LaunchScenario()
    {
        NewRandomItem();
    }

    private void InitializeItemIndices()
    {
        if (Items == null)
            return;

        if (Items.Count == 0)
            return;

        ItemIndices = Enumerable.Range(0, Items.Count).ToList();

        foreach (int index in ItemIndices.ToList())
        {
            if (Items[index] == null)
            {
                ItemIndices.Remove(index);
            }
            else
            {
                Items[index].onGrab.AddListener((_hand, _grabbable) => ItemGrabbed(_grabbable));
            }
        }
    }

    public void UpdateItemsFromParent()
    {
        Items.Clear();
        foreach (Transform child in ItemsParent)
        {
            Grabbable grabbable = child.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                Items.Add(grabbable);
            }
        }
        InitializeItemIndices();
    }

    private void NewRandomItem()
    {
        if (ItemIndices.Count > 0)
        {
            CorrectItemIndex = ItemIndices[Random.Range(0, ItemIndices.Count)];
            PhrasesPlayer.PlayTag("Find_" + CorrectItemIndex.ToString());
            Invoke("Wake", 2);
        }
        else
        {
            OnQuizCompleted.Invoke();
        }
    }

    private void Wake()
    {
        sleep = false;
    }

    public void ItemGrabbed(Grabbable _item)
    {
        if (sleep)
        {
            return;
        }
        sleep = true;
        if (_item != Items[CorrectItemIndex])
        {
            PhrasesPlayer.PlayTag("Wrong_" + CorrectItemIndex.ToString());
        }
        else
        {
            PhrasesPlayer.PlayTag("Good");
            ItemIndices.Remove(CorrectItemIndex);
            if (DestroyOnGuess)
            {
                _item.ForceHandsRelease();
                _item.gameObject.SetActive(false);
            }
        }
        Invoke("NewRandomItem", 5);
    }

    public void Skip() 
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].gameObject.SetActive(false);
        }
    }
}