using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateAfter(typeof(TransformSystemGroup))]
public class DespawnSystem : JobComponentSystem

{

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    //***********************
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();


        Entities.WithAll<Decaying>().WithAll<Destroy>().ForEach((Entity entity, in Decaying toGo, in Destroy destroy) =>
        {
            if (toGo.despawn)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }

        }).Run();

        Entities.WithAll<Destroy>().ForEach((Entity entity, in Destroy destroy) =>
        {
            if (destroy.dead)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }

        }).Run();

        return default;

        // commandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}
