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
    
    public List<MarkerData> markerDatas;
    private Vector3 imagePosition;
    private string imageName;
    private bool isGameStart;

    private void DebugObjectPosition(Vector3 position, string objectName)
    {
        _debugObjectPositionText.text = "ObjectDebugPosition" + position.ToString();
        Vector3 gamePosition = position - imagePosition;
        Debug.Log(position);
        Debug.Log(imagePosition);
        Debug.Log(gamePosition);
        MarkerData objectMarker = new MarkerData(objectName, gamePosition);
        markerDatas.Add(objectMarker);
    }

    public void SaveMarkerPosition()
    {
        SaveMarkerData markerData = new SaveMarkerData();
        markerData.SaveVector3List(markerDatas);
    }

    private void Start()
    {
        isGameStart = false;
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
        
        ARInteractorSpawnTrigger.OnPositionDebug -= DebugObjectPosition;
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
                if (isGameStart)
                {
                    return;
                }
                _imagePositionText.text = "ImagePosition"+ hit.collider.gameObject.transform.position.ToString();
                imagePosition = hit.collider.gameObject.transform.position;
                imageName = hit.collider.gameObject.name;
                Vector3 imageGamePosition = new Vector3(0, 0, 0);
                MarkerData imageMarker = new MarkerData(imageName, imageGamePosition);
                markerDatas.Add(imageMarker);
                isGameStart = true;
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
