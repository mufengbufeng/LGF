using System;
using System.Collections.Generic;

namespace LGF.Event
{
    public class EventManager : MonoSingleton<EventManager>
    {
        private Dictionary<string, Action<object[]>> _eventDictionay;

        private static EventManager eventManager;

        void Awake()
        {
            _eventDictionay = new Dictionary<string, Action<object[]>>();
        }

        /**
         * 添加事件监听
         */
        public void Add(string eventName, Action<object[]> listener)
        {
            if (Instance._eventDictionay.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent += listener;
                Instance._eventDictionay[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                Instance._eventDictionay.Add(eventName, thisEvent);
            }
        }

        /**
         * 移除事件监听
         */
        public void Off(string eventName, Action<object[]> listener)
        {
            if (eventManager == null) return;
            if (Instance._eventDictionay.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent -= listener;
                Instance._eventDictionay[eventName] = thisEvent;
            }
        }

        /**
         * 触发事件
         */
        public void Trigger(string eventName, params object[] message)
        {
            if (Instance._eventDictionay.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke(message);
            }
        }
    }
}