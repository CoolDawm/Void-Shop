using UnityEngine;

public static class GameAudioEvents
{
    public static System.Action OnAdventureMusicRequested;
    public static System.Action OnBattleMusicRequested;

    public static System.Action OnFootstepRequested;
    public static System.Action OnPlayerAttackRequested;
    public static System.Action OnPlayerMagicCastRequested;
    public static System.Action OnPlayerHurtRequested;

    public static System.Action OnBossHurtRequested;
    public static System.Action<int> OnBossAttackRequested;// for test
    public static System.Action OnFootstepStopped; 


}