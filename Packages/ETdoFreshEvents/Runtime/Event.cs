using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETdoFresh.UnityPackages.Events
{
    [CreateAssetMenu(menuName = "Events/Event")]
    public class Event : ScriptableObject
    {
        private event Action Action = delegate { };
        public List<EventData> eventData = new List<EventData>();

        public void Invoke()
        {
            Action();
        }

        public void AddListener(Action action)
        {
            eventData.Add(new EventData(action));
            Action += action;
        }

        public void RemoveListener(Action action)
        {
            var foundEvent = eventData.Find(x => x.Action == action);
            if (foundEvent != null) eventData.Remove(foundEvent);
            Action -= action;
        }
    }

    public abstract class Event<T> : ScriptableObject
    {
        private event Action<T> Action = delegate { };
        public List<EventData<T>> eventData = new List<EventData<T>>();

        public void Invoke(T data)
        {
            Action(data);
        }

        public void AddListener(Action<T> action)
        {
            eventData.Add(new EventData<T>(action));
            Action += action;
        }

        public void RemoveListener(Action<T> action)
        {
            var foundEvent = eventData.Find(x => x.Action == action);
            if (foundEvent != null) eventData.Remove(foundEvent);
            Action -= action;
        }
    }
}