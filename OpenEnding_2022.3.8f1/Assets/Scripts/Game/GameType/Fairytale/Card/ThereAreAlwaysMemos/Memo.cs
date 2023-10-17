using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Memo : MonoBehaviour
{
    public MemoData memoData;
    
    [SerializeField]
    private Transform boneTransform;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private Image image;
    
    private void Awake()
    {
        InitMemo();
    }

    private void InitMemo()
    {
        
    }

    public void Attach()
    {
        boneTransform.localRotation = Quaternion.Euler(-40, 0, 0);
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveZ(0, 1f).From(-10f).SetEase(Ease.OutCirc))
            .Append(boneTransform.DOLocalRotate(new Vector3(-10, 0, 0), 2f).SetEase(Ease.OutCubic));
    }

    public void Remove()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(boneTransform.DOLocalRotate(new Vector3(-40, 0, 0), 0.5f))
            .Append(transform.DOMoveZ(-10, 1f))
            .AppendCallback(() => Destroy(gameObject));
    }

    public void Flutter(float delay)
    {
        var targetRot = new Vector3(Random.Range(-40f, -30f), 0, 0);
        var endRot = new Vector3(Random.Range(-20f, -10f), 0, 0); 
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .SetDelay(delay)
            .Append(boneTransform.DOLocalRotate(targetRot, 2f)).SetEase(Ease.OutBounce)
            .Append(boneTransform.DOLocalRotate(endRot, 2f));
    }
}
