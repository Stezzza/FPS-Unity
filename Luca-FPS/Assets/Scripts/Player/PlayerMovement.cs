using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float JumpHeight = 2f;
    //to increase gravity speed when falling
    public float fallGravityMultiplier = 2f;
    public float mouseSensitivity = 2.0f;
    public float pitchRange = 60.0f;

    private float forwardInputValue;
    private float strafeInputValue;
    private bool jumpInput;

    //physics flal velocity
    private float terminalVelocity = 53f;
    private float verticalVelocity;

    private float rotateCameraPitch;

    private Camera FirstPersonCam;

    public CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        FirstPersonCam = GetComponentInChildren<Camera>();   
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void JumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            //apply gravity over time if under terminal velocity
            if(verticalVelocity < terminalVelocity)
            {
                //set gravity multiplier if falling downwards.
                float gravityMultiplier = 1;
                if(characterController.velocity.y < -1)
                {
                    gravityMultiplier = fallGravityMultiplier;
                }
                verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            }
        }
    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        strafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        Movement();
        JumpAndGravity();
        CameraMovement();
    }
    void CameraMovement()
    {
        //rotate the player around
        float rotateYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotateYaw, 0);

        //rotate the camera up/down
        rotateCameraPitch += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        //lock the rotation so we cannot look backwards/flip
        rotateCameraPitch = Mathf.Clamp(rotateCameraPitch, -pitchRange, pitchRange);
        FirstPersonCam.transform.localRotation = Quaternion.Euler(rotateCameraPitch, 0, 0);
    }
    void Movement()
    {
        Vector3 direction = (transform.forward * forwardInputValue + transform.right * strafeInputValue).normalized * movementSpeed * Time.deltaTime;
        characterController.Move(direction);

        //add physics usnig vector3s up direction (world coordinates) as the direction of gravity.
        direction += Vector3.up * verticalVelocity * Time.deltaTime;

        characterController.Move(direction);
    }
}
