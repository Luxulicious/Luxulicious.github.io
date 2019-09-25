using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartKeyCode.Components {
	/// <summary>
	/// Serialised write access to a SmartKeyCode.
	/// </summary>
	[AddComponentMenu("SmartData/UnityEngine.KeyCode/Write Smart UnityEngine.KeyCode", 1)]
	public class WriteSmartKeyCode : WriteSmartBase<UnityEngine.KeyCode, KeyCodeWriter> {}
}