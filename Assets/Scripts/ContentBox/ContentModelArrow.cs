using System.Collections;
using UnityEngine;

namespace ContentBox
{
    public class ContentModelArrow : MonoBehaviour
    {
        [Range(1f, 1000f)][SerializeField] private float rotateSpeed = 100.0f;
        [Range(0.01f, 3f)][SerializeField] private float moveSpeed = 1.0f;
        [Range(0.01f, 3f)][SerializeField] private float onceTime = 1.0f;

        private float time = 0;
        private bool isLeft = false;

        private void FixedUpdate()
        {
            if (time < 0)
            {
                time = onceTime;
                isLeft = !isLeft;
            }
            time -= Time.deltaTime;

            var distance = (isLeft ? -1 : 1) * moveSpeed * Time.deltaTime;

            transform.localPosition = new Vector3(transform.localPosition.x + distance, transform.localPosition.y, transform.localPosition.z);
            transform.Rotate(new Vector3(0, 1, 0) * (Time.deltaTime * rotateSpeed));
        }

    }
}