using System;
using UnityEngine.Events;

[Serializable]
public struct BoolEvents
{
    //TODO Maybe add an OnChange kind of deal here, continuous event invocation might be a bit to much lag

    [Serializable]
    public struct InvokedEvents
    {
        [Serializable]
        public class OnInvokeTrueEvent : UnityEvent
        {
        }

        [Serializable]
        public class OnInvokeFalseEvent : UnityEvent
        {
        }

        [Serializable]
        public class OnInvokeBoolEvent : UnityEvent<bool>
        {
        }

        public OnInvokeTrueEvent onInvokeTrueEvent;
        public OnInvokeFalseEvent onInvokeFalseEvent;
        public OnInvokeBoolEvent onInvokeBoolEvent;
    }

    [Serializable]
    public struct OnUpdateEvents
    {
        [Serializable]
        public class OnUpdateFalseEvent : UnityEvent
        {
        }

        [Serializable]
        public class OnUpdateTrueEvent : UnityEvent
        {
        }

        [Serializable]
        public class OnUpdateBoolEvent : UnityEvent<bool>
        {
        }

        public OnUpdateFalseEvent onUpdateTrueEvent;
        public OnUpdateTrueEvent onUpdateFalseEvent;
        public OnUpdateBoolEvent onUpdateBoolEvent;
    }

    [Serializable]
    public struct OnFixedUpdateEvents
    {
        [Serializable]
        public class OnFixedUpdateFalseEvent : UnityEvent
        {
        }

        [Serializable]
        public class OnFixedUpdateTrueEvent : UnityEvent
        {
        }

        [Serializable]
        public class OnFixedUpdateBoolEvent : UnityEvent<bool>
        {
        }

        public OnFixedUpdateFalseEvent onFixedUpdateTrueEvent;
        public OnFixedUpdateTrueEvent onFixedUpdateFalseEvent;
        public OnFixedUpdateBoolEvent onFixedUpdateBoolEvent;
    }

    public InvokedEvents invokedEvents;
    public OnUpdateEvents onUpdateEvents;
    public OnFixedUpdateEvents onFixedUpdateEvents;
}