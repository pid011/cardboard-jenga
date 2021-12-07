using System;
using TMPro;
using UnityEngine;

namespace CardboardJenga
{
    public class JengaTimer : Singleton<JengaTimer>
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        private bool _timerRun;

        public float elapsedMilliseconds { get; private set; }

        private void Start()
        {
            _timerRun = false;
        }

        private void Update()
        {
            if (_timerRun)
            {
                elapsedMilliseconds += Time.deltaTime;
            }

            _timerText.text = TimeSpan.FromSeconds(elapsedMilliseconds).ToString(@"mm\:ss\.fff");
        }

        public void StartTimer()
        {
            _timerRun = true;
        }

        public void ClearTimer(bool stop = false)
        {
            elapsedMilliseconds = 0f;
            _timerRun = !stop;
        }

        public void StopTimer()
        {
            _timerRun = false;
        }
    }
}
