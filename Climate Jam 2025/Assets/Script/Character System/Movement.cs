using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevents tipping over
    }

    void FixedUpdate() // Physics updates go here!
    {
        if (NotebookUIManager.IsOpen)
        {
            rb.linearVelocity = Vector3.zero; // Stop movement if notebook is open
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(h, 0, v).normalized * speed;

        rb.linearVelocity = move;
    }
}
