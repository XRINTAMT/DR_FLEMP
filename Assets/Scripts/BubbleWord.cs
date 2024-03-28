using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BubbleWord : MonoBehaviour
{
    public int id;
    public bool latin;
    [SerializeField] Text text;

    private BubbleSpawner bubbleSpawner;

    void Start()
    {
        bubbleSpawner = FindObjectOfType<BubbleSpawner>(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!latin)
            return;
        BubbleWord bubbleWord = collision.gameObject.GetComponent<BubbleWord>();
        Debug.Log("Collided with" + collision.gameObject.name);
        if (bubbleWord != null)
        {
            bubbleSpawner.OnBubbleMatch(this, bubbleWord);
        }
    }

    public void Init(string word)
    {
        text.text = word;
    }
}
