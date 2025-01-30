using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //public float distance;
    //public float height;
    //public float smoothness;

    //public Transform camTarget;

    //Vector3 velocity;
    //public Vector3 offset;

    //void LateUpdate()
    //{
    //    if (!camTarget)
    //        return;

    //    Vector3 pos = Vector3.zero;
    //    pos.x = camTarget.position.x + offset.x;
    //    pos.y = camTarget.position.y + height + offset.y;
    //    pos.z = camTarget.position.z - distance + offset.z;

    //    transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothness);
    //}


    public GameObject player;
    public float distance, height, smoothSpeed;

    private void Update()
    {
        // Assuming you have a reference to the target object and the camera
        float targetObjectSize = player.GetComponent<Collider>().bounds.size.magnitude;
        float desiredDistance = targetObjectSize * distance; // Adjust the multiplier as needed
        float desiredHeight = targetObjectSize * height; // Adjust the multiplier as needed

        Vector3 targetObjectPosition = player.transform.position;
        Vector3 cameraTargetPosition = targetObjectPosition - transform.forward * desiredDistance;
        cameraTargetPosition.y = targetObjectPosition.y + desiredHeight;

        // Set the camera position using smooth damping
        transform.position = Vector3.Lerp(transform.position, cameraTargetPosition, smoothSpeed * Time.deltaTime);
    }
}