using Unity.Entities;

[GenerateAuthoringComponent]
public struct BoidsAuthoringComponent : IComponentData
{
    public Entity Prefab;
}

