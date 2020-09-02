using Unity.Networking.Transport;
using Unity.NetCode;
using Unity.Mathematics;

public struct AgentSnapshotData : ISnapshotData<AgentSnapshotData>
{
    public uint tick;
    private int AgentComponentPlayerId;
    private int BackwardModifierValue;
    private int BusyTimerValue;
    private int DestinationComponentValueX;
    private int DestinationComponentValueY;
    private uint DestinationComponentValid;
    private int FreezeTimerValue;
    private int GameOrientationValueX;
    private int GameOrientationValueY;
    private int GamePositionValueX;
    private int GamePositionValueY;
    private int HealthValue;
    private int Healthregen;
    private int Healthmax;
    private int RotatingValue;
    private int SpeedValue;
    private int RotationValueX;
    private int RotationValueY;
    private int RotationValueZ;
    private int RotationValueW;
    private int TranslationValueX;
    private int TranslationValueY;
    private int TranslationValueZ;
    uint changeMask0;

    public uint Tick => tick;
    public int GetAgentComponentPlayerId(GhostDeserializerState deserializerState)
    {
        return (int)AgentComponentPlayerId;
    }
    public int GetAgentComponentPlayerId()
    {
        return (int)AgentComponentPlayerId;
    }
    public void SetAgentComponentPlayerId(int val, GhostSerializerState serializerState)
    {
        AgentComponentPlayerId = (int)val;
    }
    public void SetAgentComponentPlayerId(int val)
    {
        AgentComponentPlayerId = (int)val;
    }
    public float GetBackwardModifierValue(GhostDeserializerState deserializerState)
    {
        return BackwardModifierValue * 0.001f;
    }
    public float GetBackwardModifierValue()
    {
        return BackwardModifierValue * 0.001f;
    }
    public void SetBackwardModifierValue(float val, GhostSerializerState serializerState)
    {
        BackwardModifierValue = (int)(val * 1000);
    }
    public void SetBackwardModifierValue(float val)
    {
        BackwardModifierValue = (int)(val * 1000);
    }
    public float GetBusyTimerValue(GhostDeserializerState deserializerState)
    {
        return BusyTimerValue * 0.001f;
    }
    public float GetBusyTimerValue()
    {
        return BusyTimerValue * 0.001f;
    }
    public void SetBusyTimerValue(float val, GhostSerializerState serializerState)
    {
        BusyTimerValue = (int)(val * 1000);
    }
    public void SetBusyTimerValue(float val)
    {
        BusyTimerValue = (int)(val * 1000);
    }
    public float2 GetDestinationComponentValue(GhostDeserializerState deserializerState)
    {
        return GetDestinationComponentValue();
    }
    public float2 GetDestinationComponentValue()
    {
        return new float2(DestinationComponentValueX * 0.001f, DestinationComponentValueY * 0.001f);
    }
    public void SetDestinationComponentValue(float2 val, GhostSerializerState serializerState)
    {
        SetDestinationComponentValue(val);
    }
    public void SetDestinationComponentValue(float2 val)
    {
        DestinationComponentValueX = (int)(val.x * 1000);
        DestinationComponentValueY = (int)(val.y * 1000);
    }
    public bool GetDestinationComponentValid(GhostDeserializerState deserializerState)
    {
        return DestinationComponentValid!=0;
    }
    public bool GetDestinationComponentValid()
    {
        return DestinationComponentValid!=0;
    }
    public void SetDestinationComponentValid(bool val, GhostSerializerState serializerState)
    {
        DestinationComponentValid = val?1u:0;
    }
    public void SetDestinationComponentValid(bool val)
    {
        DestinationComponentValid = val?1u:0;
    }
    public float GetFreezeTimerValue(GhostDeserializerState deserializerState)
    {
        return FreezeTimerValue * 0.001f;
    }
    public float GetFreezeTimerValue()
    {
        return FreezeTimerValue * 0.001f;
    }
    public void SetFreezeTimerValue(float val, GhostSerializerState serializerState)
    {
        FreezeTimerValue = (int)(val * 1000);
    }
    public void SetFreezeTimerValue(float val)
    {
        FreezeTimerValue = (int)(val * 1000);
    }
    public float2 GetGameOrientationValue(GhostDeserializerState deserializerState)
    {
        return GetGameOrientationValue();
    }
    public float2 GetGameOrientationValue()
    {
        return new float2(GameOrientationValueX * 0.001f, GameOrientationValueY * 0.001f);
    }
    public void SetGameOrientationValue(float2 val, GhostSerializerState serializerState)
    {
        SetGameOrientationValue(val);
    }
    public void SetGameOrientationValue(float2 val)
    {
        GameOrientationValueX = (int)(val.x * 1000);
        GameOrientationValueY = (int)(val.y * 1000);
    }
    public float2 GetGamePositionValue(GhostDeserializerState deserializerState)
    {
        return GetGamePositionValue();
    }
    public float2 GetGamePositionValue()
    {
        return new float2(GamePositionValueX * 0.001f, GamePositionValueY * 0.001f);
    }
    public void SetGamePositionValue(float2 val, GhostSerializerState serializerState)
    {
        SetGamePositionValue(val);
    }
    public void SetGamePositionValue(float2 val)
    {
        GamePositionValueX = (int)(val.x * 1000);
        GamePositionValueY = (int)(val.y * 1000);
    }
    public float GetHealthValue(GhostDeserializerState deserializerState)
    {
        return HealthValue * 0.001f;
    }
    public float GetHealthValue()
    {
        return HealthValue * 0.001f;
    }
    public void SetHealthValue(float val, GhostSerializerState serializerState)
    {
        HealthValue = (int)(val * 1000);
    }
    public void SetHealthValue(float val)
    {
        HealthValue = (int)(val * 1000);
    }
    public float GetHealthregen(GhostDeserializerState deserializerState)
    {
        return Healthregen * 0.001f;
    }
    public float GetHealthregen()
    {
        return Healthregen * 0.001f;
    }
    public void SetHealthregen(float val, GhostSerializerState serializerState)
    {
        Healthregen = (int)(val * 1000);
    }
    public void SetHealthregen(float val)
    {
        Healthregen = (int)(val * 1000);
    }
    public float GetHealthmax(GhostDeserializerState deserializerState)
    {
        return Healthmax * 0.001f;
    }
    public float GetHealthmax()
    {
        return Healthmax * 0.001f;
    }
    public void SetHealthmax(float val, GhostSerializerState serializerState)
    {
        Healthmax = (int)(val * 1000);
    }
    public void SetHealthmax(float val)
    {
        Healthmax = (int)(val * 1000);
    }
    public int GetRotatingValue(GhostDeserializerState deserializerState)
    {
        return (int)RotatingValue;
    }
    public int GetRotatingValue()
    {
        return (int)RotatingValue;
    }
    public void SetRotatingValue(int val, GhostSerializerState serializerState)
    {
        RotatingValue = (int)val;
    }
    public void SetRotatingValue(int val)
    {
        RotatingValue = (int)val;
    }
    public float GetSpeedValue(GhostDeserializerState deserializerState)
    {
        return SpeedValue * 0.001f;
    }
    public float GetSpeedValue()
    {
        return SpeedValue * 0.001f;
    }
    public void SetSpeedValue(float val, GhostSerializerState serializerState)
    {
        SpeedValue = (int)(val * 1000);
    }
    public void SetSpeedValue(float val)
    {
        SpeedValue = (int)(val * 1000);
    }
    public quaternion GetRotationValue(GhostDeserializerState deserializerState)
    {
        return GetRotationValue();
    }
    public quaternion GetRotationValue()
    {
        return new quaternion(RotationValueX * 0.001f, RotationValueY * 0.001f, RotationValueZ * 0.001f, RotationValueW * 0.001f);
    }
    public void SetRotationValue(quaternion q, GhostSerializerState serializerState)
    {
        SetRotationValue(q);
    }
    public void SetRotationValue(quaternion q)
    {
        RotationValueX = (int)(q.value.x * 1000);
        RotationValueY = (int)(q.value.y * 1000);
        RotationValueZ = (int)(q.value.z * 1000);
        RotationValueW = (int)(q.value.w * 1000);
    }
    public float3 GetTranslationValue(GhostDeserializerState deserializerState)
    {
        return GetTranslationValue();
    }
    public float3 GetTranslationValue()
    {
        return new float3(TranslationValueX * 0.01f, TranslationValueY * 0.01f, TranslationValueZ * 0.01f);
    }
    public void SetTranslationValue(float3 val, GhostSerializerState serializerState)
    {
        SetTranslationValue(val);
    }
    public void SetTranslationValue(float3 val)
    {
        TranslationValueX = (int)(val.x * 100);
        TranslationValueY = (int)(val.y * 100);
        TranslationValueZ = (int)(val.z * 100);
    }

    public void PredictDelta(uint tick, ref AgentSnapshotData baseline1, ref AgentSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        AgentComponentPlayerId = predictor.PredictInt(AgentComponentPlayerId, baseline1.AgentComponentPlayerId, baseline2.AgentComponentPlayerId);
        BackwardModifierValue = predictor.PredictInt(BackwardModifierValue, baseline1.BackwardModifierValue, baseline2.BackwardModifierValue);
        BusyTimerValue = predictor.PredictInt(BusyTimerValue, baseline1.BusyTimerValue, baseline2.BusyTimerValue);
        DestinationComponentValueX = predictor.PredictInt(DestinationComponentValueX, baseline1.DestinationComponentValueX, baseline2.DestinationComponentValueX);
        DestinationComponentValueY = predictor.PredictInt(DestinationComponentValueY, baseline1.DestinationComponentValueY, baseline2.DestinationComponentValueY);
        DestinationComponentValid = (uint)predictor.PredictInt((int)DestinationComponentValid, (int)baseline1.DestinationComponentValid, (int)baseline2.DestinationComponentValid);
        FreezeTimerValue = predictor.PredictInt(FreezeTimerValue, baseline1.FreezeTimerValue, baseline2.FreezeTimerValue);
        GameOrientationValueX = predictor.PredictInt(GameOrientationValueX, baseline1.GameOrientationValueX, baseline2.GameOrientationValueX);
        GameOrientationValueY = predictor.PredictInt(GameOrientationValueY, baseline1.GameOrientationValueY, baseline2.GameOrientationValueY);
        GamePositionValueX = predictor.PredictInt(GamePositionValueX, baseline1.GamePositionValueX, baseline2.GamePositionValueX);
        GamePositionValueY = predictor.PredictInt(GamePositionValueY, baseline1.GamePositionValueY, baseline2.GamePositionValueY);
        HealthValue = predictor.PredictInt(HealthValue, baseline1.HealthValue, baseline2.HealthValue);
        Healthregen = predictor.PredictInt(Healthregen, baseline1.Healthregen, baseline2.Healthregen);
        Healthmax = predictor.PredictInt(Healthmax, baseline1.Healthmax, baseline2.Healthmax);
        RotatingValue = predictor.PredictInt(RotatingValue, baseline1.RotatingValue, baseline2.RotatingValue);
        SpeedValue = predictor.PredictInt(SpeedValue, baseline1.SpeedValue, baseline2.SpeedValue);
        RotationValueX = predictor.PredictInt(RotationValueX, baseline1.RotationValueX, baseline2.RotationValueX);
        RotationValueY = predictor.PredictInt(RotationValueY, baseline1.RotationValueY, baseline2.RotationValueY);
        RotationValueZ = predictor.PredictInt(RotationValueZ, baseline1.RotationValueZ, baseline2.RotationValueZ);
        RotationValueW = predictor.PredictInt(RotationValueW, baseline1.RotationValueW, baseline2.RotationValueW);
        TranslationValueX = predictor.PredictInt(TranslationValueX, baseline1.TranslationValueX, baseline2.TranslationValueX);
        TranslationValueY = predictor.PredictInt(TranslationValueY, baseline1.TranslationValueY, baseline2.TranslationValueY);
        TranslationValueZ = predictor.PredictInt(TranslationValueZ, baseline1.TranslationValueZ, baseline2.TranslationValueZ);
    }

    public void Serialize(int networkId, ref AgentSnapshotData baseline, ref DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        changeMask0 = (AgentComponentPlayerId != baseline.AgentComponentPlayerId) ? 1u : 0;
        changeMask0 |= (BackwardModifierValue != baseline.BackwardModifierValue) ? (1u<<1) : 0;
        changeMask0 |= (BusyTimerValue != baseline.BusyTimerValue) ? (1u<<2) : 0;
        changeMask0 |= (DestinationComponentValueX != baseline.DestinationComponentValueX ||
                                           DestinationComponentValueY != baseline.DestinationComponentValueY) ? (1u<<3) : 0;
        changeMask0 |= (DestinationComponentValid != baseline.DestinationComponentValid) ? (1u<<4) : 0;
        changeMask0 |= (FreezeTimerValue != baseline.FreezeTimerValue) ? (1u<<5) : 0;
        changeMask0 |= (GameOrientationValueX != baseline.GameOrientationValueX ||
                                           GameOrientationValueY != baseline.GameOrientationValueY) ? (1u<<6) : 0;
        changeMask0 |= (GamePositionValueX != baseline.GamePositionValueX ||
                                           GamePositionValueY != baseline.GamePositionValueY) ? (1u<<7) : 0;
        changeMask0 |= (HealthValue != baseline.HealthValue) ? (1u<<8) : 0;
        changeMask0 |= (Healthregen != baseline.Healthregen) ? (1u<<9) : 0;
        changeMask0 |= (Healthmax != baseline.Healthmax) ? (1u<<10) : 0;
        changeMask0 |= (RotatingValue != baseline.RotatingValue) ? (1u<<11) : 0;
        changeMask0 |= (SpeedValue != baseline.SpeedValue) ? (1u<<12) : 0;
        changeMask0 |= (RotationValueX != baseline.RotationValueX ||
                                           RotationValueY != baseline.RotationValueY ||
                                           RotationValueZ != baseline.RotationValueZ ||
                                           RotationValueW != baseline.RotationValueW) ? (1u<<13) : 0;
        changeMask0 |= (TranslationValueX != baseline.TranslationValueX ||
                                           TranslationValueY != baseline.TranslationValueY ||
                                           TranslationValueZ != baseline.TranslationValueZ) ? (1u<<14) : 0;
        writer.WritePackedUIntDelta(changeMask0, baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            writer.WritePackedIntDelta(AgentComponentPlayerId, baseline.AgentComponentPlayerId, compressionModel);
        if ((changeMask0 & (1 << 1)) != 0)
            writer.WritePackedIntDelta(BackwardModifierValue, baseline.BackwardModifierValue, compressionModel);
        if ((changeMask0 & (1 << 2)) != 0)
            writer.WritePackedIntDelta(BusyTimerValue, baseline.BusyTimerValue, compressionModel);
        if ((changeMask0 & (1 << 3)) != 0)
        {
            writer.WritePackedIntDelta(DestinationComponentValueX, baseline.DestinationComponentValueX, compressionModel);
            writer.WritePackedIntDelta(DestinationComponentValueY, baseline.DestinationComponentValueY, compressionModel);
        }
        if ((changeMask0 & (1 << 4)) != 0)
            writer.WritePackedUIntDelta(DestinationComponentValid, baseline.DestinationComponentValid, compressionModel);
        if ((changeMask0 & (1 << 5)) != 0)
            writer.WritePackedIntDelta(FreezeTimerValue, baseline.FreezeTimerValue, compressionModel);
        if ((changeMask0 & (1 << 6)) != 0)
        {
            writer.WritePackedIntDelta(GameOrientationValueX, baseline.GameOrientationValueX, compressionModel);
            writer.WritePackedIntDelta(GameOrientationValueY, baseline.GameOrientationValueY, compressionModel);
        }
        if ((changeMask0 & (1 << 7)) != 0)
        {
            writer.WritePackedIntDelta(GamePositionValueX, baseline.GamePositionValueX, compressionModel);
            writer.WritePackedIntDelta(GamePositionValueY, baseline.GamePositionValueY, compressionModel);
        }
        if ((changeMask0 & (1 << 8)) != 0)
            writer.WritePackedIntDelta(HealthValue, baseline.HealthValue, compressionModel);
        if ((changeMask0 & (1 << 9)) != 0)
            writer.WritePackedIntDelta(Healthregen, baseline.Healthregen, compressionModel);
        if ((changeMask0 & (1 << 10)) != 0)
            writer.WritePackedIntDelta(Healthmax, baseline.Healthmax, compressionModel);
        if ((changeMask0 & (1 << 11)) != 0)
            writer.WritePackedIntDelta(RotatingValue, baseline.RotatingValue, compressionModel);
        if ((changeMask0 & (1 << 12)) != 0)
            writer.WritePackedIntDelta(SpeedValue, baseline.SpeedValue, compressionModel);
        if ((changeMask0 & (1 << 13)) != 0)
        {
            writer.WritePackedIntDelta(RotationValueX, baseline.RotationValueX, compressionModel);
            writer.WritePackedIntDelta(RotationValueY, baseline.RotationValueY, compressionModel);
            writer.WritePackedIntDelta(RotationValueZ, baseline.RotationValueZ, compressionModel);
            writer.WritePackedIntDelta(RotationValueW, baseline.RotationValueW, compressionModel);
        }
        if ((changeMask0 & (1 << 14)) != 0)
        {
            writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
            writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
            writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);
        }
    }

    public void Deserialize(uint tick, ref AgentSnapshotData baseline, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        changeMask0 = reader.ReadPackedUIntDelta(baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            AgentComponentPlayerId = reader.ReadPackedIntDelta(baseline.AgentComponentPlayerId, compressionModel);
        else
            AgentComponentPlayerId = baseline.AgentComponentPlayerId;
        if ((changeMask0 & (1 << 1)) != 0)
            BackwardModifierValue = reader.ReadPackedIntDelta(baseline.BackwardModifierValue, compressionModel);
        else
            BackwardModifierValue = baseline.BackwardModifierValue;
        if ((changeMask0 & (1 << 2)) != 0)
            BusyTimerValue = reader.ReadPackedIntDelta(baseline.BusyTimerValue, compressionModel);
        else
            BusyTimerValue = baseline.BusyTimerValue;
        if ((changeMask0 & (1 << 3)) != 0)
        {
            DestinationComponentValueX = reader.ReadPackedIntDelta(baseline.DestinationComponentValueX, compressionModel);
            DestinationComponentValueY = reader.ReadPackedIntDelta(baseline.DestinationComponentValueY, compressionModel);
        }
        else
        {
            DestinationComponentValueX = baseline.DestinationComponentValueX;
            DestinationComponentValueY = baseline.DestinationComponentValueY;
        }
        if ((changeMask0 & (1 << 4)) != 0)
            DestinationComponentValid = reader.ReadPackedUIntDelta(baseline.DestinationComponentValid, compressionModel);
        else
            DestinationComponentValid = baseline.DestinationComponentValid;
        if ((changeMask0 & (1 << 5)) != 0)
            FreezeTimerValue = reader.ReadPackedIntDelta(baseline.FreezeTimerValue, compressionModel);
        else
            FreezeTimerValue = baseline.FreezeTimerValue;
        if ((changeMask0 & (1 << 6)) != 0)
        {
            GameOrientationValueX = reader.ReadPackedIntDelta(baseline.GameOrientationValueX, compressionModel);
            GameOrientationValueY = reader.ReadPackedIntDelta(baseline.GameOrientationValueY, compressionModel);
        }
        else
        {
            GameOrientationValueX = baseline.GameOrientationValueX;
            GameOrientationValueY = baseline.GameOrientationValueY;
        }
        if ((changeMask0 & (1 << 7)) != 0)
        {
            GamePositionValueX = reader.ReadPackedIntDelta(baseline.GamePositionValueX, compressionModel);
            GamePositionValueY = reader.ReadPackedIntDelta(baseline.GamePositionValueY, compressionModel);
        }
        else
        {
            GamePositionValueX = baseline.GamePositionValueX;
            GamePositionValueY = baseline.GamePositionValueY;
        }
        if ((changeMask0 & (1 << 8)) != 0)
            HealthValue = reader.ReadPackedIntDelta(baseline.HealthValue, compressionModel);
        else
            HealthValue = baseline.HealthValue;
        if ((changeMask0 & (1 << 9)) != 0)
            Healthregen = reader.ReadPackedIntDelta(baseline.Healthregen, compressionModel);
        else
            Healthregen = baseline.Healthregen;
        if ((changeMask0 & (1 << 10)) != 0)
            Healthmax = reader.ReadPackedIntDelta(baseline.Healthmax, compressionModel);
        else
            Healthmax = baseline.Healthmax;
        if ((changeMask0 & (1 << 11)) != 0)
            RotatingValue = reader.ReadPackedIntDelta(baseline.RotatingValue, compressionModel);
        else
            RotatingValue = baseline.RotatingValue;
        if ((changeMask0 & (1 << 12)) != 0)
            SpeedValue = reader.ReadPackedIntDelta(baseline.SpeedValue, compressionModel);
        else
            SpeedValue = baseline.SpeedValue;
        if ((changeMask0 & (1 << 13)) != 0)
        {
            RotationValueX = reader.ReadPackedIntDelta(baseline.RotationValueX, compressionModel);
            RotationValueY = reader.ReadPackedIntDelta(baseline.RotationValueY, compressionModel);
            RotationValueZ = reader.ReadPackedIntDelta(baseline.RotationValueZ, compressionModel);
            RotationValueW = reader.ReadPackedIntDelta(baseline.RotationValueW, compressionModel);
        }
        else
        {
            RotationValueX = baseline.RotationValueX;
            RotationValueY = baseline.RotationValueY;
            RotationValueZ = baseline.RotationValueZ;
            RotationValueW = baseline.RotationValueW;
        }
        if ((changeMask0 & (1 << 14)) != 0)
        {
            TranslationValueX = reader.ReadPackedIntDelta(baseline.TranslationValueX, compressionModel);
            TranslationValueY = reader.ReadPackedIntDelta(baseline.TranslationValueY, compressionModel);
            TranslationValueZ = reader.ReadPackedIntDelta(baseline.TranslationValueZ, compressionModel);
        }
        else
        {
            TranslationValueX = baseline.TranslationValueX;
            TranslationValueY = baseline.TranslationValueY;
            TranslationValueZ = baseline.TranslationValueZ;
        }
    }
    public void Interpolate(ref AgentSnapshotData target, float factor)
    {
        SetRotationValue(math.slerp(GetRotationValue(), target.GetRotationValue(), factor));
        SetTranslationValue(math.lerp(GetTranslationValue(), target.GetTranslationValue(), factor));
    }
}
