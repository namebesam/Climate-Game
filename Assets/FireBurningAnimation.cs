using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // Don't forget to include this for TextMeshPro
using System.Collections;

public class FireBurningAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite[] burnStages;          // Full tree burn sequence (first = normal tree)
    public float frameDelay = 0.1f;      // Time between frames
    public Image backgroundImage;        // Background Image to change color (assign in Inspector)
    public Color coolColor = new Color(0.7f, 0.9f, 1f);   // Light blue (start background color)
    public Color hotColor = new Color(1f, 0.35f, 0f);     // Dark orange (end background color)

    public ParticleSystem fireParticles; // Particle System for the fire effect
    public TextMeshProUGUI titleText;    // Reference to the TextMeshPro title text
    public float fadeDuration = 1f;      // Adjustable fade duration for background and text outline

    // New public fields to change the outline colors manually
    public Color startOutlineColor = Color.black; // Default outline color for start
    public Color endOutlineColor = Color.red;    // Default outline color for end

    private Image image;
    private Coroutine animationCoroutine;
    private Coroutine fadeCoroutine;
    private Coroutine outlineFadeCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = burnStages[0];   // Always show normal tree by default
        if (fireParticles != null)
        {
            fireParticles.Stop(); // Make sure particles are stopped initially
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine); // Stop any previous fade

        // Start the fire animation and the background fade together
        fadeCoroutine = StartCoroutine(FadeBackgroundColor(coolColor, hotColor)); // Fade from grey to orange
        outlineFadeCoroutine = StartCoroutine(FadeOutlineColor(startOutlineColor, endOutlineColor)); // Fade outline to match background
        animationCoroutine = StartCoroutine(PlayBurnAnimation(forward: true)); // Play the tree burning animation

        // Start the fire particles
        if (fireParticles != null)
        {
            fireParticles.Play(); // Play the fire particle system
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine); // Stop any previous fade

        // Start the reverse fire animation and the background fade back together
        fadeCoroutine = StartCoroutine(FadeBackgroundColor(hotColor, coolColor)); // Fade from orange to grey
        outlineFadeCoroutine = StartCoroutine(FadeOutlineColor(endOutlineColor, startOutlineColor)); // Fade outline back
        animationCoroutine = StartCoroutine(PlayBurnAnimation(forward: false)); // Reverse the tree burning animation

        // Stop the fire particles when hover ends
        if (fireParticles != null)
        {
            fireParticles.Stop(); // Stop the fire particle system
        }
    }

    IEnumerator PlayBurnAnimation(bool forward)
    {
        int start = forward ? 0 : burnStages.Length - 1;
        int end = forward ? burnStages.Length : -1;
        int step = forward ? 1 : -1;

        // Play the tree burning animation
        for (int i = start; i != end; i += step)
        {
            image.sprite = burnStages[i];
            yield return new WaitForSeconds(frameDelay);
        }

        // Ensure the final state is the correct background color at the end
        if (forward)
            backgroundImage.color = hotColor; // Final color after burning
        else
            backgroundImage.color = coolColor; // Final color after burning out

        animationCoroutine = null;
    }

    private IEnumerator FadeBackgroundColor(Color fromColor, Color toColor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            backgroundImage.color = Color.Lerp(fromColor, toColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        backgroundImage.color = toColor; // Ensure we set the final color at the end
    }

    private IEnumerator FadeOutlineColor(Color fromColor, Color toColor)
    {
        float elapsedTime = 0f;

        // Check if TextMeshProUGUI component is available
        if (titleText == null)
        {
            Debug.LogError("Title Text is not assigned.");
            yield break;
        }

        // Fade the outline color of the TextMeshPro text
        while (elapsedTime < fadeDuration)
        {
            // Use Lerp to gradually change the outline color of the text
            titleText.outlineColor = Color.Lerp(fromColor, toColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        titleText.outlineColor = toColor; // Ensure we set the final color at the end
    }
}
