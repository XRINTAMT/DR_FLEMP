using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR

[ExecuteAlways]
public class EditorDownlodArText : MonoBehaviour
{
    public ArObjectsPool arObjectsPool;
    public bool parse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (parse)
        {
            arObjectsPool.Parse();
            parse = false;
        }
    }
}
#endif