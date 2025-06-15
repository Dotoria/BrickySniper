using System.Collections;
using System.Collections.Generic;
using Common;
using Scene;
using UnityEngine;

namespace Game
{
    public class Cell : MonoBehaviour
    {
        private GameScene _game;
        private CellManager cm;

        public CellScriptableObject cellSO;

        private Paddle _paddle;

        private SpriteRenderer _prefabSprite;
        private SpriteRenderer _paddleSprite;
        private Animator _animator;
        private PolygonCollider2D _collider;

        public float skillTime;
        public int healthPoint;
        public int currentHealthPoint;
        public int attackPoint;
        public AttackLogic attackLogic;
        public MoveLogic moveLogic;

        private Rigidbody2D _cellRB;

        private void Awake()
        {
            _game = FindAnyObjectByType<GameScene>();
            cm = _game.cellManager;

            _prefabSprite = gameObject.GetComponent<SpriteRenderer>();

            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (var entry in playerObjects)
            {
                if (entry.TryGetComponent(out _paddle))
                {
                    _paddle = entry.GetComponent<Paddle>();
                    _paddleSprite = _paddle.gameObject.GetComponent<SpriteRenderer>();
                    break;
                }
            }

            _cellRB = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<PolygonCollider2D>();

            Skill();
        }

        private void FixedUpdate()
        {
            _cellRB.velocity = _cellRB.velocity.normalized * cellSO.movePoint;
            _cellRB.angularVelocity = _cellRB.angularVelocity > 1 ? 1 : _cellRB.angularVelocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _animator.SetTrigger("Attack");
            }
        }

        public void Install()
        {
            gameObject.name = cellSO.prefabName;
            _prefabSprite.sprite = cellSO.prefabSprite;
            _paddleSprite.sprite = cellSO.paddleSprite;
            _animator.runtimeAnimatorController = cellSO.prefabAnimation;

            skillTime = cellSO.skillTime;

            healthPoint = cellSO.healthPoint;
            currentHealthPoint = healthPoint;
            attackPoint = cellSO.attackPoint;

            attackLogic = cellSO.attackLogic;
            moveLogic = cellSO.moveLogic;

            // 콜라이더 초기화
            _collider.pathCount = 0;
            List<Vector2> physicsShape = new List<Vector2>();
            cellSO.prefabSprite.GetPhysicsShape(0, physicsShape);

            // 새로운 경로 설정
            _collider.pathCount = 1;
            _collider.SetPath(0, physicsShape);

            // 강제로 콜라이더 업데이트
            _collider.enabled = false;
            _collider.enabled = true;
            gameObject.SetActive(false);

            _paddle.isSetting = true;
            _paddle.cell = this;
        }

        public void Shoot()
        {
            gameObject.SetActive(true);
            transform.position = _paddle.gameObject.transform.position + new Vector3(0, 1f, 0);
            _cellRB.velocity = cellSO.movePoint * transform.up;

            cm.ReloadCell(cm.cellSO.FindIndex(cell => cell == cellSO));
            _game.GainEnergy(-5);
        }

        public void Skill()
        {
            StartCoroutine(StartSkill());
        }

        IEnumerator StartSkill()
        {
            yield return new WaitForSeconds(skillTime);
            switch (attackLogic)
            {
                case AttackLogic.Phagocytosis:
                    break;
                case AttackLogic.Apoptosis:
                    break;
                default:
                    break;
            }
        }

        // 풀로 돌려놓기
        public void Destroy()
        {
            ObjectPool.Instance["cell"].ReturnToPool(gameObject);
        }
    }
}