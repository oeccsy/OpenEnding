using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Transform BackgroundCanvas { get; set; }
    public Transform SceneUIRoot { get; set; }
    public Transform PopupUIRoot { get; set; }
    
    public GameObject ShowPopup(string resourcePath, int orderInLayer = 0)
    {
        var popupPrefab = Resources.Load<GameObject>(resourcePath);
        var popupInstance = Instantiate(popupPrefab, PopupUIRoot);
        var uiElement = popupInstance.GetComponentInChildren<IUIElement>();

        CanvasSettings(popupInstance, "PopupUI", orderInLayer);
        
        uiElement.Show();
        
        return popupInstance;
    }

    private void CanvasSettings(GameObject targetObj, string sortingLayerName, int order)
    {
        Canvas canvas = targetObj.GetComponent<Canvas>();
        CanvasScaler canvasScaler = targetObj.GetComponent<CanvasScaler>();

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.allCameras[(int)Define.CameraIndex.UI];
        canvas.overrideSorting = true;
    
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1080, 2400);
        
        canvas.sortingLayerName = sortingLayerName;
        
        canvas.sortingOrder = order;
        canvas.planeDistance = 10 - order;
    }
}