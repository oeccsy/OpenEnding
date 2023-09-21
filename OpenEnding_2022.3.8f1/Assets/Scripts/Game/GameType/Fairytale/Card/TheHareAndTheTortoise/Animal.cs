using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Define.Act curAction = Define.Act.Walk;
    public Define.Shape curShape = Define.Shape.Eyes_Blink;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        ActNaturally(curAction);
        Shape(curShape);
    }

    public void ActImmediately(Define.Act act)
    {
        curAction = act;
        animator.Play(act.ToString());
    }
    public void ActNaturally(Define.Act act, float transitionDuration = 0.5f)
    {
        curAction = act;
        animator.CrossFade(act.ToString(), transitionDuration);
    }

    public void ActWithNormalizedTime(Define.Act act, float normalizedTime = 0.2f)
    {
        curAction = act;
        animator.CrossFade(act.ToString(), 0.5f, -1, normalizedTime);
    }

    public void Shape(Define.Shape shape)
    {
        curShape = shape;
        animator.Play(shape.ToString());
    }
}