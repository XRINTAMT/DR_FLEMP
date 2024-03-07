using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
 public class Item
 {   
    public GameObject item;
    public string titleEnglish;
    public string titleGerman;
    public string functionEnglish;
    public string functionGerman;
    //public string keyTitle;
    //public string keyFunction;
    public AudioClip titleAudioEnglish;
    public AudioClip titleAudioGerman;
}

public class ArObjectsPool : MonoBehaviour
{
    public Item [] items;
    int[] langugeRaw = new int[] { 0, 1, 2 };

    private void Start()
    {
        Parse();
    }
    void Parse()
    {
        CSVParser cSVParserItems = new CSVParser("Scenarios/" + "AR1" + "/ArItems");
        CSVParser cSVParserFunctions = new CSVParser("Scenarios/" + "AR1" + "/ArFunctions");

        for (int i = 1; i < cSVParserItems.rowData.Count; i++)
        {
            items[i-1].titleEnglish= cSVParserItems.rowData[i][langugeRaw[1]];
            items[i-1].titleGerman = cSVParserItems.rowData[i][langugeRaw[2]];
        }
        for (int i = 1; i < cSVParserFunctions.rowData.Count; i++)
        {
            items[i-1].functionEnglish = cSVParserFunctions.rowData[i][langugeRaw[1]];
            items[i-1].functionGerman = cSVParserFunctions.rowData[i][langugeRaw[2]];
        }
    }
}
