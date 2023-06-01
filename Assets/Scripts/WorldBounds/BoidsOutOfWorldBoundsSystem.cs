using Unity.Entities;
using Unity.Transforms;

namespace Assets.Scripts.WorldBounds
{
	//[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	//[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
	public partial class BoidsOutOfWorldBoundsSystem : SystemBase
	{
		protected override void OnUpdate()
		{
			var settings = GetSingleton<GameSettingsComponent>();
			var xBound = settings.LevelWidth / 2;
			var yBound = settings.LevelHeight / 2;
			var zBound = settings.LevelDepth / 2;

			Entities
				.WithAll<Boid>()
				.ForEach((Entity entity, int entityInQueryIndex, ref LocalTransform localTransform) =>
				{

					if (localTransform.Position.x > xBound)
					{
						localTransform.Position.x = -xBound;
					}

					if (localTransform.Position.x < -xBound)
					{
						localTransform.Position.x = xBound;
					}

					if (localTransform.Position.y > yBound)
					{
						localTransform.Position.y = -yBound;
					}

					if (localTransform.Position.y < -yBound)
					{
						localTransform.Position.y = yBound;
					}

					if (localTransform.Position.z > zBound)
					{
						localTransform.Position.z = -zBound;
					}

					if (localTransform.Position.z < -zBound)
					{
						localTransform.Position.z = zBound;
					}
				})
				.ScheduleParallel();
		}
	}
}
