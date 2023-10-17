using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class ThereAreAlwaysMemos : Fairytale_Card
{
    public Transform camFollow;
    public List<Memo> memoList = new List<Memo>();
    public List<Vector2> availableMemoPosList = new List<Vector2>();
    
    public List<MemoData> memoDatas = new List<MemoData>();
    
    private Coroutine currentStoryRoutine = null;
    
    protected override void Awake()
    {
#if !UNITY_EDITOR
        base.Awake();
#endif
        InitMemo();
    }

    private void InitMemo()
    {
        availableMemoPosList = PoissonDiscSampling.GeneratePoints(1.8f, new Vector2(20, 5));
        var memoPrefab = Resources.Load<GameObject>("Prefabs/Memo");

        for (int i = 0; i < 3; i++)
        {
            var newMemo = Instantiate(memoPrefab, transform).GetComponent<Memo>();
            newMemo.InitMemo(memoDatas[Random.Range(0, memoDatas.Count)]);
            newMemo.transform.position = new Vector3(availableMemoPosList[i].x, availableMemoPosList[i].y, 0);
            memoList.Add(newMemo);
        }
    }

    private IEnumerator AddMemo()
    {
        var memoPrefab = Resources.Load<GameObject>("Prefabs/Memo");
        var newMemo = Instantiate(memoPrefab, transform).GetComponent<Memo>();
        newMemo.InitMemo(memoDatas[Random.Range(0, memoDatas.Count)]);
        newMemo.transform.position = new Vector3(availableMemoPosList[cardData.goal - cardData.achievement].x, availableMemoPosList[cardData.goal - cardData.achievement].y, -10);
        memoList.Add(newMemo);

        cardData.achievement--;
        
        yield return new WaitUntil(() => cardData.displayedFace == Define.DisplayedFace.Head);
        yield return new WaitForSecondsRealtime(3f);
        
        camFollow.position = new Vector3(newMemo.transform.position.x, camFollow.position.y, 0);

        yield return new WaitForSecondsRealtime(1f);

        newMemo.Attach();

        foreach (var memo in memoList)
        {
            if(memo == newMemo) continue;
            
            float delay = Vector3.Distance(newMemo.transform.position, memo.transform.position) - 9.5f;
            memo.Flutter(delay);
        }
    }

    private IEnumerator RemoveMemo()
    {
        var targetMemo = memoList[cardData.goal - cardData.achievement - 1];
        memoList.Remove(targetMemo);
        cardData.achievement++;
        
        camFollow.position = new Vector3(targetMemo.transform.position.x, camFollow.position.y, 0);
        
        yield return new WaitUntil(() => cardData.displayedFace == Define.DisplayedFace.Head);
        yield return new WaitForSecondsRealtime(3f);
        
        targetMemo.Remove();
        
        foreach (var memo in memoList)
        {
            float delay = Vector3.Distance(targetMemo.transform.position, memo.transform.position) * 0.3f;
            memo.Flutter(delay);
        }

        yield return new WaitForSecondsRealtime(3f);

        if (cardData.goal <= cardData.achievement) yield break;
        targetMemo = memoList[cardData.goal - cardData.achievement - 1];
        camFollow.position = new Vector3(targetMemo.transform.position.x, camFollow.position.y, 0);
    }

    [ContextMenu("FuncTest/ShowNextStep")]
    public override void StoryUnfoldsByTimeStep(int timeStep)
    {
        if (cardData.cardStatus != Define.FairyTaleGameCardStatus.Playing) return;
        if (cardData.runningTime <= timeStep) return;

        cardData.timeStep = timeStep;

        switch (cardData.storyLine[timeStep])
        {
            case Define.Story.TakeOneStep:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(RemoveMemo());
                break;
            case Define.Story.TakeStepBack:
                if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
                currentStoryRoutine = StartCoroutine(AddMemo());
                break;
        }
    }

    public override void GiveUp()
    {
        cardData.cardStatus = Define.FairyTaleGameCardStatus.GiveUp;
        if(currentStoryRoutine != null) StopCoroutine(currentStoryRoutine);
        PostProcess.SetPostProcess(Define.PostProcess.GrayScale);
    }
}
