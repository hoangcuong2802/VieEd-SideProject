using UnityEngine;
using Fusion;

namespace Example
{
	public sealed class MovingObject : NetworkBehaviour
	{
		public float       Speed = 5.0f;
		public Transform[] Waypoints;

		private int _currentWaypoint;

		public override void Spawned()
		{
			_currentWaypoint = -1;
			SetNextWaypoint();
		}

		public override void FixedUpdateNetwork()
		{
			if (_currentWaypoint < 0)
				return;

			Vector3 objectToTarget = Waypoints[_currentWaypoint].position - transform.position;
			transform.position += objectToTarget.normalized * Speed * Runner.DeltaTime;
			if (Vector3.SqrMagnitude(objectToTarget) < 1.0f)
			{
				SetNextWaypoint();
			}
		}

		private void SetNextWaypoint()
		{
			if (Waypoints == null || Waypoints.Length == 0)
				return;

			_currentWaypoint = (_currentWaypoint + 1) % Waypoints.Length;
		}
	}
}
