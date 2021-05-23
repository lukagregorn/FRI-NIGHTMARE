using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public float smoothing;
    public float edgePadding;


    public void SetClampValues(Vector2 newMin, Vector2 newMax) {
        minPosition = newMin;
        maxPosition = newMax;
    }


    private void Start() {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (transform.position != target.position) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, 
                                            transform.position.z);

            // clamp values
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x + edgePadding, maxPosition.x - edgePadding);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y + edgePadding, maxPosition.y - edgePadding);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

}
