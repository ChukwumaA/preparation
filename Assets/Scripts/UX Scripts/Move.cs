using UnityEngine;

namespace UX_Scripts
{
    public class Move : MonoBehaviour
    {
        public float speed;

        // Update is called once per frame
        private void Update()
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Destroy(gameObject);
        }
    }
}
