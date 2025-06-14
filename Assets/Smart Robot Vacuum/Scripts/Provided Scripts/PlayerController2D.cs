using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    // // Public variables
    // public float speed = 5f; // The speed at which the player moves
    // public bool canMoveDiagonally = true; // Controls whether the player can move diagonally

    // // Private variables 
    // private Rigidbody2D rb; // Reference to the Rigidbody2D component attached to the player
    // private Vector2 movement; // Stores the direction of player movement
    // private bool isMovingHorizontally = true; // Flag to track if the player is moving horizontally

    // private float horizontalInput = 1; // Stores horizontal input value
    // private float verticalInput = 1; // Stores vertical input value

    // void Start()
    // {
    //     // Initialize the Rigidbody2D component
    //     rb = GetComponent<Rigidbody2D>();
    //     // Prevent the player from rotating
    //     rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    // }

    // void Update()
    // {
    //     movement = new Vector2(horizontalInput, verticalInput);
    //     // Auto rotate when touch the wall or the funitures
    //     RotatePlayer(movement.x, movement.y);



    //     // // Check if diagonal movement is allowed
    //     // if (canMoveDiagonally)
    //     // {
    //     //     // Set movement direction based on input
    //     //     movement = new Vector2(horizontalInput, verticalInput);
    //     //     // Optionally rotate the player based on movement direction
    //     //     RotatePlayer(horizontalInput, verticalInput);
    //     // }
    //     // else
    //     // {
    //     //     // Determine the priority of movement based on input
    //     //     if (horizontalInput != 0)
    //     //     {
    //     //         isMovingHorizontally = true;
    //     //     }
    //     //     else if (verticalInput != 0)
    //     //     {
    //     //         isMovingHorizontally = false;
    //     //     }

    //     //     // Set movement direction and optionally rotate the player
    //     //     if (isMovingHorizontally)
    //     //     {
    //     //         movement = new Vector2(horizontalInput, 0);
    //     //         RotatePlayer(horizontalInput, 0);
    //     //     }
    //     //     else
    //     //     {
    //     //         movement = new Vector2(0, verticalInput);
    //     //         RotatePlayer(0, verticalInput);
    //     //     }
    //     // }
    // }

    // void FixedUpdate()
    // {
    //     // Apply movement to the player in FixedUpdate for physics consistency
    //     rb.linearVelocity = movement * speed;
    // }

    // void RotatePlayer(float x, float y)
    // {
    //     // If there is no input, do not rotate the player
    //     if (x == 0 && y == 0) return;

    //     // Calculate the rotation angle based on input direction
    //     float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    //     // Apply the rotation to the player
    //     transform.rotation = Quaternion.Euler(0, 0, angle);
    // }

    public float speed = 5f; // The speed at which the player moves
    public bool canMoveDiagonally = true; // Controls whether the player can move diagonally

    private Rigidbody2D rb; // Reference to the Rigidbody2D component attached to the player
    private Vector2 movement; // Stores the direction of player movement
    private Vector2 lastVelocity; // Lưu hướng di chuyển cuối cùng để phản xạ

    private float horizontalInput = 1;
    private float verticalInput = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Khởi tạo hướng ban đầu
        movement = new Vector2(horizontalInput, verticalInput).normalized;
        rb.linearVelocity = movement * speed;
    }

    void Update()
    {
        // Chỉ cập nhật hướng nếu bạn muốn điều khiển bằng input thực tế
        // Còn nếu chỉ di chuyển tự động và phản chiếu thì bỏ phần này

        // Lưu lại hướng di chuyển để dùng trong phản chiếu
        lastVelocity = rb.linearVelocity;

        RotatePlayer(rb.linearVelocity.x, rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // Duy trì tốc độ luôn đều
        rb.linearVelocity = movement * speed;
    }

    void RotatePlayer(float x, float y)
    {
        if (x == 0 && y == 0) return;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Lấy normal của điểm va chạm đầu tiên
        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        // Phản xạ theo vector tốc độ gần nhất
        Vector2 reflectDir = Vector2.Reflect(lastVelocity.normalized, normal);
        movement = reflectDir;

        // Cập nhật rotation theo hướng phản xạ
        RotatePlayer(movement.x, movement.y);

        // Reset velocity để không bị giật
        rb.linearVelocity = movement * speed;
    }
}
