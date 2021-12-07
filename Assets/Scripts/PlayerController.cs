using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CardboardJenga
{
    public class PlayerController : Singleton<PlayerController>
    {
        private bool _isRotating;

        public void RotateRight()
        {
            RotateRightAsync().Forget();
        }

        public async UniTask RotateRightAsync()
        {
            if (_isRotating)
            {
                return;
            }

            _isRotating = true;
            await transform
                .DORotate(Vector3.up * 45f, 0.5f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuad)
                .AwaitForComplete();

            _isRotating = false;
        }

        public void RotateLeft()
        {
            RotateLeftAsync().Forget();
        }

        public async UniTask RotateLeftAsync()
        {
            if (_isRotating)
            {
                return;
            }

            _isRotating = true;
            await transform
                .DORotate(Vector3.down * 45f, 0.5f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutQuad)
                .AwaitForComplete();

            _isRotating = false;
        }
    }
}
