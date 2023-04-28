using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // The movement speed of the player
    public float moveDuration = 0.5f; // The duration of the movement animation
    // public BlockType[,] map; // The game map
    public float spacing = 1.0f; // The spacing between each block in the map
    private Animator anim; // The animator component of the player
    private Vector3 movement; // The movement input vector of the player
    private bool isMoving = false; // Flag to indicate if the player is currently moving
    private Vector3 targetPosition; // The target position of the player when moving
    private float startTime; // The start time of the movement animation
    private int currentX; // The current x-coordinate of the player in the map
    private int currentY; // The current y-coordinate of the player in the map
 GridInstantiator gridInstantiator;
    void Start()
    {
        anim = GetComponent<Animator>();    
        gridInstantiator = FindObjectOfType<GridInstantiator>();
    }

    void Update()
    {
        // Check if the player is currently moving
        if (!isMoving)
        {
            // Get the horizontal and vertical movement input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            // Check if the player pressed an arrow key
            if (moveX != 0 || moveZ != 0)
            {
                // Calculate the target position of the player
                int targetX = currentX + Mathf.RoundToInt(moveX);
                int targetY = currentY + Mathf.RoundToInt(moveZ);

                // Check if the target position is within the bounds of the gridInstantiator.map
                if (targetX >= 0 && targetX < gridInstantiator.map.GetLength(0) && targetY >= 0 && targetY < gridInstantiator.map.GetLength(1))
                {
                    // Check if the target position is traversable
                    if (gridInstantiator.map[targetX, targetY] == BlockType.Traversable)
                    {
                        // Set the movement vector based on the input
                        movement = new Vector3(moveX, 0, moveZ).normalized;

                        // Set the animator parameters based on the movement input
                        if (anim != null)
                        {
                            anim.SetFloat("MoveX", moveX);
                            anim.SetFloat("MoveZ", moveZ);
                            anim.SetFloat("Speed", movement.magnitude);
                        }

                        // Calculate the target position of the player
                        targetPosition = transform.position + movement * spacing;

                        // Start the movement animation
                        isMoving = true;
                        startTime = Time.time;

                        // Update the current position of the player in the map
                        currentX = targetX;
                        currentY = targetY;
                    }
                }
            }
        }
    else
    {
        // Calculate the elapsed time since the movement animation started
        float elapsedTime = Time.time - startTime;

        // Check if the movement animation is complete
        if (elapsedTime >= moveDuration)
        {
            // Move the player to the target position
            transform.position = targetPosition;

            // Reset the movement variables
            movement = Vector3.zero;
            isMoving = false;
        }
        else
        {
            // Calculate the new position of the player based on the elapsed time
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        }
    }
    }
}