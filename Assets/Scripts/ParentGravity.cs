using UnityEngine;

public class ParentGravity : MonoBehaviour
{
    public float gravitationalConstant = 5; 
    private Rigidbody rb;
    Transform Parent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Parent = transform.parent;
        transform.SetParent(null);
    }

    void FixedUpdate()
    {
        Vector3 direction = (Parent.position - transform.position);
        float distanceSquared = direction.sqrMagnitude;
        if (distanceSquared > 0.005f)
        {
            rb.AddForce(direction.normalized * gravitationalConstant * (distanceSquared), ForceMode.Acceleration);
        }
    }
}
