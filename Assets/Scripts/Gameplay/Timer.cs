using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action<int> SecondElapsed;
    public event Action TimerComplete;
    
    

    private int _secondsLeft;
    
    private Coroutine _timerCoroutine;
    
    
    public void StartTimer(int seconds)
    {
        _secondsLeft = seconds;
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
        _timerCoroutine = StartCoroutine(CountTime());
    }

    private IEnumerator CountTime()
    {

        while (_secondsLeft>0)
        {
            yield return new WaitForSeconds(1f);
            _secondsLeft--;
            SecondElapsed?.Invoke(_secondsLeft);
        }
        TimerComplete?.Invoke();
    }
}
