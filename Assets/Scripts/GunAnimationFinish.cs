using UnityEngine;

public class GunAnimationFinish : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void FinishEmoting(){ 
        animator.SetBool("emoting", false);
    }
    public void FinishFiring(){
        animator.SetBool("firing", false);
    }
    public void FinishInspecting(){
        animator.SetBool("inspecting", false);
    }
}
