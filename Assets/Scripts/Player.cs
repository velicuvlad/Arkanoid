using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    private static Player _instance;
    public static Player Instance => _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    [SerializeField]
    private float _speed = 3.5f;

    public bool playerTransforming { get; set; }

    public Vector3 current;
    public Vector3 normalScale;
    public Vector3 initialScale;
    private BoxCollider br;
    public AudioSource tickPlayer;
    public AudioClip tickPlayerOnColl;


    void Start()
    {
        InitPlayer();
        initialScale = transform.localScale;
        current = transform.localScale;
        br = GetComponent<BoxCollider>();
        tickPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        float shift = initialScale.x / 2 - current.x /2;
        float leftClamp = -2.98f - shift;
        float rightClamp = 2.98f + shift;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftClamp, rightClamp), transform.position.y, transform.position.z);
        Vector3 playerScale = transform.localScale;
        
        
    }

    public void InitPlayer()
    {
        transform.position = new Vector3(0, -1.22f, -6f);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            AudioSource.PlayClipAtPoint(tickPlayerOnColl, transform.position, 0.2f);
            Rigidbody ballRb = coll.gameObject.GetComponent<Rigidbody>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
            ballRb.velocity = Vector2.zero;

            float diff = paddleCenter.x - hitPoint.x;
            float constant = Mathf.Abs(diff * 350);

            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector3(-constant, BallManager._initialBallSpeed, 0));
            }
            else
            {
                ballRb.AddForce(new Vector3(constant, BallManager._initialBallSpeed, 0));
            }
        }
    }

    private IEnumerator ResetPlayerWidthAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        this.StartWidthAnimation(initialScale.x);
    }

    public void StartWidthAnimation(float expand)
    {
        StartCoroutine(AnimatePlayer(expand));
    }

    private IEnumerator AnimatePlayer(float width)
    {
        this.playerTransforming = true;
        StartCoroutine(ResetPlayerWidthAfterTime(10));
        bool done = false;
        current = Instance.transform.localScale; 
        Vector3 targetScale = new Vector3(width, current.y, current.z);
        while (!done)
        {
            transform.localScale = Vector3.Lerp(current, targetScale, 2 * Time.deltaTime);
            current = transform.localScale;
            br.size = new Vector3(current.x, current.y, current.z);
            float distance = Vector3.Distance(transform.localScale, targetScale);
            if (distance <= 0.01)
            {
                done = true;
            }
            yield return null;
        }
        this.playerTransforming = false;
        
        
    }

    
}
