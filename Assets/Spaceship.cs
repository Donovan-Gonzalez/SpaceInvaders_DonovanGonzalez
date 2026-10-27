using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject dartPrefab;
    public Transform flame;          // drag the Flame object here

    public static float shotCooldown = .05f;
    public float lastShot = 0;
    public float speed = 10f;

    float leftBound = -16f;
    float rightBound = 16f;

    void Start()
    {
        // Bounds come from what the camera can actually see (minus a margin for the ship's width)
        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        rightBound = halfWidth - 1f;
        leftBound = -rightBound;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            Vector3 position = transform.position;
            position.x += speed * Time.deltaTime;
            transform.position = position;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            Vector3 position = transform.position;
            position.x -= speed * Time.deltaTime;
            transform.position = position;
        }

        // Keep the ship inside the play area
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, leftBound, rightBound);
        transform.position = clamped;

        // Lean the engine flame opposite to the direction of travel
        float move = 0f;
        if (Keyboard.current.rightArrowKey.isPressed) move += 1f;
        if (Keyboard.current.leftArrowKey.isPressed) move -= 1f;

        if (flame != null)
        {
            Quaternion target = Quaternion.Euler(0f, 0f, -move * 6f) * Quaternion.Euler(90f, 0f, 0f);
            flame.localRotation = Quaternion.Lerp(flame.localRotation, target, 10f * Time.deltaTime);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Time.time - lastShot > shotCooldown)
        {
            Vector3 position = transform.position;
            GameObject bullet = Instantiate<GameObject>(dartPrefab);
            bullet.transform.position = position;
            lastShot = Time.time;
        }
    }
}