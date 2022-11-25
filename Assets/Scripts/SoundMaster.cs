using UnityEngine;

public class SoundMaster : MonoBehaviour
{
    [SerializeField] private AudioClip[] menu;
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] grunts;
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] musicIntense;
    [SerializeField] private AudioClip[] enemysxf;
    [SerializeField] private AudioClip[] footstep;

    private AudioSource musicSource;
    private AudioSource musicSourceIntense;
    private AudioSource sfxSource;
    private bool doPlayMusic = false;
    private bool doPlaySFX=true;
    private bool doingFadeout = false;
    private int currentEncounterMusic = 0;

    private float presetVolume = 0.8f;
    private float presetSFXVolume = 0.4f;

    private float totalFadeOutTime = 3.5f;
    private float fadeOutMargin = 0.01f;
    private float currentFadeOutTime;

    private void Awake()
    {
        musicSourceIntense = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        musicSourceIntense.loop = true;
        musicSourceIntense.volume = 0.5f;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        sfxSource.volume = presetSFXVolume;
        PlayMusic();

        Inputs.Instance.Controls.Land.MusicToggle.performed += _ => MuteToggle();// = _.ReadValue<float>();
	}

    private void MuteToggle()
    {
		doPlayMusic = !doPlayMusic;
        Debug.Log("Music: "+doPlayMusic);
		if (doPlayMusic) PlayMusic();
		else
		{
			musicSource.Stop();
		}
	}
	private void Update()
    {
        if(doingFadeout) DoFadeout();
    }

	private void DoFadeout()
	{
        currentFadeOutTime += Time.deltaTime;

        float newVolume = presetVolume*(1 - currentFadeOutTime / totalFadeOutTime);
        musicSourceIntense.volume = newVolume;    
        if (currentFadeOutTime + fadeOutMargin >= totalFadeOutTime)
        {
            //Fadeout complete
            musicSourceIntense.volume = presetVolume;
            musicSourceIntense.Stop();
            doingFadeout = false;
        }
    }

	public void SetVolume(float vol)
	{
        musicSource.volume = vol;
        musicSourceIntense.volume = vol;
	}
	public void SetSFXVolume(float vol)
	{
        sfxSource.volume = vol;
	}
	private void PlayMusic()
	{
        if (doPlayMusic)
        {
            musicSource.clip = music[0];
            musicSource.Play();
        }
        else musicSource.Stop(); 
	}
    private void PlayMusicIntense()
	{
        if (doPlayMusic)
        {
            musicSourceIntense.clip = musicIntense[currentEncounterMusic];
            musicSourceIntense.Play();
        }
        else musicSourceIntense.Stop(); 
	}

    public enum SFX {Footstep,GetHit,SwingSword,SwingSwordMiss, Grunt,Yeah,PLayerDeath,MenuStep,MenuSelect,MenuError}
        

    public void PlaySFX(SFX type, bool playMulti=true)
	{

        // If not able to play multiple sounds exit if allready playing
        if (!playMulti) if (sfxSource.isPlaying) return;

        if (!doPlaySFX) return;


        switch (type)
		{
            case SFX.Footstep: 
                sfxSource.PlayOneShot(footstep[Random.Range(0,footstep.Length)]);
                break;
            case SFX.SwingSword: 
                sfxSource.PlayOneShot(sfx[0]);
                break;
            case SFX.SwingSwordMiss: 
                sfxSource.PlayOneShot(sfx[1]);
                break;
            case SFX.Grunt: 
                sfxSource.PlayOneShot(grunts[0]);
                break;
            case SFX.Yeah: 
                sfxSource.PlayOneShot(grunts[3]);
                break;
            case SFX.PLayerDeath: 
                sfxSource.PlayOneShot(grunts[4]);
                break;
			case SFX.MenuStep:
                sfxSource.PlayOneShot(menu[0]);
                break;
			case SFX.MenuSelect:
                sfxSource.PlayOneShot(menu[1]);
                break;
			case SFX.MenuError:
                sfxSource.PlayOneShot(menu[2]);
                break;
			default:
				break;
		}
	}


}
