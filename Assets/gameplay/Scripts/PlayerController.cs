using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class PlayerController : MonoBehaviour
    {
        public Action<int, int> onHPChanged;
        public Action<int> onValueUpdateChanged;
        [SerializeField] private float m_MoveSpeed;
        //[SerializeField] private ProjectileController m_Projectile;
        [SerializeField] private Transform m_FiringPoint;
        [SerializeField] private float m_FiringCooldown;
        [SerializeField] private int m_Hp;
        [SerializeField] List<Sprite> PlayerUpdate;
        [SerializeField] SpriteRenderer m_curSprite;

        private int m_CurrentHp;
        private float m_TempCooldown;
        //private SpawnManager m_SpawnManager;
        //private GameManager m_GameManager;
        //private AudioManager m_AudioManager;

        // Start is called before the first frame update
        void Start()
        {
            m_CurrentHp = m_Hp;
            if (onHPChanged != null)
                onHPChanged(m_CurrentHp, m_Hp);
            //m_SpawnManager = FindObjectOfType<SpawnManager>();
            //m_GameManager = FindObjectOfType<GameManager>();
            //m_AudioManager = FindObjectOfType<AudioManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Instance.IsActive())
                return;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(horizontal, vertical);
            //transform.Translate(direction * Time.deltaTime * m_MoveSpeed);
            Vector3 newPosition = transform.position + new Vector3(direction.x, direction.y, 0f) * Time.deltaTime * m_MoveSpeed;

            // Lấy giới hạn tọa độ của màn hình
            float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 1;// size của nhân vât với bên trái
            float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 1;// size nhân vật vs bề ngang bên phải
            float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 1;// size nhân điểm dưới
            float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - 1;// size nhân vật vs điểm trên

            // Giới hạn vị trí của người chơi trong màn hình
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;
            if (Input.GetKey(KeyCode.Space))
            {
                if (m_TempCooldown <= 0)
                {
                    Fire();
                    m_TempCooldown = m_FiringCooldown;
                }
            }

            m_TempCooldown -= Time.deltaTime;
        }

        private void Fire()
        {
            //ProjectileController projectile = Instantiate(m_Projectile, m_FiringPoint.position, Quaternion.identity, null);
            ProjectileController projectile = SpawnManager.Instance.SpawnPlayerProjectile(m_FiringPoint.position);
            projectile.Fire(1);

            SpawnManager.Instance.SpawnShootingFX(m_FiringPoint.position);
            AudioManager.Instance.PlayLazerSFX();
        }

        public void Hit(int damage)
        {
            m_CurrentHp -= damage;
            if (onHPChanged != null)
                onHPChanged(m_CurrentHp, m_Hp);
            if (m_CurrentHp <= 0)
            {
                Destroy(gameObject);

                SpawnManager.Instance.SpawnExplosionFX(transform.position);
                GameManager.Instance.Gameover(false);
                AudioManager.Instance.PlayExplosionSFX();
            }
            AudioManager.Instance.PlayHitSFX();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Bonnus"))
            {
                if (onValueUpdateChanged != null)
                    onValueUpdateChanged(1);
                Destroy(collision.gameObject);
            }
        }
        public void ReplaceSprite(int index)
        {
            m_curSprite.sprite = PlayerUpdate[index];
        }
    }
}