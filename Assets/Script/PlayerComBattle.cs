using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerComBattle : NetworkBehaviour
{
    public Animator animator;
    public PlayerMove playerMove;
    public Transform AttackPoint;
    public Rigidbody2D rigidbody2D;
    public Controller2D controller2D;

    public float attackRange = 0.1f;
    public LayerMask enemyLayer;
    int maxHealth = 100;
    public int power = 30;
    public float attackRate = 3f;
    float nextAttackTime = -1f;
    public float freeTime = -1f;
    public float contrainTime = 0.3f;

    public bool combatMode = false;

    public int currentHealth;

    [SerializeField]
    Behaviour[] Die_CompoentsDisable;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if(Time.time >= freeTime)
            {
                if (!playerMove.enabled)
                {
                    playerMove.enabled = true;
                }
            }
            if (Input.GetButtonDown("Down"))
            {
                combatMode = true;
                CmdsetCombat();
                //CombatMode();
            }
            if (Input.GetButtonUp("Down"))
            {   
                combatMode = false;

                CmdfsetCombat();
                //UnCombatMode();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (nextAttackTime <= Time.time)
                {
                    nextAttackTime = Time.time + 1 / attackRate;
                    CmdSeverAttack();
                }
            }
        }
            animator.SetBool("combat", combatMode);
    }

    [Command]
    void CmdsetCombat()
    {
        RpcgiveCombat();
    }

    [ClientRpc]
    void RpcgiveCombat()
    {
        combatMode = true;
    }

    [Command]
    void CmdfsetCombat()
    {
        RpcfgiveCombat();
    }

    [ClientRpc]
    void RpcfgiveCombat()
    {
        combatMode = false;
    }

    void CombatMode()
    {
        animator.SetBool("combat", true);
        playerMove.enabled = false;
    }

    void UnCombatMode()
    {
        animator.SetBool("combat", false);
        playerMove.enabled = true ;

    }


    [Command]
    void CmdSeverAttack()
    {
            RpcgiveAttack();
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemys)
            {
                Debug.Log(netId + "hit: " + enemy.name);

                enemy.GetComponent<PlayerComBattle>().RpcGiveTakeDamage();
            }
    }
    [ClientRpc]
    void RpcgiveAttack()
    {
        Debug.Log(netId + " Attack!");
        animator.SetTrigger("attack");
    }


    [ClientRpc]
    public void RpcGiveTakeDamage()
    {
        if (combatMode)
        {
            currentHealth -= 10;
        }
        else
        {
            currentHealth -= power;
            animator.SetTrigger("Hurt");
            freeTime = Time.time + contrainTime;
            if (playerMove.enabled)
            {
                playerMove.enabled = false;
            }

        }
        Debug.Log(netId + "'s health: " + currentHealth);
        if (currentHealth <= 0)
        {
            CmdDie();
            Debug.Log(netId + " die");
        }
    }

    [Command]
    void CmdDie()
    {
        RpcTellDie();
    }

    [ClientRpc]
    void RpcTellDie()
    {
        animator.SetBool("Die", true);
        rigidbody2D.gravityScale = 0;
        for (int i = 0; i < Die_CompoentsDisable.Length; i++)
        {
            Die_CompoentsDisable[i].enabled = false;
        }
    }
}
