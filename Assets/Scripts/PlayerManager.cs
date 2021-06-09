using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;
    Rigidbody2D rigidbody2D;
    Animator animator;

    float speed;
    float jumpPower = 400;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0)
        {
            //止まっている
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0)
        {
            //右へ
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0)
        {
            //左へ
            direction = DIRECTION_TYPE.LEFT;
        }
        if (IsGround())
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void FixedUpdate()
    {
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -3;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    void Jump()
    {
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.SetBool("isJumping", true);
    }

    bool IsGround()
    {
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "trap")
        {
            Debug.Log("game over");
            gameManager.GameOver();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("goal");
            gameManager.GameClear();
        }
        if (collision.gameObject.tag == "item")
        {
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            if (this.gameObject.transform.position.y + 0.2f > enemy.transform.position.y)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                enemy.DestroyEnemy();
            }
            else
            {
                Destroy(this.gameObject);
                gameManager.GameOver();
            }
        }
    }

}
