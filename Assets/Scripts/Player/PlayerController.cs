using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(InputActions))]
public class PlayerController : MonoBehaviour {
	public float speed = 3.0f;

	[Range(0.0f, 1.0f)]
	public float friction = 0.35f;

	Rigidbody2D  playerRB;
	InputActions input;

	void Awake() {
		playerRB = GetComponent<Rigidbody2D>();
		playerRB.gravityScale           = 0.0f;
		playerRB.constraints            = RigidbodyConstraints2D.FreezeRotation;
		playerRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		playerRB.bodyType               = RigidbodyType2D.Dynamic;
		playerRB.interpolation          = RigidbodyInterpolation2D.Interpolate;

		input = GetComponent<InputActions>();
	}

	void Update() {
		currentMovementInput = input.movement;
	}

	Vector2 currentMovementInput;
	void FixedUpdate() {
		playerRB.linearVelocity += currentMovementInput * speed;

		playerRB.linearVelocity *= (1.0f - Mathf.Clamp01(friction));
	}
}
