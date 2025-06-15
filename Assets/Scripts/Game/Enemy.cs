using System.Collections;
using System.Collections.Generic;
using Common;
using Scene;
using UnityEngine;

namespace Game
{
    public class Enemy : MonoBehaviour
    {
        private GameScene _game;
        private EnemyManager em;

        public EnemyScriptableObject enemySO;

        private SpriteRenderer _prefabSprite;
        private Animator _animator;
        private PolygonCollider2D _collider;

        public int healthPoint;
        public int currentHealthPoint;
        public int attackPoint;

        private Rigidbody2D _enemyRB;

        private void Awake()
        {
            _game = FindAnyObjectByType<GameScene>();
            em = _game.enemyManager;

            _prefabSprite = gameObject.GetComponent<SpriteRenderer>();
            _enemyRB = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<PolygonCollider2D>();
        }

        void FixedUpdate()
        {
            _enemyRB.velocity = _enemyRB.velocity.normalized * enemySO.movePoint;
            _enemyRB.angularVelocity = _enemyRB.angularVelocity > 1 ? 1 : _enemyRB.angularVelocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Cell"))
            {
                Cell attackCell = other.gameObject.GetComponent<Cell>();
                currentHealthPoint -= attackCell.attackPoint;

                // bigCell 한테 죽은 것일 때
                if (attackCell.attackLogic == AttackLogic.Phagocytosis)
                {
                    Destroy();
                }
                // bigCell 한테 죽은 것이 아닐 때
                else if (currentHealthPoint <= 0)
                {
                    StartCoroutine(DeadByCell());
                }
                else
                {
                    _animator.SetTrigger("Damage");
                }

                _game.GainScore(500);
            }
        }

        public void Install()
        {
            gameObject.name = enemySO.prefabName;
            _prefabSprite.sprite = enemySO.prefabSprite;
            _animator.runtimeAnimatorController = enemySO.prefabAnimation;

            healthPoint = enemySO.healthPoint;
            currentHealthPoint = healthPoint;
            attackPoint = enemySO.attackPoint;

            // 콜라이더 초기화
            _collider.pathCount = 0;
            List<Vector2> physicsShape = new List<Vector2>();
            enemySO.prefabSprite.GetPhysicsShape(0, physicsShape);

            // 새로운 경로 설정
            _collider.pathCount = 1;
            _collider.SetPath(0, physicsShape);

            // 강제로 콜라이더 업데이트
            _collider.enabled = false;
            _collider.enabled = true;
        }

        public void Shoot(Vector3 spawnPos, Vector3 spawnDir)
        {
            Install();
            transform.position = spawnPos;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, -spawnDir);
            _enemyRB.velocity = enemySO.movePoint * spawnDir.normalized;
        }

        IEnumerator DeadByCell()
        {
            _animator.SetTrigger("Death");
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy();
            _game.GainCoin(+10);
            if (Random.Range(0f, 100f) > 50f)
                _game.GainGem(+1);
        }

        // 풀로 돌려놓기
        public void Destroy()
        {
            ObjectPool.Instance["enemy"].ReturnToPool(gameObject);
        }
    }
}