using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraPlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new CameraPlayer
            {
                GameObject = gameObject
            });

            dstManager.AddComponentObject(entity, transform);
        }
    }
}