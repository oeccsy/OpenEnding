using System.Collections.Generic;

public abstract class Fairytale_StorylineFactory
{
    protected abstract Define.Story SelectStory(int timeStep, int runningTime, int temporaryAchievement, int goal);
    
    public List<Define.Story> CreateStoryLine(int goal, int runningTime)
    {
        $"Create Story Line : {goal}, {runningTime}".Log();

        List<Define.Story> storyLine = new List<Define.Story>();
        List<int> achievementProgress = new List<int>();

        int temporaryAchievement = 0;

        for (int i = 0; i < runningTime; i++)
        {
            Define.Story selectedStory = SelectStory(i, runningTime, temporaryAchievement, goal);

            switch (selectedStory)
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

        return storyLine;
    }

    public static List<Define.Story> GetStoryLine(int goal, int runningTime)
    {
        Define.FairyTaleGameCardType cardType = Fairytale_Scene.Instance.card.cardData.cardType;
        Fairytale_StorylineFactory factory = null;
        
        switch (cardType)
        {
            case Define.FairyTaleGameCardType.TheHareAndTheTortoise:
                factory = new TortoiseCardStoryLineFactory();
                break;
            case Define.FairyTaleGameCardType.ThereAreAlwaysMemos:
                factory = new MemoCardStoryLineFactory();
                break;
        }

        List<Define.Story> storyLine = factory.CreateStoryLine(goal, runningTime);
        return storyLine;
    }
}
