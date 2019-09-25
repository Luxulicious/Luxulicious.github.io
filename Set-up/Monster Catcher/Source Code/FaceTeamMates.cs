using System.Linq;
using UnityEngine;
using Util;

/// <summary>
/// Rotates teammates to always face each other
/// </summary>
public class FaceTeamMates : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Vector2 _midPointPos = new Vector2();
    [SerializeField]
    private Transform _myTransform;

	void Update ()
	{
	    var teamMatePositions = Player.GetLiveTeamPlayers(this.GetComponent<Player>()).Select(player => (Vector2)player.transform.position ).ToList();
	    _midPointPos = teamMatePositions.Aggregate(Vector2.zero, (ac, v) => ac + v)/teamMatePositions.Count;
	    _myTransform.up = _midPointPos - (Vector2)_myTransform.position;
	}
}
