using UnityEngine;

namespace Assets.Scripts.BaseClasses
{
    [RequireComponent(typeof(BaseUnit))]
    public class BaseComponent : MonoBehaviour
    {
        protected BaseUnit Unit { get; private set; }

        public void Start()
        {
            this.Unit = this.GetComponent<BaseUnit>();
        }
    }
}