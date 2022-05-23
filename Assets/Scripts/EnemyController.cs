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
    [SerializeField] [Range(0, 100)] int healthGlobeChance;
    [SerializeField] GameObject healthGlobePrefab;
    [SerializeField] [Range(0, 100)] int itemChance;
    [SerializeField] int itemMaxQty;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] [Range(0, 1)] float[] rarityTable;
    [SerializeField] [Range(0, 1)] float[] itemQtyTable;
    PlayerController target;
    Animator animator;
    [SerializeField] [Range(-1, 1)] int moveX;
    [SerializeField] [Range(-1, 1)] int moveY;
    GameUtils gameUtils;

    private void Awake()
    {
        entity = GetComponent<EntityObject>();
        rangeCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        gameUtils = FindObjectOfType<GameUtils>();
    }

    private void Start()
    {
        rangeCollider.radius = entity.range * (entity.GetAttributeValue(Attribute.rangeMultiplier) / 100f);
        entity.speed *= entity.GetAttributeValue(Attribute.speedMultiplier) / 100f;
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
                animator.SetBool("isWalking", true);
                animator.SetFloat("input_x", target.transform.position.x - transform.position.x);
                animator.SetFloat("input_y", target.transform.position.y - transform.position.y);
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
            DropHealthGlobe();
            DropItems();
            Destroy(this.gameObject);
        }
    }

    private void DropHealthGlobe()
    {
        if (gameUtils.CheckProbability(healthGlobeChance))
        {
            Instantiate(healthGlobePrefab, transform.position, Quaternion.identity);
        }
    }

    private void DropItems()
    {
        if (gameUtils.CheckProbability(itemChance))
        {
            int qtyToDrop = GameUtils.gameUtils.IndexProbabilityTable(itemQtyTable) + 1;
            for(int i = 0; i < qtyToDrop; i++)
            {
                ItemClass.Rarity rarity = (ItemClass.Rarity)GameUtils.gameUtils.IndexProbabilityTable(rarityTable);
                GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                item.GetComponent<DroppedItem>().item = GameUtils.gameUtils.GenerateEquipment(entity.GetAttributeValue(Attribute.level), rarity);
            }
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
            yield return new WaitForSeconds(2);
            moveX = 0;
            moveY = 0;
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator DelayAttackAnim()
    {
        yield return new WaitForSeconds(0.1f);
        if(target != null)
        {
            float multiplier = gameUtils.CheckProbability(entity.GetAttributeValue(Attribute.critChance)) ? (entity.GetAttributeValue(Attribute.critDamage)/100f) : 1;
            int physicalDamage = Mathf.RoundToInt(entity.GetAttributeValue(Attribute.attack) * multiplier);
            target.entity.calcDamageTaken(physicalDamage, 0, multiplier > 1);
        }
    }

    private IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(1 / (entity.GetAttributeValue(Attribute.attackSpeed)/100f));
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
