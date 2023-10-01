using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheHareAndTheTortoise : Fairytale_Card
{
    public List<Define.Story> storyLine = new List<Define.Story>();
    public List<int> achievementProgress = new List<int>();
    private Coroutine currentStoryRoutine = null;
    
    [SerializeField]
    private Animal hare;
    [SerializeField]
    private Animal tortoise;

    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
    }

    public override void Update()
    {
#if !UNITY_EDITOR
        base.Update();
#endif
        
        //Debug.Log((hare.orbit.Theta - tortoise.orbit.Theta) %360);
    }
    
    public override void CreateStoryLine(int goal, int runningTime)
    {
        $"Create Story Line : {goal}, {runningTime}".Log();
        cardData.runningTime = runningTime;
        cardData.goal = goal;

        int temporaryAchievement = 0;

        for (int i = 0; i < runningTime; i++)
        {
            SelectStory(i, runningTime, ref temporaryAchievement, goal);
        }
    }

    public override void SelectStory(int timeStep, int runningTime, ref int temporaryAchievement, int goal)
    {
        List<Define.Story> availableStory = new List<Define.Story>();

        if (timeStep + 1 == runningTime || temporaryAchievement + 1 != goal)
        {
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
            availableStory.Add(Define.Story.TakeOneStep);
        }

        if (temporaryAchievement + runningTime - timeStep - 1 >= goal)
        {
            availableStory.Add(Define.Story.Standstill);
            availableStory.Add(Define.Story.Standstill);
        }

        if (temporaryAchievement - 1 + runningTime - timeStep - 1 >= goal && temporaryAchievement > 0)
        {
            availableStory.Add(Define.Story.TakeStepBack);
            availableStory.Add(Define.Story.TakeStepBack);
        }

        if (runningTime - timeStep - 1 >= goal && temporaryAchievement > 0)
        {
            availableStory.Add(Define.Story.LoseAll);
        }

        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
        Utils.ListRandomShuffle(availableStory);
            
        switch (availableStory[Random.Range(0, availableStory.Count)])
        {
            case Define.Story.LoseAll:
                temporaryAchievement = 0;
                storyLine.Add(Define.Story.LoseAll);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.Standstill:
                storyLine.Add(Define.Story.Standstill);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.TakeStepBack:
                temporaryAchievement--;
                storyLine.Add(Define.Story.TakeStepBack);
                achievementProgress.Add(temporaryAchievement);
                break;
            case Define.Story.TakeOneStep:
                temporaryAchievement++;
                storyLine.Add(Define.Story.TakeOneStep);
                achievementProgress.Add(temporaryAchievement);
                break;
        }
    }

    private IEnumerator TortoiseRunFast()
    {
        "OneStep".Log();
        tortoise.orbit.Theta = hare.orbit.Theta - (cardData.goal - 1 - cardData.achievement) * 10;
        tortoise.speed = 4f;
        tortoise.ActImmediately(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        
        cardData.achievement++;
        
        yield return new WaitForSecondsRealtime(3f);
        
        tortoise.ActNaturally(Define.Act.Run);
        tortoise.Shape(Define.Shape.Eyes_Annoyed);
        tortoise.speed = 6f;
        
        yield return new WaitUntil(() => (hare.orbit.Theta - tortoise.orbit.Theta) % 360 <= (cardData.goal - 1 - cardData.achievement) * 10);

        tortoise.ActNaturally(Define.Act.Walk);
        tortoise.Shape(Define.Shape.Eyes_Blink);
        tortoise.speed = 4f;
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
        butterfliesOrbit.Theta = tortoise.orbit.Theta + 40f;
        
        yield return new WaitUntil( ()=> (butterfliesOrbit.Theta - tortoise.orbit.Theta) % 360 <= 0f);

        tortoise.ActNaturally(Define.Act.Jump);
        tortoise.Shape(Define.Shape.Eyes_LookUp);
        tortoise.speed = 0f;

        yield return new WaitForSecondsRealtime(2f);

        tortoise.ActNaturally(Define.Act.Spin);
        tortoise.Shape(Define.Shape.Eyes_Happy);
        tortoise.speed = 0f;
        
        yield return new WaitForSecondsRealtime(4f);
        
        tortoise.ActNaturally(Define.Act.Run);
        tortoise.Shape(Define.Shape.Eyes_Annoyed);
        tortoise.speed = 8f;

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
        if (cardData.cardStatus != Define.FairyTailGameCardStatus.None) return;
        if (cardData.runningTime <= timeStep) return;

        cardData.timeStep = timeStep;
        
        Debug.Log($"{timeStep} : {storyLine[timeStep].ToString()}");
        
        switch (storyLine[timeStep])
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
}