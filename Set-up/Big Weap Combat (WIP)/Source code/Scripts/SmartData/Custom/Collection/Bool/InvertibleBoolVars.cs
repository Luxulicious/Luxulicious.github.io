using System;
using System.Collections.Generic;
using System.ComponentModel;
using Assets.SmartData.Custom.Collection.Bool;
using SmartData.SmartBool.Data;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Scripts.SmartData.Custom.Collection.Bool
{
    //TODO Add OnUpdated event
    [CreateAssetMenu(menuName = "SmartData/Bool/Custom/Collection/Invertible Bool Variables", order = 4)]
    [Serializable]
    public class InvertibleBoolVars : ScriptableObject
    {
        [SerializeField] private string _description;

        [LabelOverride("And/or")] [SerializeField]
        protected LogicalOperator _logicalOperator;

        [SerializeField] protected InvertibleBoolVar[] _references;
        [SerializeField] protected InvertibleBoolVars[] _nested;

        public string Description
        {
            get { return _description; }
        }

        public bool AndAll()
        {
            CheckEmpty();
            foreach (var bv in _references)
                if (!bv.Value)
                    return false;
            foreach (var bvs in _nested)
                if (!bvs.DefaultAll())
                    return false;
            return true;
        }


        public bool OrAll()
        {
            CheckEmpty();
            foreach (var t in _references)
                if (t.Value)
                    return true;
            foreach (var bvs in _nested)
                if (bvs.DefaultAll())
                    return true;
            return false;
        }

        public bool DefaultAll()
        {
            if (_logicalOperator == LogicalOperator.And)
                return AndAll();
            else if (_logicalOperator == LogicalOperator.Or)
                return OrAll();
            else
                throw new InvalidEnumArgumentException();
        }

        public bool value
        {
            get { return DefaultAll(); }
        }

        private bool Empty()
        {
            return _references.Length <= 0 && _nested.Length <= 0;
        }

        private void CheckEmpty()
        {
            if (Empty())
                Debug.LogError("No references or nested set in bool collection.");
        }

        private InvertibleBoolVar[] GetAll()
        {
            var vars = new List<InvertibleBoolVar>();
            vars.AddRange(_references);
            foreach (var nested in _nested) vars.AddRange(nested.GetAll());
            return vars.ToArray();
        }

        private BoolVar[] GetAllBoolVars()
        {
            var vars = new List<BoolVar>();
            foreach (var ibv in _references)
            {
                vars.Add(ibv.BoolVar);
            }

            foreach (var nest in _nested)
            {
                vars.AddRange(nest.GetAllBoolVars());
            }

            return vars.ToArray();
        }
    }
}