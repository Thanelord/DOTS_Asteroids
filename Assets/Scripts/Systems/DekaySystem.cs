using Unity.Entities;
using Unity.Jobs;

public class DekaySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Decaying decaying) =>
        {
            if(decaying.value > 0)
            {
                decaying.value -= 1;
            }
            else
            {
                decaying.despawn = true;
            }

        }).Run();

        return default;
    }
}
