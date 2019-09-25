using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.SmartKeyCode.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartKeyCode.Data {
	/// <summary>
	/// Dynamic collection of KeyCodeVar assets.
	/// </summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.KeyCode/UnityEngine.KeyCode Multi", order=1)]
	public class KeyCodeMulti: SmartMulti<UnityEngine.KeyCode, KeyCodeVar>, ISmartMulti<UnityEngine.KeyCode, KeyCodeVar> {
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.KeyCode";
		const string DISPLAYTYPE = "UnityEngine.KeyCode Multi";
		#endif
	}
}

namespace SmartData.SmartKeyCode {
	/// <summary>
	/// Indexed reference into a KeyCodeMulti (read-only access).
	/// For write access make a reference to KeyCodeMultiRefWriter.
	/// </summary>
	[System.Serializable]
	public class KeyCodeMultiReader : SmartDataMultiRef<KeyCodeMulti, UnityEngine.KeyCode, KeyCodeVar>  {
		public static implicit operator UnityEngine.KeyCode(KeyCodeMultiReader r){
            return r.value;
		}
		
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onUpdate;
		
		protected override System.Action<UnityEngine.KeyCode> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Indexed reference into a KeyCodeMulti, with a built-in UnityEvent.
	/// For read-only access make a reference to KeyCodeMultiRef.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class KeyCodeMultiWriter : SmartDataMultiRefWriter<KeyCodeMulti, UnityEngine.KeyCode, KeyCodeVar> {
		public static implicit operator UnityEngine.KeyCode(KeyCodeMultiWriter r){
            return r.value;
		}
		
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onUpdate;
		
		protected override System.Action<UnityEngine.KeyCode> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(UnityEngine.KeyCode value){
			_onUpdate.Invoke(value);
		}
	}
}