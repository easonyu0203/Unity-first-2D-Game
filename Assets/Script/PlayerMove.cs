using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{

    public Controller2D controller2D;
    public Animator animator;
    public PlayerComBattle playerComBattle;
    public Rigidbody2D rigid;

    public float runSpeed = 40f;

 
    float horizontalMove = 0f;

    bool jump = false;
    bool faceRight = true;

    private void Start()
    {
        faceRight = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
             CmdSetSpeed(horizontalMove);



            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("jump");
                jump = true;
                CmdsetTJump();
                animator.SetBool("jump", true);
            }
        }
            //animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        
    }

    public void OnLand()
    {
        Debug.Log("on grand");
        CmdsetFJump();
        animator.SetBool("jump", false);
    }

    void FixedUpdate()
    {
        if (playerComBattle.combatMode)
        {
            return;
        }
        if (!isLocalPlayer)
        {
            return;
        }
        controller2D.Move(horizontalMove * Time.fixedDeltaTime, false, jump );
        jump = false;
    }

    [Command]
    void CmdsetTJump()
    {
        RpcGiveTJump();
    }

    [ClientRpc]
    void RpcGiveTJump()
    {
        animator.SetBool("jump", true);
    }
    [Command]
    void CmdsetFJump()
    {
        RpcGiveFJump();
    }

    [ClientRpc]
    void RpcGiveFJump()
    {
        animator.SetBool("jump", false);
    }

    [Command]
    void CmdSetSpeed(float speed)
    {
        RpcGiveSpeed(speed);
    }

    [ClientRpc]
    void RpcGiveSpeed(float speed)
    {
        if (faceRight && speed < 0)
        {
            controller2D.Flip();
            this.faceRight = false;
        }
        else if(!faceRight && speed > 0)
        {
            controller2D.Flip();
            this.faceRight = true;
        }
        animator.SetFloat("speed", Mathf.Abs(speed));
    }
}
