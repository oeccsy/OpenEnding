using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class TheHareAndTheTortoise : Fairytale_Card
{
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;

    private Coroutine currentStoryRoutine = null;
    
    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
    }

    private IEnumerator TortoiseRunFast()
    {
        "OneStep".Log();
        tortoise.orbit.Theta = hare.orbit.Theta - (cardData.goal - 1 - cardData.achievement) * 10;
        tortoise.speed = 4f;
        tortoise.ActImmediately(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        
        cardData.achievement++;
        
        yield return new WaitUntil(() => cardData.displayedFace == Define.DisplayedFace.Head);
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Run);
        tortoise.Shape(Define.Shape.Eyes_Annoyed);
        tortoise.speed = 6f;
        var streamPrefab = Resources.Load<GameObject>("Prefabs/Stream");
        var streamInstance = Instantiate(streamPrefab, tortoise.transform);
        
        yield return new WaitUntil(() => (hare.orbit.Theta - tortoise.orbit.Theta) % 360 <= (cardData.goal - 1 - cardData.achievement) * 10);

        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
        
        yield return new WaitForSecondsRealtime(1f);
        
        Destroy(streamInstance);
    }

    private IEnumerator TortoiseRunSlow()
    {
        "Slow".Log();
        tortoise.orbit.Theta = hare.orbit.Theta - (cardData.goal - 1 - cardData.achievement) * 10;
        tortoise.speed = 4f;
        tortoise.ActImmediately(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        
        yield return null;

        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Sad);
    }

    private IEnumerator TortoiseDance()
    {
        "Dance".Log();
        tortoise.orbit.Theta = hare.orbit.Theta - (cardData.goal - 1 - cardData.achievement) * 10;
        tortoise.speed = 4f;
        tortoise.ActImmediately(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        
        cardData.achievement--;
        
        yield return new WaitUntil(() => cardData.displayedFace == Define.DisplayedFace.Head);
        
        var butterfliesPrefab = Resources.Load<GameObject>("Prefabs/Butterflies");
        var butterfliesInstance = Instantiate(butterfliesPrefab, transform);
        var butterfliesOrbit = butterfliesInstance.GetComponent<Orbit>();
        butterfliesOrbit.Theta = tortoise.orbit.Theta + 20f;
        
        yield return new WaitUntil( ()=> (butterfliesOrbit.Theta - tortoise.orbit.Theta) % 360 <= 0f);

        tortoise.ActNaturally(Define.Act.Idle_A);
        tortoise.Shape(Define.Shape.Eyes_LookUp);
        tortoise.speed = 0f;
        yield return new WaitForSecondsRealtime(1f);
        tortoise.ActNaturally(Define.Act.Idle_B);
        yield return new WaitForSecondsRealtime(1f);
        tortoise.ActNaturally(Define.Act.Idle_C);
        
        yield return new WaitForSecondsRealtime(1f);
        
        tortoise.ActNaturally(Define.Act.Jump);
        tortoise.Shape(Define.Shape.Eyes_Excited);

        yield return new WaitForSecondsRealtime(2f);

        tortoise.ActNaturally(Define.Act.Spin);
        tortoise.Shape(Define.Shape.Eyes_Happy);
        tortoise.speed = 0f;
        
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Idle_A);
        tortoise.Shape(Define.Shape.Eyes_Squint);
        
        yield return new WaitForSecondsRealtime(0.5f);
        
        tortoise.ActNaturally(Define.Act.Run);
        tortoise.Shape(Define.Shape.Teardrop_R);
        tortoise.speed = 20f;

        yield return new WaitUntil(() => (hare.orbit.Theta - tortoise.orbit.Theta) % 360 <= (cardData.goal - 1 - cardData.achievement) * 10);
        
        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
        
        Destroy(butterfliesInstance);
    }

    private IEnumerator TortoiseAccident()
    {
        "Accident".Log();
        tortoise.orbit.Theta = hare.orbit.Theta - (cardData.goal - 1 - cardData.achievement) * 10;
        tortoise.speed = 4f;
        tortoise.ActImmediately(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        
        cardData.achievement = 0;
        
        yield return new WaitUntil(() => cardData.displayedFace == Define.DisplayedFace.Head);
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Death);
        tortoise.Shape(Define.Shape.Eyes_Dead);
        tortoise.speed = 0f;
        
        yield return new WaitUntil(()=> (hare.orbit.Theta - tortoise.orbit.Theta) % 360 >= (cardData.goal - 1 - cardData.achievement) * 10);

        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
    }

    private void ShowResult()
    {
        
    }

    [ContextMenu("FuncTest/ShowNextStep")]
    public override void StoryUnfoldsByTimeStep(int timeStep)
    {
        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
        if (cardData.runningTime <= timeStep) return;

        cardData.timeStep = timeStep;

        switch (cardData.storyLine[timeStep])
        {
            case Define.Story.LoseAll:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseAccident());
                break;
            case Define.Story.Standstill:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseRunSlow());
                break;
            case Define.Story.TakeOneStep:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseRunFast());
                break;
            case Define.Story.TakeStepBack:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(TortoiseDance());
                break;
        }

        if (cardData.achievement == cardData.goal)
        {
            ShowResult();
        }
    }
    
    public override void GiveUp()
    {
        cardData.cardStatus = Define.FairyTaleGameCardStatus.GiveUp;
        if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }
}