using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    protected Rigidbody zombieBody;

    protected SoundLibrary soundLibrary;
    protected AudioSource soundSource;

    protected int maxHealth;
    protected float movementSpeed;
    protected float rotationSpeed;
    protected int damage;

    protected float targetPositionTolerance = 3;
    protected float minX;
    protected float maxX;
    protected float minZ;
    protected float maxZ;
    protected readonly float yPosition = 10.25f;
    protected Vector3 targetPosition;

    protected Destructable destructable;
    protected int damageMinForLargeSound;
    protected bool alive;
    protected bool playerFound;

    public int fieldOfView;
    public int viewDistance;
    protected Transform playerTransform;
    protected PlayerStats playerStats;
    protected Vector3 rayDirection;

    protected float nextWalkingSound;
    protected float timeBetweenGruntMin;
    protected float timeBetweenGruntMax;

    protected float nextDetection;
    protected float detectionInterval;

    public delegate void OnPlayerFound();
    public event OnPlayerFound AttackPlayer;


    public class Destructable
    {
        public int healthRemaining;

        public bool takeDamage(int amount) //returns true if dead, false if still alive
        {
            if (healthRemaining <= amount)
                return true;

            healthRemaining -= amount;
            return false;
        }
    }


    public virtual void Start()
    {

        AssignStats();
        destructable = new Destructable();
        destructable.healthRemaining = maxHealth;
        alive = true;

        soundLibrary = GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>();
        soundSource = gameObject.GetComponent<AudioSource>();
        zombieBody = gameObject.GetComponent<Rigidbody>();

        playerTransform = GameObject.Find("Player").transform;
        playerStats = playerTransform.GetComponent<PlayerStats>();

        GetNextPosition();
    }

    public virtual void Update()
    {
        if (alive)
        {
            if (playerFound)
            {
                Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
            } else
            {
                WanderAimlessly();
                if (nextDetection <= Time.time)
                {
                    DetectAspect();
                    nextDetection = Time.time + detectionInterval;
                }

            }


            if (nextWalkingSound <= Time.time)
            {
                soundSource.PlayOneShot(soundLibrary.GetRandomWalkSound());
                nextWalkingSound = Time.time + Random.Range(timeBetweenGruntMin, timeBetweenGruntMax);
            }


        }


    }

    public virtual void WanderAimlessly()
    {
        if (Vector3.Distance(targetPosition, transform.position) <= targetPositionTolerance)
        {
            GetNextPosition();
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 nextPosition = new Vector3(0, 0, movementSpeed * Time.deltaTime);

        transform.Translate(nextPosition);
    }

    public virtual void GetNextPosition()
    {
        targetPosition = new Vector3(Random.Range(minX, maxX), yPosition, Random.Range(minZ, maxZ));
    }

    public virtual void takeDamage(int amount)
    {
        bool dead = destructable.takeDamage(amount);
        if (dead)
        {
            StartCoroutine(Die());
        } else
        {
            if (amount >= damageMinForLargeSound)
                soundSource.PlayOneShot(soundLibrary.GetRandomHighDamageSound());
            else
                soundSource.PlayOneShot(soundLibrary.GetRandomLowDamageSound());
        }
    }

    public void DetectAspect()
    {
        RaycastHit hit;
        rayDirection = playerTransform.position - transform.position;

        if (Vector3.Angle(rayDirection, transform.forward) < fieldOfView)
        {
            if (Physics.Raycast(transform.position, rayDirection, out hit, viewDistance))
            {
                var player = hit.collider.GetComponent<Player>();
                if (player != null)
                    playerFound = true;
            }
        }

    }

    public abstract void AssignStats();

    


    public virtual IEnumerator Die()
    {
        alive = false;
        soundSource.PlayOneShot(soundLibrary.GetRandomDeathSound());
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
        
    }


}
