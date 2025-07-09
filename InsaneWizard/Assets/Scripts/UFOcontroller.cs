using UnityEngine;
using Cinemachine;
using System.Collections;

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
    public Transform bottomPoint; // assign "BottomGun" here

    [Header("Movement Limits")]
    public float groundClearance = 1.0f;

    private bool isDriving = false;
    private Vector3 entryPosition;

    void Start()
    {
        entryPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDriving = true;

            if (player != null)
                player.gameObject.SetActive(false);

            if (ufoCam != null) ufoCam.Priority = 20;
            if (playerCam != null) playerCam.Priority = 10;

            entryPosition = transform.position;
        }
    }

    void Update()
    {
        if (!isDriving) return;

        float h = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S
        float y = 0f;
        if (Input.GetKey(KeyCode.Space)) y = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) y = -1f;

        Vector3 move = new Vector3(
            h * strafeSpeed,
            y * verticalSpeed,
            z * forwardSpeed
        );

        transform.Translate(move * Time.deltaTime, Space.Self);

        // Maintain minimum clearance from ground
        if (bottomPoint != null && Physics.Raycast(bottomPoint.position, Vector3.down, out RaycastHit hit, 100f))
        {
            float minY = hit.point.y + groundClearance;
            if (bottomPoint.position.y < minY)
            {
                float deltaY = minY - bottomPoint.position.y;
                transform.position += new Vector3(0f, deltaY, 0f);
            }
        }

        // Exit craft
        if (Input.GetKeyDown(KeyCode.V))
        {
            isDriving = false;

            if (player != null)
            {
                // Raycast down from UFO to place player safely
                Vector3 dropPoint = transform.position;

                if (bottomPoint != null && Physics.Raycast(bottomPoint.position, Vector3.down, out RaycastHit groundHit, 100f))
                {
                    dropPoint = groundHit.point + Vector3.up * 0.1f;
                }

                player.position = dropPoint;
                player.gameObject.SetActive(true);

                // Reset movement/velocity
                var cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.Move(Vector3.zero);

                var input = player.GetComponent<StarterAssets.StarterAssetsInputs>();
                if (input != null) input.MoveInput(Vector2.zero);
            }

            if (ufoCam != null) ufoCam.Priority = 10;
            if (playerCam != null) playerCam.Priority = 20;

            // Reset UFO to entry point
            transform.position = entryPosition;
        }
    }
}