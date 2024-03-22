using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.ZombieStairs
{
    public interface IPlayerGun
    {
        void ShootGun(InputAction.CallbackContext context);
        float GetAim();
        void UpdateAim();
    }
}
