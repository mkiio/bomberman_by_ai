using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float spacing = 1.0f;
    public float moveDuration = 1.0f; // the duration of the movement animation
    public GameObject bombPrefab;

    private Animator anim;
    private Vector3 movement;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private int currentX;
    private int currentY;
    private float startTime; // the time the movement animation started

    bool hasStartedMoving = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isMoving)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            if (moveX != 0 || moveZ != 0)
            {
                if (!hasStartedMoving)
                {
                    int targetX = currentX + Mathf.RoundToInt(moveX);
                    int targetY = currentY + Mathf.RoundToInt(moveZ);

                    if (IsTargetPositionTraversable(targetX, targetY))
                    {
                        movement = new Vector3(moveX, 0, moveZ).normalized;

                        if (anim != null)
                        {
                            anim.SetFloat("MoveX", moveX);
                            anim.SetFloat("MoveZ", moveZ);
                            anim.SetFloat("Speed", movement.magnitude);
                        }

                        targetPosition = transform.position + movement * spacing;

                        isMoving = true;
                        startTime = Time.time;

                        currentX = targetX;
                        currentY = targetY;
                        hasStartedMoving = true;
                    }
                }
            }
            else
            {
                hasStartedMoving = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                int x = GetCurrentMapPositionX();
                int y = GetCurrentMapPositionY();

                if (IsTargetPositionTraversable(x, y))
                {
                    Instantiate(bombPrefab, new Vector3(x * spacing, 0.0f, y * spacing), Quaternion.identity);
                }
            }
        }
        else
        {
            float elapsedTime = Time.time - (Time.time - startTime);
            float t = elapsedTime / moveDuration;

            if (t >= 1.0f)
            {
                transform.position = targetPosition;

                movement = Vector3.zero;
                isMoving = false;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            }
        }
    }


    bool IsTargetPositionTraversable(int x, int y)
    {
        return x >= 0 && x < GridInstantiator.instance.map.GetLength(0) && y >= 0 && y < GridInstantiator.instance.map.GetLength(1)
            && GridInstantiator.instance.map[x, y] == BlockType.Traversable;
    }

    int GetCurrentMapPositionX()
    {
        return Mathf.RoundToInt(transform.position.x / spacing);
    }

    int GetCurrentMapPositionY()
    {
        return Mathf.RoundToInt(transform.position.z / spacing);
    }
}
