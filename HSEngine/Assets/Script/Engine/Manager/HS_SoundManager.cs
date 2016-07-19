using UnityEngine;
using System.Collections;
using HS.Base;

namespace HS.Manager
{
    public class HS_SoundManager : HS_SingletonGameObject<HS_SoundManager>
    {
        public AudioSource PlaySound(string clipName, bool simpleTube = false)
        {
            return PlaySound(HS_ResourceManager.LoadAsset<AudioClip>(clipName), 1f, 1f, simpleTube);
        }

        public AudioSource PlaySound(AudioClip clip, float volume, float pitch, bool simpleTube)
        {
            if (clip != null && volume > 0.01f)
            {
                AudioSource source = gameObject.GetComponent<AudioSource>();
                if (source == null)
                {
                    source = gameObject.AddComponent<AudioSource>();
                }
                if (simpleTube)
                {
                    source.clip = clip;
                    source.volume = volume;
                    source.Play();
                }
                else
                {
                    source.pitch = pitch;
                    source.PlayOneShot(clip, volume);
                }
            }
            return null;
        }

        public AudioSource PlayMusic(string clipName, bool loop = true)
        {
            return PlayMusic(HS_ResourceManager.LoadAsset<AudioClip>(clipName), 1f, 1f, loop);
        }

        public AudioSource PlayMusic(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = true)
        {
            if (volume > 0.01f)
            {
                Transform music = transform.Find("Music");
                AudioSource source = null;
                if (music == null)
                {
                    GameObject go = new GameObject();
                    music = go.transform;
                    music.transform.SetParent(transform);
                    music.transform.localPosition = Vector3.zero;
                    music.transform.localRotation = Quaternion.identity;
                    music.name = "Music";
                    source = music.gameObject.AddComponent<AudioSource>();
                }
                else
                {
                    source = music.GetComponent<AudioSource>();
                }
                if (clip == null)
                {
                    source.clip = null;
                    source.Stop();
                    return source;
                }
                source.loop = loop;
                source.volume = volume;
                source.pitch = pitch;
                source.clip = clip;
                source.Play();
                return source;
            }
            return null;
        }
    }
}

