using UnityEngine;
using UnityFx.Outline;

namespace CardboardJenga
{
    public class JengaPiece : MonoBehaviour
    {
        [SerializeField] private OutlineBehaviour _outline;
        [SerializeField] private OutlineSettings _outlineHighlight;
        [SerializeField] private OutlineSettings _outlineSeleceted;
        [SerializeField] private AudioClip _highlightAudio;
        [SerializeField] private AudioClip _selectAudio;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _outline.Camera = Camera.main;
            _outline.enabled = false;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Highlight()
        {
            if (JengaController.instance.selectedPiece == this)
            {
                return;
            }

            _outline.OutlineSettings = _outlineHighlight;
            _outline.enabled = true;
            CameraPointer.playerAudio.PlayOneShot(_highlightAudio);
        }

        public void CancelHighlight()
        {
            if (JengaController.instance.selectedPiece == this)
            {
                return;
            }

            _outline.enabled = false;
        }

        public void Select()
        {
            if (JengaController.instance.isBuilding)
            {
                return;
            }

            if (JengaController.instance.selectedPiece == this)
            {
                var lookPos = Camera.main.transform.position;
                lookPos.y = transform.position.y;
                var dir = transform.position - lookPos;

                Debug.Log("Push");
                _rigidbody.AddForce(dir.normalized * 3f);
                return;
            }

            if (JengaController.instance.selectedPiece != null)
            {
                JengaController.instance.selectedPiece.Deselect();
            }

            JengaController.instance.selectedPiece = this;

            _outline.OutlineSettings = _outlineSeleceted;
            _outline.enabled = true;
            CameraPointer.playerAudio.PlayOneShot(_selectAudio);

            if (_rigidbody.isKinematic)
            {
                // _rigidbody.isKinematic = false;
                return;
            }
        }

        public void Deselect()
        {
            if (JengaController.instance.selectedPiece == this)
            {
                JengaController.instance.selectedPiece = null;
            }

            _outline.enabled = false;

            // _rigidbody.isKinematic = true;
        }
    }
}
