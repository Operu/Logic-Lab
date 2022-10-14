using Managers;
using UnityEngine;

namespace Systems
{
    public class Pin : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnMouseEnter()
        {
            spriteRenderer.color = Manager.Instance.pinHighlight;
        }

        private void OnMouseExit()
        {
            spriteRenderer.color = Manager.Instance.pinDefault;
        }
    }
} 