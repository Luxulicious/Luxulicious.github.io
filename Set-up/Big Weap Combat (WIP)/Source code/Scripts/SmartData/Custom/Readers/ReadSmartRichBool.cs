using System;
using UnityEngine;
using UnityEngine.Events;
using BoolEvent = SmartData.SmartBool.Data.BoolVar.BoolEvent;

namespace Assets.Scripts.SmartData.Custom.Readers
{
    //TODO Event invocation takes a while to set-up this way, ultimately it should just come from the scriptable object "layer"...
    [AddComponentMenu("SmartData/Bool/Custom/Collection/Read Smart Rich Bool", 0)]
    public class ReadSmartRichBool : MonoBehaviour
    {
        //TODO Maybe turn this struct into a scriptable object
        [Serializable]
        internal struct PerformanceSettings
        {
            internal enum CheckTiming
            {
                FixedUpdate,
                Update
            };

            [SerializeField] internal CheckTiming _timing;
        }

        [SerializeField] private RichBoolReader _reader = new RichBoolReader();
        [SerializeField] private BoolEvent _onUpdated = new BoolEvent();
        private bool? _currentValue;
        [SerializeField] internal bool _autoListen;
        [SerializeField] private PerformanceSettings _settings = new PerformanceSettings();


        private void CheckValidReference()
        {
            _reader.CheckValidReference();
        }

        private void Update()
        {
            if (_settings._timing == PerformanceSettings.CheckTiming.Update) Listen();
        }

        private void FixedUpdate()
        {
            if (_settings._timing == PerformanceSettings.CheckTiming.FixedUpdate) Listen();
        }

        private void Listen()
        {
            if (_autoListen)
                Dispatch();
            else
                CheckForUpdate();
        }

        public void CheckForUpdate()
        {
            if (_currentValue != null)
            {
                if (_currentValue != _reader.value)
                {
                    _currentValue = _reader.value;
                    Dispatch();
                }
            }
            else
            {
                _currentValue = _reader.value;
                Dispatch();
            }
        }


        public void Dispatch()
        {
            _onUpdated.Invoke(_reader.value);
        }

        public bool value
        {
            get { return _reader.value; }
        }
    }
}