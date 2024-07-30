using System.Collections;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public ParticleSystem particleSys;
    public float colorTransitionDuration = 1.0f;

    private Color _originalLiquidColour;
    private Color _firstColour;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        var liquidSpriteRenderer = GetChildSpriteRenderer("Liquid");
        if (liquidSpriteRenderer != null)
        {
            _originalLiquidColour = liquidSpriteRenderer.color;
            _firstColour = liquidSpriteRenderer.color;
        }
    }

    SpriteRenderer GetChildSpriteRenderer(string name) => transform.Find(name)?.GetComponent<SpriteRenderer>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        var ingredientSlot = collision.gameObject.GetComponent<IngredientSlot>();
        var collidedColor = (ingredientSlot != null && ingredientSlot.ingredient != null && ingredientSlot.ingredient.itemtype != itemType.Bottle)
            ? ingredientSlot.ingredient.itemColour
            : _originalLiquidColour;

        var mixedColor = MixColors(collidedColor, _originalLiquidColour);

        var instantiatedParticleSys = Instantiate(particleSys, collision.transform.position, Quaternion.identity);
        instantiatedParticleSys.Play();
        Destroy(instantiatedParticleSys.gameObject, instantiatedParticleSys.main.duration);

        StartCoroutine(TransitionColor("Liquid", _originalLiquidColour, mixedColor, colorTransitionDuration));
        _originalLiquidColour = mixedColor;

        PlayRandomClip();
        Destroy(collision.gameObject);
    }

    public void ToPotionColour(Color potionColor)
    {
        StartCoroutine(TransitionColor("Liquid", _originalLiquidColour, potionColor, colorTransitionDuration));
        _originalLiquidColour = potionColor;
    }

    public void RestartCauldron()
    {
        StartCoroutine(TransitionColor("Liquid", _originalLiquidColour, _firstColour, colorTransitionDuration));
        _originalLiquidColour = Color.blue;
    }

    public Color MixColors(Color color1, Color color2) => (color1 + color2) / 2f;

    public void PlayRandomClip()
    {
        if (audioClips.Length > 0)
        {
            _audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            _audioSource.Play();
        }
    }

    IEnumerator TransitionColor(string childName, Color startColor, Color endColor, float duration)
    {
        var child = transform.Find(childName);
        var spriteRenderer = child?.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                spriteRenderer.color = Color.Lerp(startColor, endColor, t / duration);
                yield return null;
            }
            spriteRenderer.color = endColor;
        }
    }
}
