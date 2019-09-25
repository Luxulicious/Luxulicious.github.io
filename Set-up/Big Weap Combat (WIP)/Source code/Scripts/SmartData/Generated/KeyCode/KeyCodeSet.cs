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
	/// ScriptableObject data set which fires a Relay on data addition/removal.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.KeyCode/UnityEngine.KeyCode Set", order=2)]
	public class KeyCodeSet : SmartSet<UnityEngine.KeyCode>, ISmartDataSet<UnityEngine.KeyCode> {
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.KeyCode";
		const string DISPLAYTYPE = "UnityEngine.KeyCode Set";
		#endif
	}
}

namespace SmartData.SmartKeyCode {
	/// <summary>
	/// Read-only access to KeyCodeSet or List<0>, with built-in UnityEvent.
	/// For write access make a KeyCodeSetWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class KeyCodeSetReader : SmartSetRefBase<UnityEngine.KeyCode, KeyCodeSet>, ISmartSetRefReader<UnityEngine.KeyCode> {
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onAdd;
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onRemove;
		
		protected override System.Action<UnityEngine.KeyCode, bool> GetUnityEventInvoke(){
			return (e,a)=>{
				if (a){
					_onAdd.Invoke(e);
				} else {
					_onRemove.Invoke(e);
				}
			};
		}
	}
	/// <summary>
	/// Write access to KeyCodeSet or List<UnityEngine.KeyCode>, with built-in UnityEvent.
	/// For read-only access make a KeyCodeSetRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class KeyCodeSetWriter : SmartSetRefWriterBase<UnityEngine.KeyCode, KeyCodeSet>, ISmartSetRefReader<UnityEngine.KeyCode> {
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onAdd;
		[SerializeField]
		Data.KeyCodeVar.KeyCodeEvent _onRemove;
		
		protected override System.Action<UnityEngine.KeyCode, bool> GetUnityEventInvoke(){
			return (e,a)=>{
				if (a){
					_onAdd.Invoke(e);
				} else {
					_onRemove.Invoke(e);
				}
			};
		}
		
		protected sealed override void InvokeUnityEvent(UnityEngine.KeyCode value, bool added){
			if (added){
				_onAdd.Invoke(value);
			} else {
				_onRemove.Invoke(value);
			}
		}
		
	}
}