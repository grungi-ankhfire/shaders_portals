using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class PortalCamera : MonoBehaviour
{

    [SerializeField] Camera portalCamera;
    Camera mainCamera;

    [SerializeField] Transform portal1, portal2;

    RenderTexture portalTexture1, portalTexture2;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        portalTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        portalTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    void Start()
    {
        portal1.GetComponent<Renderer>().material.SetTexture("_Render_Texture", portalTexture1);
        portal2.GetComponent<Renderer>().material.SetTexture("_Render_Texture", portalTexture2);
    }

    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdateRenderTextures;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateRenderTextures;
    }


    void UpdateRenderTextures(ScriptableRenderContext SRC, Camera camera)
    {
        if (portal1.GetComponent<Renderer>().isVisible)
        {
            portalCamera.targetTexture = portalTexture1;
            RenderPortal(portal1, portal2, SRC);
        }

        if (portal2.GetComponent<Renderer>().isVisible)
        {
            portalCamera.targetTexture = portalTexture2;
            RenderPortal(portal2, portal1, SRC);
        }

    }


    // Update is called once per frame
    void RenderPortal(Transform portalLookAt, Transform portalLookFrom, ScriptableRenderContext SRC)
    {
        Vector3 localPosition = portalLookAt.InverseTransformPoint(transform.position);
        localPosition = Quaternion.AngleAxis(180, Vector3.up) * localPosition;
        portalCamera.transform.position = portalLookFrom.TransformPoint(localPosition);
    
        Quaternion localRotation = Quaternion.Inverse(portalLookAt.rotation) * transform.rotation;
        localRotation = Quaternion.Euler(0, 180, 0) * localRotation;
        portalCamera.transform.rotation = portalLookFrom.rotation * localRotation;

        Plane p = new Plane(-portalLookFrom.forward, portalLookFrom.position);
        Vector4 planeWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        
        Vector4 planeCameraSpace = Matrix4x4.Transpose(portalCamera.cameraToWorldMatrix) * planeWorldSpace;

        Matrix4x4 projection = portalCamera.CalculateObliqueMatrix(planeCameraSpace);
        portalCamera.projectionMatrix = projection;

        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCamera);
    }
}
