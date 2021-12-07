using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardboardJenga
{
    public class JengaController : Singleton<JengaController>
    {
        [SerializeField] private GameObject _piecePrefab;

        private static bool s_initializing;

        public List<JengaPiece> pieces { get; } = new();
        public JengaPiece selectedPiece { get; set; }
        public bool isBuilding { get; private set; }

        private void Start()
        {
            InitalizeJengaAsync(3 * 20).Forget();
        }

        private async UniTask InitalizeJengaAsync(int pieceCount)
        {
            if (s_initializing)
            {
                return;
            }

            foreach (var piece in pieces)
            {
                Destroy(piece.gameObject);
            }

            for (int i = 0; i < pieceCount; i++)
            {
                var piece = Instantiate(_piecePrefab, transform);
                piece.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                piece.SetActive(false);
                pieces.Add(piece.GetComponent<JengaPiece>());
            }
            await RepositionJengaPiecesAsync();
        }

        public void RepositionJengaPieces()
        {
            RepositionJengaPiecesAsync().Forget();
        }

        public async UniTask RepositionJengaPiecesAsync()
        {
            if (s_initializing)
            {
                return;
            }
            isBuilding = true;
            s_initializing = true;
            JengaTimer.instance.ClearTimer(true);

            if (selectedPiece != null)
            {
                selectedPiece.Deselect();
            }

            foreach (var piece in pieces)
            {
                piece.gameObject.SetActive(false);
            }

            var position = transform.position;

            var rotate = false;
            var floorIndex = 0;
            foreach (var piece in pieces)
            {
                var piecePos = position;
                piecePos += floorIndex switch
                {
                    0 => (rotate ? Vector3.forward : Vector3.left) * 0.102f,
                    1 => Vector3.zero,
                    2 => (rotate ? Vector3.back : Vector3.right) * 0.102f,
                    _ => Vector3.zero
                };

                var pieceRot = Quaternion.Euler(0, rotate ? 90 : 0, 0);

                var rigidbody = piece.GetComponent<Rigidbody>();
                // rigidbody.isKinematic = true;
                piece.gameObject.SetActive(true);
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                piece.transform.SetPositionAndRotation(piecePos, pieceRot);
                // rigidbody.isKinematic = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

                if (++floorIndex > 2)
                {
                    floorIndex = 0;
                    position.y += 0.05f;
                    rotate = !rotate;
                }

                await UniTask.Delay(50);
            }

            foreach (var piece in pieces)
            {
                piece.GetComponent<Rigidbody>().ResetCenterOfMass();
            }

            isBuilding = false;
            s_initializing = false;
            JengaTimer.instance.StartTimer();
        }
    }
}
