using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class PTDimmerTrigger : MonoBehaviour
    {
        public string playerHeadTag = "PlayerHead";
        private PTDimmer _ptDimmer;
        [SerializeField] private string entryLayer = "[BuildingBlock] Passthrough";
        [SerializeField] private string exitLayer = "[BuildingBlock] Passthrough_Dark";

        private void Awake()
        {
            _ptDimmer = GameObject.FindObjectOfType<PTDimmer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Trigger Enter: {other.gameObject.name}");

            if (other.gameObject.CompareTag(playerHeadTag))
            {
                _ptDimmer.SetActiveLayer(entryLayer);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Debug.Log($"Trigger Exit: {other.gameObject.name}");

            if (other.gameObject.CompareTag(playerHeadTag))
            {
                _ptDimmer.SetActiveLayer(exitLayer);
            }
        }
    }
}
