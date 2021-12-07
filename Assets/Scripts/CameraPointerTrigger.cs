
using UnityEngine;
using UnityEngine.Events;

namespace CardboardJenga
{
    [RequireComponent(typeof(Collider))]
    public class CameraPointerTrigger : MonoBehaviour
    {
        public UnityEvent onPointerEnter;
        public UnityEvent onPointerClick;
        public UnityEvent onPointerExit;
    }
}
