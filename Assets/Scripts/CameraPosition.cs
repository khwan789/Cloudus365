using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public GameManager gameManager;
    public ObjectPlayer player; // Reference to the player
    private Vector3 playerPosition;
    public Transform planet; // Reference to the planet
    public float baseFollowDistance = 5f; // Default follow distance
    public float sideFollowDistance = 3f; // Reduced follow distance on the sides
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement

    private Vector3 offset; // Offset for the camera position

    void LateUpdate()
    {
        if (player == null || planet == null) return;

        playerPosition = player.basePosition;

        // Calculate the direction from the planet to the player
        Vector3 playerToPlanetDirection = (playerPosition - planet.position).normalized;

        // Determine the vertical position relative to the planet's center
        float verticalFactor = Mathf.Abs(playerToPlanetDirection.y); // Range: 0 (equator) to 1 (top/bottom)

        // Dynamically adjust follow distance
        float followDistance = Mathf.Lerp(sideFollowDistance, baseFollowDistance, verticalFactor);

        // Calculate the perpendicular direction to player's orbit for camera offset
        Vector3 tangentialDirection = Vector3.Cross(playerToPlanetDirection, Vector3.forward).normalized;

        // Determine the camera's target position by placing it ahead of the player in the tangential direction
        Vector3 targetPosition = playerPosition + tangentialDirection * followDistance * player.direction;

        // Keep Z consistent for the 2D camera
        targetPosition.z = transform.position.z;

        if (gameManager.isPlaying)
        {
            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}
