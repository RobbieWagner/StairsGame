using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.ZombieStairs
{
    public class PlayerGun : MonoBehaviour, IPlayerGun
    {
        public Gun currentGun; 
        public AimDirection currentAimDirection = AimDirection.NONE;
        public bool aiming;
        public SpriteRenderer gunRenderer;
        public GunControls gunControls;

        public static PlayerGun Instance {get; private set;}

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(gameObject); 
            } 
            else 
            { 
                Instance = this; 
            } 

            gunControls = new GunControls();
            gunControls.Gun.Shoot.performed += ShootGun;
        }

        private void FixedUpdate()
        {
            UpdateAim();
        }

        public void ShootGun(InputAction.CallbackContext context)
        {
            Debug.Log("BANG!");
        }

        public float GetAim()
        {
            Vector2 side1 = Vector2.up;
            Vector2 side2 = (Vector2) (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            return side1.x < side2.x ? Vector2.Angle(side1, side2) : 360 - Vector2.Angle(side1, side2);
        }

        public void UpdateAim()
        {
            if(aiming)
            {
                float angle = GetAim();

                int index = (int) (angle/(360/(currentGun.GunSprites.Count - 1))) + 1;
                //Debug.Log(index);

                if(index < currentGun.GunSprites.Count && index >= 0)
                    gunRenderer.sprite = currentGun.GunSprites.ElementAt(index).Value;
            }
            else
                gunRenderer.sprite = null;
        }
    }
}
