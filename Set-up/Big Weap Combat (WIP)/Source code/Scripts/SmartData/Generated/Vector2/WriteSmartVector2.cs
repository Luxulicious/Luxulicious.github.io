using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartVector2.Components {
	/// <summary>
	/// Serialised write access to a SmartVector2.
	/// </summary>
	[AddComponentMenu("SmartData/UnityEngine.Vector2/Write Smart UnityEngine.Vector2", 1)]
	public class WriteSmartVector2 : WriteSmartBase<UnityEngine.Vector2, Vector2Writer> {}
}