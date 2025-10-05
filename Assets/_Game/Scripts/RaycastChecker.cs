using UnityEngine;

public class RaycastChecker : MonoBehaviour
{
    /// <summary>
    /// Sprawdza, czy mi�dzy dwiema pozycjami (start i end) nie ma �adnego kolidera.
    /// </summary>
    /// <param name="startPosition">Pozycja pocz�tkowa promienia.</param>
    /// <param name="endPosition">Pozycja ko�cowa promienia.</param>
    /// <param name="layerMask">Maska warstwy do uwzgl�dnienia w rayca�cie (opcjonalnie).</param>
    /// <returns>True, je�li nie ma kolider�w pomi�dzy pozycjami, False w przeciwnym przypadku.</returns>
    public static bool IsPathClear(GameObject startObject, GameObject endObject)
    {
        // Pobierz pozycje obu obiekt�w
        Vector3 startPosition = startObject.transform.position;
        Vector3 endPosition = endObject.transform.position;

        // Oblicz kierunek i odleg�o�� mi�dzy obiektami
        Vector3 direction = endPosition - startPosition;
        float distance = direction.magnitude;

        // Przeprowad� raycast
        if (Physics.Raycast(startPosition, direction.normalized, out RaycastHit hitInfo, distance))
        {
            // Je�li trafiony kolider nie nale�y do startObject ani endObject, to jest inny obiekt na drodze
            if (hitInfo.collider.gameObject != startObject && hitInfo.collider.gameObject != endObject)
            {
                print($"{hitInfo.collider.gameObject} na drodze");
                return false; // Kolider innego obiektu pomi�dzy startObject i endObject
            }
        }

        return true; // Brak innych kolider�w na trasie
    }
}

