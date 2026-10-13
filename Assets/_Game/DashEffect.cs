using System.Collections;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private int fadeSteps = 3;
    [SerializeField] private int ghostCount = 4;
    [SerializeField] private float fadeAmount = 0.4f;
    [SerializeField] private float stepTime = 0.1f;
    [SerializeField] private float ghostInterval = 0.05f;
    [SerializeField] private float opacity = 0.1f;
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Play()
    {
        StartCoroutine(SpawnGhosts());
    }

    private IEnumerator FadeGhost()
    {
        var ghost = new GameObject("DashGhost");
        var source = spriteRenderer.transform;
        ghost.transform.SetPositionAndRotation(source.position, source.rotation);
        ghost.transform.localScale = source.lossyScale;

        var ghostSprite = ghost.AddComponent<SpriteRenderer>();
        ghostSprite.sprite = spriteRenderer.sprite;
        ghostSprite.flipX = spriteRenderer.flipX;
        ghostSprite.color = spriteRenderer.color;
        ghostSprite.sharedMaterial = ghostMaterial;
        var block = new MaterialPropertyBlock();
        var color = Color.white;
        color.a = opacity;
        block.SetColor(BaseColorId, color);
        ghostSprite.SetPropertyBlock(block);
        ghostSprite.sortingLayerID = spriteRenderer.sortingLayerID;
        ghostSprite.sortingOrder = spriteRenderer.sortingOrder - 1;

        for (int i = 0; i < fadeSteps; i++)
        {
            yield return new WaitForSeconds(stepTime);
            color.a -= fadeAmount;
            block.SetColor(BaseColorId, color);
            ghostSprite.SetPropertyBlock(block);
        }

        Destroy(ghost);
    }

    private IEnumerator SpawnGhosts()
    {
        for (int i = 0; i < ghostCount; i++)
        {
            StartCoroutine(FadeGhost());
            yield return new WaitForSeconds(ghostInterval);
        }
    }
}

