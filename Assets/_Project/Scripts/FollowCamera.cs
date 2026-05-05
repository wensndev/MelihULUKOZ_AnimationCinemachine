using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float followSpeed = 8f;
    public float lookSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Quaternion desiredRotation = Quaternion.LookRotation(target.position + Vector3.up - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            lookSpeed * Time.deltaTime
        );
    }
}