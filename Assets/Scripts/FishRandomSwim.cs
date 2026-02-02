using UnityEngine;

public class FishRandomSwim : MonoBehaviour
{
    [Header("Tank Bounds")]
    public Vector3 tankCenter;
    public Vector3 tankSize = new Vector3(2f, 1f, 1f);

    [Header("Movement")]
    public float swimSpeed = 0.5f;
    public float turnSpeed = 2f;
    public float targetReachDistance = 0.1f;

    private Vector3 targetPosition;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        // Direction to target
        Vector3 direction = targetPosition - transform.position;

        // Rotate smoothly toward target
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        // Move forward
        transform.Translate(Vector3.forward * swimSpeed * Time.deltaTime);

        // Pick new target if close enough
        if (Vector3.Distance(transform.position, targetPosition) < targetReachDistance)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        targetPosition = tankCenter + new Vector3(
            Random.Range(-tankSize.x / 2f, tankSize.x / 2f),
            Random.Range(-tankSize.y / 2f, tankSize.y / 2f),
            Random.Range(-tankSize.z / 2f, tankSize.z / 2f)
        );
    }
}
