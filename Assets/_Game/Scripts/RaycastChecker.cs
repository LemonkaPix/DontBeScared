using UnityEngine;

public class RaycastChecker : MonoBehaviour
{
    /// <summary>
    /// Sprawdza, czy miêdzy dwiema pozycjami (start i end) nie ma ¿adnego kolidera.
    /// </summary>
    /// <param name="startPosition">Pozycja pocz¹tkowa promienia.</param>
    /// <param name="endPosition">Pozycja koñcowa promienia.</param>
    /// <param name="layerMask">Maska warstwy do uwzglêdnienia w raycaœcie (opcjonalnie).</param>
    /// <returns>True, jeœli nie ma koliderów pomiêdzy pozycjami, False w przeciwnym przypadku.</returns>
    public static bool IsPathClear(GameObject startObject, GameObject endObject)
    {
        // Pobierz pozycje obu obiektów
        Vector3 startPosition = startObject.transform.position;
        Vector3 endPosition = endObject.transform.position;

        // Oblicz kierunek i odleg³oœæ miêdzy obiektami
        Vector3 direction = endPosition - startPosition;
        float distance = direction.magnitude;

        // PrzeprowadŸ raycast
        if (Physics.Raycast(startPosition, direction.normalized, out RaycastHit hitInfo, distance))
        {
            // Jeœli trafiony kolider nie nale¿y do startObject ani endObject, to jest inny obiekt na drodze
            if (hitInfo.collider.gameObject != startObject && hitInfo.collider.gameObject != endObject)
            {
                print($"{hitInfo.collider.gameObject} na drodze");
                return false; // Kolider innego obiektu pomiêdzy startObject i endObject
            }
        }

        return true; // Brak innych koliderów na trasie
    }
}

