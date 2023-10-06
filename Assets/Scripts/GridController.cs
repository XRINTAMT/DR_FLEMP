using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridController : MonoBehaviour
{
    [SerializeField] List<RowSentence> row = new List<RowSentence>();
    //[SerializeField] List<Words> row =  new List<Words>();
    [SerializeField] RowSentence rowPrefab;
    [SerializeField] Transform content;
    [SerializeField] float contentWidth;
    GameObject checkObj;
    bool setPos;
    // Start is called before the first frame update
    void Start()
    {
        SetNewRow();
    }

    float WordsLenghts(float instObgWidth) 
    {
        float lenght = instObgWidth;

        for (int i = 0; i < row[row.Count - 1].word.Count; i++)
        {
            lenght = lenght + row[row.Count - 1].word[i].GetComponent<RectTransform>().rect.width;
        }
        return lenght;
    
    }
    public GameObject  Instantiate(GameObject obj, string text) 
    {
        checkObj = null;
        setPos = false;
        GameObject newObj=null;

        newObj = Instantiate(obj,transform);
        newObj.GetComponentInChildren<TextMeshProUGUI>().text = text;

        if (row[row.Count - 1].word.Count == 0)
        {
            //newObj=Instantiate(obj, row[row.Count - 1].spawnPoint.transform.position, Quaternion.identity, row[row.Count - 1].transform);
            newObj.transform.position = row[row.Count - 1].spawnPoint.transform.position;
            newObj.transform.parent = row[row.Count - 1].transform;
            row[row.Count - 1].word.Add(newObj.GetComponent<ButtonSentence>());

            return newObj;
        }
        if (row[row.Count - 1].word.Count != 0)
        {
            checkObj = newObj;
        }
      
        return newObj;
    }
    public void Remove(ButtonSentence obj)
    {

        for (int i = 0; i < row.Count; i++)
        {
            for (int j = 0; j < row[i].word.Count; j++)
            {
                if (row[i].word[j] == obj)
                {
                    row[i].word.RemoveAt(j);

                    if (j - 1 == row[i].word.Count-1 )
                    {
                        if (i != row.Count - 1)
                        {
                            UpdatePostionsToPrevRow(i);
                        }
                        return;
                    }
                    if (j - 1 < row[i].word.Count - 1)
                    {
                        if (i != row.Count-1)
                        {
                            UpdatePostionsToPrevRow(i);
                        }
                        if (i == row.Count - 1)
                        {
                            UpdatePostions();
                        }

                    }
                }

           

                //if ((j - 1) >= 0)
                //{
                //    row[i].word[j].transform.position = row[i].word[j - 1].spawnPoint.transform.position;
                //    return;
                //}
                //if ((j - 1) >= 0)
                //{
                //    row[i].word[j].transform.position = row[i].spawnPoint.transform.position;
                //    return;
                //}

            }
        }

        for (int i = 0; i < row.Count; i++)
        {
            if (row[i].word.Count==0)
            {
                Destroy(row[i].gameObject);
                row.RemoveAt(i);
            }
        }

    }

    void UpdatePostions() 
    {
        for (int i = 0; i < row.Count; i++)
        {
            for (int j = 0; j < row[i].word.Count; j++)
            {
                if (j == 0)
                {
                    row[i].word[j].transform.position = row[i].spawnPoint.transform.position;
                }
                else
                {
                    row[i].word[j].transform.position = row[i].word[j - 1].spawnPoint.transform.position;
                }

            }
        }
    }
    void UpdatePostionsToPrevRow(int rowIndex)
    {
        if (GetRowLenght(rowIndex) + row[rowIndex + 1].word[0].GetComponent<RectTransform>().rect.width<contentWidth)
        {
            row[rowIndex + 1].word[0].transform.parent = row[rowIndex].transform;
            row[rowIndex + 1].word[0].transform.position = row[rowIndex].word[row[rowIndex].word.Count - 1].spawnPoint.transform.position;
            row[rowIndex].word.Add(row[rowIndex + 1].word[0]);
            row[rowIndex + 1].word.RemoveAt(0);

            UpdatePostions();
        }
        else
        {
            UpdatePostions();
        }
    }

    float GetRowLenght(int indexRow) 
    {
        float lenght=0;
        for (int i = 0; i < row[indexRow].word.Count; i++)
        {
            lenght = lenght + row[row.Count - 1].word[i].GetComponent<RectTransform>().rect.width;
        }
        return lenght;
    }
    public void SetNewRow() 
    {
        row.Add(Instantiate(rowPrefab, content));
    }


    // Update is called once per frame
    void Update()
    {

        if (checkObj && checkObj.GetComponent<RectTransform>().rect.width>0 && !setPos)
        {
            if (contentWidth > WordsLenghts(checkObj.GetComponent<RectTransform>().rect.width))
            {
                //newObj=Instantiate(obj, row[row.Count - 1].word[row[row.Count - 1].word.Count - 1].spawnPoint.transform.position, Quaternion.identity, row[row.Count - 1].transform);
                checkObj.transform.position = row[row.Count - 1].word[row[row.Count - 1].word.Count - 1].spawnPoint.transform.position;
                checkObj.transform.parent = row[row.Count - 1].transform;
                row[row.Count - 1].word.Add(checkObj.GetComponent<ButtonSentence>());
                setPos = true;
                return;

            }
            if (contentWidth < WordsLenghts(checkObj.GetComponent<RectTransform>().rect.width))
            {
                SetNewRow();
                //newObj=Instantiate(obj, row[row.Count - 1].spawnPoint.transform.position, Quaternion.identity, row[row.Count - 1].transform);
                checkObj.transform.position = row[row.Count - 1].spawnPoint.transform.position;
                checkObj.transform.parent = row[row.Count - 1].transform;
                row[row.Count - 1].word.Add(checkObj.GetComponent<ButtonSentence>());
                setPos = true;
                return;

            }
        }


    }
}
