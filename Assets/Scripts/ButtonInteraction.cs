using UnityEngine;
using UnityEngine.UI;

namespace CardboardJenga
{
    [RequireComponent(typeof(CameraPointerTrigger), typeof(Image))]
    public class ButtonInteraction : MonoBehaviour
    {
        [SerializeField] private AudioClip _highlightAudio;
        [SerializeField] private AudioClip _clickAudio;

        private Image _image;

        private void Start()
        {
            var trigger = GetComponent<CameraPointerTrigger>();
            _image = GetComponent<Image>();

            trigger.onPointerEnter.AddListener(OnPointerEnter);
            trigger.onPointerClick.AddListener(OnPointerClick);
            trigger.onPointerExit.AddListener(OnPointerExit);
        }

        private void OnPointerEnter()
        {
            CameraPointer.playerAudio.PlayOneShot(_highlightAudio);
            _image.color = new Color(0.8f, 0.8f, 0.8f);
        }

        private void OnPointerClick()
        {
            CameraPointer.playerAudio.PlayOneShot(_clickAudio);
        }

        private void OnPointerExit()
        {
            _image.color = Color.white;
        }
    }
}
