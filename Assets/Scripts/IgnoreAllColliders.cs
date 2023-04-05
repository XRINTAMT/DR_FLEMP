using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreAllColliders : MonoBehaviour
{

    public List<Collider> Exceptions;

    // Start is called before the first frame update
    void Start()
    {
        if(Exceptions == null)
        {
            Exceptions = new List<Collider>();
        }
        Collider thisCol = GetComponent<Collider>();
        Collider[] CollidersToIgnore = FindObjectsOfType<Collider>();
        if (CollidersToIgnore != null)
        {
            foreach (Collider col in CollidersToIgnore)
            {
                if (col && col.enabled && !Exceptions.Contains(col))
                {
                    Physics.IgnoreCollision(thisCol, col, true);
                }
            }
        }
    }
}
