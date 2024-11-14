using System;
using UnityEngine;

namespace InterpolationCurves
{
    public class SecondOrder : MonoBehaviour
    {
        public Transform target;
        public float speed, rotationSpeed;
        [SerializeField] [Range(0, 1)] private float bounce;

        public Vector3 oldPos, curPos, nextPos;

        private float s, t;

        private void FixedUpdate()
        {
            // OrderSystem();
            // Second();
            test();
        }

        private void OrderSystem()
        {
            var position = transform.position;
            var curPos = position;
            position = Vector3.Lerp(position, target.position, (1 - bounce) * speed * Time.fixedDeltaTime);
            position += bounce * (position - oldPos);
            oldPos = curPos;
            transform.SetPositionAndRotation(position,
                Quaternion.Slerp(transform.rotation, target.rotation, rotationSpeed * Time.fixedDeltaTime));
        }

        private void Second()
        {
            var position = transform.position;
            var curPos = Vector3.Lerp(position, target.position, (1 - bounce) * speed * Time.fixedDeltaTime);
            curPos += bounce * (position - oldPos);
            oldPos = position;
            transform.position = curPos;
        }

        
            private void Second()
            {
                curPos = transform.position;
                nextPos = curPos;
                curPos = Vector3.Lerp(curPos, target.position,
                    (1 - bounce) * speed * Time.fixedDeltaTime);

                curPos = (1 + bounce) * curPos - bounce * oldPos;
                transform.position = curPos;
                oldPos = nextPos;
            }
        
    }
}