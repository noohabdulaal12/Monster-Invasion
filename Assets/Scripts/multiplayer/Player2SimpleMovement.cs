using UnityEngine;

public class Player2SimpleMovement : MonoBehaviour
{
    public int playerNumber = 1;

    public float moveSpeed = 5f;
    public float turnSpeed = 90f;
    public float lookSpeed = 60f;

    public CharacterController controller;
    public Transform playerCamera;

    private float cameraPitch = 0f;

    void Update()
    {
        float x = 0f;
        float z = 0f;

        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.A)) x = -1f;
            if (Input.GetKey(KeyCode.D)) x = 1f;
            if (Input.GetKey(KeyCode.W)) z = 1f;
            if (Input.GetKey(KeyCode.S)) z = -1f;

            if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.R)) cameraPitch -= lookSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.F)) cameraPitch += lookSpeed * Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) x = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) z = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) z = -1f;

            if (Input.GetKey(KeyCode.J)) transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.L)) transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.I)) cameraPitch -= lookSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.K)) cameraPitch += lookSpeed * Time.deltaTime;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (controller != null)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        cameraPitch = Mathf.Clamp(cameraPitch, -60f, 60f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }
}