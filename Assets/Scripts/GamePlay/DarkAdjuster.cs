using UnityEngine;
using Meta.XR.MRUtilityKit;
using Meta.WitAi.Attributes;

namespace GamePlay
{
    public class DarkAdjuster : MonoBehaviour
    {
        [SerializeField] 
        private OVRPassthroughLayer passthroughLayer;

        [SerializeField]
        private MRUK mruk; // Reference to MRUK instance

        [SerializeField] 
        private float fadeDuration = 2f;

        private float currentOpacity = 1f;
        private float fadeSpeed;
        private bool sceneLoaded = false;

        private void Awake()
        {
            if (passthroughLayer == null)
            {
                Debug.LogError("OVRPassthroughLayer reference is not set.");
            }
            
            if (mruk == null)
            {
                Debug.LogError("MRUK reference is not set.");
            }
            
            fadeSpeed = 1f / fadeDuration;
        }

        private void OnEnable()
        {
            if (mruk != null)
            {
                mruk.SceneLoadedEvent.AddListener(OnSceneLoaded);
            }
        }

        private void OnDisable()
        {
            if (mruk != null)
            {
                mruk.SceneLoadedEvent.RemoveListener(OnSceneLoaded);
            }
        }

        private void OnSceneLoaded()
        {
            Debug.Log("Scene loaded, starting opacity fade.");
            sceneLoaded = true;
        }

        private void Update()
        {
            if (sceneLoaded && currentOpacity > 0)
            {
                currentOpacity = Mathf.Max(0, currentOpacity - fadeSpeed * Time.deltaTime);
                passthroughLayer.textureOpacity = currentOpacity;
                passthroughLayer.colorMapEditorBrightness = currentOpacity;
            }
        }

        public void ResetOpacity()
        {
            currentOpacity = 1f;
        }
    }
}