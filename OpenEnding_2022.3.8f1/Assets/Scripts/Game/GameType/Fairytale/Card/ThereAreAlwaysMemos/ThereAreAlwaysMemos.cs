using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThereAreAlwaysMemos : Fairytale_Card
{
    public Transform camFollow;
    public List<Memo> memoList = new List<Memo>();
    public List<Vector2> availableMemoPosList = new List<Vector2>();
    
    private Coroutine currentStoryRoutine = null;
    
    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
        InitMemo();
        cardData.goal = 3;
        cardData.achievement = 0;
    }

    private void InitMemo()
    {
        availableMemoPosList = PoissonDiscSampling.GeneratePoints(1.8f, new Vector2(8, 5));
        var prefab = Resources.Load<GameObject>("Prefabs/Memo");

        for (int i = 0; i < 3; i++)
        {
            var instance = Instantiate(prefab, transform);
            instance.transform.position = new Vector3(availableMemoPosList[i].x, availableMemoPosList[i].y, 0);
            memoList.Add(instance.GetComponent<Memo>());
        }
    }

    private void AddMemo()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Memo");
        var instance = Instantiate(prefab, transform);
        instance.transform.position = new Vector3(availableMemoPosList[cardData.goal - cardData.achievement].x, availableMemoPosList[cardData.goal - cardData.achievement].y, 0);
        memoList.Add(instance.GetComponent<Memo>());

        camFollow.position = new Vector3(instance.transform.position.x, camFollow.position.y, 0);

        cardData.achievement--;
    }

    private void RemoveMemo()
    {
        var targetMemo = memoList[cardData.goal - cardData.achievement - 1];
        memoList.Remove(targetMemo);
        Destroy(targetMemo.gameObject);

        camFollow.position = new Vector3(targetMemo.transform.position.x, camFollow.position.y, 0);
        
        cardData.achievement++;
        
        targetMemo = memoList[cardData.goal - cardData.achievement - 1];
        camFollow.position = new Vector3(targetMemo.transform.position.x, camFollow.position.y, 0);
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
        
        Debug.Log($"{timeStep} : {cardData.storyLine[timeStep].ToString()}");
        
        switch (cardData.storyLine[timeStep])
        {
            case Define.Story.TakeOneStep:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                //currentStoryRoutine = StartCoroutine(TortoiseRunFast());
                RemoveMemo();
                break;
            case Define.Story.TakeStepBack:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                //currentStoryRoutine = StartCoroutine(TortoiseDance());
                AddMemo();
                break;
        }

        if (cardData.achievement == cardData.goal)
        {
            ShowResult();
        }
    }
}
