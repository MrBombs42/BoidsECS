using Assets.Scripts.Movement;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Assets.Scripts.Wander
{
	[UpdateBefore(typeof(MovementSystem))]
	public partial class WanderSystem : SystemBase
	{

		public const float CircleDistance = 2;
		public const float CircleRadius = 1;
		public const float TurnChance = 25;

		protected override void OnUpdate()
		{
			var random = new Random((uint)(CircleDistance* TurnChance + CircleRadius));
			Entities
				.WithAll<WanderComponent>()
				.ForEach((ref VelocityComponent velocity, in Translation position) =>
				{
					var circleCenter = new float3(velocity.Velocity.x, velocity.Velocity.y, velocity.Velocity.z);
					circleCenter = math.normalize(circleCenter);
					circleCenter *= CircleDistance;

					if (random.NextFloat(0, 100) > TurnChance)
					{
						var displacement = random.NextFloat3Direction();
						displacement *= CircleRadius;
						var wander = circleCenter + displacement;
						Debug.DrawRay(position.Value, wander, Color.green, 2);
						velocity.Steering += wander;
					}



					//rotacionar displacement na direcao do wanderAngle
					//rotacionar um pouco o angleDepois

				})
				.ScheduleParallel();
		}
	}
}