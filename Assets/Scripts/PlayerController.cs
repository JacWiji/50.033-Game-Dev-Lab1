using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform enemyLocation;
    public Text scoreText;


    public float speed;
    public float upSpeed;
    public float maxSpeed;
    private float moveHorizontal;
    private int score = 0;
    

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private  Animator marioAnimator;
    private AudioSource marioAudio;


    private bool onGroundState = true;
    private bool faceRightState = true;
    private bool countScoreState = false;

    private float time=1.0f;




    void Start()
    {
        // set to 30fps
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator  =  GetComponent<Animator>();
        marioAudio=GetComponent<AudioSource>();
    }

    
    void FixedUpdate()
    {
        if ((Input.GetKeyUp("a") || Input.GetKeyUp("d"))&& onGroundState){
            // stop
            marioBody.velocity = Vector2.zero;
        }

        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }
        
        if (Input.GetKeyDown("space") && onGroundState){
          marioBody.AddForce(Vector2.up *upSpeed, ForceMode2D.Impulse);
          onGroundState = false;
          marioAnimator.SetBool("onGround", onGroundState);
          countScoreState = true;
          
      }
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
            countScoreState = false;
            scoreText.text = "Score: " + score.ToString();
            
        }
        if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            Time.timeScale = 0.0f;
            
            //SceneManager.LoadScene("SampleScene");
        }
    }
    void  PlayJumpSound(){
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true;
          if (Mathf.Abs(marioBody.velocity.x) >  1.0){
              marioAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) >  1.0){
                marioAnimator.SetTrigger("onSkid");
            }
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }
        
        
    }
}
