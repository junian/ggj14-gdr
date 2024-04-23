using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public delegate void PlayerMeetEventHandler(object sender, System.EventArgs e);
	public event PlayerMeetEventHandler PlayerMeet;

	public Joystick leftTouchPad;
	public Joystick rightTouchPad;

	public float moveSpeed = 10.0f;
	public float jumpForce = 10.0f;
	public int maxJump = 2;
	public int hp = 1;

	private bool isGrounded;
	private int jumpCount;

	private Transform groundChecker;
	private Animator animator;

	public bool rightDirection = true;
	public bool mirror = false;

	private const string groundCheckerName = "groundChecker";
	private float mirrorValue;
	private float horizontalSpeed;
	public PlayerState playerState;

	public bool IsGrounded
	{
		get{ return isGrounded;}
	}

	void Awake()
	{
		mirrorValue =  mirror?-1.0f:1.0f;
		groundChecker = this.transform.Find(groundCheckerName);
		animator = this.GetComponent<Animator>();
		isGrounded = false;
		jumpCount = maxJump;
		playerState = PlayerState.None;
		horizontalSpeed = 0.0f;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.CompareTag(Tags.Coin))
		{
			Game.Instance.score++;
			Destroy(collider.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag(Tags.Monster))
		{
			if(playerState == PlayerState.Win)
				return;

			if(playerState == PlayerState.Die)
				return;

			playerState = PlayerState.Die;
			hp--;
			animator.SetInteger("HP", hp);
			this.GetComponent<Rigidbody2D>().fixedAngle = false;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
			var colliders = this.GetComponents<Collider2D>();
			foreach(var c in colliders)
				Destroy(c);
			Destroy(this, 3.0f);
		}
		else if(collision.gameObject.CompareTag(Tags.Player))
		{
			Debug.Log("collide with player?");
			if(playerState == PlayerState.Win)
				return;
			
			if(playerState == PlayerState.Die)
				return;

			/*
			if(!isGrounded)
				return;

			if(!collision.gameObject.GetComponent<PlayerController>().IsGrounded)
				return;
			 */
			
			playerState = PlayerState.Win;
			Game.Instance.score++;

			this.gameObject.layer = LayerMask.NameToLayer(Layers.Player);

			var colliders = this.GetComponents<Collider2D>();
			foreach(var c in colliders)
				c.enabled = false;

			this.GetComponent<BoxCollider2D>().enabled = true;
			//this.rigidbody2D.Sleep();

			animator.SetTrigger("Love");
			OnPlayerMeet(null);

		}
	}

	private void OnPlayerMeet(System.EventArgs evt)
	{
		if(PlayerMeet!=null)
		{
			PlayerMeet(this, evt);
		}
	}

	void OnDestroy()
	{
		if(playerState == PlayerState.Die)
			SceneManager.LoadScene(Scenes.MainMenu);
	}
	
	// Update is called once per frame
	void Update () {

		if(playerState == PlayerState.Win)
			return;

		if(playerState == PlayerState.Die)
			return;

		isGrounded = Physics2D.Linecast(
			transform.position, 
			groundChecker.position, 
			1 << LayerMask.NameToLayer(Layers.Platform));

		animator.SetBool("Grounded", isGrounded);

		Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

		float speedFactor = isGrounded ? 1.0f : 0.5f;

		float axisVal = Mathf.Abs(leftTouchPad.position.x);
		float hardVal = Mathf.Abs(Input.GetAxis("Horizontal"));
		float maxHorizontalAxis = axisVal > hardVal ? leftTouchPad.position.x : Input.GetAxis("Horizontal");

		horizontalSpeed = maxHorizontalAxis * moveSpeed * mirrorValue * Time.deltaTime * speedFactor;
        animator.SetFloat("Speed", Mathf.Abs(horizontalSpeed));
		//DebugConsole.Log("isGrounded: " + isGrounded); 

		if((horizontalSpeed > 0.0f && !rightDirection) || 
		   (horizontalSpeed<0.0f && rightDirection))
		{
			FlipDirection();
		}

		if(rightTouchPad.lastFingerId >= 0)
			Debug.Log(this.name + "; " + rightTouchPad.lastFingerId + "; " + Input.GetTouch(rightTouchPad.lastFingerId).phase);

		if(Input.GetButtonDown("Jump") || (rightTouchPad.lastFingerId >= 0 && Input.GetTouch(rightTouchPad.lastFingerId).phase == TouchPhase.Began))
		{
			bool isJumpAllowed = false;
			if(isGrounded)
			{
				jumpCount = 1;
				isJumpAllowed = true;
			}
			else if(jumpCount < maxJump)
			{
				jumpCount++;
				isJumpAllowed = true;
			}

            // TODO: FIX THIS, uncommend `if(isJumpAllowed)`
            //if(isJumpAllowed)
            {
                velocity.y = jumpForce;
				GetComponent<Rigidbody2D>().velocity = velocity;
			}
		}
		KeepPlayerInsideCamera();
	}

	void FixedUpdate()
	{
		if(playerState == PlayerState.Win)
			return;
		
		if(playerState == PlayerState.Die)
			return;

		this.transform.Translate(new Vector3(horizontalSpeed , 0));
	}
	
	void KeepPlayerInsideCamera()
	{
		var dist = (transform.position - Camera.main.transform.position).z;
		
		var leftBorder   = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
		var rightBorder  = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
		var topBorder    = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
		var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;
		
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
			transform.position.z);
	}

	private void FlipDirection()
	{
		rightDirection = !rightDirection;
		Vector3 scale = this.transform.localScale;
		scale.x *= -1.0f;
		this.transform.localScale = scale;
	}
}
