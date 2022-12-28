using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
	public class SetGameSettingsSystem : MonoBehaviour, IConvertGameObjectToEntity
	{
		public int NumberOfBoids = 100;
		public float LevelWidth = 50;
		public float LevelHeight = 50;
		public float LevelDepth = 50;
		public float AsteroidVelocity = 30;

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			var settings = new GameSettingsComponent
			{
				NumberOfBoids = this.NumberOfBoids,
				LevelWidth = this.LevelWidth,
				LevelDepth = this.LevelDepth,
				LevelHeight = this.LevelHeight,
				AsteroidVelocity = this.AsteroidVelocity,
			};

			dstManager.AddComponentData(entity, settings);
		}
	}
}
