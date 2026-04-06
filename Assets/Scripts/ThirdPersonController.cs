using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public Vector2 lookInput;
    public CinemachineCamera tpCamera;
    

    public InputSystem_Actions inputs;
    private CharacterController controller;



    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public float verticalVelocity = 0;
    public float jumpForce = 10;

    public float pushForce = 4;

    private bool IsDashing;
    public float dashForce;
    public float dashDuration = 0.2f;
    private float dashTimer;




    [SerializeField] private Vector2 moveInput;




    private void Awake()
    {
        inputs = new();
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;


        inputs.Player.Jump.performed += OnJump;

        inputs.Player.Sprint.performed += OnDash;

        inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputs.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        //tpCamera.
    }
    void Start()
    {

    }
    void Update()
    {

        OnMove();
        //OnSimpleMove();
    }

    public void OnMove()
    {
      

     //   transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);

        Vector3 camForwardDir = tpCamera.transform.forward;
        camForwardDir.y = 0;   
        camForwardDir.Normalize();


        if(moveInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForwardDir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
                );

        }
        //Vector3 moveDir = camForwardDir * moveSpeed * moveInput.y;
        Vector3 moveDir = (camForwardDir * moveInput.y + transform.right * moveInput.x) * moveSpeed;



        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;


        moveDir.y = verticalVelocity;

        if (IsDashing)
        {
            //->convertir el dash a un barrido por el piso! dash con gravedad integrada omaegoto!
            moveDir = transform.forward * dashForce * (dashTimer / dashDuration);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
                IsDashing = false;
        }




        controller.Move(moveDir * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded) return;

        verticalVelocity = jumpForce;
    }
    public void OnSimpleMove()
    {
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        Vector3 moveDir = transform.forward * moveSpeed * moveInput.y;
        controller.SimpleMove(moveDir);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {


        Vector3 pushDir = (hit.transform.position - transform.position).normalized;

        if (hit.rigidbody != null && hit.rigidbody.linearVelocity == Vector3.zero)
        {
            print(hit.gameObject.name);
            hit.rigidbody.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        IsDashing = true;
        dashTimer = dashDuration;
    }
}

