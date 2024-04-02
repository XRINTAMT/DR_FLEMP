using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus.Interaction.Samples
{
    public class LookAtMainCam : MonoBehaviour
    {
        [SerializeField]
        private Transform _toRotate;

        protected virtual void Start()
        {
            this.AssertField(_toRotate, nameof(_toRotate));
        }

        protected virtual void Update()
        {
            Vector3 dirToTarget = (Camera.main.transform.position - _toRotate.position).normalized;
            _toRotate.LookAt(_toRotate.position - dirToTarget, Vector3.up);
        }
    }
}
