using Managers;
using UnityEngine;

namespace Systems
{
    public class Pin : MonoBehaviour
    {
        public PinType pinType;
        
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void MouseEnter()
        {
            spriteRenderer.color = Manager.Instance.pinHighlight;
        }

        public void MouseExit()
        {
            spriteRenderer.color = Manager.Instance.pinDefault;
        }
    }

    public enum PinType
    {
        INPUT,
        OUTPUT
    }
} 