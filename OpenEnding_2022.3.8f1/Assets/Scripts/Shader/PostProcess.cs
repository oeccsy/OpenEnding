using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostProcess : MonoBehaviour
{
    private static Define.PostProcess processType = Define.PostProcess.None;
    [SerializeField]
    private Shader grayScaleShader;
    private Material _grayScaleMaterial;

    private void Awake()
    {
        processType = Define.PostProcess.None;
        _grayScaleMaterial = new Material(grayScaleShader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        switch (processType)
        {
            case Define.PostProcess.None :
                Graphics.Blit(src, dest);
                break;
            case Define.PostProcess.GrayScale :
                Graphics.Blit(src, dest, _grayScaleMaterial);
                break;
        }
    }

    public static void SetPostProcess(Define.PostProcess process)
    {
        processType = process;
    }
}