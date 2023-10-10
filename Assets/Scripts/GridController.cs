using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridController : MonoBehaviour
{
    public List<RowSentence> rowsCheck = new List<RowSentence>();

    public List<RowSentence> rowsSentence = new List<RowSentence>();
    //[SerializeField] List<Words> row =  new List<Words>();
    public List<RowSentence> rowsChoose = new List<RowSentence>();
    [SerializeField] RowSentence rowSentence;
    [SerializeField] RowSentence rowChoose;
    [SerializeField] Transform contentSentence;
    [SerializeField] Transform contentChoose;
    public float contentWidth;
    GameObject checkObj;
    bool setPos;
 
    // Start is called before the first frame update
    void Awake()
    {
        SetNewRow(rowSentence);
        SetNewRow(rowChoose);
    }

    public float WordsLenghts(float instObgWidth) 
    {
        float lenght = instObgWidth;

        for (int i = 0; i < rowsSentence[rowsSentence.Count - 1].word.Count; i++)
        {
            lenght = lenght + rowsSentence[rowsSentence.Count - 1].word[i].GetComponent<RectTransform>().rect.width;
        }
        return lenght;
    
    }
    public float WordsLenghts(float instObjWidth, RowSentence row)
    {
        float lenght = instObjWidth;
        for (int i = 0; i < row.word.Count; i++)
        {
            lenght = lenght + row.word[i].GetComponent<RectTransform>().rect.width;
        }

        return lenght;
    }
    public float WordsLenghts(ButtonSentence buttonSentence)
    {
        int indexRow=0;
        int indexWord=0;
        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].GetComponent<RowSentence>().word.Count; j++)
            {
                if (rowsSentence[i].GetComponent<RowSentence>().word[j]==buttonSentence)
                {
                    indexRow = i;
                    indexWord = j;
                }
            }
        }

        float lenght = 0;

        for (int i = 0; i < rowsSentence[indexRow].word.Count; i++)
        {
            lenght = lenght + rowsSentence[indexRow].word[i].GetComponent<RectTransform>().rect.width;
        }
        return lenght;

    }    
    
    float GetRowLenght(int indexRow) 
    {
        float lenght=0;
        for (int i = 0; i < rowsSentence[indexRow].word.Count; i++)
        {
            lenght = lenght + rowsSentence[rowsSentence.Count - 1].word[i].GetComponent<RectTransform>().rect.width;
        }
        return lenght;
    }

    float GetRowLenght(RowSentence row) 
    {
        float lenght = 0;
        for (int i = 0; i < row.word.Count; i++)
            lenght=lenght+ row.word[i].GetComponent<RectTransform>().rect.width;

        return lenght;
    }

    public float GetRowLenghts(ButtonSentence buttonSentence)
    {
        List<RowSentence> rowsList = new List<RowSentence>();
        int indexRow = 0;
        float lenght = 0;

        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].GetComponent<RowSentence>().word.Count; j++)
            {
                if (rowsSentence[i].GetComponent<RowSentence>().word[j] == buttonSentence)
                {
                    indexRow = i;
                    rowsList = rowsSentence;
                }
            }
        }
        for (int i = 0; i < rowsChoose.Count; i++)
        {
            for (int j = 0; j < rowsChoose[i].GetComponent<RowSentence>().word.Count; j++)
            {
                if (rowsChoose[i].GetComponent<RowSentence>().word[j] == buttonSentence)
                {
                    indexRow = i;
                    rowsList = rowsChoose;
                }
            }
        }

        for (int i = 0; i < rowsList[indexRow].word.Count; i++)
        {
            lenght = lenght + rowsList[indexRow].word[i].GetComponent<RectTransform>().rect.width;
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

        if (rowsSentence[rowsSentence.Count - 1].word.Count == 0)
        {
            //newObj=Instantiate(obj, row[row.Count - 1].spawnPoint.transform.position, Quaternion.identity, row[row.Count - 1].transform);
            newObj.transform.position = rowsSentence[rowsSentence.Count - 1].spawnPoint.transform.position;
            newObj.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
            rowsSentence[rowsSentence.Count - 1].word.Add(newObj.GetComponent<ButtonSentence>());

            return newObj;
        }
        if (rowsSentence[rowsSentence.Count - 1].word.Count != 0)
        {
            checkObj = newObj;
        }
      
        return newObj;
    }

    public GameObject InstantiateWordChoose(GameObject obj, string text)
    {
        RowSentence rowSentence = rowsChoose[rowsChoose.Count - 1];
        GameObject word = Instantiate(obj, rowSentence.spawnPoint.transform.parent);
        word.GetComponentInChildren<TextMeshProUGUI>().text = text;
        rowSentence.word.Add(word.GetComponent<ButtonSentence>());

        return word;
    }


    IEnumerator UpdaterowChooseCenter(GameObject word, RowSentence rowSentence) 
    {
        yield return new WaitForEndOfFrame();
        rowsChoose[rowsChoose.Count - 1].spawnPoint.transform.parent.transform.position = new Vector3(WordsLenghts(word.GetComponent<RectTransform>().rect.width, rowSentence) / 2, 0, 0);
    }
    public void Remove(ButtonSentence obj)
    {

        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].word.Count; j++)
            {
                if (rowsSentence[i].word[j] == obj)
                {
                    rowsSentence[i].word.RemoveAt(j);

                    if (j - 1 == rowsSentence[i].word.Count-1 )
                    {
                        if (i != rowsSentence.Count - 1)
                        {
                            UpdatePostionsToPrevRow(i);
                        }
                        return;
                    }
                    if (j - 1 < rowsSentence[i].word.Count - 1)
                    {
                        if (i != rowsSentence.Count-1)
                        {
                            UpdatePostionsToPrevRow(i);
                        }
                        if (i == rowsSentence.Count - 1)
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

        for (int i = 0; i < rowsSentence.Count; i++)
        {
            if (rowsSentence[i].word.Count==0)
            {
                Destroy(rowsSentence[i].gameObject);
                rowsSentence.RemoveAt(i);
            }
        }

    }

    public void UpdatePostions() 
    {
        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].word.Count; j++)
            {
                if (j == 0)
                {
                    rowsSentence[i].word[j].transform.position = rowsSentence[i].spawnPoint.transform.position;
                }
                else
                {
                    rowsSentence[i].word[j].transform.position = rowsSentence[i].word[j - 1].spawnPoint.transform.position;
                }

            }
        }
    }
    public void UpdatePostions(List<RowSentence> row)
    {
        for (int i = 0; i < row.Count; i++)
        {
            if (GetRowLenght(row[i]) < contentWidth)
            {
                for (int j = 0; j < row[i].word.Count; j++)
                {
                    if (j == 0)
                        row[i].word[j].transform.position = row[i].spawnPoint.transform.position;
                    else
                        row[i].word[j].transform.position = row[i].word[j - 1].spawnPoint.transform.position;

                }
            }
            if (GetRowLenght(row[i]) >= contentWidth)
            {
                SetNewRow(rowChoose);
                row[i].word[row[i].word.Count - 1].transform.parent = row[i+1].spawnPoint.transform.parent;
                row[i].word[row[i].word.Count - 1].transform.position = row[i+1].spawnPoint.transform.position;

                row[i + 1].word.Add(row[i].word[row[i].word.Count - 1]);
                row[i].word.RemoveAt(row[i].word.Count - 1);
            }


        }
    }

   

    public void UpdatePostionsRowChoose()
    {
        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].word.Count; j++)
            {
                if (j == 0)
                {
                    rowsSentence[i].word[j].transform.position = rowsSentence[i].spawnPoint.transform.position;
                }
                else
                {
                    rowsSentence[i].word[j].transform.position = rowsSentence[i].word[j - 1].spawnPoint.transform.position;
                }

            }
        }
    }
    void UpdatePostionsToPrevRow(int rowIndex)
    {
        if (GetRowLenght(rowIndex) + rowsSentence[rowIndex + 1].word[0].GetComponent<RectTransform>().rect.width<contentWidth)
        {
            rowsSentence[rowIndex + 1].word[0].transform.parent = rowsSentence[rowIndex].transform;
            rowsSentence[rowIndex + 1].word[0].transform.position = rowsSentence[rowIndex].word[rowsSentence[rowIndex].word.Count - 1].spawnPoint.transform.position;
            rowsSentence[rowIndex].word.Add(rowsSentence[rowIndex + 1].word[0]);
            rowsSentence[rowIndex + 1].word.RemoveAt(0);

            UpdatePostions();
        }
        else
        {
            UpdatePostions();
        }
    }


    public void SetNewRow() 
    {
        rowsSentence.Add(Instantiate(rowSentence, contentSentence));
    }
  

    public void SetNewRow(RowSentence row)
    {
        if (rowSentence.gameObject == row.gameObject)
            rowsSentence.Add(Instantiate(rowSentence, contentSentence));

        if (rowChoose.gameObject==row.gameObject)
            rowsChoose.Add(Instantiate(rowChoose, contentChoose));
      
    }
    IEnumerator UpdateInstantPos(GameObject word, RowSentence rowSentence, bool constructor)
    {
        yield return new WaitForEndOfFrame();
        if (constructor)
        {
            if (contentWidth > WordsLenghts(word.GetComponent<RectTransform>().rect.width))
            {
                word.transform.position = rowsSentence[rowsSentence.Count - 1].word[rowsSentence[rowsSentence.Count - 1].word.Count - 1].spawnPoint.transform.position;
                word.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
                rowsSentence[rowsSentence.Count - 1].word.Add(word.GetComponent<ButtonSentence>());

            }
            if (contentWidth < WordsLenghts(word.GetComponent<RectTransform>().rect.width))
            {
                SetNewRow();
                word.transform.position = rowsSentence[rowsSentence.Count - 1].spawnPoint.transform.position;
                word.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
                rowsSentence[rowsSentence.Count - 1].word.Add(word.GetComponent<ButtonSentence>());
            }
        }
        if (!constructor)
        {
            word.transform.position = rowSentence.word[rowSentence.word.Count - 1].spawnPoint.transform.position;
            word.transform.parent = rowSentence.spawnPoint.transform.parent;
            rowSentence.word.Add(word.GetComponent<ButtonSentence>());

            //if (contentWidth > WordsLenghts(word.GetComponent<RectTransform>().rect.width, rowSentence))
            //{
            //    word.transform.position = rowSentence.word[rowSentence.word.Count - 1].spawnPoint.transform.position;
            //    word.transform.parent = rowSentence.spawnPoint.transform.parent;
            //    rowSentence.word.Add(word.GetComponent<ButtonSentence>());

            //}
            //if (contentWidth < WordsLenghts(word.GetComponent<RectTransform>().rect.width,rowSentence))
            //{
            //    SetNewRow();
            //    word.transform.position = rowChoose[rowChoose.Count - 1].spawnPoint.transform.position;
            //    word.transform.parent = rowChoose[rowChoose.Count - 1].spawnPoint.transform.parent;
            //    rowChoose[rowChoose.Count - 1].word.Add(word.GetComponent<ButtonSentence>());
            //}

            //rowChoose[rowChoose.Count - 1].spawnPoint.transform.parent.transform.position = new Vector3(WordsLenghts(word.GetComponent<RectTransform>().rect.width, rowSentence) / 2, 0, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {


        if (checkObj && checkObj.GetComponent<RectTransform>().rect.width>0 && !setPos)
        {
            if (contentWidth > WordsLenghts(checkObj.GetComponent<RectTransform>().rect.width))
            {
                //newObj=Instantiate(obj, row[row.Count - 1].word[row[row.Count - 1].word.Count - 1].spawnPoint.transform.position, Quaternion.identity, row[row.Count - 1].transform);
                checkObj.transform.position = rowsSentence[rowsSentence.Count - 1].word[rowsSentence[rowsSentence.Count - 1].word.Count - 1].spawnPoint.transform.position;
                checkObj.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
                rowsSentence[rowsSentence.Count - 1].word.Add(checkObj.GetComponent<ButtonSentence>());
                setPos = true;
                return;

            }
            if (contentWidth < WordsLenghts(checkObj.GetComponent<RectTransform>().rect.width))
            {
                SetNewRow();
                //newObj=Instantiate(obj, row[row.Count - 1].spawnPoint.transform.position, Quaternion.identity, row[row.Count - 1].transform);
                checkObj.transform.position = rowsSentence[rowsSentence.Count - 1].spawnPoint.transform.position;
                checkObj.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
                rowsSentence[rowsSentence.Count - 1].word.Add(checkObj.GetComponent<ButtonSentence>());
                setPos = true;
                return;

            }
        }


    }
}
