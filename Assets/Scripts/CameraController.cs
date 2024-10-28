using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] private float speed;
    private float currentPosX;
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
        
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        
        if (followTarget != null)
        {
            transform.position = new Vector3(followTarget.position.x + lookAhead, transform.position.y, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * followTarget.localScale.x), Time.deltaTime * cameraSpeed);
        }
    }

   
    public void SetFollowTarget(Transform newTarget)
    {
        followTarget = newTarget;
    }

   
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
