using Assets.Scripts.Movement;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class MovementSystem : SystemBase
{
	//FixedGroup
	protected override void OnUpdate()
	{
		var deltaTime = UnityEngine.Time.deltaTime;

		Entities
		.ForEach((ref LocalTransform localTransform, ref VelocityComponent velocity) =>
		{
			Vector3 vel = velocity.Velocity;
			var steering = Vector3.ClampMagnitude(velocity.Steering, velocity.MaxForce);
			Debug.DrawRay(localTransform.Position, steering, Color.red, 2);
			// mass can be added steering  = steering / mass
			vel = Vector3.ClampMagnitude(vel + steering, velocity.MaxSpeed);
			velocity.Velocity = vel;
			Debug.DrawRay(localTransform.Position, vel, Color.blue, 2);
			localTransform.Position += velocity.Velocity * deltaTime;
		}).ScheduleParallel();
	}
}
