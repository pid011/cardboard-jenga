using DG.Tweening;
using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.UI;

namespace CardboardJenga
{
    public class CameraPointer : Singleton<CameraPointer>
    {
        [SerializeField] private float _maxDistance = 20;
        [SerializeField] private Image _crossHair;
        [SerializeField] private AudioSource _audio;

        public static AudioSource playerAudio => instance._audio;

        private CameraPointerTrigger _gazedTrigger;
        private Sequence _sequence;

        public void Update()
        {
            if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance)
                || !hit.transform.gameObject.TryGetComponent<CameraPointerTrigger>(out var targetTrigger))
            {
                if (_gazedTrigger)
                {
                    // Debug.Log("OnPointerExit");
                    _gazedTrigger.onPointerExit?.Invoke();
                    _gazedTrigger = null;
                    ResetCrosshair();
                }
                return;
            }

            if (targetTrigger == _gazedTrigger)
            {
                if (Api.IsTriggerPressed || Input.GetMouseButtonDown(0))
                {
                    // Debug.Log("OnPointerClick");
                    _gazedTrigger.onPointerClick?.Invoke();
                    ClickCrosshair();
                }
                return;
            }

            if (_gazedTrigger)
            {
                // Debug.Log("OnPointerExit");
                _gazedTrigger.onPointerExit?.Invoke();
            }
            else
            {
                HighlightCrosshair();
            }

            // Debug.Log("OnPointerEnter");
            _gazedTrigger = targetTrigger;
            _gazedTrigger.onPointerEnter?.Invoke();
        }

        private void HighlightCrosshair()
        {
            if (!_crossHair)
            {
                return;
            }

            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(_crossHair.DOColor(Color.green, 0.25f))
                .Join(_crossHair.transform.DOScale(Vector3.one * 1.2f, 0.25f));
        }

        private void ClickCrosshair()
        {
            if (!_crossHair)
            {
                return;
            }

            _sequence?.Kill();
            _crossHair.transform.localScale = Vector3.one * 2f;

            if (_gazedTrigger)
            {
                HighlightCrosshair();
            }
            else
            {
                ResetCrosshair();
            }
        }

        private void ResetCrosshair()
        {
            if (!_crossHair)
            {
                return;
            }

            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(_crossHair.DOColor(Color.white, 0.25f))
                .Join(_crossHair.transform.DOScale(Vector3.one, 0.25f));
        }
    }
}
