using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //public Entity entity;
    public EntityObject entity;
    CircleCollider2D rangeCollider;
    bool inRange = false;
    bool canAttack = true;
    bool isWalking = false;
    bool inBattle = false;
    [SerializeField] [Range(0, 1)] float healthGlobeChance;
    [SerializeField] GameObject healthGlobePrefab;
    PlayerController target;
    Animator animator;
    [SerializeField] [Range(-1, 1)] int moveX;
    [SerializeField] [Range(-1, 1)] int moveY;
    GameUtils gameUtils;

    private void Awake()
    {
        entity = GetComponent<EntityObject>();
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.radius = entity.range;
        animator = GetComponent<Animator>();
        gameUtils = FindObjectOfType<GameUtils>();
    }

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        if (!inBattle)
        {
            isWalking = moveX != 0 || moveY != 0;
            if (isWalking)
            {
                animator.SetFloat("input_x", moveX);
                animator.SetFloat("input_y", moveY);
            }
            animator.SetBool("isWalking", isWalking);
        }
        else
        {
            if (inRange)
            {
                animator.SetBool("isWalking", false);
                moveX = 0;
                moveY = 0;
                if (canAttack)
                {
                    canAttack = false;
                    StartCoroutine(CooldownAttack());
                    if (target != null)
                    {
                        animator.SetTrigger("attack");
                        StartCoroutine(DelayAttackAnim());
                    }
                }
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetFloat("input_x", target.transform.position.x - transform.position.x);
                animator.SetFloat("input_y", target.transform.position.y - transform.position.y);
            }       
        }

        //death
        if (entity.currentHealth == 0)
        {
            target.targets.Remove(this.gameObject);
            target.inRange = false;
            if (gameUtils.CheckProbability(healthGlobeChance))
            {
                Instantiate(healthGlobePrefab, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (inRange)
        {
            return;
        }
        Vector3 move;
        if (!inBattle)
        {
            move = new Vector3(moveX, moveY).normalized;
            transform.position += move * entity.speed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, entity.speed * Time.deltaTime);
        }
    }

    private IEnumerator ChangeDirection()
    {
        while (!inBattle)
        {
            moveX = Random.Range(-1, 2);
            moveY = Random.Range(-1, 2);
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator DelayAttackAnim()
    {
        yield return new WaitForSeconds(0.1f);
        if(target != null)
        {
            float multiplier = gameUtils.CheckProbability(entity.critChance) ? entity.critDamage : 1;
            int physicalDamage = Mathf.RoundToInt(entity.attack * multiplier);
            target.entity.calcDamageTaken(physicalDamage, 0, multiplier > 1);
        }
    }

    private IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(1 / entity.attackSpeed);
        canAttack = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = true;
        }
        else if(collision.tag == "Detection")
        {
            target = collision.GetComponent<DetectionController>().player.GetComponent<PlayerController>();
            inBattle = true;
            if(!target.targets.Contains(this.gameObject))
                target.targets.Add(this.gameObject);
            target.inBattle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inRange = false; 
        }
    }
}
