using UnityEngine;

public class Alien : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject bulletPrefab;
    public AlienFleet fleet;
    public GameObject explosionPrefab;   // drag the Explosion prefab here

    static float bulletSpawnRate = .002f;
    public float lastShot = 0;
    public static float shotCooldown = .1f;

    void FixedUpdate()
    {
        Vector3 position = transform.position;

        if (UnityEngine.Random.value < bulletSpawnRate && Time.time - lastShot > shotCooldown)
        {
            GameObject bullet = Instantiate<GameObject>(bulletPrefab);
            bullet.transform.position = position;
            lastShot = Time.time;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        // An alien touching the ship ends the game (and the ship survives this collision)
        if (coll.gameObject.GetComponent<Spaceship>() != null)
        {
            fleet.GameOver("An alien crashed into your ship!");
            return;
        }

        // Explosion where the alien was
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        fleet.aliens.Remove(gameObject);
        Destroy(gameObject);
        Destroy(coll.gameObject);
    }
}