using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartVector2.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartVector2.Data {
	/// <summary>
	/// ScriptableObject data set which fires a Relay on data addition/removal.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.Vector2/UnityEngine.Vector2 Set", order=2)]
	public class Vector2Set : SmartSet<UnityEngine.Vector2>, ISmartDataSet<UnityEngine.Vector2> {
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.Vector2";
		const string DISPLAYTYPE = "UnityEngine.Vector2 Set";
		#endif
	}
}

namespace SmartData.SmartVector2 {
	/// <summary>
	/// Read-only access to Vector2Set or List<0>, with built-in UnityEvent.
	/// For write access make a Vector2SetWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class Vector2SetReader : SmartSetRefBase<UnityEngine.Vector2, Vector2Set>, ISmartSetRefReader<UnityEngine.Vector2> {
		[SerializeField]
		Data.Vector2Var.Vector2Event _onAdd;
		[SerializeField]
		Data.Vector2Var.Vector2Event _onRemove;
		
		protected override System.Action<UnityEngine.Vector2, bool> GetUnityEventInvoke(){
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
	/// Write access to Vector2Set or List<UnityEngine.Vector2>, with built-in UnityEvent.
	/// For read-only access make a Vector2SetRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class Vector2SetWriter : SmartSetRefWriterBase<UnityEngine.Vector2, Vector2Set>, ISmartSetRefReader<UnityEngine.Vector2> {
		[SerializeField]
		Data.Vector2Var.Vector2Event _onAdd;
		[SerializeField]
		Data.Vector2Var.Vector2Event _onRemove;
		
		protected override System.Action<UnityEngine.Vector2, bool> GetUnityEventInvoke(){
			return (e,a)=>{
				if (a){
					_onAdd.Invoke(e);
				} else {
					_onRemove.Invoke(e);
				}
			};
		}
		
		protected sealed override void InvokeUnityEvent(UnityEngine.Vector2 value, bool added){
			if (added){
				_onAdd.Invoke(value);
			} else {
				_onRemove.Invoke(value);
			}
		}
		
	}
}