using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
    
    // [minValue,maxValue]의 int 중 중복하지 않게 amount개를 뽑아서 return 합니다.
    public static List<int> GetCombinationInt(int minValue, int maxValue, int amount)
    {
        List<int> available = new List<int>();
        List<int> selected = new List<int>();
        
        for (int i = minValue; i <= maxValue; i++)
        {
            available.Add(i);
        }

        for (int i = available.Count - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i);
            (available[i], available[randomIndex]) = (available[randomIndex], available[i]);
        }

        for (int i = 0; i < amount; i++)
        {
            selected.Add(available[i]);
        }

        return selected;
    }
    
    // inputList 중 amount 개의 데이터를 뽑고, Copy하여 return 합니다.
    public static List<T> GetCombination<T>(List<T> inputList, int amount)
    {
        List<T> available = new List<T>(inputList);
        List<T> selected = new List<T>();
            
        for (int i = available.Count - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i);
            (available[i], available[randomIndex]) = (available[randomIndex], available[i]);
        }
        
        for (int i = 0; i < amount; i++)
        {
            selected.Add(available[i]);
        }
        
        return selected;
    }

    public static int ReturnMin(int a, int b)
    {
        int result = (a < b) ? a : b;
        return result;
    }

    public static int ReturnMax(int a, int b)
    {
        int result = (a > b) ? a : b;
        return result;
    }
}
