using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace _3rdMFD
{
    class DamageUI : MonoBehaviour
    {

        public SpriteRenderer sprite;
        public List<Health> healths = new List<Health>();
        const float maxVal = 1f / 3f;

        private float totalMaxHealth = 0;
        private void Start()
        {
            totalMaxHealth = 0;
            foreach(Health h in healths)
            {
                totalMaxHealth += h.maxHealth;
            }
        }
        private void Update()
        {
            float totalCurrentHealth = 0;

            foreach(Health h in healths)
            {
                totalCurrentHealth += h.currentHealth;
            }
            float healthFraction = totalCurrentHealth / totalMaxHealth;
            sprite.color = Color.HSVToRGB(maxVal * healthFraction, 1, 0.7f);

        }

    
    }
}
