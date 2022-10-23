using UnityEngine;

[System.Serializable]
public class Save 
{
    public int _savedGreens, _levelsBeaten, _lives, _availableKeys, _unlockedExtraLevels, _beatenExtraLevels;
    public bool _sfxMuted, _musicMuted;
    public bool[] _keysCollected;
    public bool _playIntro, _tripleJump, _lessDamage, _moreGreen, _noFlyConsume, _lessImpactDamage, _moreStartGreen, _hasRecievedEndReward, _hasPaid, _bumbleBee, _fingerPos,
            _tripleJumpPaidS, _lessDamagePaidS, _moreGreenPaidS, _noFlyConsumePaidS, _lessImpactDamagePaidS, _moreStartGreenPaidS;
}
