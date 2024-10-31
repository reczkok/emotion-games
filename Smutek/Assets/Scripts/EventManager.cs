using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    // Start is called before the first frame update
    public static EventManager Instance
    {
        get
        {
            if (eventManager) return eventManager;
            eventManager = FindFirstObjectByType<EventManager>();

            if (!eventManager)
            {
                Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
            }
            else
            {
                eventManager.Init();
            }

            return eventManager;
        }
    }
    void Init()
    {
        eventDictionary ??= new Dictionary<string, UnityEvent>();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (!eventManager) return;
        if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke();
        }
    }

}
