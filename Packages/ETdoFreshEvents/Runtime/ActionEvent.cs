using System;
using System.Collections.Generic;
using ETdoFreshExtensionMethods;
using UnityEngine;

namespace ETdoFreshEvents
{
    [CreateAssetMenu(menuName = "Events/ActionEvent")]
    public class ActionEvent : ScriptableObject
    {
        private event Action Action = delegate { };
        public List<EventData> eventData = new List<EventData>();

        public void Raise()
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

        public static ActionEvent FindEventInEditor(string eventName) =>
            AssetDatabaseUtil.FindObjectOfType<ActionEvent>(eventName);
    }

    public abstract class ActionEvent<T> : ScriptableObject
    {
        private event Action<T> Action = delegate { };
        public List<EventData<T>> eventData = new List<EventData<T>>();

        public void Raise(T data)
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