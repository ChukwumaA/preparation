using UnityEngine;
using UnityEngine.UI;

namespace UX_Scripts
{
    public class NameToTitle : MonoBehaviour
    {
        public Text title;

        private void OnMouseEnter()
        {
            switch(name)
            {
                case "Pac-Man":
                    title.color = Color.yellow;
                    break;

                case "Blinky":
                    title.color = Color.red;
                    break;

                case "Pinky":
                    title.color = new Color(254f/255f, 152f/255f, 203f/255f);
                    break;

                case "Inky":
                    title.color = Color.cyan;
                    break;
            
                case "Clyde":
                    title.color = new Color(254f/255f, 203f/255f, 51f/255f);
                    break;
            }

            title.text = name;
        }

        private void OnMouseExit()
        {
            title.text = "Pac-Man";
            title.color = Color.white;
        }
    }
}
