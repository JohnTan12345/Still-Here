// Created by: Xander
// Description: Fish swimming behaviour

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

        // Turn the fish to target point
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        // Move the fish to target point
        transform.position += transform.forward * swimSpeed * Time.deltaTime;

        // If the fish reaches the target point
        if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
        {
            SwitchTarget();
        }
    }

    // Switches the target point
    void SwitchTarget()
    {
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
    }
}