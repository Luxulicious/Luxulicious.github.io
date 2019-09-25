using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartVector2.Components {
	/// <summary>
	/// Automatically listens to a <cref>SmartVector2</cref> and fires a <cref>UnityEvent<UnityEngine.Vector2></cref> when data changes.
	/// </summary>
	[AddComponentMenu("SmartData/UnityEngine.Vector2/Read Smart UnityEngine.Vector2", 0)]
	public class ReadSmartVector2 : ReadSmartBase<Vector2Reader> {}
}