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
				.WithAll<BoidComponent>()
				.ForEach((Entity entity, int entityInQueryIndex, ref Translation position) =>
				{

					if (position.Value.x > xBound)
					{
						position.Value.x = -xBound;
					}

					if (position.Value.x < -xBound)
					{
						position.Value.x = xBound;
					}

					if (position.Value.y > yBound)
					{
						position.Value.y = -yBound;
					}

					if (position.Value.y < -yBound)
					{
						position.Value.y = yBound;
					}

					if (position.Value.z > zBound)
					{
						position.Value.z = -zBound;
					}

					if (position.Value.z < -zBound)
					{
						position.Value.z = zBound;
					}
				})
				.ScheduleParallel();
		}
	}
}
