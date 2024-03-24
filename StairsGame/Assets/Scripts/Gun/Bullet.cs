using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class Bullet : MonoBehaviour, IBullet
    {
        public void KillZombie(Zombie zombie)
        {
            GameManager.Instance.Score += zombie.killScore;
            ZombieManager.Instance?.RemoveZombieFromScene(zombie);
        }

        public virtual void Launch(Vector2 direction)
        {
            var hitObjects = Physics2D.RaycastAll(PlayerGun.Instance.transform.position, direction, PlayerGun.Instance.currentGun.range);
            var hitZombies = hitObjects.Select(o => o.collider.GetComponentInParent<Zombie>()).Where(z => z != null).Distinct();
            foreach(Zombie zombie in hitZombies)
                KillZombie(zombie);
            Destroy(gameObject);
        }
    }
}