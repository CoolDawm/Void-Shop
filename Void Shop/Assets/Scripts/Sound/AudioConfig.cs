using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "AudioSO/AudioConfig")]
public class AudioConfig : ScriptableObject
{

    [System.Serializable]
    public class SoundCategory
    {
        public string categoryName;
        public AudioClip[] clips;
        [Range(0.1f, 2f)] public float pitchVariation = 1f;
        [Range(0f, 10f)] public float volume = 1f;  
    }

    [Header("Global Volume")]
    [Range(0f, 1f)] public float globalSFXVolume = 1f;  
    [Range(0f, 1f)] public float globalMusicVolume = 1f; 

    [Header("Music")]
    public AudioClip adventureMusic;
    public AudioClip battleMusic;

    [Header("Player Sounds")]
    public SoundCategory footsteps;
    public SoundCategory meleeAttacks;
    public SoundCategory magicCasts;
    public SoundCategory playerHits;

    [Header("Boss Sounds")]
    public SoundCategory bossHits;
    public SoundCategory bossSpecialAttacks;
}