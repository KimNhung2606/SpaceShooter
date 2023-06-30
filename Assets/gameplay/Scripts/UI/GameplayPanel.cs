using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Section3
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtScore;
        [SerializeField] private Image m_ImgHpBar;
        [SerializeField] private Slider m_UpdateChange;
        [SerializeField] List<int> m_ValueUpdates;
        int curUpdate = 0;
        private void OnEnable()
        {
            curUpdate = 0;
            GameManager.Instance.onScoreChanged += OnScoreChanged;
            SpawnManager.Instance.Player.onHPChanged += OnHpChanged;
            SpawnManager.Instance.Player.onValueUpdateChanged += OnUpdateChange;
            m_UpdateChange.maxValue = m_ValueUpdates[curUpdate];
            SpawnManager.Instance.Player.ReplaceSprite(curUpdate);
            SpawnManager.Instance.index = 0;
        }

        private void OnDisable()
        {
            GameManager.Instance.onScoreChanged -= OnScoreChanged;
            SpawnManager.Instance.Player.onHPChanged -= OnHpChanged;
            SpawnManager.Instance.Player.onValueUpdateChanged -= OnUpdateChange;
            curUpdate = 0;
            SpawnManager.Instance.Player.ReplaceSprite(curUpdate);
            SpawnManager.Instance.index = 0;
        }

        private void OnHpChanged(int curHp, int maxHp)
        {
            m_ImgHpBar.fillAmount = curHp * 1f / maxHp;
        }
        private void OnUpdateChange(int bonnus)
        {
            m_UpdateChange.value += bonnus;
            if (m_UpdateChange.value >= m_ValueUpdates[curUpdate]&&curUpdate < 5)
            {
                curUpdate++;
                m_UpdateChange.value = 0;
                m_UpdateChange.maxValue = m_ValueUpdates[curUpdate];
                SpawnManager.Instance.Player.ReplaceSprite(curUpdate);
                SpawnManager.Instance.index = curUpdate;
            }


        }
        public void BtnPause_Pressed()
        {
            GameManager.Instance.Pause();
        }

        private void OnScoreChanged(int score)
        {
            m_TxtScore.text = "SCORE: " + score;
        }
    }
}