using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombie : Zombie
{
    [SerializeField] Animator animator;
    private float attackRange;
    private float originalMoveSpeed;
    private bool attacking;

    public override void Start()
    {
        animator.SetBool("Walk", true);
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(playerTransform.position, transform.position) <= attackRange && !attacking)
        {
            StartCoroutine(Attack());
        }
    }

    public override void AssignStats()
    {
        maxHealth = 10;
        movementSpeed = 1;
        originalMoveSpeed = movementSpeed;
        rotationSpeed = 2;
        minX = minZ = -10;
        maxX = maxZ = 10;
        damageMinForLargeSound = 5;
        fieldOfView = 45;
        viewDistance = 4;
        timeBetweenGruntMin = 6;
        timeBetweenGruntMax = 9;
        detectionInterval = 2;
        attackRange = 1.5f;
        damage = 10;
    }

    public override IEnumerator Die()
    {
        animator.SetBool("Dead", true);
        return base.Die();
    }

    IEnumerator Attack()
    {
        movementSpeed = 0;
        animator.SetTrigger("Attack");
        attacking = true;
        playerStats.handleDamage(damage);
        yield return new WaitForSeconds(2.5f);

        movementSpeed = originalMoveSpeed;
        attacking = false;

    }
}
