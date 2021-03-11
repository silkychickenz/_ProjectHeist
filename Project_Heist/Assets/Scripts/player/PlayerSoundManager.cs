using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerSoundManager : MonoBehaviour
{
    private Animator animator;
    public GameObject playerAvatar;
    [Header("Audio Sources")]
    [SerializeField]
    public AudioSource FootstepsAudio;
    [SerializeField]
    public AudioSource GravityFlipAudio;
    [SerializeField]
    public AudioSource BGMusicSource;
    //[SerializeField]
    //public AudioSource Footsteps;
    // [SerializeField]
    //public AudioSource Footsteps;

    Dictionary<string, bool> soundFlags;
    private bool previousSprintStatus = true;
    private bool gravityFlipAudioStatus = true;
    private void Start()
    {
        animator = playerAvatar.GetComponent<Animator>();
        soundFlags = new Dictionary<string, bool>();

        soundFlags.Add("gravityFlip",true);
       
    }


    public enum PlayerSounds
    {
        Run,
        Sprint,
        Shoot,
        Jump,
        GravityFlip,
        BG_Music_loop,
        BG_Music_UpBeat,

    }
   
    public PlayerAudioClips[] playerAudioClipsArray;
    bool looping = false;
    #region footsteps 
    public void Sound(bool startSprinting)
    {


        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("movement"))
        {
            if (animator.GetFloat("moveX") != 0 || animator.GetFloat("moveY") != 0)
            {
                if (animator.GetFloat("moveY") >= 1.5 )
                {

                   
                    PlaySoundLoop(FootstepsAudio, PlayerSounds.Sprint);
                }
                
                else
                {
                    
                    PlaySoundLoop(FootstepsAudio, PlayerSounds.Run);
                }

            }
            else
            {
               
                StopSound(FootstepsAudio);
            }
        }
        else
        {
           
            StopSound(FootstepsAudio);
        }
        if (startSprinting != previousSprintStatus)
        {
           
            StopSound(FootstepsAudio);
            previousSprintStatus = startSprinting;
        }
        
        

        #region gravity flip
        

        #endregion



    }

    public void BGMusic()
    {
        
            PlaySoundLoop(BGMusicSource, PlayerSounds.BG_Music_UpBeat);
            //soundFlags["BG_MusicLoop"] = false;
        
    }
    public void GravityFlipSound(Vector2 gravityFlipDirection, bool gravityFlipWheel)
    {
        if (gravityFlipWheel && gravityFlipDirection != Vector2.zero && soundFlags["gravityFlip"])
        {


            PlaySoundOnce(GravityFlipAudio, PlayerSounds.GravityFlip, 0.5f, "gravityFlip");
            //soundFlags["gravityFlip"] = false;
        }
    }
    #endregion
    public void PlaySoundOnce(AudioSource audioSource,PlayerSounds sound, float volume, string key)
    {
        audioSource.PlayOneShot(GetAudioClip(sound), volume);
        StartCoroutine(SoundLoopReset(GetAudioClip(sound), key));
    }
    public void PlaySoundLoop(AudioSource audioSource, PlayerSounds sound)
    {
       // if (!looping)
        //{
            audioSource.clip = GetAudioClip(sound);
            audioSource.loop = true;
            audioSource.Play();
            looping = true;
            
       // }
       
    }
    public void StopSound(AudioSource audioSource)
    {

        audioSource.loop = false;
        audioSource.Stop();
        looping = false;

    }
    public IEnumerator SoundLoopReset(AudioClip audioClip, string key) // fravity flip cooldown timer
    {
        yield return new WaitForSeconds(audioClip.length);
        soundFlags[key] = !soundFlags[key];
    }


    public AudioClip GetAudioClip(PlayerSounds sound)
    {
        foreach (PlayerAudioClips playerAudioClips in playerAudioClipsArray)
        {
            if (playerAudioClips.playerSounds == sound)
            {
                return playerAudioClips.audioClip;
            }
            
        }

        return null;
    }


   

    [System.Serializable]
    public class PlayerAudioClips
    {

        public PlayerSoundManager.PlayerSounds playerSounds;
        public AudioClip audioClip;

    }
}








