using UnityEngine;

public class AudioService : MonoBehaviour
{
    public static AudioService Instance { get; private set; }

    [SerializeField] private AudioController _audioController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        _audioController?.UpdateMusicVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        _audioController?.UpdateSfxVolume(volume);
    }
}
