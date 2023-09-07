using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class DeviceObject : MonoBehaviour, IPointerClickHandler
{
    public MeshRenderer meshRenderer;
    public ColorPalette.ColorName curColor = ColorPalette.ColorName.DeviceDefault;
    public ColorPalette.ColorName ownColor = ColorPalette.ColorName.Pink;
    public Sequence flipSequence;
    public Sequence colorSequence;

    public delegate void DeviceHandler(DeviceObject deviceObject);
    public static event DeviceHandler OnTouchAnyDevice;
    public event DeviceHandler OnTouchDevice;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void Flip()
    {
        if (flipSequence.IsActive()) return;
        flipSequence = DOTween.Sequence()
            .Append(transform.DORotate(transform.localRotation.eulerAngles + Vector3.right * 360, 1f, RotateMode.FastBeyond360))
            .Join(transform.DOLocalJump(transform.position, 3f, 1, 1f));
    }

    public void SetDeviceColor(ColorPalette.ColorName targetColor)
    {
        if (colorSequence.IsActive()) return;

        colorSequence = DOTween.Sequence()
            .Append(meshRenderer.materials[0].DOColor(ColorPalette.GetColor(targetColor), 1f))
            .AppendCallback(() => curColor = targetColor);
    }

    public void ResetDeviceColor()
    {
        if (colorSequence.IsActive()) return;
        Material prevMat = meshRenderer.materials[0];
        Material nextMat = Resources.Load<Material>($"Materials/Device");
        meshRenderer.materials[0] = nextMat;

        colorSequence = DOTween.Sequence()
            .Append(meshRenderer.materials[0].DOColor(nextMat.color, 1f).From(prevMat.color));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Flip();
        OnTouchDevice?.Invoke(this);
        OnTouchAnyDevice?.Invoke(this);
    }
}
