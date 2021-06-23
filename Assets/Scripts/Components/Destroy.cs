using Unity.Entities;

[GenerateAuthoringComponent]
public struct Destroy : IComponentData
{
    public bool dead;
    public float life;
}
