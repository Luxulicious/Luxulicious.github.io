using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.SmartVector2.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartVector2.Data {
	/// <summary>
	/// Dynamic collection of Vector2Var assets.
	/// </summary>
	[CreateAssetMenu(menuName="SmartData/UnityEngine.Vector2/UnityEngine.Vector2 Multi", order=1)]
	public class Vector2Multi: SmartMulti<UnityEngine.Vector2, Vector2Var>, ISmartMulti<UnityEngine.Vector2, Vector2Var> {
		#if UNITY_EDITOR
		const string VALUETYPE = "UnityEngine.Vector2";
		const string DISPLAYTYPE = "UnityEngine.Vector2 Multi";
		#endif
	}
}

namespace SmartData.SmartVector2 {
	/// <summary>
	/// Indexed reference into a Vector2Multi (read-only access).
	/// For write access make a reference to Vector2MultiRefWriter.
	/// </summary>
	[System.Serializable]
	public class Vector2MultiReader : SmartDataMultiRef<Vector2Multi, UnityEngine.Vector2, Vector2Var>  {
		public static implicit operator UnityEngine.Vector2(Vector2MultiReader r){
            return r.value;
		}
		
		[SerializeField]
		Data.Vector2Var.Vector2Event _onUpdate;
		
		protected override System.Action<UnityEngine.Vector2> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Indexed reference into a Vector2Multi, with a built-in UnityEvent.
	/// For read-only access make a reference to Vector2MultiRef.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class Vector2MultiWriter : SmartDataMultiRefWriter<Vector2Multi, UnityEngine.Vector2, Vector2Var> {
		public static implicit operator UnityEngine.Vector2(Vector2MultiWriter r){
            return r.value;
		}
		
		[SerializeField]
		Data.Vector2Var.Vector2Event _onUpdate;
		
		protected override System.Action<UnityEngine.Vector2> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(UnityEngine.Vector2 value){
			_onUpdate.Invoke(value);
		}
	}
}