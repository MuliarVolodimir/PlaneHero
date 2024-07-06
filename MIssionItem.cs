using System;
using UnityEngine;

[Serializable]
public class MissionItem
{
    public string MissionCondition;
    public int CurProgress;
    public int MaxProgress;
    public Sprite RewardSprite;
    public int Reward;
    public RewardType Type;

    public enum RewardType
    {
        Coins,
        Crowbars,
        Exp
    }
}
