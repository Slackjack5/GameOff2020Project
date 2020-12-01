using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    public AudioMixerGroup mixerGroupEffects;
    public AudioMixerGroup mixerGroupMusic;
    public AudioMixer musicMixer;

    private bool tutorialMusicStopped=false;
    private bool endReached = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
            if (s.name=="Music-Tutorial")
            {
            s.source.outputAudioMixerGroup = mixerGroupMusic;
            }
            else if (s.name == "Music-RunFastShootAliens")
            {
            s.source.outputAudioMixerGroup = mixerGroupMusic;
            }
            else
            {
                s.source.outputAudioMixerGroup = mixerGroupEffects; 
            }
        }

       
        
    }

    private void Start()
    {

        //Play Sound
        instance.Play("Music-Tutorial");
        instance.Play("Train-Ambience");

    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        if (sceneName == "Level 1-Baby's First Steps" && !tutorialMusicStopped)
        {
            Stop("Music-Tutorial");
            Play("Music-RunFastShootAliens");
            tutorialMusicStopped = true;
        }
        else if (sceneName == "Level 2-Tread Carefully" && !tutorialMusicStopped)
        {
            Stop("Music-Tutorial");
            Play("Music-RunFastShootAliens");
            tutorialMusicStopped = true;
        }
        else if (sceneName == "Level 3-Not All Robots Kill" && !tutorialMusicStopped)
        {
            Stop("Music-Tutorial");
            Play("Music-RunFastShootAliens");
            tutorialMusicStopped = true;
        }
        else if (sceneName == "Level 4-Combined Issues" && !tutorialMusicStopped)
        {
            Stop("Music-Tutorial");
            Play("Music-RunFastShootAliens");
            tutorialMusicStopped = true;
        }
        else if (sceneName == "Level 5-Charged" && !tutorialMusicStopped)
        {
            Stop("Music-Tutorial");
            Play("Music-RunFastShootAliens");
            tutorialMusicStopped = true;
        }
        else if (sceneName == "End Screen" && endReached==false)
        {
            Stop("Music-RunFastShootAliens");
            Play("Music-Tutorial");
            endReached = true;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    // Update is called once per frame
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    // Update is called once per frame
    public void PlaySound(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = pitch;
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.volume > 0)
        {
            s.source.volume -= .003f;
        }
        else
        {
            s.source.Pause();
        }
    }

    public void lowPassEnable()
    {
        musicMixer.SetFloat("lowPassLevel", 300);
        musicMixer.SetFloat("lowPassLevel2", 10000);
    }
    public void lowPassDisable()
    {
        musicMixer.SetFloat("lowPassLevel", 22000);
        musicMixer.SetFloat("lowPassLevel2", 22000);
    }

    public void SetVolume(float volume)
    {
        musicMixer.SetFloat("volume", volume);
    }
}
