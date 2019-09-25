using System;
using Assets.Scripts.SmartData.Custom.Collection.Bool;
using BoolEvent = SmartData.SmartBool.Data.BoolVar.BoolEvent;
using SmartData.SmartBool.Data;
using UnityEngine;

namespace Assets.Scripts.SmartData.Custom.Readers
{
    //TODO Get OnUpdated event from reference
    [Serializable]
    public class RichBoolReader
    {
        //TODO Add a custom picker that only allows of types specified
        [Observe("CheckValidReference")] [SerializeField]
        private ScriptableObject _reference;

        [SerializeField] private bool _invert;

        //TODO This does not get observed
        public bool CheckValidReference()
        {
            if (_reference as InvertibleBoolVars == null && _reference as BoolVar == null)
            {
                Debug.LogError(NeitherTypeMessage());
                _reference = null;
                return false;
            }

            return true;
        }

        public bool value
        {
            get
            {
                bool? result = null;
                try
                {
                    result = ((InvertibleBoolVars) _reference).value;
                }
                catch (InvalidCastException)
                {
                    try
                    {
                        result = ((BoolVar) _reference).value;
                    }
                    catch (InvalidCastException)
                    {
                        var v = _reference;
                        _reference = null;
                        throw new Exception(NeitherTypeMessage());
                    }
                }

                if (_invert != true)
                    return result.Value;
                else return !result.Value;
            }
        }

        private string NeitherTypeMessage()
        {
            return _reference + " is neither of type " + typeof(InvertibleBoolVars).Name + " or " +
                   typeof(BoolVar).Name;
        }
    }
}