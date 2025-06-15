using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MGLibrary
{
    public static class GlobalRegistry
    {
        public enum RegistryKey
        {
            Player,
            Camera,
        }

        private static readonly Dictionary<RegistryKey, object> RegistryDic = new();

        // Object 등록
        public static void RegisterObject(RegistryKey key, object obj)
        {
            if (RegistryDic.ContainsKey(key))
            {
                Debug.LogWarning($"Object with key '{key}' is already registered. Replacing with new object.");
                RegistryDic[key] = obj; // 기존 객체를 대체
            }
            else
            {
                RegistryDic.Add(key, obj);
            }
        }

        // Object 가져오기
        public static T GetObject<T>(RegistryKey key) where T : class
        {
            if (RegistryDic.TryGetValue(key, out var obj))
            {
                return obj as T;
            }
            else
            {
                Debug.LogWarning($"Object with key '{key}' not found.");
                return null;
            }
        }
    }
}