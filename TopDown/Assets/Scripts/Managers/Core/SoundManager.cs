using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount]; // 3D 음향을 고려한다면 각 객체마다 오디오 소스를 가지고 있는 편이 나을테디 딕셔너리로 관리하거나 별도 베이스 컴퍼넌트에서 오디오 소스를 가지고 상속받는 식으로 하는 편이 좋을 듯
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundName = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundName.Length-1; i ++)
            {
                GameObject go = new GameObject { name = soundName[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            _audioSources[(int)Define.Sound.BGM].loop = true;
            _audioSources[(int)Define.Sound.Effect].spatialBlend = 1.0f;
        }
    }


    public void Play( string path, Define.Sound type = Define.Sound.Effect, float pitch =1.0f)
    {
        AudioClip audio = GetOrAddAudioClips(path,type); 
        Play(audio, type, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;
 
        if (type == Define.Sound.BGM)
        {
            AudioSource audiosource = _audioSources[(int)type];
            if (audiosource.isPlaying)
                audiosource.Stop();
            audiosource.pitch = pitch;
            audiosource.clip = audioClip;
            audiosource.Play();
        }

        else
        {
  
            AudioSource audiosource = _audioSources[(int)type];
            audiosource.pitch = pitch;
            audiosource.PlayOneShot(audioClip);
        }
    }
        

    public void PlayClipAtObject(string path, GameObject go,Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audio = GetOrAddAudioClips(path);
        PlayClipAtObject(audio, go, type, pitch);
    }

    public void PlayClipAtObject(AudioClip audioClip, GameObject go, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null || go == null)
            return;
        AudioSource audiosource = go.GetOrAddComponent<AudioSource>();
        if (type == Define.Sound.BGM)
        {
            if (audiosource.isPlaying)
                audiosource.Stop();
            audiosource.loop = true;
            audiosource.pitch = pitch;
            audiosource.clip = audioClip;
            audiosource.Play();
        }

        else
        {
            audiosource.loop = false;
            audiosource.pitch = pitch;
            audiosource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClips(string path, Define.Sound type = Define.Sound.Effect)
    {

        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.BGM)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }

        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }

        }
        if (audioClip == null)
            Debug.LogWarning($"AudioClip Missing Failed {path}");

        return audioClip;
    }

    public  void    Clear()
    {
        foreach(AudioSource source in _audioSources)
        {
            source.clip = null;
            source.Stop();
        }
        _audioClips.Clear();
    }
}
