using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    [Header("Inscribed")]

    static float speed = .1f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        position.y -= speed;
        transform.position = position;
        if (position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        // Hitting the ship costs a heart; only the bullet is destroyed, not the ship
        ShipHealth ship = coll.gameObject.GetComponent<ShipHealth>();
        if (ship != null)
        {
            ship.TakeDamage();
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
        Destroy(coll.gameObject);
    }
}