using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class DeviceObject : MonoBehaviour, IPointerClickHandler
{
    public MeshRenderer meshRenderer;
    public Define.PastelColor ownColor;
    public Sequence flipSequence;
    public Sequence colorSequence;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    private void Flip()
    {
        if (flipSequence.IsActive()) return;
        flipSequence = DOTween.Sequence()
            .Append(transform.DORotate(transform.localRotation.eulerAngles + Vector3.right * 360, 1f, RotateMode.FastBeyond360))
            .Join(transform.DOLocalJump(transform.position, 3f, 1, 1f));
    }

    private void SetDeviceColor(Define.PastelColor color)
    {
        Material prevMat = meshRenderer.materials[0];
        Material nextMat = Resources.Load<Material>($"Materials/Pastel_GroupA/{ownColor.ToString()}");
        meshRenderer.materials[0] = nextMat;

        colorSequence = DOTween.Sequence()
            .Append(meshRenderer.materials[0].DOColor(nextMat.color, 1f).From(prevMat.color));
    }

    private void StartConnect()
    {
        switch (ownColor)
        {
            case Define.PastelColor.Pink:
                NetworkManager.Instance.connectType = Define.ConnectType.Server;
                NetworkManager.Instance.StartServer();
                break;
            default:
                NetworkManager.Instance.connectType = Define.ConnectType.Client;
                NetworkManager.Instance.StartClient();
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Flip();
        SetDeviceColor(ownColor);
        StartConnect();
    }
}
