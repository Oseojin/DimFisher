using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Camera cam;

    public WaterSurface water;

    WaterSearchParameters Search;
    WaterSearchResult SearchResult;

    [Header("Rotate")]
    public float mouseSpeed;
    float yRotation;
    float xRotation;

    [Header("Move")]
    [SerializeField]
    Vector3 moveVec;
    public float moveSpeed;
    float h, v;

    [Header("Jump")]
    public float jumpForce;

    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        cam = gameObject.GetComponentInChildren<Camera>();
        water = GameObject.Find("Ocean").GetComponent<WaterSurface>();
    }

    void Update()
    {
        Jump();
        Rotate();
        Move();
    }

    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Search.startPositionWS = transform.position;

        water.ProjectPointOnWaterSurface(Search, out SearchResult);

        if (transform.position.y < SearchResult.projectedPositionWS.y)
        {
            moveVec = h * transform.right * moveSpeed * 0.5f + v * transform.forward * moveSpeed * 0.5f + rb.velocity.y * Vector3.up;
        }
        else
        {
            moveVec = h * transform.right * moveSpeed + v * transform.forward * moveSpeed + rb.velocity.y * Vector3.up;
        }

        rb.velocity = moveVec;
    }

    void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void Jump()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);

        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
