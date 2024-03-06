using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    public GameObject item;
    [SerializeField] Transform head;
    [SerializeField] Canvas canvas;
    [SerializeField] Vector3 Offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(head.position.x, head.position.y, head.position.z);
        canvas.transform.LookAt(targetPosition);
        if (item && canvas.transform.position != item.transform.position + Offset) 
        {
            canvas.transform.position = item.transform.position + Offset;
            StartCoroutine(ActivateCanvas());
        }
    }
    IEnumerator ActivateCanvas() 
    {
        yield return new WaitForEndOfFrame();
        canvas.enabled = true;

    }
}
