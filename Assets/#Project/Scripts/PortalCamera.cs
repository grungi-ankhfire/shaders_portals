using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PortalCamera : MonoBehaviour
{

    [SerializeField] Camera portalCamera;
    Camera mainCamera;

    [SerializeField] Transform portal1, portal2;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localPosition = portal1.InverseTransformPoint(transform.position);
        localPosition = Quaternion.AngleAxis(180, Vector3.up) * localPosition;
        portalCamera.transform.position = portal2.TransformPoint(localPosition);
    
        Quaternion localRotation = Quaternion.Inverse(portal1.rotation) * transform.rotation;
        localRotation = Quaternion.Euler(0, 180, 0) * localRotation;
        portalCamera.transform.rotation = portal2.rotation * localRotation;

        Plane p = new Plane(-portal2.forward, portal2.position);
        Vector4 planeWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        //portalCamera.cameraToWorldMatrix
        





    }
}
