using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Thrown when a catch missed or failed
/// </summary>
[Serializable]
public class OnCatchFailedEvent : UnityEvent<List<Player>> { }

/// <summary>
/// Thrown when a catch succeeded and/or hit
/// </summary>
[Serializable]
public class OnCatchSuccessEvent : UnityEvent<List<Player>, Monster> { }
