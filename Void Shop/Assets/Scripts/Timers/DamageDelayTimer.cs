using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageDelayTimer : MonoBehaviour
{
    [SerializeField] private float _damageDelayInterval;
    [SerializeField] private UnityEvent _onTimerOver; //сюда положить функцию применения дамаги (если будет вызываться свой метод)
    private float _currentTime;
    private bool _tick;
    public bool Tick => _tick;
    private void Awake()
    {
        _currentTime = _damageDelayInterval;
        _tick = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tick)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                Reset();
                InvokeTimerMethod();
            }
        }
    }

    private void Reset()
    {
        _currentTime = 0;
        _tick = false;
    }

    private void InvokeTimerMethod()
    {
        if(_onTimerOver != null) 
            _onTimerOver.Invoke();
    }
    public void Restart()
    {
        _tick = true;
        _currentTime = _damageDelayInterval;
    }
}
