using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    Vector3 offset;
    float y;
    float x;
    [SerializeField] float speedFollow = 1f;

    void LateUpdate()
    {
        Vector3 followPos = playerTransform.position + offset;
        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, 5f))
        {
            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime + speedFollow);
        }
        else
            y = Mathf.Lerp(y, playerTransform.position.y, Time.deltaTime + speedFollow);
        x = Mathf.Lerp(x, playerTransform.position.x, Time.deltaTime + speedFollow);
        followPos.y = offset.y + y;
        followPos.x = offset.x + x;
        transform.position = followPos;
    }
}
