using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputSystem_Actions playerInputActions;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    public float defMassValue = 1f;
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float maxGroundSpeed = 15f;
    public float slowDownRate = 2f;
    public float slopeSlowDownRate = 5f;
    public bool onGround = false;
    public bool sliding = false;
    public bool crouching = false;
    public bool canJump = false;
    private float defCharacterHeight;
    private float characterCrouchHeight;
    

    

    private void Awake(){
        defCharacterHeight = GetComponent<CapsuleCollider>().height;
        characterCrouchHeight = defCharacterHeight / 2;
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += jumpPerformed;
        playerInputActions.Player.Crouch.performed += crouchPerformed;
        playerInputActions.Player.Crouch.canceled += crouchCanceled;
        
    }
    private void FixedUpdate(){
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = transform.TransformDirection(new Vector3(inputVector.x, 0, inputVector.y));

        if (IsOnSlope() && onGround){
            Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDirection, GetSlopeNormal());
            playerRigidbody.AddForce(slopeDirection * moveSpeed, ForceMode.Force);
            Vector3 adjustedVelocity = Vector3.ProjectOnPlane(playerRigidbody.linearVelocity, GetSlopeNormal());
            playerRigidbody.linearVelocity = adjustedVelocity + slopeDirection * moveSpeed * Time.fixedDeltaTime;
        }
            playerRigidbody.AddForce(moveDirection * moveSpeed, ForceMode.Force);
  
        if (onGround && playerRigidbody.linearVelocity.magnitude > maxGroundSpeed){
            playerRigidbody.linearVelocity = Vector3.Lerp(playerRigidbody.linearVelocity, Vector3.ClampMagnitude(playerRigidbody.linearVelocity, maxGroundSpeed), Time.deltaTime * slowDownRate);
        }
        if (IsOnSlope() && onGround && inputVector == Vector2.zero){
            playerRigidbody.linearVelocity = Vector3.Lerp(playerRigidbody.linearVelocity,Vector3.zero,Time.fixedDeltaTime * slopeSlowDownRate);
        }
    }

    private void crouchPerformed(InputAction.CallbackContext context){
        crouching = true;
        characterSizeChanger('2');
    }
    private void crouchCanceled(InputAction.CallbackContext context){
        crouching = false;
        characterSizeChanger('1');
        if (sliding){
            sliding= false;
        }
    }
    private void jumpPerformed(InputAction.CallbackContext context){
        if (onGround){
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;

            if (crouching){
                characterSizeChanger('1');
                crouching = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            onGround = true;
        }
    }
    private bool IsOnSlope(){
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f)){
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > 0 && slopeAngle <= 45f; // 45 derece limit
        }
        return false;
    }
    private Vector3 GetSlopeNormal(){
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f)){
            return hit.normal;
        }
        return Vector3.up;
    }
    public void characterSizeChanger(char value){
        if (value=='1'){
            gameObject.GetComponent<CapsuleCollider>().height = defCharacterHeight;
            gameObject.transform.localPosition += new Vector3(0, 0.5f, 0);
            gameObject.GetComponent<Rigidbody>().mass = defMassValue;
        }
        if (value == '2'){
            gameObject.GetComponent<CapsuleCollider>().height = characterCrouchHeight;
            gameObject.transform.localPosition -= new Vector3(0, 0.5f, 0);
            gameObject.GetComponent<Rigidbody>().mass = defMassValue - (defMassValue / 2.5f);
        }
    }
}
