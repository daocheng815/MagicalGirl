using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Eeemy
{
    public class BlackEnemyNew : EnemySate
    {
        private SearchToPlayers _searchToPlayers;
        private Rigidbody2D _rb;
        private Damageable _damageable;

        
        [Header("基礎數值")]
        public bool isAwake;
        public float speed = 3f;
        public bool isMove;
        
        [Header("攻擊")]
        public DetectionZone attackZone;
        public bool triggerAttack;
        public float nowAttackDelayTime;
        public float attackDelay = 0.5f;

        [Header("暴衝")]
        public float changeTime = 1f;
        private float _quickDashDirection;
        private float _nowChangeTime;
        public float quickDashChange = 0.15f;
        public float speedMagnification = 5;
        public GameObject quickAttack;
        public float quickDashTime = 1f;
        public float nowQuickDashTime;

        [Header("逃跑")] 
        public float runAwayTime = 0.8f;
        private float _nowRunAwayTime;
        public float runAwaySpeed = 4f;

        [Header("怒氣")] 
        public float fury;

        [Header("怒氣攻擊")]
        public GameObject furyAttack;
        public DetectionZone furyAttackZone;
        public GameObject furyAttackParticle;
        public bool triggerFuryAttack;
        public float nowFuryAttackTime;
        public float furyAttackTime = 0.9f;
        
        
        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
            _searchToPlayers = GetComponent<SearchToPlayers>();
            _damageable = GetComponent<Damageable>();
        }

        public void IsAwake()
        {
            if (!isAwake)
            {
                isAwake = true;
                isMove = true;
                ChangeAnimState(StateAnimator.InTrue);
                WaitForAnim(StateAnimator.Idle, (() => ChangeState(State.Pursue)));
                CharacterEvents.characterText.Invoke(gameObject, "!");
            }
        }

        private void Update()
        {
            ValueFunc();
            switch (state)
            {
                case State.Idle:
                    _rb.velocity = Vector2.zero;
                    break;
                case State.Pursue:
                    Pursue();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.FuryAttack:
                    FuryAttack();
                    break;
                case State.Hit:
                    break;
                case State.QuickDash:
                    QuickDash();
                    break;
                case State.RunAway:
                    RunAway();
                    break;
            }
        }

        private void ValueFunc()
        {
            if (!_damageable.IsAlive)
            {
                ChangeState(State.Death);
                ChangeAnimState(StateAnimator.Death,animatorEnd: (() => Destroy(gameObject)));
                return;
            }
            if(isMove && _searchToPlayers.distanceBools && !CheckState(State.QuickDash))
                FlipDirection();
            if(nowAttackDelayTime > 0)
                nowAttackDelayTime -= Time.deltaTime;
            triggerAttack = attackZone.detectColliders.Count >= 1;
            triggerFuryAttack = furyAttackZone.detectColliders.Count >= 1;
            if (triggerFuryAttack && isAwake && fury >= 150 && !CheckState(State.QuickDash))
            {
                //先暫時關閉特殊攻擊
                /*
                ChangeState(State.FuryAttack,end: () =>
                {
                    nowFuryAttackTime = 0f;
                    furyAttack.SetActive(false);
                    furyAttackParticle.SetActive(false);
                });
                */
                fury -= 150;
                return;
            }
            if (triggerAttack && isAwake && nowAttackDelayTime <= 0 && !CheckState(State.QuickDash) && !CheckState(State.FuryAttack))
                ChangeState(State.Attack);
        }

        public void OnAttack(int damage)
        {
            fury+= damage;
        }
        public void OnHit(int damage, Vector2 knockback,attack.AttackType type)
        {
            isMove = true;
            isAwake = true;
            fury+= damage;
            Debug.Log("逃跑");
            ChangeAnimState(StateAnimator.Hit);
            ChangeState(State.Hit);
            
            WaitForAnim(StateAnimator.Idle, (() =>
            {
                if (type == attack.AttackType.Melee)
                {
                    var minValueChanger = 1 - ((float)_damageable.health / _damageable.MaxHealth);
                    if(minValueChanger > 0.8) minValueChanger = 0.8f ;
                    var randomValue = Random.value;
                    //Debug.Log($"逃跑機率 {minValueChanger} < 機率{randomValue}");
                    if (minValueChanger < randomValue)
                    {
                        _nowRunAwayTime = 0f;
                        ChangeState(State.RunAway);
                        Debug.Log("逃跑");
                        return;
                    }
                }
                else if (type == attack.AttackType.Remotely)
                {
                    ChangeState(State.QuickDash,(() =>
                    {
                        quickAttack.SetActive(true);
                        _quickDashDirection = _searchToPlayers.direction.x;
                        nowQuickDashTime = 0;
                    }),(() => quickAttack.SetActive(false)));
                }
                ChangeState(State.Pursue);
            }));
            Debug.Log("擊退");
            _rb.velocity = new Vector2(knockback.x, _rb.velocity.y + knockback.y);
        } 

        private void RunAway()
        {
            ChangeAnimState(StateAnimator.Idle);
            _rb.velocity = new Vector2(-_searchToPlayers.direction.x*runAwaySpeed,0);
            _nowRunAwayTime += Time.deltaTime;
            if(_nowRunAwayTime >= runAwayTime)
                ChangeState(State.Pursue);
        }
        
        private void Pursue()
        {
            ChangeAnimState(StateAnimator.Idle);
            _rb.velocity = new Vector2(_searchToPlayers.direction.x*speed,0);

            _nowChangeTime += Time.deltaTime;
            if (!(_nowChangeTime >= changeTime)) return;
            _nowChangeTime = 0;
            var randomValue = Random.value;
            if (randomValue < quickDashChange)
            {
                ChangeState(State.QuickDash,(() =>
                {
                    quickAttack.SetActive(true);
                    _quickDashDirection = _searchToPlayers.direction.x;
                    nowQuickDashTime = 0;
                }),(() => quickAttack.SetActive(false)));
            }

        }

        private void QuickDash()
        {
            _rb.velocity = new Vector2(_quickDashDirection*speed*speedMagnification,0);
            nowQuickDashTime += Time.deltaTime;
            if (nowQuickDashTime >= quickDashTime)
            {
                ChangeState(State.Pursue);
            }
        }
        
        private void Attack()
        {
            ChangeAnimState(StateAnimator.Attack, () =>
            {
                nowAttackDelayTime = attackDelay;
                WaitForAnim(StateAnimator.Idle, (() => ChangeState(State.Pursue)));
            });
        }

        private void FuryAttack()
        {
            furyAttack.SetActive(true);
            nowFuryAttackTime += Time.deltaTime;
            furyAttackParticle.SetActive(true);
                
            if (nowFuryAttackTime >= furyAttackTime)
            {
                nowFuryAttackTime = 0f;
                furyAttack.SetActive(false);
                furyAttackParticle.SetActive(false);
                //ChangeState(State.Pursue);
            }
        }
    }
}
