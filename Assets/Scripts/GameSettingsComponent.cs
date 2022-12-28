using Unity.Entities;

namespace Assets.Scripts
{
	public struct GameSettingsComponent : IComponentData
	{
		public int NumberOfBoids;
		public float LevelWidth;
		public float LevelDepth;
		public float LevelHeight;
		public float AsteroidVelocity;
	}
}
