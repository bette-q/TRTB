using UnityEngine;

public class MainPlayerController: MonoBehaviour
{
    public float moveSpeed = 4f;
    public bool rotateTowardsMovement = true;

    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Get raw input
        float h = Input.GetAxisRaw("Horizontal"); // A/D (-1 to 1)
        float v = Input.GetAxisRaw("Vertical");   // W/S (-1 to 1)

        // Create movement vector in world space
        moveDirection = new Vector3(h, 0, v).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Apply movement
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Optional: rotate character to face movement direction
            if (rotateTowardsMovement)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
            }
        }
    }
}
