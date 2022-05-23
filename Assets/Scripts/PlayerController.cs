using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public EntityObject entity;
    Animator animator;
    CircleCollider2D rangeCollider;
    bool isWalking = false;
    public bool inRange = false;
    bool canAttack = true;
    public bool inBattle = false;
    float input_x;
    float input_y;
    public float detection;
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> interactables = new List<GameObject>();
    [SerializeField] int gold = 0;
    public GameUtils gameUtils;

    private void Awake()
    {
        entity = GetComponent<EntityObject>();
        animator = GetComponent<Animator>();
        rangeCollider = GetComponent<CircleCollider2D>();
        gameUtils = FindObjectOfType<GameUtils>();
    }
    void Start()
    {
        rangeCollider.radius = entity.range * (entity.GetAttributeValue(Attribute.rangeMultiplier) / 100f);
        entity.speed *= entity.GetAttributeValue(Attribute.speedMultiplier) / 100f;
    }

    void Update()
    {
        if (!inBattle)
        {
            input_x = Input.GetAxisRaw("Horizontal");
            input_y = Input.GetAxisRaw("Vertical");

            isWalking = input_x != 0 || input_y != 0;

            if (isWalking)
            {
                animator.SetFloat("input_x", input_x);
                animator.SetFloat("input_y", input_y);
            }
            animator.SetBool("isWalking", isWalking);

            if (Input.GetKey(KeyCode.Space) && !isWalking)
            {
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.E) && interactables.Count > 0) //interact
            {
                Interactable interactable = interactables[0].GetComponent<Interactable>();
                interactable.OnInteract();
                if (interactable.destroyOnInteract)
                {
                    Destroy(interactable.gameObject);
                }
            }
        }
        else
        {
            if (targets.Count == 0)
            {
                inBattle = false;
                return;
            }
            animator.SetFloat("input_x", targets[0].transform.position.x - transform.position.x);
            animator.SetFloat("input_y", targets[0].transform.position.y - transform.position.y);
            if (inRange)
            {
                animator.SetBool("isWalking", false);
                Attack();
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(CooldownAttack());
            animator.SetTrigger("attack");
            if (inRange)
            {
                StartCoroutine(DelayAttackAnim());
            }
        }  
    }
    private IEnumerator DelayAttackAnim()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject target = targets[0];
        if (target != null)
        {
            if (target.GetComponent<AttackableObject>() != null)
            {
                target.GetComponent<AttackableObject>().OnAttacked();
                targets.Remove(target);
            }
            else if (target.GetComponent<EnemyController>() != null)
            {
                //damage calculation
                float multiplier = gameUtils.CheckProbability(entity.GetAttributeValue(Attribute.critChance)) ? entity.GetAttributeValue(Attribute.critDamage)/100f : 1;
                int physicalDamage = Mathf.RoundToInt(entity.GetAttributeValue(Attribute.attack) * multiplier);
                target.GetComponent<EnemyController>().entity.calcDamageTaken(physicalDamage, 0, multiplier > 1);
            }
        }
    }

    public void CollectGold(int qty)
    {
        gameUtils.ShowText(this.gameObject, "+" + qty.ToString() + " Gold", Color.yellow);
        gold+=qty;
    }

    public void Recover(int healthValue, int manaValue)
    {
        entity.currentHealth = Mathf.Min(entity.currentHealth + healthValue, entity.GetAttributeValue(Attribute.maxHealth));
        entity.healthBarSlider.value = entity.currentHealth;

        entity.currentMana = Mathf.Min(entity.currentMana + manaValue, entity.GetAttributeValue(Attribute.maxMana));
    }

    private IEnumerator CooldownAttack()
    {
        float cooldown = inBattle ? 1 / (entity.GetAttributeValue(Attribute.attackSpeed)/100f) : 1 / 1.5f;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        if (!inBattle)
        {
            if (isWalking)
            {
                var move = new Vector3(input_x, input_y, 0).normalized;
                transform.position += move * entity.speed * Time.deltaTime;
            }
        }
        else if(!inRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, targets[0].transform.position, entity.speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            if (!interactables.Contains(collision.gameObject))
                interactables.Add(collision.gameObject);
        }
        else if(collision.tag != "NotAttackable" && collision.tag != "Detection")
        {
            inRange = true;
            if(!targets.Contains(collision.gameObject))
                targets.Add(collision.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            interactables.Remove(collision.gameObject);
        }
        else if(collision.tag != "NotAttackable" && collision.tag != "Detection")
        {
            targets.Remove(collision.gameObject);
            if (targets.Count == 0)
            {
                inRange = false;
            }
        }

    }
}
