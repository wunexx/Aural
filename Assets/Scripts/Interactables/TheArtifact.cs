using UnityEngine;

public class TheArtifact : InteractableBase
{
    [SerializeField] int scoreCost = 150;

    [Header("Win SFX")]
    [SerializeField] AudioClip sfx;
    [SerializeField] float volume = 0.2f;

    //[Header("Win Music")]
    //[SerializeField] AudioClip music;
    //[SerializeField] float musicVolume = 0.2f;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnInteract(GameObject origin)
    {
        if (!canInteract) return;

        base.OnInteract(origin);

        PlayerScore score = origin.GetComponent<PlayerScore>();

        score.OnWin();

        SoundManager.Instance.PlayOtherSFX(sfx, volume);

        PlayerScore.Instance.AddScore(scoreCost);

        //SoundManager.Instance.PlayMusic(music, musicVolume);
    }
}
