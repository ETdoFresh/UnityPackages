using System;
using ETdoFresh.UnityPackages.ExtensionMethods;
using Object = UnityEngine.Object;

namespace ETdoFresh.UnityPackages.Events
{
    [Serializable]
    public class EventData
    {
#if UNITY_EDITOR
        public string name;
        public Object sender;
        public string senderName;
#endif

        public Action Action;

        public EventData(Action action)
        {
            Action = action;

#if UNITY_EDITOR
            sender = action.Target as Object;
            senderName = sender != null ? sender.GetObjectPath() : null;
            name = action.Method.DeclaringType != null
                ? action.Method.DeclaringType.FullName + "." + action.Method.Name + "()"
                : action.Method.Name + "()";
#endif
        }
    }


    [Serializable]
    public class EventData<T>
    {
#if UNITY_EDITOR
        public string name;
        public Object sender;
        public string senderName;
#endif

        public Action<T> Action;

        public EventData(Action<T> action)
        {
            Action = action;

#if UNITY_EDITOR
            sender = action.Target as Object;
            senderName = sender != null ? sender.GetObjectPath() : null;
            name = action.Method.DeclaringType != null
                ? action.Method.DeclaringType.FullName + "." + action.Method.Name + $"({typeof(T).Name})"
                : action.Method.Name + $"({typeof(T).Name})";
#endif
        }
    }
}