using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour {

	private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
	private Transform m_Cam;                  // A reference to the main camera in the scenes transform
	private Vector3 m_CamForward;             // The current forward direction of the camera
	private Vector3 m_Move;
	[HideInInspector]
	public float Hinput;
	[HideInInspector]
	public float Vinput;

	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;
	public TextMeshProUGUI gameInstructions;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private string[] INSTRUCTIONS = { "Orange", "Green", "Purple", "Blue" };
	private int count;
	private float groundDimension = 9.4f;
	private float groundScale = 2;

	// At the start of the game..
	void Start ()
	{
		// get the third person character ( this should never be null due to require component )
		m_Character = GetComponent<ThirdPersonCharacter>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";

		//set a random instruction color at the begining
		System.Random random = new System.Random();
		gameInstructions.text = INSTRUCTIONS[random.Next(0, INSTRUCTIONS.Length)];
		Debug.Log("Start : " );

		
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		//float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveVertical = Input.GetAxis ("Vertical");

		if (m_Cam != null)
		{
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = Vinput * m_CamForward + Hinput * m_Cam.right;
		}
		else
		{
			// we use world-relative directions in the case of no main camera
			m_Move = Vinput * Vector3.forward + Hinput * Vector3.right;
		}
#if !MOBILE_INPUT
		// walk speed multiplier
		if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

		// pass all parameters to the character control script
		m_Character.Move(m_Move);

	}

	void Restart()
	{
		SceneManager.LoadScene(0);
	}



	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	IEnumerator OnTriggerEnter(Collider other) 
	{
		
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag(gameInstructions.text))
		{
			//changePositionDisappear(other);

			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive(false);

			// Add one to the score variable 'count'
			count++;

			return transfromObjectPosition(other);

		}

		else
		{
			other.gameObject.SetActive(false);

			count--;

			

			return transfromObjectPosition(other);
		}

	}

	IEnumerator transfromObjectPosition (Collider other)
	{
		// Run the 'SetCountText()' function (see below)
		SetCountText();

		float sideLength = groundDimension * groundScale;

		other.gameObject.transform.position = 
			new Vector3(Random.Range( -1 * sideLength, sideLength), (float)0.5 * groundScale, Random.Range(-1 * sideLength, sideLength));
		//Print the time of when the function is first called.
		Debug.Log("Started Coroutine at timestamp : " + Time.time);

		//yield on a new YieldInstruction that waits for 5 seconds.
		yield return new WaitForSeconds(Mathf.CeilToInt(Random.Range(1.0f, 4.0f)));

		//After we have waited 5 seconds print the time again.
		Debug.Log("Finished Coroutine at timestamp : " + Time.time);

		other.gameObject.SetActive(true);
	}

	void SetGameInstructions()
	{
		System.Random random = new System.Random();
		int value = random.Next(0, INSTRUCTIONS.Length);
		gameInstructions.text = INSTRUCTIONS[value];
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		if (count % 3 == 0)
		{
			SetGameInstructions();
		}

	}
}