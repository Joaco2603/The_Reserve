using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class AnimationLogo : MonoBehaviour
{
    public Image imageToFade; // Referencia a tu imagen de UI
    public Button Exit;
    public float fadeInDuration = 2.0f; // Duración del efecto de desvanecimiento en segundos
    public float buttonToAppear = 8f;
    public float delayBeforeFade = 3.0f; // Tiempo de espera antes de comenzar el efecto

    // Iniciar el efecto al cargar la escena
    private void Start()
    {
        Exit.gameObject.SetActive(false);
        // Comenzar el efecto de desvanecimiento con retraso
        StartCoroutine(FadeImage(imageToFade, fadeInDuration, delayBeforeFade));
    }

    IEnumerator FadeButton(Button button, float duration)
    {
        button.gameObject.SetActive(true); // Activar el botón
        Color originalColor = button.image.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        button.image.color = transparentColor;

        float endTime = Time.time + duration;

        while (Time.time <= endTime)
        {
            float newAlpha = Mathf.InverseLerp(endTime - duration, endTime, Time.time);
            button.image.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        button.image.color = originalColor; // Asegurarse de que el alfa sea 1 al final
    }


    IEnumerator FadeImage(Image image, float duration, float delay)
    {
        // Asegurarse de que la imagen está habilitada pero completamente transparente
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.gameObject.SetActive(true);

        // Esperar por el tiempo de retraso especificado
        yield return new WaitForSeconds(delay);

        StartCoroutine(FadeButton(Exit, duration));

        // Calcular el tiempo de finalización
        float endTime = Time.time + duration;

        // Mientras haya tiempo restante...
        while (Time.time <= endTime)
        {
            // ... interpolar el valor alfa de la imagen de 0 a 1
            float newAlpha = Mathf.InverseLerp(endTime - duration, endTime, Time.time);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);

            // Esperar al siguiente frame
            yield return null;
        }

        

        // Asegurarse de que el alfa sea 1 al final
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }
}
