using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartVector2.Components {
	/// <summary>
	/// Serialised write access to a SmartVector2Set.
	/// </summary>
	[AddComponentMenu("SmartData/UnityEngine.Vector2/Write Smart UnityEngine.Vector2 Set", 3)]
	public class WriteSmartVector2Set : WriteSetBase<UnityEngine.Vector2, Vector2SetWriter> {}
}