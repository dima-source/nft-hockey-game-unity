using System.Collections;
using UnityEngine;

namespace UI.Main_menu
{
    public class Pack : MonoBehaviour
    {
        [SerializeField] private Transform receivedCardPopupTransform;
        
        private Animation _animation;

        private void Awake()
        {
            _animation = transform.GetComponent<Animation>();
        }

        public void BuyPuck()
        {
            _animation.Play();

            StartCoroutine(AnimationPlaying());
        }

        private IEnumerator AnimationPlaying()
        {
            while (_animation.isPlaying)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            gameObject.SetActive(false);
            receivedCardPopupTransform.gameObject.SetActive(true);
        }
    }
}