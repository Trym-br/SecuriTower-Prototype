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

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.TryGetComponent<MakeMoveable>(out var moveable)) {
			var delta = new Vector2(collision.transform.position.x,
			                        collision.transform.position.y)
			            - playerRB.position;

			if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) && delta.x != 0.0f) {
				Vector2 moveBy = delta;
				moveBy.x /= Mathf.Abs(moveBy.x);
				moveBy.y = 0.0f;
				moveable.TryMoveBy(moveBy);
			} else if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y) && delta.y != 0.0f) {
				Vector2 moveBy = delta;
				moveBy.x = 0.0f;
				moveBy.y /= Mathf.Abs(moveBy.y);
				moveable.TryMoveBy(moveBy);
			} else {
				// Do nothing!
			}
		}
	}
}
