using UnityEngine;

namespace GameControl
{
    [System.Serializable]
    public class Cooldown
    {
        [SerializeField] private float cooldownTime;
        private float _nextFireTime;

        public bool IsCoolingDown => Time.time < _nextFireTime;
        public void StartCoolDown() => _nextFireTime = Time.time + cooldownTime;
    }
}


public class TestClass : MonoBehaviour
{
    private Cooldown _cooldown;
    
    private void Attack()
    {
        if (_cooldown.IsCoolingDown) return;
        // attack Logic
        _cooldown.StartCoolDown();
        //StartCoolDown()으로 nextFireTime에 현재 time에 cooldownTime
        //더해서 IsCoolingDown 체크
    }
}