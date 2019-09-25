using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LaunchEvent : UnityEvent<float> { }
[Serializable]
public class OnGrowthVelocityYChangedEvent : UnityEvent<float> { }
[Serializable]
public class OnScaleUpYEndedEvent : UnityEvent<float> { }

