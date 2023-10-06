using System.Collections;
using System.Collections.Generic;
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
        
    }
    
    public void Update()
    {
        //boneTransform.Rotate(Vector3.right * Time.deltaTime);
    }

    public void InitMemo()
    {
        
    }
}
