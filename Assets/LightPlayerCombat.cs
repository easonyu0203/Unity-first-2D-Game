using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlayerCombat : MonoBehaviour
{

    public Animator animator;
    public LightPlayerMovement playermovement;
    public Transform lightAttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    float triggerTime = -1f;
    public bool combatMode = false;
    public const int maxHealth = 100;
    public int currentHeath = 100;
    public int defense = 0;
    public const int combatDefense = 15;
    public const int power = 20;


    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !combatMode)
            {
                //Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                triggerTime = Time.time + 1f / (attackRange + 3);
                //play animation
                animator.SetTrigger("Attack");
            }

        }
        if (triggerTime != -1f && triggerTime <= Time.time)
        {
            Attack();
            triggerTime = -1f;
        }
        if (Input.GetButtonDown("Down"))
        {
            animator.SetBool("CombatMode", true);
            combatMode = true;
            defense = combatDefense;
        }
        if (Input.GetButtonUp("Down"))
        {
            animator.SetBool("CombatMode", false);
            combatMode = false;
            defense = 0;
        }
    }

    void Attack()
    {


        //Detect hit enemy or not
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(lightAttackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHeath -= (power - defense);
    }

}
