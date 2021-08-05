using Unity.Entities;

[GenerateAuthoringComponent]
public struct Destroy : IComponentData
{
    public bool dead;
    public bool split;
    public float life;
}
