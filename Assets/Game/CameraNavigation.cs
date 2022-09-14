using System;
using UI.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using Input = UnityEngine.Input;

namespace GameScene
{
    [RequireComponent(typeof(Camera))]
    public class CameraNavigation : MonoBehaviour
    {

        [SerializeField]
        private Rect bounds;
        
        private Camera _camera;
        private Vector2 _dragOrigin;
        private static readonly Vector2 InvalidOrigin = new(-10e9f, -10e9f);
        

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (!IsCameraValid())
            {
                NotifyInvalidCamera();
            }
        }
        
        private bool IsCameraValid()
        {
            return _camera.orthographic;
        }

        private void NotifyInvalidCamera()
        {
            throw new ApplicationException("Camera is invalid");
        }

        private void Update()
        {
            PanCamera();
        }

        private void PanCamera()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetDragOrigin();
            }
            
            if (Input.GetMouseButton(0) && _dragOrigin != InvalidOrigin)
            {
                Vector3 difference = _dragOrigin - GetMouseWorldPosition();
                _camera.transform.position = ClampCamera(_camera.transform.position + difference); 
            }
        }

        private void SetDragOrigin()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _dragOrigin = InvalidOrigin;
                return;
            }
            _dragOrigin = GetMouseWorldPosition();
        }

        private Vector2 GetMouseWorldPosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        private Vector3 ClampCamera(Vector3 position)
        {
            Vector2 cameraSize = GetCameraSize();
            float minX = bounds.min.x + cameraSize.x;
            float maxX = bounds.max.x - cameraSize.x;
            float minY = bounds.min.y + cameraSize.y;
            float maxY = bounds.max.y - cameraSize.y;
 
            float newX = Mathf.Clamp(position.x, minX, maxX);
            float newY = Mathf.Clamp(position.y, minY, maxY);
            
            return new Vector3(newX, newY, position.z);
        }

        private Vector2 GetCameraSize()
        {
            float cameraWidth = _camera.orthographicSize;
            return new Vector2(cameraWidth * _camera.aspect, cameraWidth);
        }
        
    }
}
