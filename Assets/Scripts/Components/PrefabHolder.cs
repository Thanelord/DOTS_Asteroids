using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabHolder : IComponentData
{
    //Rocks
    public int rocksSpawned;
    public Entity prefabRock;
    public Entity prefabRockM;
    public Entity prefabRockS;
    //Laser and spawnpoint ref
    public Entity prefabLaser;
    public Entity player;
}
