using UnityEngine;

public class TableItemSpawner : MonoBehaviour
{
    public GameObject[] itemsToSpawn; 
    public InstScript table; 

    public void SpawnItems()
    {
        if (table == null || itemsToSpawn == null || itemsToSpawn.Length == 0)
        {
            Debug.LogWarning("Table or items to spawn not assigned.");
            return;
        }

        //Shuffle(itemsToSpawn);

        int itemCount = itemsToSpawn.Length;
        Debug.Log(itemCount);

        Vector3 tableCenter = (table.pointLeft.transform.position + table.pointRight.transform.position) / 2;
        float tableWidth = Mathf.Abs(table.pointRight.transform.position.x - table.pointLeft.transform.position.x);
        float tableLength = Mathf.Abs(table.pointRight.transform.position.z - table.pointLeft.transform.position.z);

        float aspectRatio = tableWidth / tableLength;

        int rows = Mathf.CeilToInt(Mathf.Sqrt(itemCount / aspectRatio));
        int columns = Mathf.CeilToInt((float)itemCount / rows);
        Debug.Log($"{rows}:{columns}");

        float spacingX = tableWidth / columns;
        float spacingZ = tableLength / rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                if (index >= itemCount) return; 
                float xPos = tableCenter.x - tableWidth / 2 + j * spacingX + spacingX / 2;
                float zPos = tableCenter.z - tableLength / 2 + i * spacingZ + spacingZ / 2;
                Vector3 itemPosition = new Vector3(xPos, tableCenter.y + 0.05f, zPos);
                GameObject newItem = Instantiate(itemsToSpawn[index], itemPosition, Quaternion.identity);
                newItem.transform.SetParent(transform);
            }
        }
    }

    void Shuffle(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
