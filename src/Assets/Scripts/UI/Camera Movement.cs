using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;
    public float smoothing;
    private Vector2 minPos;
    private Vector2 maxPos;

    // Start is called before the first frame update
    void Start()
    {

        UpdateMinMax();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(transform.position != target.position) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);         
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPos.x, maxPos.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPos.y, maxPos.y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    void UpdateMinMax() {
        minPos = target.position; maxPos = target.position;
    }
}
