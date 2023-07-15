using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHOOT : MonoBehaviour
{
    public GameObject projectile; //The Prefab that will be instantiated per left mouse click
	
    public Camera playerCamera; //Self explainatory
	
    public float launchVelocity = 50f; //Also self explainatory
	
	public AudioSource timerSound; //Actually just the audio source that plays the timer beeps
	
	public AudioSource resetSound; //Actually just the audio source that plays the timer reset sound
	
	public AudioClip[] timerSounds; //An array of two audio clips that serve as the beeps in the timer
	
	public AudioClip[] resetSounds; //An array of audio clips that serve as the reset in the timer
	
    GameObject firedProjectile; //The instantiated prefab declared ahead of time
	
    Vector3 startPos; // This will hold the (x,y,z) of the player so the instantiated projectile spawns from the right place. I'm using a temporary holder because I want to add an offset later
	
    public Object[] textures; //An array of possible textures that our projectile can have
	
	public Text countDown; //The timer text to let the pkayer know when shit will switch up
	
	public float Timer = 10.0f; //The time in seconds (kinda) that it takes before the player's shot switches
	
	private int remainingSeconds; // A variable to hold the second value to display on timer
	
	private int randomIndex; //The random texture array index
	
	private Sprite projectileTexture; //The random texture selected from the array
	
	private bool hasCycledBefore = false; //Evil little hack because I'm lazy. The first time the game starts it hands the player the same abillity first. Then this is flipped so the game selects the texture and whatnot from the index
	
	private bool soundIndex = true; //Another little trick. Later, you'll see why I use a bool
	
	
	private float projectileDynamicFriction = 0.6f;	//
	
	private float projectileStaticFriction = 0.6f;	// PROJECTILE MATERIAL SETTINGS
	
	private float projectileBounciness = 0.0f;		//
	
	private SphereCollider projectileCollider;
	

    void Start()
    {
        textures = Resources.LoadAll("projectileTextures", typeof(Sprite)); // Load sprite textures from "projectileTextures" folder

        if (textures.Length == 0) //If there are no textures found...
        {
            Debug.LogError("No textures found in 'projectileTextures' folder");
        }
		
		resetSound.clip = resetSounds[0];
		
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //Check if player is clicking
        {
			
            startPos = transform.position + transform.forward; //Store the transform of the player in startPos
            startPos.y += 1f; //offset the y...

            firedProjectile = Instantiate(projectile, startPos, playerCamera.transform.rotation); ///creates a "clone" of the prefab
			
			projectileCollider = firedProjectile.GetComponent<SphereCollider>(); //Set the projectile collider to the fired projectile collider
			
			projectileCollider.material.dynamicFriction = projectileDynamicFriction; //Duh
			
			projectileCollider.material.staticFriction = projectileStaticFriction; //DUH
			
			projectileCollider.material.bounciness = projectileBounciness; //DUH DUH DUR
			
			if(hasCycledBefore == false){ //checks if the timer has been reset before
				firedProjectile.GetComponent<SpriteRenderer>().sprite = (Sprite)textures[0]; //if it hasn't, just use index 0
			}
			
			else if(hasCycledBefore == true){ //if it has...
            firedProjectile.GetComponent<SpriteRenderer>().sprite = projectileTexture; //set it to the random one
			}
			
            firedProjectile.GetComponent<Rigidbody>().velocity = playerCamera.transform.forward * launchVelocity; //give that lil bastard some force to make it move
        }

        if (firedProjectile != null) //check if there's a firedProjectile in the list
        {
            Destroy(firedProjectile, 10.0f); //if so, if a firedProjectile has lasted 5 seconds, kill the sucker
        }
		
		Timer -= Time.deltaTime; //decriment the timer by delta time
		
		int tempSecondHolder = remainingSeconds; //It's a suprise tool that'll help us later
		
		remainingSeconds = (int)(Timer % 6); //Convert unity time to actual seconds
		
		if(tempSecondHolder > remainingSeconds){				// NOTE: Before you give me hell for this, I know this sucks. I couldn't figure out to to call on every other deltaTime update neatly
			
			soundIndex = !soundIndex;							// so instead, I just check if the tempSecondHolder is smaller than the remainingSecond. It always will be, right? (I hope)
			
			timerSound.clip = timerSounds[soundIndex ? 1 : 0];	// So, that means that it'll happen roughly every delta time. Kinda.  All this code does is invert the bool by setting it to it's opposite state
			timerSound.Play();									// then I set the index to the int value of the bool using the ? operator. This will set the audio index to either 1 or 0, and then I play the sound at that index.
		}
		
		countDown.text = "Time Remaining: " + remainingSeconds.ToString(); //I concatenate the string for the timer so it displays the time in seconds
		
		if(remainingSeconds == 0){ //If the timer has counted down
		
			Timer = 10.0f; //Reset the timer
			
			resetSound.Play();	
			
			if(hasCycledBefore == false){ //Flip the bool that checks if this cycle has occurred before
				
				hasCycledBefore = true;
			}
			
			if (textures.Length == 0) //If there are no textures loaded...
            {
                Debug.LogError("No textures loaded!"); //..then tell me. Although, this really should never happen.
                return;
            }

            randomIndex = Random.Range(0, textures.Length); //Generate a random index to select
			
			projectileDynamicFriction = Random.Range(0.0f, 1.0f); //
			projectileStaticFriction = Random.Range(0.0f, 1.0f); // RANDOMISE DYNAMIC FRICTION, STATIC FRICTION, BOUNCINESS
			projectileBounciness = Random.Range(0.0f, 1.0f);	//
			
			launchVelocity = Random.Range(20f, 100f); // RANDOMISE VELOCITY

            if (randomIndex >= textures.Length) //If this messes up somehow, let me know
            {
                Debug.LogError("Invalid texture index???");
                return;
            }

            if (!(textures[randomIndex] is Sprite)) //Actually check that the sprite is properly converted. This can mess up when Unity tricks you into thinking your texture format is okay to use
            {
                Debug.LogError("Invalid texture type???");
                return;
            }

            projectileTexture = (Sprite)textures[randomIndex]; //Set the projectile texture to the texture at the random index
		
		}
    }
}

