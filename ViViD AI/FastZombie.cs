using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastZombie : Zombie
{
    [SerializeField] Animator animator;
	[SerializeField] GameObject explosionPrefab;
    private float jumpCooldown = 10;
    private float lastJump;
    private bool isGrounded;
    private bool wasGrounded;
    protected List<Collider> m_collisions = new List<Collider>();

    public override void Start()
    {
        animator.SetBool("Walk", true);
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        animator.SetBool("Grounded", isGrounded);
        if (playerFound && (lastJump + jumpCooldown <= Time.time))
        {
            JumpTowardsPlayer();
            
        }

        animator.SetFloat("MoveSpeed", 3);

        if (!wasGrounded && isGrounded)
            animator.SetTrigger("Land");
        if (!isGrounded && wasGrounded)
            animator.SetTrigger("Jump");

        wasGrounded = isGrounded;
    }

    

    public void JumpTowardsPlayer()
    {
        transform.LookAt(playerTransform);
        zombieBody.AddForce(Vector3.up * 6, ForceMode.Impulse);
        zombieBody.AddRelativeForce(Vector3.forward * 8, ForceMode.Impulse);
        lastJump = Time.time;
    }

    public override void AssignStats()
    {
        maxHealth = 1;
        movementSpeed = 3;
        rotationSpeed = 3;
        minX = minZ = -10;
        maxX = maxZ = 10;
        damageMinForLargeSound = 5;
        fieldOfView = 45;
        viewDistance = 9;
        timeBetweenGruntMin = 6;
        timeBetweenGruntMax = 9;
        detectionInterval = 2;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.GetComponent<Player>() != null)
		{
			Instantiate(explosionPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}

        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    public virtual void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { isGrounded = false; }
        }
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { isGrounded = false; }
    }

}
