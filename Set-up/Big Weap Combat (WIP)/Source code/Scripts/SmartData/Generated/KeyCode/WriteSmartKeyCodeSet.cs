using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartKeyCode.Components {
	/// <summary>
	/// Serialised write access to a SmartKeyCodeSet.
	/// </summary>
	[AddComponentMenu("SmartData/UnityEngine.KeyCode/Write Smart UnityEngine.KeyCode Set", 3)]
	public class WriteSmartKeyCodeSet : WriteSetBase<UnityEngine.KeyCode, KeyCodeSetWriter> {}
}