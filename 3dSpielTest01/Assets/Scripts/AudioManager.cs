using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    public Sound[] sounds;
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
        }
    }


  public void Play (string name)
    {
        //Sound s = Array.prototype.Find(sounds, Sound => Sound.name == name);
        //s.source.Play();
    }
    
}
