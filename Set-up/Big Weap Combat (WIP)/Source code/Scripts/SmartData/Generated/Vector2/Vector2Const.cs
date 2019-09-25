using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;
using SmartData.Interfaces;

namespace SmartData.SmartVector2.Data {
	/// <summary>
	/// ScriptableObject constant UnityEngine.Vector2.
	/// </summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.Vector2/UnityEngine.Vector2 Const", order=3)]
	public class Vector2Const : SmartConst<UnityEngine.Vector2>, ISmartConst<UnityEngine.Vector2> {
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.Vector2";
		const string DISPLAYTYPE = "UnityEngine.Vector2 Const";
		#endif
	}
}