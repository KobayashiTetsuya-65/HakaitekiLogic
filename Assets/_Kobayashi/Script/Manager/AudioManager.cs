using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SoundDataBaseSo _soundDataBase;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private Transform _seRoot;
    [SerializeField] private int _sePoolSize = 10;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _seGroup;

    private readonly Queue<AudioSource> _seAudioSourcePools = new Queue<AudioSource>();
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        LoadVolume();
    }
    private void Start()
    {
        if(_seRoot == null)
        {
            _seRoot = transform;
        }
        for(int i = 0; i < _sePoolSize; i++)
        {
            GameObject instance = new GameObject("SeAudioSource_"+ i, typeof(AudioSource));
            instance.transform.SetParent(_seRoot);
            AudioSource audioSource = instance.GetComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = _seGroup;
            audioSource.playOnAwake = false;
            instance.gameObject.SetActive(false);
            _seAudioSourcePools.Enqueue(audioSource);
        }
    }
    /// <summary>
    /// éwíËÇÃBGMÇó¨Ç∑
    /// </summary>
    /// <param name="key"></param>
    public void PlayBGM(string key)
    {
        StopBGM();
        var soundData = _soundDataBase.GetSoundData(key);
        if(soundData == null)
        {
            Debug.LogWarning("Sound Data not found: " + key);
            return;
        }
        _bgmSource.PrepareAudioSource(soundData);
        _bgmSource.Play();
    }
    /// <summary>
    /// ó¨ÇÍÇƒÇ¢ÇÈBGMÇé~ÇﬂÇÈ
    /// </summary>
    public void StopBGM()
    {
        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
    }
    public void PlaySe(string key)
    {
        var soundData = _soundDataBase.GetSoundData(key);
        if (soundData == null)
        {
            Debug.LogWarning("Sound Data not found: " + key);
            return;
        }
        AudioSource seAudioSource = default;
        if(_seAudioSourcePools.TryDequeue(out AudioSource source))
        {
            seAudioSource = source;
        }
        else
        {
            seAudioSource = new GameObject("seAudioSource_" + "NewInstance", typeof(AudioSource)).GetComponent<AudioSource>();
        }
        seAudioSource.PrepareAudioSource(soundData);
        seAudioSource.gameObject.SetActive(true);
        seAudioSource.Play();
        StartCoroutine(ReturnToPoolAfterPlaying(seAudioSource));
    }
    /// <summary>
    /// ï€ë∂Ç≥ÇÍÇΩâπó Çì«Ç›çûÇﬁ
    /// </summary>
    private void LoadVolume()
    {
        string[] parameters = { "Master", "BGM", "SE" };
        foreach(string p in parameters)
        {
            if (PlayerPrefs.HasKey(p))
            {
                float v = PlayerPrefs.GetFloat(p,1f);
                float dB = Mathf.Log10(Mathf.Clamp(v, 0.0001f, 1f)) * 20f;
                _mixer.SetFloat(p, dB);
            }
        }
    }
    /// <summary>
    /// égÇ¢èIÇÌÇ¡ÇΩAudioSourceÇPoolÇ÷ï‘Ç∑
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private IEnumerator ReturnToPoolAfterPlaying(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        source.gameObject.SetActive(false);
        _seAudioSourcePools.Enqueue(source);
    }
}
