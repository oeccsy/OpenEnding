using System.Collections.Generic;
using UnityEngine;

public class TortoiseCardStoryLineFactory : Fairytale_StorylineFactory
{
    protected override Define.Story SelectStory(int timeStep, int runningTime, int temporaryAchievement, int goal)
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
        
        return availableStory[Random.Range(0, availableStory.Count)];
    }
}