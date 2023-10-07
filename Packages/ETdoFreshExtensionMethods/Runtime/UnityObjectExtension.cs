using UnityEngine;

namespace ETdoFresh.UnityPackages.ExtensionMethods
{
    public static class UnityObjectExtension
    {
        public static string GetObjectPath(this Object obj)
        {
            if (obj is GameObject gameObject)
            {
                if (gameObject.transform.parent == null)
                    return gameObject.name;
                return gameObject.transform.parent.GetObjectPath() + "/" + gameObject.name;
            }
            if (obj is Component component)
            {
                if (component.transform.parent == null)
                    return component.name;
                return component.transform.parent.GetObjectPath() + "/" + component.name;
            }
            return obj.name;
        } 
    }
}