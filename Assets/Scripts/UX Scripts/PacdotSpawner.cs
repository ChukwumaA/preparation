using UnityEngine;

namespace UX_Scripts
{
    public class PacdotSpawner : MonoBehaviour
    {
        public GameObject pacdot;
        public float interval;
        public float startOffset;
        public float startTime;

        // Start is called before the first frame update
        private void Start()
        {
            startTime = Time.time + startOffset;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!(Time.time > startTime + interval)) return;
            var obj = Instantiate(pacdot, transform.position, Quaternion.identity);
            obj.transform.parent = transform;

            startTime = Time.time;
        }
    }
}
