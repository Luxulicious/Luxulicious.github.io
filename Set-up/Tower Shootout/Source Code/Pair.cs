using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Main.Scripts.Util
{
    [Serializable]
    public class Pair<TKey, TValue> : MonoBehaviour
    {
        [SerializeField]
        public TKey Key { get; set; }
        [SerializeField]
        public TValue Value { get; set; }

        public Pair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public Pair()
        {
        }
    }

}
