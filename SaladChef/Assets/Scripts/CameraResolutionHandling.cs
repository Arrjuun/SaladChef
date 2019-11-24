using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjust Camera to handle different Screen Resolutions
/// </summary>
public class CameraResolutionHandling : MonoBehaviour
{
     public float orthographicSize = 5;
     public float aspect = 1.33333f; // Game built with a reference of 1024x768

    void Start()
    {
        //Change Camera's projection matrix based on Change in Screen/Window resolution
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                Camera.main.nearClipPlane, Camera.main.farClipPlane);
    }
}
