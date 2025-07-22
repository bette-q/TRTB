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

    void FixedUpdate()
    {
        if (NotebookUIController.IsOpen || UIManager.Instance.IsDialogueActive)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        // Get the ground normal below the player
        RaycastHit hit;
        Vector3 move;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 1.5f))
        {
            // Project movement onto the ground (works for slopes/ramps)
            move = Vector3.ProjectOnPlane(input, hit.normal).normalized * speed;
        }
        else
        {
            move = input * speed;
        }

        // Preserve vertical velocity for gravity/jumping
        float yVel = rb.linearVelocity.y;
        move.y = yVel;

        // Apply gravity manually (since we set velocity directly)
        move.y += Physics.gravity.y * Time.fixedDeltaTime;

        rb.linearVelocity = move;
    }

}
