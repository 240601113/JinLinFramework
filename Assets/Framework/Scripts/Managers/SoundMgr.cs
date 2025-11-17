using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : UnitySingleton<SoundMgr>
{
    private const int MAX_SOUNDS = 8; // 同时播放8个音效;
    private const string MusicMuteKey = "isMusicMute";
    private const string SoundMuteKey = "isSoundMute";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    private List<AudioSource> sounds;  //使用List保存AudioSource音效
    private int curIndex; // 当前使用的第几个AudioSource来播放音效;
    private AudioSource musicSource = null;  //背景音乐

    private int isMusicMute = 0;  //是否音乐静音，0表示不静音，之所以不用bool类型，是因为后面用PlayerPrefs.GetInt
    private int isSoundMute = 0;  //是否音效静音，0表示不静音

    public void Init() 
    {
        this.sounds = new List<AudioSource>();

        for (int i = 0; i < MAX_SOUNDS; i++)  //创建播放MAX_SOUNDS音效的AudioSource
        {
            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
            this.sounds.Add(audioSource);
        }

        this.musicSource = this.gameObject.AddComponent<AudioSource>();  //创建播放音乐的AudioSource
        this.curIndex = 0;

        this.isMusicMute = 0;

        //通过本地文件获取是否静音
        if (PlayerPrefs.HasKey(MusicMuteKey)) 
        {
            this.isMusicMute = PlayerPrefs.GetInt(MusicMuteKey);
        }
        
        this.isSoundMute = 0;
        if (PlayerPrefs.HasKey(SoundMuteKey)) 
        {
            this.isSoundMute = PlayerPrefs.GetInt(SoundMuteKey);
        }

        float soundVolume = 1.0f;

        //通过本地文件获取音效音量大小
        if (PlayerPrefs.HasKey(SoundVolumeKey)) 
        {
            soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey);
        }

        this.SetSoundVolume(soundVolume);

        float musicVolume = 1.0f;
        if (PlayerPrefs.HasKey(MusicVolumeKey)) 
        {
            musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
        }
        this.SetMusicVolume(musicVolume);

    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="musicName">音乐存放的路径，从Asset开始的路径</param>
    /// <param name="loop">是否循环播放</param>
    public void PlayMusic(string musicName, bool loop = true) 
    {
        ////AudioClip clip = ResMgr.Instance.LoadAssetAsync<AudioClip>(musicName);
        ////先用同步加载测试
        //AudioClip clip = ResMgr.Instance.LoadAsset<AudioClip>(musicName);
        //if (clip == null) 
        //{
        //    return;
        //}

        //this.musicSource.clip = clip;  //把加载的音乐片段clip给AudioSource
        //this.musicSource.loop = loop;

        //if (this.isMusicMute != 0) 
        //{
        //    return;
        //}
        //this.musicSource.Play();  //播放音乐
    }

    /// <summary>
    /// 停止播放音乐
    /// </summary>
    public void StopMusic() 
    {
        this.musicSource.Stop();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">音效存放的路径，从Asset开始的路径</param>
    /// <param name="loop">是否循环</param>
    /// <returns></returns>
    //public int PlaySound(string soundName, bool loop = false) 
    //{
    //    if (this.isSoundMute != 0)   
    //    {
    //        return -1;
    //    }

    //    ////先用同步加载测试
    //    //AudioClip clip = ResMgr.Instance.LoadAsset<AudioClip>(soundName);
    //    //if (clip == null) 
    //    //{
    //    //    return -1;
    //    //}

    //    //int soundID = this.curIndex;    
    //    //AudioSource audioSource = this.sounds[this.curIndex];
    //    //this.curIndex++;
    //    //this.curIndex = (this.curIndex >= this.sounds.Count) ? 0 : this.curIndex;

    //    //audioSource.clip = clip;  //把加载的音效片段clip给AudioSource
    //    //audioSource.loop = loop;

    //    //if (this.isSoundMute != 0) 
    //    //{
    //    //    return soundID;
    //    //}

    //    //audioSource.Play();

    //    //return soundID;
    //}

    //public int PlayOneShot(string soundName, bool loop = false) 
    //{
    //    if (this.isSoundMute != 0) 
    //    {
    //        return -1;
    //    }

    //    //先用同步加载测试
    //    AudioClip clip = ResMgr.Instance.LoadAsset<AudioClip>(soundName);
    //    if (clip == null) {
    //        return -1;
    //    }

    //    int soundId = this.curIndex;
    //    AudioSource audioSource = this.sounds[this.curIndex];
    //    this.curIndex++;
    //    this.curIndex = (this.curIndex >= this.sounds.Count) ? 0 : this.curIndex;

    //    audioSource.clip = clip;
    //    audioSource.loop = loop;

    //    if (this.isSoundMute != 0) {
    //        return soundId;
    //    }

    //    audioSource.PlayOneShot(clip);

    //    return soundId;
    //}

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="soundId"></param>
    public void StopSound(int soundID) 
    {
        if (soundID < 0 || soundID >= this.sounds.Count) 
        {
            return;
        }

        AudioSource audioSource = this.sounds[soundID];
        audioSource.Stop();
    }

    /// <summary>
    /// 停掉所有音效
    /// </summary>
    public void StopAllSound() 
    {
        for (int i = 0; i < this.sounds.Count; i++) 
        {
            AudioSource audioSource = this.sounds[i];
            audioSource.Stop();
        }
    }

    /// <summary>
    /// 设置音乐是否要静音，并通过PlayerPrefs.SetInt将具体信息保存到本地
    /// </summary>
    /// <param name="isMute">是否要静音</param>
    public void SetMusicMute(bool isMute) 
    {
        bool isMuscMute = (this.isMusicMute != 0);
        if (isMuscMute == isMute) 
        {
            return;
        }

        this.isMusicMute = isMute ? 1 : 0;
        PlayerPrefs.SetInt(MusicMuteKey, this.isMusicMute);
        this.musicSource.mute = isMute;
    }

    /// <summary>
    /// 设置音效是否要静音，并通过PlayerPrefs.SetInt将具体信息保存到本地
    /// </summary>
    /// <param name="isMute">是否要静音</param>
    public void SetSoundMute(bool isMute) 
    {
        bool isSoundMute = (this.isSoundMute != 0);
        if (isSoundMute == isMute) 
        {
            return;
        }

        this.isSoundMute = isMute ? 1 : 0;
        PlayerPrefs.SetInt(SoundMuteKey, this.isSoundMute);

        // 音效都是短暂的，所以我们这里就不管它了;
        for (int i = 0; i < this.sounds.Count; i++) 
        {
            this.sounds[i].mute = isMute;
        }
        // end 
    }

    /// <summary>
    /// 设置音乐音量，并通过PlayerPrefs.SetInt将具体信息保存到本地
    /// </summary>
    /// <param name="per">音量，取值范围0～1</param>
    public void SetMusicVolume(float per) 
    {
        per = Mathf.Clamp(per, 0, 1);
        this.musicSource.volume = per;

        PlayerPrefs.SetFloat(MusicVolumeKey, per);
    }

    /// <summary>
    /// 设置音效音量，并通过PlayerPrefs.SetInt将具体信息保存到本地
    /// </summary>
    /// <param name="per">音量，取值范围0～1</param>
    public void SetSoundVolume(float per) 
    {
        per = Mathf.Clamp(per, 0, 1);
        for (int i = 0; i < this.sounds.Count; i++) 
        {
            this.sounds[i].volume = per;
        }

        PlayerPrefs.SetFloat(SoundVolumeKey, per);
    }
}
