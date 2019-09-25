using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartData.SmartVector2;
using UnityEngine;

namespace Assets.SmartData.Custom.Writers
{
    public class RigidbodyVelocityWriter : MonoBehaviour
    {
        [SerializeField, LabelOverride("Rigidbody")]
        private Rigidbody2D _rb;
        [SerializeField] private Vector2Writer _writer;

        void FixedUpdate()
        {
            _writer.value = _rb.velocity;
        }
    }
}