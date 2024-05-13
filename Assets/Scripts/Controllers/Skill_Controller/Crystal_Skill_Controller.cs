using Unity.VisualScripting;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim=>GetComponent<Animator>();
    private CircleCollider2D cd=>GetComponent<CircleCollider2D>();

    private Player player;

    private float crystalExitTimer;

    private bool canExplode;
    private bool canGrow;
    private float growSpeed = 3.5f;
    private float explodeRadius;

    private bool canMove;
    private float moveSpeed;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;


    public void SetupCrystal(float _crystalDuration,bool _canExplode,bool _canMove,float _moveSpeed,float _explodeRadius,Transform _closestTarget,Player _player)
    {
        player = _player;
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        explodeRadius = _explodeRadius;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()//在黑洞范围内随机选择目标
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius,whatIsEnemy);

        if(colliders.Length>0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }

    }

    private void Update()
    {
        crystalExitTimer-=Time.deltaTime;

        if(crystalExitTimer<0)
        {
            FinishCrystal();
        }

        if(canMove)
        {
            if(closestTarget!= null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, closestTarget.position) < 1)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
        }

        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(explodeRadius, explodeRadius),growSpeed*Time.deltaTime);
            cd.radius = Mathf.Lerp(cd.radius, explodeRadius/2, growSpeed * Time.deltaTime);//使cd保持
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet=Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if(equipedAmulet!= null)
                {
                    equipedAmulet.Effect(hit.transform);
                }
            }
        }
    }
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy()=>Destroy(gameObject);
}
