using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;
    public LightPlayerCombat playerCombat;

    float horizontalMove = 0f;
    public float runSpeed = 40f;
    public bool jump = false;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            jump = true;
        }
        if (playerCombat.combatMode)
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
