using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Orbit : MonoBehaviour
{
   
    public bool hideCursor = false;
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    [Header("Collision")]
    public bool cameraCollision = false;
    public float rayDistance = 1000f;
    public LayerMask ignoreLayer;
    private float camRadius = 1f;
    private Vector3 originalOffset;
    public float distanceMin = .5f;
    public float distanceMax = 15f;



    float x = 0.0f;
    float y = 0.0f;
    private void FixedUpdate()
    {
        if (target)
        {
            if (cameraCollision)
            {
                Ray camRay = new Ray(target.position, -transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(camRay, camRadius, out hit, rayDistance, ~ignoreLayer, QueryTriggerInteraction.Ignore))
                {
                    distance = hit.distance;
                    // return - exit the function
                    return;
                }
            }

            // Reset distance if not hitting
            distance = originalOffset.magnitude;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, camRadius);
    }
    // Use this for initialization
    void Start()
    {
        //Calulate offset of camera at start
        originalOffset = transform.position - target.position;
        // Ray distance is as long the magnitude of offset 
        rayDistance = originalOffset.magnitude;
        transform.SetParent(null);
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;


    }

    public void Look(float mouseX, float mouseY)
    {
        x += mouseX * xSpeed * Time.deltaTime;
        y -= mouseY * ySpeed * Time.deltaTime;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
    }
    void LateUpdate()
    {
        if (target)
        {
            Vector3 localOffest = transform.TransformDirection(offset);
            // distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            transform.position = (target.position + localOffest)+ -transform.forward * distance;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

}
