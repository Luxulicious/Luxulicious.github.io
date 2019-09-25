using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartKeyCode.Components {
	/// <summary>
	/// Automatically listens to a <cref>SmartKeyCode</cref> and fires a <cref>UnityEvent<UnityEngine.KeyCode></cref> when data changes.
	/// </summary>
	[AddComponentMenu("SmartData/UnityEngine.KeyCode/Read Smart UnityEngine.KeyCode", 0)]
	public class ReadSmartKeyCode : ReadSmartBase<KeyCodeReader> {}
}