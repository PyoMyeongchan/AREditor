using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

public class SearchPosition : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _imagePositionText;
    [SerializeField] private TextMeshProUGUI _objectPositionText;
    [SerializeField] private TextMeshProUGUI _debugObjectPositionText;
    
    [SerializeField] private ARInteractorSpawnTrigger _arInteractorSpawnTrigger;
    
    [SerializeField] private Camera arCamera;

    [SerializeField] private InputActionReference tapPosition;
    [SerializeField] private InputActionReference tapPress;

    private bool isPressed;
    private Vector2 cachedTouch;
    
    public List<Vector3> markerPositions;

    private void DebugObjectPosition(Vector3 position)
    {
        _debugObjectPositionText.text = "ObjectDebugPosition" + position.ToString();
        markerPositions.Add(position);
    }

    public void SaveMarkerPosition()
    {
        SaveMarkerData markerData = new SaveMarkerData();
        markerData.SaveVector3List(markerPositions);
    }



    private void OnEnable()
    {
        tapPosition.action.performed += OnTouchPosition;
        tapPress.action.performed += OnTouchPress;

        tapPosition.action.Enable();
        tapPress.action.Enable();

        ARInteractorSpawnTrigger.OnPositionDebug += DebugObjectPosition;
        
    }

    private void OnDisable()
    {
        tapPosition.action.performed -= OnTouchPosition;
        tapPress.action.performed -= OnTouchPress;

        tapPosition.action.Disable();
        tapPress.action.Disable();
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        cachedTouch = context.ReadValue<Vector2>();
    }

    private void OnTouchPress(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            CheckTouch();
        }
    }

    private void CheckTouch()
    {
        _imagePositionText.text = null;
        _objectPositionText.text = null;
        Ray ray = arCamera.ScreenPointToRay(cachedTouch);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Raycast");
            if (hit.collider != null && hit.collider.CompareTag("ImagePosition"))
            {
                _imagePositionText.text = "ImagePosition"+ hit.collider.gameObject.transform.position.ToString();
                markerPositions.Add(hit.collider.gameObject.transform.position);
            }
            else if (hit.collider != null && hit.collider.CompareTag("ObjectPosition"))
            {
                _objectPositionText.text = "MarkerPosition" + hit.collider.gameObject.transform.position.ToString();
            }
        }
        else
        {
            
        }

    }
}
