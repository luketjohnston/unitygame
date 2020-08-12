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
public class AgentGhostUpdateSystem : JobComponentSystem
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
        [ReadOnly] public ArchetypeChunkBufferType<AgentSnapshotData> ghostSnapshotDataType;
        [ReadOnly] public ArchetypeChunkEntityType ghostEntityType;
        public ArchetypeChunkComponentType<AgentComponent> ghostAgentComponentType;
        public ArchetypeChunkComponentType<BackwardModifier> ghostBackwardModifierType;
        public ArchetypeChunkComponentType<CanMove> ghostCanMoveType;
        public ArchetypeChunkComponentType<DestinationComponent> ghostDestinationComponentType;
        public ArchetypeChunkComponentType<GameOrientation> ghostGameOrientationType;
        public ArchetypeChunkComponentType<GamePosition> ghostGamePositionType;
        public ArchetypeChunkComponentType<Health> ghostHealthType;
        public ArchetypeChunkComponentType<Speed> ghostSpeedType;
        [ReadOnly] public ArchetypeChunkBufferType<LinkedEntityGroup> ghostLinkedEntityGroupType;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Rotation> ghostRotationFromEntity;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Translation> ghostTranslationFromEntity;

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
            var ghostAgentComponentArray = chunk.GetNativeArray(ghostAgentComponentType);
            var ghostBackwardModifierArray = chunk.GetNativeArray(ghostBackwardModifierType);
            var ghostCanMoveArray = chunk.GetNativeArray(ghostCanMoveType);
            var ghostDestinationComponentArray = chunk.GetNativeArray(ghostDestinationComponentType);
            var ghostGameOrientationArray = chunk.GetNativeArray(ghostGameOrientationType);
            var ghostGamePositionArray = chunk.GetNativeArray(ghostGamePositionType);
            var ghostHealthArray = chunk.GetNativeArray(ghostHealthType);
            var ghostSpeedArray = chunk.GetNativeArray(ghostSpeedType);
            var ghostLinkedEntityGroupArray = chunk.GetBufferAccessor(ghostLinkedEntityGroupType);
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
                AgentSnapshotData snapshotData;
                if (!snapshot.GetDataAtTick(targetTick, targetTickFraction, out snapshotData))
                    return;

                var ghostAgentComponent = ghostAgentComponentArray[entityIndex];
                var ghostBackwardModifier = ghostBackwardModifierArray[entityIndex];
                var ghostCanMove = ghostCanMoveArray[entityIndex];
                var ghostDestinationComponent = ghostDestinationComponentArray[entityIndex];
                var ghostGameOrientation = ghostGameOrientationArray[entityIndex];
                var ghostGamePosition = ghostGamePositionArray[entityIndex];
                var ghostHealth = ghostHealthArray[entityIndex];
                var ghostSpeed = ghostSpeedArray[entityIndex];
                var ghostRotation = ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value];
                var ghostTranslation = ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value];
                var ghostChild0Rotation = ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value];
                var ghostChild0Translation = ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value];
                var ghostChild1Rotation = ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value];
                var ghostChild1Translation = ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value];
                ghostAgentComponent.PlayerId = snapshotData.GetAgentComponentPlayerId(deserializerState);
                ghostBackwardModifier.Value = snapshotData.GetBackwardModifierValue(deserializerState);
                ghostCanMove.Value = snapshotData.GetCanMoveValue(deserializerState);
                ghostDestinationComponent.Value = snapshotData.GetDestinationComponentValue(deserializerState);
                ghostDestinationComponent.Valid = snapshotData.GetDestinationComponentValid(deserializerState);
                ghostGameOrientation.Value = snapshotData.GetGameOrientationValue(deserializerState);
                ghostGamePosition.Value = snapshotData.GetGamePositionValue(deserializerState);
                ghostHealth.Value = snapshotData.GetHealthValue(deserializerState);
                ghostHealth.regen = snapshotData.GetHealthregen(deserializerState);
                ghostHealth.max = snapshotData.GetHealthmax(deserializerState);
                ghostSpeed.Value = snapshotData.GetSpeedValue(deserializerState);
                ghostRotation.Value = snapshotData.GetRotationValue(deserializerState);
                ghostTranslation.Value = snapshotData.GetTranslationValue(deserializerState);
                ghostChild0Rotation.Value = snapshotData.GetChild0RotationValue(deserializerState);
                ghostChild0Translation.Value = snapshotData.GetChild0TranslationValue(deserializerState);
                ghostChild1Rotation.Value = snapshotData.GetChild1RotationValue(deserializerState);
                ghostChild1Translation.Value = snapshotData.GetChild1TranslationValue(deserializerState);
                ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value] = ghostRotation;
                ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value] = ghostTranslation;
                ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value] = ghostChild0Rotation;
                ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value] = ghostChild0Translation;
                ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value] = ghostChild1Rotation;
                ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value] = ghostChild1Translation;
                ghostAgentComponentArray[entityIndex] = ghostAgentComponent;
                ghostBackwardModifierArray[entityIndex] = ghostBackwardModifier;
                ghostCanMoveArray[entityIndex] = ghostCanMove;
                ghostDestinationComponentArray[entityIndex] = ghostDestinationComponent;
                ghostGameOrientationArray[entityIndex] = ghostGameOrientation;
                ghostGamePositionArray[entityIndex] = ghostGamePosition;
                ghostHealthArray[entityIndex] = ghostHealth;
                ghostSpeedArray[entityIndex] = ghostSpeed;
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
        [ReadOnly] public ArchetypeChunkBufferType<AgentSnapshotData> ghostSnapshotDataType;
        [ReadOnly] public ArchetypeChunkEntityType ghostEntityType;
        public ArchetypeChunkComponentType<PredictedGhostComponent> predictedGhostComponentType;
        public ArchetypeChunkComponentType<AgentComponent> ghostAgentComponentType;
        public ArchetypeChunkComponentType<BackwardModifier> ghostBackwardModifierType;
        public ArchetypeChunkComponentType<CanMove> ghostCanMoveType;
        public ArchetypeChunkComponentType<DestinationComponent> ghostDestinationComponentType;
        public ArchetypeChunkComponentType<GameOrientation> ghostGameOrientationType;
        public ArchetypeChunkComponentType<GamePosition> ghostGamePositionType;
        public ArchetypeChunkComponentType<Health> ghostHealthType;
        public ArchetypeChunkComponentType<Speed> ghostSpeedType;
        [ReadOnly] public ArchetypeChunkBufferType<LinkedEntityGroup> ghostLinkedEntityGroupType;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Rotation> ghostRotationFromEntity;
        [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Translation> ghostTranslationFromEntity;
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
            var ghostAgentComponentArray = chunk.GetNativeArray(ghostAgentComponentType);
            var ghostBackwardModifierArray = chunk.GetNativeArray(ghostBackwardModifierType);
            var ghostCanMoveArray = chunk.GetNativeArray(ghostCanMoveType);
            var ghostDestinationComponentArray = chunk.GetNativeArray(ghostDestinationComponentType);
            var ghostGameOrientationArray = chunk.GetNativeArray(ghostGameOrientationType);
            var ghostGamePositionArray = chunk.GetNativeArray(ghostGamePositionType);
            var ghostHealthArray = chunk.GetNativeArray(ghostHealthType);
            var ghostSpeedArray = chunk.GetNativeArray(ghostSpeedType);
            var ghostLinkedEntityGroupArray = chunk.GetBufferAccessor(ghostLinkedEntityGroupType);
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
                AgentSnapshotData snapshotData;
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

                var ghostAgentComponent = ghostAgentComponentArray[entityIndex];
                var ghostBackwardModifier = ghostBackwardModifierArray[entityIndex];
                var ghostCanMove = ghostCanMoveArray[entityIndex];
                var ghostDestinationComponent = ghostDestinationComponentArray[entityIndex];
                var ghostGameOrientation = ghostGameOrientationArray[entityIndex];
                var ghostGamePosition = ghostGamePositionArray[entityIndex];
                var ghostHealth = ghostHealthArray[entityIndex];
                var ghostSpeed = ghostSpeedArray[entityIndex];
                var ghostRotation = ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value];
                var ghostTranslation = ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value];
                var ghostChild0Rotation = ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value];
                var ghostChild0Translation = ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value];
                var ghostChild1Rotation = ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value];
                var ghostChild1Translation = ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value];
                ghostAgentComponent.PlayerId = snapshotData.GetAgentComponentPlayerId(deserializerState);
                ghostBackwardModifier.Value = snapshotData.GetBackwardModifierValue(deserializerState);
                ghostCanMove.Value = snapshotData.GetCanMoveValue(deserializerState);
                ghostDestinationComponent.Value = snapshotData.GetDestinationComponentValue(deserializerState);
                ghostDestinationComponent.Valid = snapshotData.GetDestinationComponentValid(deserializerState);
                ghostGameOrientation.Value = snapshotData.GetGameOrientationValue(deserializerState);
                ghostGamePosition.Value = snapshotData.GetGamePositionValue(deserializerState);
                ghostHealth.Value = snapshotData.GetHealthValue(deserializerState);
                ghostHealth.regen = snapshotData.GetHealthregen(deserializerState);
                ghostHealth.max = snapshotData.GetHealthmax(deserializerState);
                ghostSpeed.Value = snapshotData.GetSpeedValue(deserializerState);
                ghostRotation.Value = snapshotData.GetRotationValue(deserializerState);
                ghostTranslation.Value = snapshotData.GetTranslationValue(deserializerState);
                ghostChild0Rotation.Value = snapshotData.GetChild0RotationValue(deserializerState);
                ghostChild0Translation.Value = snapshotData.GetChild0TranslationValue(deserializerState);
                ghostChild1Rotation.Value = snapshotData.GetChild1RotationValue(deserializerState);
                ghostChild1Translation.Value = snapshotData.GetChild1TranslationValue(deserializerState);
                ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value] = ghostRotation;
                ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][0].Value] = ghostTranslation;
                ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value] = ghostChild0Rotation;
                ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][1].Value] = ghostChild0Translation;
                ghostRotationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value] = ghostChild1Rotation;
                ghostTranslationFromEntity[ghostLinkedEntityGroupArray[entityIndex][2].Value] = ghostChild1Translation;
                ghostAgentComponentArray[entityIndex] = ghostAgentComponent;
                ghostBackwardModifierArray[entityIndex] = ghostBackwardModifier;
                ghostCanMoveArray[entityIndex] = ghostCanMove;
                ghostDestinationComponentArray[entityIndex] = ghostDestinationComponent;
                ghostGameOrientationArray[entityIndex] = ghostGameOrientation;
                ghostGamePositionArray[entityIndex] = ghostGamePosition;
                ghostHealthArray[entityIndex] = ghostHealth;
                ghostSpeedArray[entityIndex] = ghostSpeed;
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
                ComponentType.ReadWrite<AgentSnapshotData>(),
                ComponentType.ReadOnly<GhostComponent>(),
                ComponentType.ReadWrite<AgentComponent>(),
                ComponentType.ReadWrite<BackwardModifier>(),
                ComponentType.ReadWrite<CanMove>(),
                ComponentType.ReadWrite<DestinationComponent>(),
                ComponentType.ReadWrite<GameOrientation>(),
                ComponentType.ReadWrite<GamePosition>(),
                ComponentType.ReadWrite<Health>(),
                ComponentType.ReadWrite<Speed>(),
                ComponentType.ReadOnly<LinkedEntityGroup>(),
            },
            None = new []{ComponentType.ReadWrite<PredictedGhostComponent>()}
        });
        m_predictedQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new []{
                ComponentType.ReadOnly<AgentSnapshotData>(),
                ComponentType.ReadOnly<GhostComponent>(),
                ComponentType.ReadOnly<PredictedGhostComponent>(),
                ComponentType.ReadWrite<AgentComponent>(),
                ComponentType.ReadWrite<BackwardModifier>(),
                ComponentType.ReadWrite<CanMove>(),
                ComponentType.ReadWrite<DestinationComponent>(),
                ComponentType.ReadWrite<GameOrientation>(),
                ComponentType.ReadWrite<GamePosition>(),
                ComponentType.ReadWrite<Health>(),
                ComponentType.ReadWrite<Speed>(),
                ComponentType.ReadOnly<LinkedEntityGroup>(),
            }
        });
        RequireForUpdate(GetEntityQuery(ComponentType.ReadWrite<AgentSnapshotData>(),
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
                ghostSnapshotDataType = GetArchetypeChunkBufferType<AgentSnapshotData>(true),
                ghostEntityType = GetArchetypeChunkEntityType(),
                predictedGhostComponentType = GetArchetypeChunkComponentType<PredictedGhostComponent>(),
                ghostAgentComponentType = GetArchetypeChunkComponentType<AgentComponent>(),
                ghostBackwardModifierType = GetArchetypeChunkComponentType<BackwardModifier>(),
                ghostCanMoveType = GetArchetypeChunkComponentType<CanMove>(),
                ghostDestinationComponentType = GetArchetypeChunkComponentType<DestinationComponent>(),
                ghostGameOrientationType = GetArchetypeChunkComponentType<GameOrientation>(),
                ghostGamePositionType = GetArchetypeChunkComponentType<GamePosition>(),
                ghostHealthType = GetArchetypeChunkComponentType<Health>(),
                ghostSpeedType = GetArchetypeChunkComponentType<Speed>(),
                ghostLinkedEntityGroupType = GetArchetypeChunkBufferType<LinkedEntityGroup>(true),
                ghostRotationFromEntity = GetComponentDataFromEntity<Rotation>(),
                ghostTranslationFromEntity = GetComponentDataFromEntity<Translation>(),

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
                ghostSnapshotDataType = GetArchetypeChunkBufferType<AgentSnapshotData>(true),
                ghostEntityType = GetArchetypeChunkEntityType(),
                ghostAgentComponentType = GetArchetypeChunkComponentType<AgentComponent>(),
                ghostBackwardModifierType = GetArchetypeChunkComponentType<BackwardModifier>(),
                ghostCanMoveType = GetArchetypeChunkComponentType<CanMove>(),
                ghostDestinationComponentType = GetArchetypeChunkComponentType<DestinationComponent>(),
                ghostGameOrientationType = GetArchetypeChunkComponentType<GameOrientation>(),
                ghostGamePositionType = GetArchetypeChunkComponentType<GamePosition>(),
                ghostHealthType = GetArchetypeChunkComponentType<Health>(),
                ghostSpeedType = GetArchetypeChunkComponentType<Speed>(),
                ghostLinkedEntityGroupType = GetArchetypeChunkBufferType<LinkedEntityGroup>(true),
                ghostRotationFromEntity = GetComponentDataFromEntity<Rotation>(),
                ghostTranslationFromEntity = GetComponentDataFromEntity<Translation>(),
                targetTick = m_ClientSimulationSystemGroup.InterpolationTick,
                targetTickFraction = m_ClientSimulationSystemGroup.InterpolationTickFraction
            };
            inputDeps = updateInterpolatedJob.Schedule(m_interpolatedQuery, JobHandle.CombineDependencies(inputDeps, m_GhostUpdateSystemGroup.LastGhostMapWriter));
        }
        return inputDeps;
    }
}
public partial class AgentGhostSpawnSystem : DefaultGhostSpawnSystem<AgentSnapshotData>
{
    struct SetPredictedDefault : IJobParallelFor
    {
        [ReadOnly] public NativeArray<AgentSnapshotData> snapshots;
        public NativeArray<int> predictionMask;
        [ReadOnly][DeallocateOnJobCompletion] public NativeArray<NetworkIdComponent> localPlayerId;
        public void Execute(int index)
        {
            if (localPlayerId.Length == 1 && snapshots[index].GetAgentComponentPlayerId() == localPlayerId[0].Value)
                predictionMask[index] = 1;
        }
    }
    protected override JobHandle SetPredictedGhostDefaults(NativeArray<AgentSnapshotData> snapshots, NativeArray<int> predictionMask, JobHandle inputDeps)
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
