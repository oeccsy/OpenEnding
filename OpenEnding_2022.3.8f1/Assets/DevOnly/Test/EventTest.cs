using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTest : MonoBehaviour
{
    public EventTestEvent myEvent;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("EventTest2");
    }

    private void TestFunc()
    {
        Debug.Log("TestFunc");
    }

    private void OnEnable()
    {
        myEvent.OnTest += TestFunc;
    }

    private void OnDisable()
    {
        myEvent.OnTest -= TestFunc;
    }
}
