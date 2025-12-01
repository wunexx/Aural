using UnityEngine;

public class Teleporter : InteractableBase
{
    [SerializeField] int scoreCost = 50;
    [SerializeField] EnvironmentType environmentType;

    [SerializeField] AudioClip teleportSound;
    [SerializeField] float volume = 0.2f;

    DungeonGenerator dungeonGenerator;
    protected override void Start()
    {
        base.Start();

        dungeonGenerator = FindFirstObjectByType<DungeonGenerator>();
    }

    public override void OnInteract(GameObject origin)
    {
        if (!canInteract) return;

        base.OnInteract(origin);

        //Debug.Log("Interacted with teleporter");

        SoundManager.Instance.PlayOtherSFX(teleportSound, volume);

        PlayerScore.Instance.AddScore(scoreCost);

        StartCoroutine(dungeonGenerator.GenerateDungeon(environmentType));
    }
}
