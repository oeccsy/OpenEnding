using System.Collections;
using UnityEngine;

public abstract class GameFramework : MonoBehaviour
{
    protected Coroutine gameFramework = null;
    protected abstract IEnumerator GameFrameworkCoroutine();

    public void StartGameFramework()
    {
        if (gameFramework != null) StopCoroutine(gameFramework);
        
        gameFramework = StartCoroutine(GameFrameworkCoroutine());
    }

    public void StopGameFramework()
    {
        if (gameFramework != null)
        {
            StopCoroutine(gameFramework);
        }
    }

    public virtual void GameOver()
    {
        StopGameFramework();
    }
}
