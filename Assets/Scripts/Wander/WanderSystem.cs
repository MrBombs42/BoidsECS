using Assets.Scripts.Movement;
using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Wander
{
	[UpdateBefore(typeof(MovementSystem))]
	public partial class WanderSystem : SystemBase
	{

		public const float CircleDistance = 2;
		public const float CircleRadius = 1;
		protected override void OnUpdate()
		{
			Entities
				.WithAll<WanderComponent>()
				.ForEach((ref VelocityComponent velocity) =>
				{
					var circleCenter = new float3(velocity.Velocity.x, velocity.Velocity.y, velocity.Velocity.z);
					circleCenter = math.normalize(circleCenter);
					circleCenter *= CircleDistance;

					var displacement = new float3(0, CircleRadius, 0);

					//rotacionar displacement na direcao do wanderAngle
					//rotacionar um pouco o angleDepois

				})
				.ScheduleParallel();
		}
	}
}
