using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private Transform followTarget;

    private void Start()
    {
        followTarget = player;
    }

    private void Update()
    {
        if (followTarget != null)
        {
            // Set target position to follow the player both horizontally and vertically
            Vector3 targetPosition = new Vector3(
                followTarget.position.x + lookAhead, 
                followTarget.position.y, 
                transform.position.z
            );

            // Smoothly move camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, speed);

            // Update the lookAhead based on the player's movement direction
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * followTarget.localScale.x), Time.deltaTime * cameraSpeed);
        }
    }

    public void SetFollowTarget(Transform newTarget)
    {
        followTarget = newTarget;
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        // Update both x and y for the new room's position if you want to recenter vertically as well
        Vector3 newRoomPosition = _newRoom.position;
        transform.position = new Vector3(newRoomPosition.x, newRoomPosition.y, transform.position.z);
    }
}