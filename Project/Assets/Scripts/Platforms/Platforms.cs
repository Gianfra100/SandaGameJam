using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;
    [SerializeField] private float speed = 2f;

    private Vector2 targetPointA;
    private Vector2 targetPointB;

    private Vector2 target;

    private bool isPlaying = false;

    private Vector3 lastPosition;

    public Vector2 PlatformVelocity { get; private set; }

    protected virtual void Start()
    {
        targetPointA = transform.position + (Vector3)pointA;
        targetPointB = transform.position + (Vector3)pointB;
        target = targetPointB;
        isPlaying = true;
        lastPosition = transform.position;
    }

    protected virtual void Update()
    {
        Vector3 oldPos = transform.position;

        Move();

        PlatformVelocity = (transform.position - oldPos) / Time.deltaTime;
        lastPosition = transform.position;
    }

    protected virtual void Move()
    { 
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == targetPointA) ? targetPointB : targetPointA;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 worldPointA;
        Vector3 worldPointB;

        if (!isPlaying)
        {
            worldPointA = transform.position + (Vector3)pointA;
            worldPointB = transform.position + (Vector3)pointB;
        }
        else
        {
            worldPointA = targetPointA;
            worldPointB = targetPointB;
        }


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldPointA, 0.1f);
        Gizmos.DrawSphere(worldPointB, 0.1f);
        Gizmos.DrawLine(worldPointA, worldPointB);
    }
}
