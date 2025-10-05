using UnityEngine;

[RequireComponent(typeof(Light))]
public class SmoothSpotlight : MonoBehaviour
{
    // Docelowe warto�ci dla intensywno�ci, k�t�w i zasi�gu �wiat�a
    public float targetIntensity;
    public float targetInnerSpotAngle;
    public float targetOuterSpotAngle;
    public float targetRange;

    // Czas, w kt�rym chcemy osi�gn�� docelowe warto�ci
    public float duration = 1.0f;

    // Prywatne zmienne do przechowywania aktualnego stanu
    private Light spotlight;
    private float initialIntensity;
    private float initialInnerSpotAngle;
    private float initialOuterSpotAngle;
    private float initialRange;
    private float elapsedTime = 0.0f;

    void Start()
    {
        // Pobranie komponentu Light
        spotlight = GetComponent<Light>();

        // Ustawienie �wiat�a na typ Spot, je�li nie jest
        spotlight.type = LightType.Spot;

        // Zainicjalizowanie pocz�tkowych warto�ci
        initialIntensity = spotlight.intensity;
        initialInnerSpotAngle = spotlight.innerSpotAngle;
        initialOuterSpotAngle = spotlight.spotAngle;
        initialRange = spotlight.range;
    }

    // Metoda, kt�ra inicjuje przej�cie do nowych warto�ci
    public void SetTargetValues(float newIntensity, float newInnerAngle, float newOuterAngle, float newRange, float newDuration)
    {
        targetIntensity = newIntensity;
        targetInnerSpotAngle = newInnerAngle;
        targetOuterSpotAngle = newOuterAngle;
        targetRange = newRange;
        duration = newDuration;
        elapsedTime = 0.0f;

        // Zapisanie obecnych warto�ci jako pocz�tkowych
        initialIntensity = spotlight.intensity;
        initialInnerSpotAngle = spotlight.innerSpotAngle;
        initialOuterSpotAngle = spotlight.spotAngle;
        initialRange = spotlight.range;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Zwi�kszanie up�ywu czasu
            elapsedTime += Time.deltaTime;

            // Obliczanie interpolacji dla intensywno�ci
            spotlight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / duration);

            // Interpolacja dla k�t�w spot
            spotlight.innerSpotAngle = Mathf.Lerp(initialInnerSpotAngle, targetInnerSpotAngle, elapsedTime / duration);
            spotlight.spotAngle = Mathf.Lerp(initialOuterSpotAngle, targetOuterSpotAngle, elapsedTime / duration);

            // Interpolacja dla zasi�gu �wiat�a
            spotlight.range = Mathf.Lerp(initialRange, targetRange, elapsedTime / duration);

            // Opcjonalnie: zako�cz interpolacj�, je�li osi�gn�li�my docelowy czas
            if (elapsedTime >= duration)
            {
                spotlight.intensity = targetIntensity;
                spotlight.innerSpotAngle = targetInnerSpotAngle;
                spotlight.spotAngle = targetOuterSpotAngle;
                spotlight.range = targetRange;
            }
        }
    }
}
