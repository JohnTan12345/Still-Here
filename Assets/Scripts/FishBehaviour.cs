using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float swimSpeed = 0.4f;
    public float turnSpeed = 2f;
    public float reachDistance = 0.05f;

    private Transform currentTarget;

    void Start()
    {
        currentTarget = pointB;
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        Vector3 direction = currentTarget.position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        transform.position += transform.forward * swimSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
        {
            SwitchTarget();
        }
    }

    void SwitchTarget()
    {
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }
}