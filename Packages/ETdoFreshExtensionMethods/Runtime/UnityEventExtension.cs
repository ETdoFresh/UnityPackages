using UnityEngine.Events;

namespace ETdoFresh.UnityPackages.ExtensionMethods
{
    public static class UnityEventExtension
    {
        public static void AddPersistentListener(this UnityEvent unityEvent, UnityAction action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, action);
#else
            unityEvent.AddListener(action);
#endif
        }

        public static void RemovePersistentListener(this UnityEvent unityEvent, UnityAction action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, action);
#else
            unityEvent.RemoveListener(action);
#endif
        }

        public static void AddPersistentListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, action);
#else
            unityEvent.AddListener(action);
#endif
        }

        public static void RemovePersistentListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, action);
#else
            unityEvent.RemoveListener(action);
#endif
        }

        public static void AddPersistentListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, action);
#else
            unityEvent.AddListener(action);
#endif
        }

        public static void RemovePersistentListener<T0, T1>(this UnityEvent<T0, T1> unityEvent,
            UnityAction<T0, T1> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, action);
#else
            unityEvent.RemoveListener(action);
#endif
        }

        public static void AddPersistentListener<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent,
            UnityAction<T0, T1, T2> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, action);
#else
            unityEvent.AddListener(action);
#endif
        }

        public static void RemovePersistentListener<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent,
            UnityAction<T0, T1, T2> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, action);
#else
            unityEvent.RemoveListener(action);
#endif
        }

        public static void AddPersistentListener<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent,
            UnityAction<T0, T1, T2, T3> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, action);
#else
            unityEvent.AddListener(action);
#endif
        }

        public static void RemovePersistentListener<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent,
            UnityAction<T0, T1, T2, T3> action)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, action);
#else
            unityEvent.RemoveListener(action);
#endif
        }
    }
}