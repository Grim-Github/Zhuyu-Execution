using Lexone.UnityTwitchChat;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct MessageEvent
{
    public string message;
    public int timesToExecute;
    public int times;
    public UnityEvent _OnTimesReach;

}


public class TwitchMessageEventExecutor : MonoBehaviour
{
    [SerializeField] public MessageEvent[] events;

    public void TryToParseMessageOnEvents(string message)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (message.Contains(events[i].message))
            {
                events[i].times++;
                Debug.Log($"<color=#449310><b>[EXECUTOR]</b></color> Detected message: " + events[i].message);


                if (events[i].times >= events[i].timesToExecute)
                {
                    events[i]._OnTimesReach.Invoke();
                    events[i].times = 0;
                    Debug.Log($"<color=#449310><b>[EXECUTOR]</b></color> Events started for : " + events[i].message);

                }
            }
        }
    }


}
