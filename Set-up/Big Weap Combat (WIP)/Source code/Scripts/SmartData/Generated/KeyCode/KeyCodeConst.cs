using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;
using SmartData.Interfaces;

namespace SmartData.SmartKeyCode.Data {
	/// <summary>
	/// ScriptableObject constant UnityEngine.KeyCode.
	/// </summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.KeyCode/UnityEngine.KeyCode Const", order=3)]
	public class KeyCodeConst : SmartConst<UnityEngine.KeyCode>, ISmartConst<UnityEngine.KeyCode> {
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.KeyCode";
		const string DISPLAYTYPE = "UnityEngine.KeyCode Const";
		#endif
	}
}