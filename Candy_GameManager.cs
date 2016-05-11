using UnityEngine;
using UnityStandardAssets.ImageEffects;


public class Candy_GameManager : MonoBehaviour
{
    //Public
    public float BreakBetweenLevels;
    public int DistanceBetweenLevels = 20;
    public GameObject ThePlayer;
    public GameObject FlyingObjectReference;
    public ParticleSystemRenderer HappinessParticles;
    public ScreenOverlay ScreenOverlayEffect;

    [HideInInspector]
    public bool WinCondition;
    [HideInInspector]
    public int CurrentLevel;

    //Textures and Material for ParticleSystem
    public Texture HappyTexture;
    public Texture SatisfiedTexture;
    public Texture UnsatisfiedTexture;
    public Texture UnHappyTexture;
    public Material HappinessMaterial;

    //Audio
    public AudioClip HappySFX;
    public AudioClip UnhappySFX;
    public AudioClip[] SuccessAudio;

    //PRIVATE
    private float Timer;
    private bool VictoryPlayed;
    private bool SecondPass;    
    private float HappinessLevel;
    private bool LiftingInitiated = false;
    private FlyAway CameraElevatorAbilities;
    private MinecraftLevelGenerator ThisMinecraftLevelGenerator;
    private AudioSource[] GameManagerAudio;
    private AudioSource AudioPlayer;

    //Constants
    private const float cMaxHappinessLevel = 100;
    private const float cFirstQuarterHappinessLevel = cMaxHappinessLevel / 4;
    private const float cThirdQuarterHappinessLevel = cMaxHappinessLevel * 3 / 4;
    private const float cHalfHappinessLevel = cMaxHappinessLevel / 2;
    private const float cMinHappinessLevel = 0;

    void Start()
    {
        HappinessLevel = 0;
        CurrentLevel = 1;
        VictoryPlayed = false;
        WinCondition = false;
        Cursor.visible = false;

        //Get components
        AudioPlayer = GetComponent<AudioSource>();
        CameraElevatorAbilities = FlyingObjectReference.GetComponent<FlyAway>();
        ThisMinecraftLevelGenerator = GetComponent<MinecraftLevelGenerator>();
        GameManagerAudio = GetComponents<AudioSource>();
        GameManagerAudio[1].clip = SuccessAudio[Random.Range(0, SuccessAudio.Length)];
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (WinCondition == true)
        {
            if (GameManagerAudio[1].isPlaying == false && VictoryPlayed == false)
            {
                GameManagerAudio[1].Play();
                VictoryPlayed = true;
            }

            //When the player finishes the level
            if (Timer < BreakBetweenLevels)
            {
                Timer += Time.deltaTime;

            }
            else
            {
                if (LiftingInitiated == false)
                {
                    ThisMinecraftLevelGenerator.CurrentLevel = CurrentLevel;
                    ThisMinecraftLevelGenerator.ChangeDifficulty();
                    ThisMinecraftLevelGenerator.NewStage(DistanceBetweenLevels);
                    FlyingObjectReference.SetActive(true);

                    LiftingInitiated = true;
                    FlyingObjectReference.transform.position = ThePlayer.transform.position;

                }

                if (FlyingObjectReference.activeInHierarchy == true && LiftingInitiated == true && SecondPass == true)
                {
                    CameraElevatorAbilities.ActivateFlotation();
                }
                SecondPass = true;

                ThePlayer.transform.position = FlyingObjectReference.transform.position;
            }
        }

        if (ThePlayer.transform.position.y >= (DistanceBetweenLevels * CurrentLevel) + 20 && WinCondition == true)
        {
            ResetForNewLevel();
            ThisMinecraftLevelGenerator.DeleteOldLevel();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            WinCondition = true;
        }

    }

    /// <summary>
    /// Resets player preferences to prepare for next level.
    /// </summary>
    private void ResetForNewLevel()
    {
        Timer = 0;
        CurrentLevel += 1;
        HappinessLevel = 0;
        SecondPass = false;
        WinCondition = false;
        VictoryPlayed = false;
        LiftingInitiated = false;
        CheckHappinessLevel();
        CameraElevatorAbilities.DeActivateFlotation();
        GameManagerAudio[1].clip = SuccessAudio[Random.Range(0, SuccessAudio.Length)];
    }

    /// <summary>
    /// Changes Player's happiness level based on what they ate
    /// </summary>
    /// <param name="happinessValue">Happiness value to add/remove.</param>
    public void ChangeHappinessLevel(float happinessValue)
    {
        HappinessLevel += happinessValue;

        if (HappinessLevel < cMinHappinessLevel)
        {
            HappinessLevel = cMinHappinessLevel;
        }
        else if (HappinessLevel > cMaxHappinessLevel)
        {
            HappinessLevel = cMaxHappinessLevel;
        }

        CheckHappinessLevel();
    }

    /// <summary>
    /// Checks for Player's happiness level every frame.
    /// </summary>
    public void CheckHappinessLevel()
    {
        if (HappinessLevel < cFirstQuarterHappinessLevel) //quarter happiness level
        {
            HappinessMaterial.mainTexture = UnHappyTexture;
            ScreenOverlayEffect.intensity = -(cFirstQuarterHappinessLevel) / 100;

            if (AudioPlayer.clip != UnhappySFX)
            {
                AudioPlayer.clip = UnhappySFX;
                AudioPlayer.Play();
            }
        }
        else if (HappinessLevel < cHalfHappinessLevel)
        {
            HappinessMaterial.mainTexture = UnsatisfiedTexture;
            ScreenOverlayEffect.intensity = cHalfHappinessLevel / 100;
        }
        else if (HappinessLevel < cThirdQuarterHappinessLevel)
        {
            HappinessMaterial.mainTexture = SatisfiedTexture;
            ScreenOverlayEffect.intensity = cThirdQuarterHappinessLevel / 100;
        }
        else
        {
            HappinessMaterial.mainTexture = HappyTexture;
            ScreenOverlayEffect.intensity = cMaxHappinessLevel / 100;

            if (AudioPlayer.clip != HappySFX)
            {
                AudioPlayer.clip = HappySFX;
                AudioPlayer.Play();
            }

            WinCondition = true;
        }

        HappinessParticles.material = HappinessMaterial;

    }

    public float GetHappinessLevel()
    {
        return HappinessLevel;
    }
}
