using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Networking.Transport.Utilities;
using Unity.NetCode;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(GhostUpdateSystemGroup))]
public class DashGhostUpdateSystem : JobComponentSystem
{
    [BurstCompile]
    struct UpdateInterpolatedJob : IJobChunk
    {
        [ReadOnly] public NativeHashMap<int, GhostEntity> GhostMap;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [NativeDisableContainerSafetyRestriction] public NativeArray<uint> minMaxSnapshotTick;
#pragma warning disable 649
        [NativeSetThreadIndex]
        public int ThreadIndex;
#pragma warning restore 649
#endif
        [ReadOnly] public ArchetypeChunkBufferType<DashSnapshotData> ghostSnapshotDataType;
        [ReadOnly] public ArchetypeChunkEntityType ghostEntityType;
        public ArchetypeChunkComponentType<Cooldown> ghostCooldownType;
        public ArchetypeChunkComponentType<Dash> ghostDashType;
        public ArchetypeChunkComponentType<OwningPlayer> ghostOwningPlayerType;
        public ArchetypeChunkComponentType<Rotation> ghostRotationType;
        public ArchetypeChunkComponentType<Translation> ghostTranslationType;
        public ArchetypeChunkComponentType<Usable> ghostUsableType;

        public uint targetTick;
        public float targetTickFraction;
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var deserializerState = new GhostDeserializerState
            {
                GhostMap = GhostMap
            };
            var ghostEntityArray = chunk.GetNativeArray(ghostEntityType);
            var ghostSnapshotDataArray = chunk.GetBufferAccessor(ghostSnapshotDataType);
            var ghostCooldownArray = chunk.GetNativeArray(ghostCooldownType);
            var ghostDashArray = chunk.GetNativeArray(ghostDashType);
            var ghostOwningPlayerArray = chunk.GetNativeArray(ghostOwningPlayerType);
            var ghostRotationArray = chunk.GetNativeArray(ghostRotationType);
            var ghostTranslationArray = chunk.GetNativeArray(ghostTranslationType);
            var ghostUsableArray = chunk.GetNativeArray(ghostUsableType);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            var minMaxOffset = ThreadIndex * (JobsUtility.CacheLineSize/4);
#endif
            for (int entityIndex = 0; entityIndex < ghostEntityArray.Length; ++entityIndex)
            {
                var snapshot = ghostSnapshotDataArray[entityIndex];
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                var latestTick = snapshot.GetLatestTick();
                if (latestTick != 0)
                {
                    if (minMaxSnapshotTick[minMaxOffset] == 0 || SequenceHelpers.IsNewer(minMaxSnapshotTick[minMaxOffset], latestTick))
                        minMaxSnapshotTick[minMaxOffset] = latestTick;
                    if (minMaxSnapshotTick[minMaxOffset + 1] == 0 || SequenceHelpers.IsNewer(latestTick, minMaxSnapshotTick[minMaxOffset + 1]))
                        minMaxSnapshotTick[minMaxOffset + 1] = latestTick;
                }
#endif
                // If there is no data found don't apply anything (would be default state), required for prespawned ghosts
                DashSnapshotData snapshotData;
                if (!snapshot.GetDataAtTick(targetTick, targetTickFraction, out snapshotData))
                    return;

                var ghostCooldown = ghostCooldownArray[entityIndex];
                var ghostDash = ghostDashArray[entityIndex];
                var ghostOwningPlayer = ghostOwningPlayerArray[entityIndex];
                var ghostRotation = ghostRotationArray[entityIndex];
                var ghostTranslation = ghostTranslationArray[entityIndex];
                var ghostUsable = ghostUsableArray[entityIndex];
                ghostCooldown.timer = snapshotData.GetCooldowntimer(deserializerState);
                ghostCooldown.duration = snapshotData.GetCooldownduration(deserializerState);
                ghostDash.max_distance = snapshotData.GetDashmax_distance(deserializerState);
                ghostDash.speed = snapshotData.GetDashspeed(deserializerState);
                ghostDash.dir = snapshotData.GetDashdir(deserializerState);
                ghostOwningPlayer.Value = snapshotData.GetOwningPlayerValue(deserializerState);
                ghostOwningPlayer.PlayerId = snapshotData.GetOwningPlayerPlayerId(deserializerState);
                ghostRotation.Value = snapshotData.GetRotationValue(deserializerState);
                ghostTranslation.Value = snapshotData.GetTranslationValue(deserializerState);
                ghostUsable.canuse = snapshotData.GetUsablecanuse(deserializerState);
                ghostCooldownArray[entityIndex] = ghostCooldown;
                ghostDashArray[entityIndex] = ghostDash;
                ghostOwningPlayerArray[entityIndex] = ghostOwningPlayer;
                ghostRotationArray[entityIndex] = ghostRotation;
                ghostTranslationArray[entityIndex] = ghostTranslation;
                ghostUsableArray[entityIndex] = ghostUsable;
            }
        }
    }
    [BurstCompile]
    struct UpdatePredictedJob : IJobChunk
    {
        [ReadOnly] public NativeHashMap<int, GhostEntity> GhostMap;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [NativeDisableContainerSafetyRestriction] public NativeArray<uint> minMaxSnapshotTick;
#endif
#pragma warning disable 649
        [NativeSetThreadIndex]
        public int ThreadIndex;
#pragma warning restore 649
        [NativeDisableParallelForRestriction] public NativeArray<uint> minPredictedTick;
        [ReadOnly] public ArchetypeChunkBufferType<DashSnapshotData> ghostSnapshotDataType;
        [ReadOnly] public ArchetypeChunkEntityType ghostEntityType;
        public ArchetypeChunkComponentType<PredictedGhostComponent> predictedGhostComponentType;
        public ArchetypeChunkComponentType<Cooldown> ghostCooldownType;
        public ArchetypeChunkComponentType<Dash> ghostDashType;
        public ArchetypeChunkComponentType<OwningPlayer> ghostOwningPlayerType;
        public ArchetypeChunkComponentType<Rotation> ghostRotationType;
        public ArchetypeChunkComponentType<Translation> ghostTranslationType;
        public ArchetypeChunkComponentType<Usable> ghostUsableType;
        public uint targetTick;
        public uint lastPredictedTick;
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var deserializerState = new GhostDeserializerState
            {
                GhostMap = GhostMap
            };
            var ghostEntityArray = chunk.GetNativeArray(ghostEntityType);
            var ghostSnapshotDataArray = chunk.GetBufferAccessor(ghostSnapshotDataType);
            var predictedGhostComponentArray = chunk.GetNativeArray(predictedGhostComponentType);
            var ghostCooldownArray = chunk.GetNativeArray(ghostCooldownType);
            var ghostDashArray = chunk.GetNativeArray(ghostDashType);
            var ghostOwningPlayerArray = chunk.GetNativeArray(ghostOwningPlayerType);
            var ghostRotationArray = chunk.GetNativeArray(ghostRotationType);
            var ghostTranslationArray = chunk.GetNativeArray(ghostTranslationType);
            var ghostUsableArray = chunk.GetNativeArray(ghostUsableType);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            var minMaxOffset = ThreadIndex * (JobsUtility.CacheLineSize/4);
#endif
            for (int entityIndex = 0; entityIndex < ghostEntityArray.Length; ++entityIndex)
            {
                var snapshot = ghostSnapshotDataArray[entityIndex];
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                var latestTick = snapshot.GetLatestTick();
                if (latestTick != 0)
                {
                    if (minMaxSnapshotTick[minMaxOffset] == 0 || SequenceHelpers.IsNewer(minMaxSnapshotTick[minMaxOffset], latestTick))
                        minMaxSnapshotTick[minMaxOffset] = latestTick;
                    if (minMaxSnapshotTick[minMaxOffset + 1] == 0 || SequenceHelpers.IsNewer(latestTick, minMaxSnapshotTick[minMaxOffset + 1]))
                        minMaxSnapshotTick[minMaxOffset + 1] = latestTick;
                }
#endif
                DashSnapshotData snapshotData;
                snapshot.GetDataAtTick(targetTick, out snapshotData);

                var predictedData = predictedGhostComponentArray[entityIndex];
                var lastPredictedTickInst = lastPredictedTick;
                if (lastPredictedTickInst == 0 || predictedData.AppliedTick != snapshotData.Tick)
                    lastPredictedTickInst = snapshotData.Tick;
                else if (!SequenceHelpers.IsNewer(lastPredictedTickInst, snapshotData.Tick))
                    lastPredictedTickInst = snapshotData.Tick;
                if (minPredictedTick[ThreadIndex] == 0 || SequenceHelpers.IsNewer(minPredictedTick[ThreadIndex], lastPredictedTickInst))
                    minPredictedTick[ThreadIndex] = lastPredictedTickInst;
                predictedGhostComponentArray[entityIndex] = new PredictedGhostComponent{AppliedTick = snapshotData.Tick, PredictionStartTick = lastPredictedTickInst};
                if (lastPredictedTickInst != snapshotData.Tick)
                    continue;

                var ghostCooldown = ghostCooldownArray[entityIndex];
                var ghostDash = ghostDashArray[entityIndex];
                var ghostOwningPlayer = ghostOwningPlayerArray[entityIndex];
                var ghostRotation = ghostRotationArray[entityIndex];
                var ghostTranslation = ghostTranslationArray[entityIndex];
                var ghostUsable = ghostUsableArray[entityIndex];
                ghostCooldown.timer = snapshotData.GetCooldowntimer(deserializerState);
                ghostCooldown.duration = snapshotData.GetCooldownduration(deserializerState);
                ghostDash.max_distance = snapshotData.GetDashmax_distance(deserializerState);
                ghostDash.speed = snapshotData.GetDashspeed(deserializerState);
                ghostDash.dir = snapshotData.GetDashdir(deserializerState);
                ghostOwningPlayer.Value = snapshotData.GetOwningPlayerValue(deserializerState);
                ghostOwningPlayer.PlayerId = snapshotData.GetOwningPlayerPlayerId(deserializerState);
                ghostRotation.Value = snapshotData.GetRotationValue(deserializerState);
                ghostTranslation.Value = snapshotData.GetTranslationValue(deserializerState);
                ghostUsable.canuse = snapshotData.GetUsablecanuse(deserializerState);
                ghostCooldownArray[entityIndex] = ghostCooldown;
                ghostDashArray[entityIndex] = ghostDash;
                ghostOwningPlayerArray[entityIndex] = ghostOwningPlayer;
                ghostRotationArray[entityIndex] = ghostRotation;
                ghostTranslationArray[entityIndex] = ghostTranslation;
                ghostUsableArray[entityIndex] = ghostUsable;
            }
        }
    }
    private ClientSimulationSystemGroup m_ClientSimulationSystemGroup;
    private GhostPredictionSystemGroup m_GhostPredictionSystemGroup;
    private EntityQuery m_interpolatedQuery;
    private EntityQuery m_predictedQuery;
    private GhostUpdateSystemGroup m_GhostUpdateSystemGroup;
    private uint m_LastPredictedTick;
    protected override void OnCreate()
    {
        m_GhostUpdateSystemGroup = World.GetOrCreateSystem<GhostUpdateSystemGroup>();
        m_ClientSimulationSystemGroup = World.GetOrCreateSystem<ClientSimulationSystemGroup>();
        m_GhostPredictionSystemGroup = World.GetOrCreateSystem<GhostPredictionSystemGroup>();
        m_interpolatedQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new []{
                ComponentType.ReadWrite<DashSnapshotData>(),
                ComponentType.ReadOnly<GhostComponent>(),
                ComponentType.ReadWrite<Cooldown>(),
                ComponentType.ReadWrite<Dash>(),
                ComponentType.ReadWrite<OwningPlayer>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Usable>(),
            },
            None = new []{ComponentType.ReadWrite<PredictedGhostComponent>()}
        });
        m_predictedQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new []{
                ComponentType.ReadOnly<DashSnapshotData>(),
                ComponentType.ReadOnly<GhostComponent>(),
                ComponentType.ReadOnly<PredictedGhostComponent>(),
                ComponentType.ReadWrite<Cooldown>(),
                ComponentType.ReadWrite<Dash>(),
                ComponentType.ReadWrite<OwningPlayer>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Usable>(),
            }
        });
        RequireForUpdate(GetEntityQuery(ComponentType.ReadWrite<DashSnapshotData>(),
            ComponentType.ReadOnly<GhostComponent>()));
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ghostEntityMap = m_GhostUpdateSystemGroup.GhostEntityMap;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        var ghostMinMaxSnapshotTick = m_GhostUpdateSystemGroup.GhostSnapshotTickMinMax;
#endif
        if (!m_predictedQuery.IsEmptyIgnoreFilter)
        {
            var updatePredictedJob = new UpdatePredictedJob
            {
                GhostMap = ghostEntityMap,
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                minMaxSnapshotTick = ghostMinMaxSnapshotTick,
#endif
                minPredictedTick = m_GhostPredictionSystemGroup.OldestPredictedTick,
                ghostSnapshotDataType = GetArchetypeChunkBufferType<DashSnapshotData>(true),
                ghostEntityType = GetArchetypeChunkEntityType(),
                predictedGhostComponentType = GetArchetypeChunkComponentType<PredictedGhostComponent>(),
                ghostCooldownType = GetArchetypeChunkComponentType<Cooldown>(),
                ghostDashType = GetArchetypeChunkComponentType<Dash>(),
                ghostOwningPlayerType = GetArchetypeChunkComponentType<OwningPlayer>(),
                ghostRotationType = GetArchetypeChunkComponentType<Rotation>(),
                ghostTranslationType = GetArchetypeChunkComponentType<Translation>(),
                ghostUsableType = GetArchetypeChunkComponentType<Usable>(),

                targetTick = m_ClientSimulationSystemGroup.ServerTick,
                lastPredictedTick = m_LastPredictedTick
            };
            m_LastPredictedTick = m_ClientSimulationSystemGroup.ServerTick;
            if (m_ClientSimulationSystemGroup.ServerTickFraction < 1)
                m_LastPredictedTick = 0;
            inputDeps = updatePredictedJob.Schedule(m_predictedQuery, JobHandle.CombineDependencies(inputDeps, m_GhostUpdateSystemGroup.LastGhostMapWriter));
            m_GhostPredictionSystemGroup.AddPredictedTickWriter(inputDeps);
        }
        if (!m_interpolatedQuery.IsEmptyIgnoreFilter)
        {
            var updateInterpolatedJob = new UpdateInterpolatedJob
            {
                GhostMap = ghostEntityMap,
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                minMaxSnapshotTick = ghostMinMaxSnapshotTick,
#endif
                ghostSnapshotDataType = GetArchetypeChunkBufferType<DashSnapshotData>(true),
                ghostEntityType = GetArchetypeChunkEntityType(),
                ghostCooldownType = GetArchetypeChunkComponentType<Cooldown>(),
                ghostDashType = GetArchetypeChunkComponentType<Dash>(),
                ghostOwningPlayerType = GetArchetypeChunkComponentType<OwningPlayer>(),
                ghostRotationType = GetArchetypeChunkComponentType<Rotation>(),
                ghostTranslationType = GetArchetypeChunkComponentType<Translation>(),
                ghostUsableType = GetArchetypeChunkComponentType<Usable>(),
                targetTick = m_ClientSimulationSystemGroup.InterpolationTick,
                targetTickFraction = m_ClientSimulationSystemGroup.InterpolationTickFraction
            };
            inputDeps = updateInterpolatedJob.Schedule(m_interpolatedQuery, JobHandle.CombineDependencies(inputDeps, m_GhostUpdateSystemGroup.LastGhostMapWriter));
        }
        return inputDeps;
    }
}
public partial class DashGhostSpawnSystem : DefaultGhostSpawnSystem<DashSnapshotData>
{
    struct SetPredictedDefault : IJobParallelFor
    {
        [ReadOnly] public NativeArray<DashSnapshotData> snapshots;
        public NativeArray<int> predictionMask;
        [ReadOnly][DeallocateOnJobCompletion] public NativeArray<NetworkIdComponent> localPlayerId;
        public void Execute(int index)
        {
            if (localPlayerId.Length == 1 && snapshots[index].GetOwningPlayerPlayerId() == localPlayerId[0].Value)
                predictionMask[index] = 1;
        }
    }
    protected override JobHandle SetPredictedGhostDefaults(NativeArray<DashSnapshotData> snapshots, NativeArray<int> predictionMask, JobHandle inputDeps)
    {
        JobHandle playerHandle;
        var job = new SetPredictedDefault
        {
            snapshots = snapshots,
            predictionMask = predictionMask,
            localPlayerId = m_PlayerGroup.ToComponentDataArrayAsync<NetworkIdComponent>(Allocator.TempJob, out playerHandle),
        };
        return job.Schedule(predictionMask.Length, 8, JobHandle.CombineDependencies(playerHandle, inputDeps));
    }
}