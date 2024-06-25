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
    }
}
