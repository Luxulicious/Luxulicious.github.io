using System;
using System.ComponentModel;
using SmartData.Abstract;
using SmartData.SmartBool.Data;
using UnityEngine;

namespace Assets.SmartData.Custom.Collection.Bool
{
    [Obsolete("Use RichBoolMulti instead")]
    [Serializable] 
    public abstract class SmartBoolRefsBase<T> where T : SmartDataRefBase<bool, BoolVar, BoolConst, BoolMulti>
    {
        [LabelOverride("And/or")] [SerializeField]
        protected LogicalOperator _op;

        [SerializeField] private bool _default;
        [SerializeField] protected string _description;
        [SerializeField] protected T[] _items;

        public T[] Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public bool AndAll()
        {
            if (_items.Length <= 0)
                return _default;
            foreach (var v in _items)
                if (!v.value)
                    return false;
            return true;
        }

        public bool OrAll()
        {
            if (_items.Length <= 0)
                return _default;
            foreach (var v in _items)
                if (v.value)
                    return true;
            return false;
        }

        public bool DefaultAll()
        {
            if (_op == LogicalOperator.And)
                return AndAll();
            else if (_op == LogicalOperator.Or)
                return OrAll();
            else
                throw new InvalidEnumArgumentException();
        }

        public bool And(int indexA, int indexB)
        {
            return _items[indexA].value && _items[indexB].value;
        }

        public bool Or(int indexA, int indexB)
        {
            return _items[indexA].value || _items[indexB].value;
        }
    }
}