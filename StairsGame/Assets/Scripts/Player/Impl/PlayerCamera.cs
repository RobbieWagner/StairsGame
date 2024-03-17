using UnityEngine;

namespace RobbieWagnerGames.ZombieStairs
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float cameraYOffset = 1.5f;

        private void Update()
        {
            if (PlayerInstance.Instance.transform.position.y + cameraYOffset > transform.position.y)
                transform.position = new Vector3(transform.position.x, PlayerInstance.Instance.transform.position.y + cameraYOffset, -10);
        }
    }
}
