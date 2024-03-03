using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Portal_Script : MonoBehaviour
{
    public Camera portalCamera;
    public Transform pairPortal;

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += UpdateCamera;
        RenderPipelineManager.beginCameraRendering -= UpdateCamera;
    }

    private void OnDisable()
    {
        
    }

    void UpdateCamera(ScriptableRenderContext context, Camera camera)
    {
        if ((camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView) && camera.tag != "Portal Camera")
        {
            portalCamera.projectionMatrix = camera.projectionMatrix;

            var relativePosition = transform.InverseTransformPoint(camera.transform.position);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
            portalCamera.transform.position = pairPortal.TransformPoint(relativePosition);

            var relativeRotation = transform.InverseTransformDirection(camera.transform.forward);
            relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = pairPortal.TransformDirection(relativeRotation);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
