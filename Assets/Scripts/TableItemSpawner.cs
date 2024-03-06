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
        int itemCount = itemsToSpawn.Length;
        Debug.Log(itemCount);

        Vector3 direction = table.pointRight.transform.position - table.pointLeft.transform.position;
        Vector3 rotatedDirection = Quaternion.Euler(0, table.angle, 0) * direction;
        float tableWidth = Mathf.Abs(rotatedDirection.x);
        float tableLength = Mathf.Abs(rotatedDirection.z);

        float aspectRatio = tableWidth / tableLength;

        int rows = Mathf.CeilToInt(Mathf.Sqrt(itemCount / aspectRatio));
        int columns = Mathf.CeilToInt((float)itemCount / rows);
        Debug.Log($"{rows}:{columns}");

        float spacingX = tableWidth / columns;
        float spacingZ = tableLength / rows;

        Vector3 tableCenter = (table.pointLeft.transform.position + table.pointRight.transform.position) / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = i * columns + j;
                if (index >= itemCount) return;
                float xOffset = (j - (columns - 1) / 2f) * spacingX;
                float zOffset = (i - (rows - 1) / 2f) * spacingZ;
                Vector3 offset = Quaternion.Euler(0, -table.angle, 0) * new Vector3(xOffset, 0, zOffset);
                Vector3 spawnPosition = tableCenter + offset;
                spawnPosition.y = tableCenter.y + 0.02f; // Adjust height
                GameObject newItem = Instantiate(itemsToSpawn[index], spawnPosition, Quaternion.identity);
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
