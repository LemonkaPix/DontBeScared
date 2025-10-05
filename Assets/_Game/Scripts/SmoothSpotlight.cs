using UnityEngine;

[RequireComponent(typeof(Light))]
public class SmoothSpotlight : MonoBehaviour
{
    // Docelowe wartoœci dla intensywnoœci, k¹tów i zasiêgu œwiat³a
    public float targetIntensity;
    public float targetInnerSpotAngle;
    public float targetOuterSpotAngle;
    public float targetRange;

    // Czas, w którym chcemy osi¹gn¹æ docelowe wartoœci
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

        // Ustawienie œwiat³a na typ Spot, jeœli nie jest
        spotlight.type = LightType.Spot;

        // Zainicjalizowanie pocz¹tkowych wartoœci
        initialIntensity = spotlight.intensity;
        initialInnerSpotAngle = spotlight.innerSpotAngle;
        initialOuterSpotAngle = spotlight.spotAngle;
        initialRange = spotlight.range;
    }

    // Metoda, która inicjuje przejœcie do nowych wartoœci
    public void SetTargetValues(float newIntensity, float newInnerAngle, float newOuterAngle, float newRange, float newDuration)
    {
        targetIntensity = newIntensity;
        targetInnerSpotAngle = newInnerAngle;
        targetOuterSpotAngle = newOuterAngle;
        targetRange = newRange;
        duration = newDuration;
        elapsedTime = 0.0f;

        // Zapisanie obecnych wartoœci jako pocz¹tkowych
        initialIntensity = spotlight.intensity;
        initialInnerSpotAngle = spotlight.innerSpotAngle;
        initialOuterSpotAngle = spotlight.spotAngle;
        initialRange = spotlight.range;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Zwiêkszanie up³ywu czasu
            elapsedTime += Time.deltaTime;

            // Obliczanie interpolacji dla intensywnoœci
            spotlight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / duration);

            // Interpolacja dla k¹tów spot
            spotlight.innerSpotAngle = Mathf.Lerp(initialInnerSpotAngle, targetInnerSpotAngle, elapsedTime / duration);
            spotlight.spotAngle = Mathf.Lerp(initialOuterSpotAngle, targetOuterSpotAngle, elapsedTime / duration);

            // Interpolacja dla zasiêgu œwiat³a
            spotlight.range = Mathf.Lerp(initialRange, targetRange, elapsedTime / duration);

            // Opcjonalnie: zakoñcz interpolacjê, jeœli osi¹gnêliœmy docelowy czas
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
