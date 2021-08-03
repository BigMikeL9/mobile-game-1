using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // Configs
    [Header("Chicken Configs")]
    [SerializeField] float minChickenSpeed;
    [SerializeField] float maxChickenSpeed;
    [SerializeField] float enemySpeed = 1.3f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] int chickenHealth = 2;
    [SerializeField] float destroyDelay= 0.3f;
    [SerializeField] int pushBackForce = 200;


    [Header("Boundaries")]
    [SerializeField] GameObject leftBoxCollider;
    [SerializeField] GameObject rightBoxCollider;
    
    // States
    bool isGoingLeft;
    bool isGoingRight = true;
    
    // Cache
    Rigidbody2D _rigidbody2D;
    PolygonCollider2D _bodyCollider;
    CircleCollider2D _jumpDetectionCollider;
    Cat _cat;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodyCollider = GetComponent<PolygonCollider2D>();
        _jumpDetectionCollider = GetComponent<CircleCollider2D>();
        
        _cat = FindObjectOfType<Cat>();
    }


    private void Update()
    {
        EnemyMove();
        JumpOverObstacle();
        FlipSprite();
        Die();
    }
    
    private void EnemyMove()
    {
        // Maybe randomize speed later *****
        
        _rigidbody2D.velocity = new Vector2(enemySpeed, _rigidbody2D.velocity.y);
    }
    
    
    private void JumpOverObstacle()
    {
        bool isTouchingObstacle = _jumpDetectionCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle"));
        if (!isTouchingObstacle) { return; }
        
        _rigidbody2D.AddForce(Vector2.up * jumpForce);
    }
    
    
    // Control the direction the chicken sprite is facing
    private void FlipSprite()
    {
        bool isEnemyMoving = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (isEnemyMoving)
        {
            transform.localScale = new Vector3(Mathf.Sign(-_rigidbody2D.velocity.x), 1f);
        }
    }
    
    
    // Cat Damage to Chicken
    public void TakeDamage(int damageAmount)
    {
        chickenHealth -= damageAmount;
        enemySpeed = -enemySpeed;
        // pushes chicken away from cat, on hit
        // Vector2 awayFromCat = (transform.position - _cat.transform.position).normalized;
        // _rigidbody2D.AddForce(awayFromCat * pushBackForce);
    }


    private void Die()
    {
        if (chickenHealth <= 0)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
    
    // Detects which collider the chicken hit, inorder to go in the opposite direction
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        // Chicken Movement Boundaries
        if (otherObject == leftBoxCollider)
        {
            isGoingLeft = false;
            isGoingRight = true;
            enemySpeed = Mathf.Abs(enemySpeed); // Controls the chicken move direction
        }
        else if (otherObject == rightBoxCollider)
        {
            isGoingLeft = true;
            isGoingRight = false;
            enemySpeed = -enemySpeed; // Controls the chicken move direction
        }
    }
    
}
