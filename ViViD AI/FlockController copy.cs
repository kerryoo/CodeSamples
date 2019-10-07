using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    private int flockSize = 20;
    private List<Boid> flockList;
    public float SpeedModifier = 5;
    [SerializeField] private float alignmentWeight = 1;
    [SerializeField] private float cohesionWeight = 1;
    [SerializeField] private float separationWeight = 1;
    [SerializeField] private float followWeight = 5;
    [SerializeField] private Boid prefab;
    [SerializeField] private float spawnRadius = 3.0f;

    private Vector3 spawnLocation = Vector3.zero;
    [SerializeField] public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        flockList = new List<Boid>(flockSize);
        for (int i = 0; i < flockSize; i++)
        {
            spawnLocation = Random.insideUnitSphere * spawnRadius + transform.position;
            Boid boid = Instantiate(prefab, spawnLocation, transform.rotation) as Boid;
            boid.transform.parent = transform;
            boid.FlockController = this;
            flockList.Add(boid);
        }
    }

    public Vector3 Flock(Boid boid, Vector3 boidPosition, Vector3 boidDirection)
    {
        Vector3 flockDirection = Vector3.zero;
        Vector3 flockCenter = Vector3.zero;
        Vector3 targetDirection = Vector3.zero;
        Vector3 separation = Vector3.zero;

        for (int i = 0; i < flockList.Count; ++i)
        {
            Boid neighbor = flockList[i];
            if (neighbor != boid)
            {
                flockDirection += neighbor.Direction;
                flockCenter += neighbor.transform.localPosition;
                separation += neighbor.transform.localPosition - boidPosition;
                separation *= -1;
            }
        }

        flockDirection /= flockSize;
        flockDirection = flockDirection.normalized * alignmentWeight;

        flockCenter /= flockSize;
        flockCenter = flockCenter.normalized * cohesionWeight;

        separation /= flockSize;
        separation = separation.normalized * separationWeight;

        targetDirection = target.localPosition - boidPosition;
        targetDirection *= followWeight;

        return flockDirection + flockCenter + separation + targetDirection;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
