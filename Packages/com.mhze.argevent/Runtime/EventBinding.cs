using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ArgEvent
{
    [Serializable]
    public class ArgEventBinding
    {
        [SerializeField] private List<Listener> _listeners = new List<Listener>();

        public int ListenerCount => _listeners.Count;

        public IReadOnlyList<Listener> Listeners => _listeners;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke()
        {
            List<Listener> listeners = _listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke();
        }

        public Listener AddListener()
        {
            var listener = new Listener();
            _listeners.Add(listener);
            return listener;
        }

        public void RemoveListener(Listener listener)
        {
            _listeners.Remove(listener);
        }

        public void RemoveListenerAt(int index)
        {
            if (index >= 0 && index < _listeners.Count)
                _listeners.RemoveAt(index);
        }

        public void MoveListener(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= _listeners.Count) return;
            if (toIndex < 0 || toIndex >= _listeners.Count) return;
            if (fromIndex == toIndex) return;
            var listener = _listeners[fromIndex];
            _listeners.RemoveAt(fromIndex);
            _listeners.Insert(toIndex, listener);
        }

        public void Clear()
        {
            _listeners.Clear();
        }
    }

    [Serializable]
    public class ArgEventBinding<T> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T arg)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2 });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2, T3> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2, arg3 });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2, T3, T4> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2, arg3, arg4 });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2, T3, T4, T5> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2, arg3, arg4, arg5 });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2, T3, T4, T5, T6> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2, T3, T4, T5, T6, T7> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        }
    }

    [Serializable]
    public class ArgEventBinding<T1, T2, T3, T4, T5, T6, T7, T8> : ArgEventBinding
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            var listeners = Listeners;
            int count = listeners.Count;
            for (int i = 0; i < count; i++)
                listeners[i].Invoke(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        }
    }
}
