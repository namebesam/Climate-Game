using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; 
using System.Collections;

public class FireBurningAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite[] burnStages;         
    public float frameDelay = 0.1f;    
    public Image backgroundImage;      
    public Color coolColor = new Color(0.7f, 0.9f, 1f);   
    public Color hotColor = new Color(1f, 0.35f, 0f);    

    public ParticleSystem fireParticles; 
    public TextMeshProUGUI titleText;   
    public float fadeDuration = 1f;     

    public Color startOutlineColor = Color.black;
    public Color endOutlineColor = Color.red;    

    private Image image;
    private Coroutine animationCoroutine;
    private Coroutine fadeCoroutine;
    private Coroutine outlineFadeCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = burnStages[0];   
        if (fireParticles != null)
        {
            fireParticles.Stop();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine); 

        fadeCoroutine = StartCoroutine(FadeBackgroundColor(coolColor, hotColor)); 
        outlineFadeCoroutine = StartCoroutine(FadeOutlineColor(startOutlineColor, endOutlineColor)); 
        animationCoroutine = StartCoroutine(PlayBurnAnimation(forward: true)); 

        if (fireParticles != null)
        {
            fireParticles.Play(); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine); 

        fadeCoroutine = StartCoroutine(FadeBackgroundColor(hotColor, coolColor)); 
        outlineFadeCoroutine = StartCoroutine(FadeOutlineColor(endOutlineColor, startOutlineColor)); 
        animationCoroutine = StartCoroutine(PlayBurnAnimation(forward: false)); 

        if (fireParticles != null)
        {
            fireParticles.Stop(); 
        }
    }

    IEnumerator PlayBurnAnimation(bool forward)
    {
        int start = forward ? 0 : burnStages.Length - 1;
        int end = forward ? burnStages.Length : -1;
        int step = forward ? 1 : -1;

        for (int i = start; i != end; i += step)
        {
            image.sprite = burnStages[i];
            yield return new WaitForSeconds(frameDelay);
        }

        if (forward)
            backgroundImage.color = hotColor; 
        else
            backgroundImage.color = coolColor;

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

        backgroundImage.color = toColor; 
    }

    private IEnumerator FadeOutlineColor(Color fromColor, Color toColor)
    {
        float elapsedTime = 0f;

        if (titleText == null)
        {
            Debug.LogError("Title Text is not assigned.");
            yield break;
        }

        while (elapsedTime < fadeDuration)
        {
            titleText.outlineColor = Color.Lerp(fromColor, toColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        titleText.outlineColor = toColor; 
    }
}
