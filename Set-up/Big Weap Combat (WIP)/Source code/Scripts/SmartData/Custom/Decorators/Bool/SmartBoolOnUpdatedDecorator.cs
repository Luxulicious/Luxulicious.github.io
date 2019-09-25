using System;
using Sigtrap.Relays;
using SmartData;
using SmartData.Abstract;
using UnityEngine;
using BoolEvent = SmartData.SmartBool.Data.BoolVar.BoolEvent;

namespace Assets.SmartData.Custom.Decorators
{
    [Serializable]
    [DecoratorDescription("Invokes events during OnUpdated. " +
                          "Can be used to invoke from SmartObject to ScriptableObject." +
                          "In this case ensure that the events are set to both \"Editor and runtime\"")]
    public class SmartBoolOnUpdatedDecorator : SmartDataDecoratorBase<bool>
    {
        [SerializeField] private InvertibleBoolUpdate[] _callbacks;

        public override bool OnUpdated(bool newValue)
        {
            foreach (var toUpdate in _callbacks)
            {
                toUpdate.OnUpdated(newValue);
            }
            return base.OnUpdated(newValue);
        }
        
    }

    //TODO Maybe implement IRelayLink
    [Serializable]
    public struct InvertibleBoolUpdate
    {
        [SerializeField] private bool _invert;
        [SerializeField] private BoolEvent _onUpdated;

        public bool Invert
        {
            get { return _invert; }
            set { _invert = value; }
        }

        public void OnUpdated(bool value)
        {
            if (_invert)
                _onUpdated.Invoke(!value);
            else
                _onUpdated.Invoke(value);
        }
    }
}