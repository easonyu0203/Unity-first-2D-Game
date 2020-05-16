using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool combatMode = false;

    // Update is called once per frame
    void Update() { 
    
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            jump = true;
        }
        if (Input.GetButtonDown("Down"))
        {
                animator.SetBool("CombatMode", true);
            combatMode = true;
        }
        if (Input.GetButtonUp("Down"))
        {
            animator.SetBool("CombatMode", false);
            combatMode = false;
        }
        if (combatMode)
        {
            jump = false;
            horizontalMove = 0f;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

}
