using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunScript : MonoBehaviour
{
    [SerializeField] GameObject Gun;

    Animator animator;
    InputSystem_Actions playerInputActions;
    public AudioSource fireSound;
    public bool cooling = false;
    public float GunCooldownTime = 3f;
    public float CameraShakeDuration = 0.2f;
    public CameraShake CameraShake;
    public Camera camera;
    void Awake()
    {
        CameraShake = camera.GetComponent<CameraShake>();
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Attack.performed += attackPerformed;
        playerInputActions.Player.Reload.performed += ReloadPerformed;
        playerInputActions.Player.Inspect.performed += InspectPerformed;
        playerInputActions.Player.Emote.performed += EmotePerformed;
    }
    private void Start()
    {
        animator = Gun.GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    private void attackPerformed(InputAction.CallbackContext context){
        if (!cooling){
            fire();
        }
    }
    private void InspectPerformed(InputAction.CallbackContext context){ 
        animator.SetBool("inspecting", true);
    }
    private void EmotePerformed(InputAction.CallbackContext context){ 
        animator.SetBool("emoting", true);
    }
    private void ReloadPerformed(InputAction.CallbackContext context){ 

    }
    public IEnumerator fireCooldown()
    {
        yield return new WaitForSeconds(GunCooldownTime);
        cooling = false;
    }
    public void fire(){
        fireSound.Play();
        animator.SetBool("firing", true);
        CameraShake.shakeDuration = CameraShakeDuration;
        cooling = true;
        //coming soon

        StartCoroutine(fireCooldown());
    }
}
