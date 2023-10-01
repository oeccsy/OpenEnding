using UnityEngine;

public class Animal : MonoBehaviour
{
    public Define.Act curAction = Define.Act.Walk;
    public Define.Shape curShape = Define.Shape.Eyes_Blink;
    public float speed;
    
    private Animator _animator;
    public Orbit orbit;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        orbit = GetComponent<Orbit>();
        ActNaturally(Define.Act.Walk);
        Shape(Define.Shape.Eyes_Blink);
    }

    public void Update()
    {
        orbit.Theta += speed * Time.deltaTime;
    }

    public void ActImmediately(Define.Act act)
    {
        curAction = act;
        _animator.Play(act.ToString());
    }
    public void ActNaturally(Define.Act act, float transitionDuration = 0.5f)
    {
        curAction = act;
        _animator.CrossFade(act.ToString(), transitionDuration);
    }

    public void ActWithNormalizedTime(Define.Act act, float normalizedTime = 0.2f)
    {
        curAction = act;
        _animator.CrossFade(act.ToString(), 0.5f, -1, normalizedTime);
    }

    public void Shape(Define.Shape shape)
    {
        curShape = shape;
        _animator.Play(shape.ToString());
    }
}