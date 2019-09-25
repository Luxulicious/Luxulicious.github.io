using System;
using System.ComponentModel;
using SmartData.SmartBool.Data;
using UnityEngine;

namespace Assets.SmartData.Custom.Collection.Bool
{
    [Serializable]
    public class InvertibleBoolVar
    {
        [SerializeField] private BoolVar _boolVar;
        [SerializeField] private bool _invert;

        public bool Invert
        {
            set { _invert = value; }
        }

        public bool Value
        {
            get
            {
                if (_invert) return !_boolVar.value;
                return _boolVar.value;
            }
            set { _boolVar.value = value; }
        }

        public BoolVar BoolVar
        {
            get { return _boolVar; }
        }
    }
}