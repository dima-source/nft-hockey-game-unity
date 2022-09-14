using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_menu.UIPopups
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private float speedAnimation;
        [SerializeField] private Image image;
        private RectTransform imageRectTransform;
        
        public void Start()
        {
            imageRectTransform = image.GetComponent<RectTransform>();
        }

        private void Update()
        {
            var rotation = imageRectTransform.eulerAngles;
            var newRotation = rotation.z - speedAnimation * 1/60;
           
            imageRectTransform.eulerAngles= new Vector3(
                rotation.x,
                rotation.y,
                newRotation
                );
        }

        private void OnDisable()
        {
            imageRectTransform = image.GetComponent<RectTransform>();
            var rotation = imageRectTransform.localRotation;
           
            imageRectTransform.localRotation = new Quaternion(
                rotation.x,
                rotation.y,
                0,
                rotation.w
            ); 
        }
    }
}