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
    [SerializeField] private UnityEvent OnQuizCompleted;
    [SerializeField] private List<int> ItemIndices;
    [SerializeField] private int CorrectItemIndex;

    private void Start()
    {
        InitializeItemIndices();
        NewRandomItem();
    }

    private void InitializeItemIndices()
    {
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

    private void NewRandomItem()
    {
        if (ItemIndices.Count > 0)
        {
            CorrectItemIndex = ItemIndices[Random.Range(0, ItemIndices.Count)];
            PhrasesPlayer.PlayTag("Find_" + CorrectItemIndex.ToString());
        }
        else
        {
            OnQuizCompleted.Invoke();
        }
    }

    public void ItemGrabbed(Grabbable _item)
    {
        if (_item != Items[CorrectItemIndex])
        {
            PhrasesPlayer.PlayTag("Wrong_" + CorrectItemIndex.ToString());
        }
        else
        {
            PhrasesPlayer.PlayTag("Good");
            ItemIndices.Remove(CorrectItemIndex);
        }
        Invoke("NewRandomItem", 5);
    }
}
