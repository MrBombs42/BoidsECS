using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
	public class SetGameSettingsSystem : MonoBehaviour
	{
		public int NumberOfBoids = 100;
		public float LevelWidth = 50;
		public float LevelHeight = 50;
		public float LevelDepth = 50;
		public float AsteroidVelocity = 30;

		class Baker : Baker<SetGameSettingsSystem>
		{
			public override void Bake(SetGameSettingsSystem authoring)
			{
				var entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new GameSettingsComponent
				{
					NumberOfBoids = authoring.NumberOfBoids,
					LevelWidth = authoring.LevelWidth,
					LevelDepth = authoring.LevelDepth,
					LevelHeight = authoring.LevelHeight,
					AsteroidVelocity = authoring.AsteroidVelocity,
				});
			}
		}

	}
}
