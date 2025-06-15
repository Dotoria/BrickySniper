using Common;
using Scene;
using UnityEngine;

namespace Game
{
    public class Paddle : InputManager
    {
        private GameScene _game;

        public GameObject touchArea;
        private Vector2 spriteSize = new Vector2(3.8f, 0.8f);
        private Vector2 cameraSize;

        public float paddleSpeed;
        public bool isSetting;
        [HideInInspector] public Cell cell;

        void Awake()
        {
            _game = FindAnyObjectByType<GameScene>();
            Camera _camera = Camera.main;
            cameraSize = new Vector2(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
            isSetting = false;
            base.Initialize(_camera, touchArea.GetComponent<BoxCollider2D>(), "Paddle");
        }

        void Update()
        {
            if (cell && isSetting)
            {
                Time.timeScale = 0.2f;
            }

            InputUpdate(transform.position);
        }

        protected override void HandleDragMove(Vector3 pos, Vector3 input, Vector3 init)
        {
            base.HandleDragMove(pos, input, init);
            float deltaX = (inputPosition.x - initPosition.x) * paddleSpeed;
            float clampedX = Mathf.Clamp(pos.x + deltaX, -cameraSize.x + spriteSize.x / 2,
                cameraSize.x - spriteSize.x / 2);

            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }

        protected override void HandleDragEnd()
        {
            if (cell && isSetting)
            {
                cell.Shoot();
                _game.SetSpeed(Speed.Current);
            }

            isSetting = false;
            cell = default;

            base.HandleDragEnd();
        }
    }
}