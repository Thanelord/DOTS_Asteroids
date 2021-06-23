using Unity.Entities;

[GenerateAuthoringComponent]
public struct Decaying : IComponentData
{
    public int value;
    public bool despawn;
}
