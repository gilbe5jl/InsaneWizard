using UnityEngine;
using Cinemachine;

public class UFOController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float forwardSpeed = 10f;
    public float strafeSpeed = 10f;
    public float verticalSpeed = 5f;

    [Header("References")]
    public Transform player;
    public CinemachineVirtualCamera ufoCam;
    public CinemachineVirtualCamera playerCam;
    public Transform bottomPoint; // 👈 Drag your BottomGun object here

    [Header("Movement Limits")]
    [Tooltip("Minimum clearance above terrain")]
    public float groundClearance = 1.0f;

    private bool isDriving = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDriving = true;

            if (player != null)
                player.gameObject.SetActive(false);

            if (ufoCam != null) ufoCam.Priority = 20;
            if (playerCam != null) playerCam.Priority = 10;
        }
    }

    void Update()
    {
        if (!isDriving) return;

        // Input
        float h = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0f;
        if (Input.GetKey(KeyCode.Space)) y = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) y = -1f;

        // Move
        Vector3 move = new Vector3(
            h * strafeSpeed,
            y * verticalSpeed,
            z * forwardSpeed
        );
        transform.Translate(move * Time.deltaTime, Space.Self);

        // Raycast from the BottomGun (bottomPoint) downward
        if (bottomPoint != null && Physics.Raycast(bottomPoint.position, Vector3.down, out RaycastHit hit, 100f))
        {
            float minY = hit.point.y + groundClearance;
            if (bottomPoint.position.y < minY)
            {
                float deltaY = minY - bottomPoint.position.y;
                transform.position += new Vector3(0f, deltaY, 0f);
            }
        }

        // Exit UFO
        if (Input.GetKeyDown(KeyCode.V))
        {
            isDriving = false;

            if (player != null)
                player.gameObject.SetActive(true);

            if (ufoCam != null) ufoCam.Priority = 10;
            if (playerCam != null) playerCam.Priority = 20;
        }
    }
}