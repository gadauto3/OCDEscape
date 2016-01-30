using System;
using UnityEngine;
using System.Collections;

public class HighlightManager : MonoBehaviour
{

    public MeshRenderer[] meshRenderers;

    public Material outlineMaterial;

    protected class HighlightInfo
    {
        public MeshRenderer meshRenderer;
        public Material[] normalMaterials;
        public Material[] highlightMaterials;
    }

    protected HighlightInfo[] highlightInfos;

    protected void Awake()
    {
        if (meshRenderers.Length == 0) return;

        highlightInfos = new HighlightInfo[meshRenderers.Length];

        for (var i = 0; i < meshRenderers.Length; i++)
        {
            var highlightInfo = new HighlightInfo();

            highlightInfo.meshRenderer = meshRenderers[i];

            highlightInfo.normalMaterials = meshRenderers[i].sharedMaterials;

            highlightInfo.highlightMaterials = new Material[highlightInfo.normalMaterials.Length + 1];

            for (var j = 0; j < highlightInfo.normalMaterials.Length; j++)
            {
                highlightInfo.highlightMaterials[j] = highlightInfo.normalMaterials[j];
            }

            highlightInfo.highlightMaterials[highlightInfo.highlightMaterials.Length - 1] = outlineMaterial;
            highlightInfos[i] = highlightInfo;
        }
    }


    public void OnHighlight()
    {
        foreach (var highlightInfo in highlightInfos)
        {
            highlightInfo.meshRenderer.sharedMaterials = highlightInfo.highlightMaterials;
        }
    }

    public void OffHighlight()
    {
        foreach (var highlightInfo in highlightInfos)
        {
            highlightInfo.meshRenderer.sharedMaterials = highlightInfo.normalMaterials;
        }
    }
}
