// SMARTTYPE UnityEngine.Vector2
// Do not move or delete the above line

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
	/// ScriptableObject data which fires a Relay on data change.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.Vector2/UnityEngine.Vector2 Variable", order=0)]
	public partial class Vector2Var : SmartVar<UnityEngine.Vector2>, ISmartVar<UnityEngine.Vector2> {	// partial to allow overrides that don't get overwritten on regeneration
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.Vector2";
		const string DISPLAYTYPE = "UnityEngine.Vector2";
		#endif

		[System.Serializable]
		public class Vector2Event : UnityEvent<UnityEngine.Vector2>{}
	}
}

namespace SmartData.SmartVector2 {
	/// <summary>
	/// Read-only access to SmartVector2 or UnityEngine.Vector2, with built-in UnityEvent.
	/// For write access make a Vector2RefWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public partial class Vector2Reader : SmartDataRefBase<UnityEngine.Vector2, Vector2Var, Vector2Const, Vector2Multi> {
		[SerializeField]
		Data.Vector2Var.Vector2Event _onUpdate;
		
		protected sealed override System.Action<UnityEngine.Vector2> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Write access to SmartVector2Writer or UnityEngine.Vector2, with built-in UnityEvent.
	/// For read-only access make a Vector2Ref reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class Vector2Writer : SmartDataRefWriter<UnityEngine.Vector2, Vector2Var, Vector2Const, Vector2Multi> {
		[SerializeField]
		Data.Vector2Var.Vector2Event _onUpdate;
		
		protected sealed override System.Action<UnityEngine.Vector2> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(UnityEngine.Vector2 value){
			_onUpdate.Invoke(value);
		}
	}
}