using UnityEngine;

public class TheArtifact : InteractableBase
{
    [SerializeField] AudioClip sfx;
    [SerializeField] float volume = 0.2f;

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
    }
}
