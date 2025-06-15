using System.Collections.Generic;
using UnityEngine;

namespace ImprovedTimers
{
    public static class TimerManager
    {
        static readonly List<Timer> timers = new();
        
        public static void RegisterTimer(Timer timer) => timers.Add(timer);
        public static void UnregisterTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateTimers()
        {
            foreach (var timer in new List<Timer>(timers))
            {
                timer.Tick();
            }
            
            // 위 혹은 아래 둘 중 하나 써보시길
            // for (var i = timers.Count - 1; i >= 0; i--)
            // {
            //     timers[i].Tick();
            // }
        }
        
        public static void Clear() => timers.Clear();
    }
}