using UnityEngine;
using UnityEngine.InputSystem;

public class SoundMaster : MonoBehaviour
{
    [SerializeField] private AudioClip[] menu;
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] fishing;
    [SerializeField] private AudioClip[] axe;
    [SerializeField] private AudioClip[] pickaxe;
    [SerializeField] private AudioClip[] cultivating;
    [SerializeField] private AudioClip[] swordHits;
    [SerializeField] private AudioClip[] swordMiss;
    [SerializeField] private AudioClip[] arrowSounds;
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

    private float presetVolume = 0.3f;
    //private float presetSFXVolume = 0.1f;
    private float presetSFXStepVolume = 0.3f;

    private float totalFadeOutTime = 3.5f;
    private float fadeOutMargin = 0.01f;
    private float currentFadeOutTime;


    private void Awake()
    {   

        GameObject musicSourceHolder = new GameObject("Music");
        GameObject sfxSourceHolder = new GameObject("SFX");
        musicSourceHolder.transform.SetParent(this.transform);
		//musicSourceHolder.name = "Music";
        sfxSourceHolder.transform.SetParent(this.transform);
        //sfxSourceHolder.name = "SFX";

        musicSourceIntense = gameObject.AddComponent<AudioSource>();
        musicSource = musicSourceHolder.AddComponent<AudioSource>();
        sfxSource = sfxSourceHolder.AddComponent<AudioSource>();
    }
    private void OnDestroy()
    {
        Inputs.Instance.Controls.Land.MusicToggle.performed -= MuteToggle;// = _.ReadValue<float>();
    }
    private void Start()
    {
        musicSourceIntense.loop = true;
        musicSourceIntense.volume = 0.5f;
        musicSource.loop = true;
        musicSource.volume = presetVolume;
        sfxSource.volume = presetSFXStepVolume;
        PlayMusic();

        Inputs.Instance.Controls.Land.MusicToggle.performed += MuteToggle;// = _.ReadValue<float>();
	}

    private void MuteToggle(InputAction.CallbackContext context)
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
        sfxSource.volume = presetSFXStepVolume;
	}
	private void PlayMusic()
	{
        if (doPlayMusic)
        {
            if (music.Length == 0 || musicSource == null) return;

            //float percentagePlayed = musicSource.timeSamples / musicSource.clip.samples * 100f;
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

    public enum SFX {Footstep,GetHit,SwingSword,SwingSwordMiss,SwordHit,ShootArrow,Grunt,Yeah,PlayerDeath,MenuStep,MenuSelect,MenuError, Gather,FishingSwing,AxeSwing,PickaxeSwing,Cultivating }


    public void StopStepSFX()
    {
		sfxSource.Stop();
	}

    public void PlayStepSFX()
    {
		sfxSource.PlayOneShot(footstep[Random.Range(0, footstep.Length)]);
	}

    public void StopSFX()
    {
        sfxSource.Stop();
    }
    public void PlaySFX(SFX type, bool playMulti=true)
	{

        // If not able to play multiple sounds exit if already playing
        if (!playMulti) if (sfxSource.isPlaying) return;

        if (!doPlaySFX) return;


        switch (type)
		{
            case SFX.Footstep: 
                sfxSource.PlayOneShot(PlayRandomFromArray(footstep));
                break;
            case SFX.Gather: 
                sfxSource.PlayOneShot(sfx[2]);
                break;
            case SFX.AxeSwing: 
                sfxSource.PlayOneShot(PlayRandomFromArray(axe));
                break;
            case SFX.PickaxeSwing: 
                sfxSource.PlayOneShot(PlayRandomFromArray(pickaxe));
                break;
            case SFX.FishingSwing: 
                sfxSource.PlayOneShot(PlayRandomFromArray(fishing));
                break;
            case SFX.Cultivating: 
                sfxSource.PlayOneShot(PlayRandomFromArray(cultivating));
                break;
            case SFX.SwingSword: 
                sfxSource.PlayOneShot(sfx[0]);
                break;
            case SFX.SwordHit: 
                sfxSource.PlayOneShot(PlayRandomFromArray(swordHits));
                break;
            case SFX.SwingSwordMiss: 
                sfxSource.PlayOneShot(PlayRandomFromArray(swordMiss));
                break;
            case SFX.ShootArrow: 
                sfxSource.PlayOneShot(PlayRandomFromArray(arrowSounds));
                break;
            case SFX.Grunt: 
                sfxSource.PlayOneShot(grunts[0]);
                break;
            case SFX.Yeah: 
                sfxSource.PlayOneShot(grunts[3]);
                break;
            case SFX.PlayerDeath: 
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

    private AudioClip PlayRandomFromArray(AudioClip[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

}
