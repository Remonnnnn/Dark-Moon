using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;

    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;//复制概率
    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer<0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime*colorLosingSpeed));

            if(sr.color.a<=0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _cloestEnemy,bool _canDuplicate,float _chanaceToDuplicate,Player _player,float _attackMultiplier)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }
        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransform.position+_offset;
        cloneTimer = _cloneDuration;

        closestEnemy= _cloestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanaceToDuplicate;
        FaceClosestTarget();

    }

    private void AnimationTrigger()
    {
        cloneTimer = -1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats=player.GetComponent<PlayerStats>();
                EnemyStats enemyStats=hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamge(enemyStats, attackMultiplier);

                if(player.skill.clone.canApplyOnHitEffect)//拓展：判断能否触发武器特效
                {
                    ItemData_Equipment weaponData1 = Inventory.instance.GetEquipment(EquipmentType.Weapon);//判断武器附加效果(有目标)
                    if (weaponData1 != null)
                    {
                        weaponData1.Effect(hit.transform);
                    }
                }

                if(canDuplicateClone)//拓展：判断能否再额外产生分身
                {
                    if(Random.Range(0,100)<chanceToDuplicate)//判断概率
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f*facingDir, 0));//再次产生分身
                    }   
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if(closestEnemy!=null)
        {
            if(transform.position.x>closestEnemy.transform.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }

    }
}
