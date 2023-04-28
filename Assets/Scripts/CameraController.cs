using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    public float cameraHeight = 10.0f; // The height of the camera above the grid
    public float aspectRatio = 16.0f / 9.0f; // The aspect ratio of the camera
    public float fieldOfView = 60.0f; // The field of view of the camera
    public float margin = 1.0f; // The margin around the grid to fit in the field of view

    void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate()
    {
        // Check if the camera has been moved or resized
        if (transform.hasChanged || Camera.main.aspect != aspectRatio)
        {
            // Calculate the width of the grid based on the aspect ratio
            float gridWidth = aspectRatio * cameraHeight;

            // Calculate the size of the orthographic camera
            float cameraSize = (11.0f + 2.0f * margin) / 2.0f;

            // Set the position and rotation of the camera
            transform.position = new Vector3(gridWidth / 2.0f, cameraHeight, -gridWidth / 2.0f);
            transform.rotation = Quaternion.Euler(45.0f, 0.0f, 0.0f);

            // Set the size and aspect ratio of the camera
            Camera.main.orthographicSize = cameraSize;
            Camera.main.aspect = aspectRatio;

            // Reset the transform change flag
            transform.hasChanged = false;
        }
    }
}
