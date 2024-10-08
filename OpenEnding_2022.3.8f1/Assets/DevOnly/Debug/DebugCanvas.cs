#if DEVELOPMENT_BUILD || UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugCanvas : Singleton<DebugCanvas>
{
    private Canvas _canvas;
    private CanvasScaler _canvasScaler;
    private GraphicRaycaster _graphicRaycaster;
    
    private TextMeshProUGUI _tmp;
    private RectTransform _tmpRectTransform;

    public void InitDebugCanvas()
    {
        transform.SetParent(DontDestroyOnLoadContainer.Instance.transform);
        
        if(_canvas == null) _canvas = gameObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.allCameras[(int)Define.CameraIndex.UI];
        _canvas.planeDistance = 10;
        _canvas.sortingLayerName = "Debug";
        gameObject.layer = 10;
        
        if (_canvasScaler == null)
        {
            _canvasScaler = gameObject.AddComponent<CanvasScaler>();
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1080, 2400);
        }

        if (_graphicRaycaster == null)
        {
            // _graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>(); // Physics Raycaster 우선순위가 밀리는 문제로 기능 보류
        }

        if (_tmp == null)
        {
            GameObject textObj = new GameObject("TMP");
            textObj.transform.SetParent(transform);
            
            textObj.AddComponent<CanvasRenderer>();
            textObj.layer = 10;
            _tmpRectTransform = textObj.AddComponent<RectTransform>();
            _tmpRectTransform.sizeDelta = Vector2.zero;
            _tmpRectTransform.anchorMax = Vector2.one;
            _tmpRectTransform.anchorMin = Vector2.zero;

            textObj.transform.localPosition = Vector3.zero;
            textObj.transform.localRotation = Quaternion.Euler(0,0,0);
            textObj.transform.localScale = Vector3.one;

            _tmp = textObj.AddComponent<TextMeshProUGUI>();
            _tmp.alignment = TextAlignmentOptions.Top;
            _tmp.fontSize = 40;
            _tmp.color = Color.black;
        }
        _tmp.text = "";
        
    }

    public void AddText(object msg)
    {
        _tmp.text = $"{_tmp.text} \n {msg}";
        if(_tmp.text.Length > 1500) SetText(msg);
    }

    public void SetText(object msg)
    {
        _tmp.text = $"{msg}";
    }
}
#endif

public static class DebugExtension
{
    
    [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public static void Log(this object value)
    {
#if DEVELOPMENT_BUILD
        string curSceneName = SceneManager.GetActiveScene().name;
        string prevClassName = new StackTrace().GetFrame(1).GetMethod().ReflectedType?.Name;

        DebugCanvas.Instance.AddText(value);

        UnityEngine.Debug.Log($"OpenEnding : {curSceneName} : {prevClassName} : {value}");
#endif    
    }
    
    [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
    public static void LogError(this object value)
    {
#if DEVELOPMENT_BUILD
        string curSceneName = SceneManager.GetActiveScene().name;
        string prevClassName = new StackTrace().GetFrame(1).GetMethod().ReflectedType?.Name;

        DebugCanvas.Instance.AddText(value);
            
        UnityEngine.Debug.LogError($"OpenEnding : {curSceneName} : {prevClassName} : {value}");
#endif    
    }
}