using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to a gameobject. The transform of the gameobject has the position of the camera (In Main Menu, In Game, etc.)")]
    private GameObject m_generalCameraPosition = null;
    [SerializeField, Tooltip("Reference to a gameobject. The transform of the gameobject has the position of the camera in skin selection menu.")]
    private GameObject m_skinSelectionCameraPosition = null;
    [SerializeField, Tooltip("The move speed of the camera when transitioning between two places.")]
    private float m_cameraSpeed = 5.0f;

    private Coroutine m_cameraMovementCoroutine;


    private void OnEnable()
    {
        GameManager.OnTitleScreen += MoveToGeneralPosition;
        GameManager.OnSkinScreen += MoveToSkinSelectionPosition;
    }

    private void OnDisable()
    {
        GameManager.OnTitleScreen -= MoveToGeneralPosition;
        GameManager.OnSkinScreen -= MoveToSkinSelectionPosition;
    }

    private void Start()
    {
        if (m_generalCameraPosition == null)
            Debug.LogError("The general camera position is missing!", gameObject);
        if (m_skinSelectionCameraPosition == null)
            Debug.LogError("The skin selection camera position is missing!", gameObject);
    }

    private void MoveToGeneralPosition()
    {
        MoveCameraTo(m_generalCameraPosition);
    }

    private void MoveToSkinSelectionPosition()
    {
        MoveCameraTo(m_skinSelectionCameraPosition);
    }


    /// <summary>
    /// Moves the camera towards the new position
    /// </summary>
    /// <param name="newLocation"></param>
    public void MoveCameraTo(GameObject newLocation)
    {
        if (m_cameraMovementCoroutine != null)
            StopCoroutine(m_cameraMovementCoroutine);

        m_cameraMovementCoroutine = StartCoroutine(MoveCameraCoroutine(newLocation.transform));
    }

    private IEnumerator MoveCameraCoroutine(Transform newPositionTransform)
    {
        while (Vector3.Distance(newPositionTransform.position, transform.position) > 0.1f)
        {
            transform.LerpTransform(newPositionTransform, Time.deltaTime * m_cameraSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

}
