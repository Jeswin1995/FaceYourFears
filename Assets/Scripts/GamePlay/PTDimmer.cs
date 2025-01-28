using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTDimmer : MonoBehaviour
{
    public float layerTransitionSmoothing = 1f;
    private OVRPassthroughLayer _activeLayer;
    private OVRPassthroughLayer[] _passthroughLayers;

    private void Awake()
    {
        _passthroughLayers = GameObject.FindObjectsByType<OVRPassthroughLayer>(FindObjectsSortMode.None);
    }

    public void SetActiveLayer(OVRPassthroughLayer layer)
    {
        _activeLayer = layer;
    }

    public void SetActiveLayer(string layerName)
    {
        _activeLayer = GetPassthroughLayer(layerName);
    }

    private OVRPassthroughLayer GetPassthroughLayer(string layerName)
    {
        OVRPassthroughLayer active = null;

        foreach (var layer in _passthroughLayers)
        {
            if (layer.name.Contains(layerName))
            {
                active = layer;
            }
        }

        return active;
    }

    private void FadeIn()
    {
        foreach (var layer in _passthroughLayers)
        {
            if (layer == _activeLayer)
            {
                layer.textureOpacity = Mathf.Clamp01(layer.textureOpacity + (Time.deltaTime * layerTransitionSmoothing));
                layer.enabled = true;
            }
            else
            {
                layer.textureOpacity = Mathf.Clamp01(layer.textureOpacity - (Time.deltaTime * layerTransitionSmoothing));
                if (layer.textureOpacity < 0.1f)
                {
                    layer.enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        if (_activeLayer && _activeLayer.textureOpacity < 1)
        {
            FadeIn();
        }
    }
}
