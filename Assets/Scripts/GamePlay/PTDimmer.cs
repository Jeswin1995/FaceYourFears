using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay
{
    public class PTDimmer : MonoBehaviour
    {
        [SerializeField] private OVRPassthroughLayer litLayer;
        [SerializeField] private OVRPassthroughLayer dimLayer;

        private void Start()
        {
            Assert.IsNotNull(litLayer, "LitLayer (Passthrough) is not assigned");
            Assert.IsNotNull(dimLayer, "DimLayer (Passthrough) is not assigned");
        }

        public void SetToDim()
        {
            dimLayer.textureOpacity = 1;
            dimLayer.enabled = true;
            
            litLayer.textureOpacity = 0;
            litLayer.enabled = false;
        }

        public void SetToLit()
        {
            litLayer.textureOpacity = 1;
            litLayer.enabled = true;
            
            dimLayer.textureOpacity = 0;
            dimLayer.enabled = false;
        }
    }
}
