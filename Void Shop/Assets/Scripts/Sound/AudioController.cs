using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    [Header("Audio Config")]
    [SerializeField] private AudioConfig _audioConfig;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;
    private AudioSource _footstepSource;

    private void Awake()
    {
        // Инициализация музыкального источника
        _musicSource = GetComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = _musicMixerGroup;
        _musicSource.loop = true;

        // Инициализация общего SFX источника
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.outputAudioMixerGroup = _sfxMixerGroup;

        // Инициализация отдельного источника для шагов
        GameObject footstepObj = new GameObject("FootstepSource");
        footstepObj.transform.SetParent(transform);
        _footstepSource = footstepObj.AddComponent<AudioSource>();
        _footstepSource.outputAudioMixerGroup = _sfxMixerGroup;
        _footstepSource.playOnAwake = false;
        SubscribeToEvents();
        PlayAdventureMusic();
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
        GameAudioEvents.OnFootstepStopped += StopFootstep;

    }

    private void OnDestroy()
    {
        GameAudioEvents.OnAdventureMusicRequested -= PlayAdventureMusic;
        GameAudioEvents.OnBattleMusicRequested -= PlayBattleMusic;
        GameAudioEvents.OnFootstepRequested -= PlayFootstep;
        GameAudioEvents.OnPlayerAttackRequested -= PlayPlayerAttack;
        GameAudioEvents.OnPlayerMagicCastRequested -= PlayPlayerMagicCast;
        GameAudioEvents.OnPlayerHurtRequested -= PlayPlayerHurt;
        GameAudioEvents.OnBossHurtRequested -= PlayBossHurt;
        GameAudioEvents.OnBossAttackRequested -= PlayBossAttack;
        GameAudioEvents.OnFootstepStopped -= StopFootstep;
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
        if (_audioConfig.footsteps.clips.Length == 0 || _footstepSource.isPlaying)
            return;

        AudioClip clip = _audioConfig.footsteps.clips[Random.Range(0, _audioConfig.footsteps.clips.Length)];
        _footstepSource.clip = clip; // Назначаем клип
        _footstepSource.pitch = Random.Range(
            1f - _audioConfig.footsteps.pitchVariation / 2,
            1f + _audioConfig.footsteps.pitchVariation / 2
        );
        _footstepSource.volume = _audioConfig.footsteps.volume * _audioConfig.globalSFXVolume;
        _footstepSource.Play(); // Используем Play() вместо PlayOneShot()
    }

    public void StopFootstep()
    {
        if (_footstepSource.isPlaying)
        {
            _footstepSource.Stop();
        }
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
        if (_audioConfig.bossSpecialAttacks.clips == null ||
            attackIndex < 0 ||
            attackIndex >= _audioConfig.bossSpecialAttacks.clips.Length)
            return;

        PlaySFX(_audioConfig.bossSpecialAttacks.clips[attackIndex]);
    }

    private void PlayRandomizedSFX(AudioConfig.SoundCategory category)
    {
        if (category.clips == null || category.clips.Length == 0) return;

        AudioClip clip = category.clips[Random.Range(0, category.clips.Length)];
        _sfxSource.pitch = Random.Range(
            1f - category.pitchVariation / 2,
            1f + category.pitchVariation / 2
        );
        _sfxSource.PlayOneShot(clip, category.volume * _audioConfig.globalSFXVolume);
    }

    private void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        GameObject sfxObject = new GameObject("SFX_" + clip.name);
        AudioSource source = sfxObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = _sfxMixerGroup;
        source.clip = clip;
        source.volume = volume * _audioConfig.globalSFXVolume;
        source.Play();
        Destroy(sfxObject, clip.length);
    }

    public void UpdateMusicVolume(float volume)
    {
        _audioConfig.globalMusicVolume = volume;
        _musicSource.volume = volume;
    }

    public void UpdateSfxVolume(float volume)
    {
        _audioConfig.globalSFXVolume = volume;
        _sfxSource.volume = volume;
        _footstepSource.volume = volume;
    }
}