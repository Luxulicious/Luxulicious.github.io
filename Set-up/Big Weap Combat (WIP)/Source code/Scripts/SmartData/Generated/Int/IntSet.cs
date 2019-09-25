using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartInt.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartInt.Data {
	/// <summary>
	/// ScriptableObject data set which fires a Relay on data addition/removal.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/Int/Int Set", order=2)]
	public class IntSet : SmartSet<int>, ISmartDataSet<int> {
		#if UNITY_EDITOR
		const string VALUETYPE = "int";
		const string DISPLAYTYPE = "Int Set";
		#endif
	}
}

namespace SmartData.SmartInt {
	/// <summary>
	/// Read-only access to IntSet or List<0>, with built-in UnityEvent.
	/// For write access make a IntSetWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class IntSetReader : SmartSetRefBase<int, IntSet>, ISmartSetRefReader<int> {
		[SerializeField]
		Data.IntVar.IntEvent _onAdd;
		[SerializeField]
		Data.IntVar.IntEvent _onRemove;
		
		protected override System.Action<int, bool> GetUnityEventInvoke(){
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
	/// Write access to IntSet or List<int>, with built-in UnityEvent.
	/// For read-only access make a IntSetRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class IntSetWriter : SmartSetRefWriterBase<int, IntSet>, ISmartSetRefReader<int> {
		[SerializeField]
		Data.IntVar.IntEvent _onAdd;
		[SerializeField]
		Data.IntVar.IntEvent _onRemove;
		
		protected override System.Action<int, bool> GetUnityEventInvoke(){
			return (e,a)=>{
				if (a){
					_onAdd.Invoke(e);
				} else {
					_onRemove.Invoke(e);
				}
			};
		}
		
		protected sealed override void InvokeUnityEvent(int value, bool added){
			if (added){
				_onAdd.Invoke(value);
			} else {
				_onRemove.Invoke(value);
			}
		}
		
	}
}