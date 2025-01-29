using UnityEngine;

namespace GamePlay
{
    public class PTDimmerTrigger : MonoBehaviour
    {
        public string playerHeadTag = "PlayerHead";
        private PTDimmer _ptDimmer;

        private void Awake()
        {
            _ptDimmer = FindObjectOfType<PTDimmer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Trigger Enter: {other.gameObject.name}");

            if (other.gameObject.CompareTag(playerHeadTag))
            {
                _ptDimmer.SetToDim();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Debug.Log($"Trigger Exit: {other.gameObject.name}");

            if (other.gameObject.CompareTag(playerHeadTag))
            {
                _ptDimmer.SetToLit();
            }
        }
    }
}
