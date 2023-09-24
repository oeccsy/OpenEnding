using System.Collections.Generic;
using UnityEngine;

namespace DevOnly.Test
{
    public class StoryLineTest : MonoBehaviour
    {
        public List<Define.Story> storyLine = new List<Define.Story>();
        public List<int> debugList = new List<int>();

        private void Awake()
        {
            CreateStoryLine(5, 20);
        }
        
        public void CreateStoryLine(int goal, int runningTime)
        {
            int cur = 0;

            for (int i = 0; i < runningTime; i++)
            {
                SetTimeLine(i, runningTime, ref cur, goal);
            }
        }

        public void SetTimeLine(int timeStep, int runningTime, ref int cur, int goal)
        {
            List<Define.Story> availableStory = new List<Define.Story>();

            if (timeStep + 1 == runningTime || cur + 1 != goal)
            {
                availableStory.Add(Define.Story.TakeOneStep);
                availableStory.Add(Define.Story.TakeOneStep);
                availableStory.Add(Define.Story.TakeOneStep);
                availableStory.Add(Define.Story.TakeOneStep);
            }

            if (cur + runningTime - timeStep - 1 >= goal)
            {
                availableStory.Add(Define.Story.Standstill);
                availableStory.Add(Define.Story.Standstill);
            }

            if (cur - 1 + runningTime - timeStep - 1 >= goal && cur > 0)
            {
                availableStory.Add(Define.Story.TakeStepBack);
                availableStory.Add(Define.Story.TakeStepBack);
            }

            if (runningTime - timeStep - 1 >= goal && cur > 0)
            {
                availableStory.Add(Define.Story.LoseAll);
            }

            Utils.ListRandomShuffle(availableStory);
            Utils.ListRandomShuffle(availableStory);
            Utils.ListRandomShuffle(availableStory);
            
            switch (availableStory[Random.Range(0, availableStory.Count)])
            {
                case Define.Story.LoseAll:
                    cur = 0;
                    storyLine.Add(Define.Story.LoseAll);
                    debugList.Add(cur);
                    break;
                case Define.Story.Standstill:
                    storyLine.Add(Define.Story.Standstill);
                    debugList.Add(cur);
                    break;
                case Define.Story.TakeStepBack:
                    cur -= 1;
                    storyLine.Add(Define.Story.TakeStepBack);
                    debugList.Add(cur);
                    break;
                case Define.Story.TakeOneStep:
                    cur += 1;
                    storyLine.Add(Define.Story.TakeOneStep);
                    debugList.Add(cur);
                    break;
            }
        }
    }
}