using UnityEngine;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
//make volume changes function for settings
public class AudioController : MonoBehaviour
{
    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    [Header("Audio Config")]
    [SerializeField] private AudioConfig _audioConfig;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    private void Awake()
    {
        _musicSource = GetComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = _musicMixerGroup;
        _musicSource.loop = true;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.outputAudioMixerGroup = _sfxMixerGroup;

        SubscribeToEvents();
        PlayAdventureMusic();
    }

    
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameAudioEvents.OnAdventureMusicRequested += PlayAdventureMusic;
        GameAudioEvents.OnBattleMusicRequested += PlayBattleMusic;
        GameAudioEvents.OnFootstepRequested += PlayFootstep;
        GameAudioEvents.OnPlayerAttackRequested += PlayPlayerAttack;
        GameAudioEvents.OnPlayerMagicCastRequested += PlayPlayerMagicCast;
        GameAudioEvents.OnPlayerHurtRequested += PlayPlayerHurt;
        GameAudioEvents.OnBossHurtRequested += PlayBossHurt;
        GameAudioEvents.OnBossAttackRequested += PlayBossAttack;
    }

    private void UnsubscribeFromEvents()
    {
        GameAudioEvents.OnAdventureMusicRequested -= PlayAdventureMusic;
        GameAudioEvents.OnBattleMusicRequested -= PlayBattleMusic;
        GameAudioEvents.OnFootstepRequested -= PlayFootstep;
        GameAudioEvents.OnPlayerAttackRequested -= PlayPlayerAttack;
        GameAudioEvents.OnPlayerMagicCastRequested -= PlayPlayerMagicCast;
        GameAudioEvents.OnPlayerHurtRequested -= PlayPlayerHurt;
        GameAudioEvents.OnBossHurtRequested -= PlayBossHurt;
    }

    public void PlayAdventureMusic()
    {
        if (_musicSource.clip != _audioConfig.adventureMusic)
        {
            _musicSource.clip = _audioConfig.adventureMusic;
            _musicSource.volume = _audioConfig.globalMusicVolume;
            _musicSource.Play();
        }
    }

    public void PlayBattleMusic()
    {
        if (_musicSource.clip != _audioConfig.battleMusic)
        {
            _musicSource.clip = _audioConfig.battleMusic;
            _musicSource.volume = _audioConfig.globalMusicVolume;
            _musicSource.Play();
        }
    }

    public void PlayFootstep()
    {
        PlayRandomizedSFX(_audioConfig.footsteps);
    }

    public void PlayPlayerAttack()
    {
        PlayRandomizedSFX(_audioConfig.meleeAttacks);
    }

    public void PlayPlayerMagicCast()
    {
        PlayRandomizedSFX(_audioConfig.magicCasts);
    }

    public void PlayPlayerHurt()
    {
        PlayRandomizedSFX(_audioConfig.playerHits);
    }

    public void PlayBossHurt()
    {
        PlayRandomizedSFX(_audioConfig.bossHits);
    }
    public void PlayBossAttack(int attackIndex)
    {
        PlaySFX(_audioConfig.bossSpecialAttacks.clips[attackIndex]);// maybe kostil maybe not - for checking
    }
    private void PlayRandomizedSFX(AudioConfig.SoundCategory category)
    {
        if (category.clips == null || category.clips.Length == 0) return;

        AudioClip clip = category.clips[Random.Range(0, category.clips.Length)];
        _sfxSource.pitch = Random.Range(1f - category.pitchVariation / 2, 1f + category.pitchVariation / 2);
        _sfxSource.PlayOneShot(clip, category.volume);
    }
    private void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        GameObject sfxObject = new GameObject("SFX_" + clip.name);
        AudioSource source = sfxObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = _sfxMixerGroup;
        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(sfxObject, clip.length);
    }

    // for real-time music and sfx change
    public void UpdateMusicVolume(float volume)
    {
        _audioConfig.globalMusicVolume = volume;
        _musicSource.volume = volume;
    }

    public void UpdateSfxVolume(float volume)
    {
        _audioConfig.globalSFXVolume = volume;
        _sfxSource.volume = volume;
    }
}