// SMARTTYPE UnityEngine.KeyCode
// Do not move or delete the above line

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartKeyCode.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartKeyCode.Data {
	/// <summary>
	/// ScriptableObject data which fires a Relay on data change.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.KeyCode/UnityEngine.KeyCode Variable", order=0)]
	public partial class KeyCodeVar : SmartVar<UnityEngine.KeyCode>, ISmartVar<UnityEngine.KeyCode> {	// partial to allow overrides that don't get overwritten on regeneration
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.KeyCode";
		const string DISPLAYTYPE = "UnityEngine.KeyCode";
		#endif

		[System.Serializable]
		public class KeyCodeEvent : UnityEvent<UnityEngine.KeyCode>{}
	}
}

namespace SmartData.SmartKeyCode {
	/// <summary>
	/// Read-only access to SmartKeyCode or UnityEngine.KeyCode, with built-in UnityEvent.
	/// For write access make a KeyCodeRefWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public partial class KeyCodeReader : SmartDataRefBase<UnityEngine.KeyCode, KeyCodeVar, KeyCodeConst, KeyCodeMulti> {
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onUpdate;
		
		protected sealed override System.Action<UnityEngine.KeyCode> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Write access to SmartKeyCodeWriter or UnityEngine.KeyCode, with built-in UnityEvent.
	/// For read-only access make a KeyCodeRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class KeyCodeWriter : SmartDataRefWriter<UnityEngine.KeyCode, KeyCodeVar, KeyCodeConst, KeyCodeMulti> {
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onUpdate;
		
		protected sealed override System.Action<UnityEngine.KeyCode> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(UnityEngine.KeyCode value){
			_onUpdate.Invoke(value);
		}
	}
}