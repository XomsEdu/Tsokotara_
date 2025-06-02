using System;
using System.Collections;
using UnityEngine;


public class GameState : MonoBehaviour
{
    public static GameState instance;
    public int dayTime, dayCount;
    public float inGameHour = 120;
    public Action<float> tickDayTime;

    public void Awake() => instance = this;
    private void Start() => StartCoroutine(DayTick());

    public IEnumerator DayTick()
        {
            while (true)
                {
                    yield return new WaitForSeconds(inGameHour);
                    dayTime += 1;
                    
                    if (dayTime >= 24)
                        {
                            dayCount += 1;
                            dayTime = 0;
                        }

                    tickDayTime?.Invoke(dayTime);
                }
        }
}