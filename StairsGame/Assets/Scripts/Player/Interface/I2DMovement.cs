using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.ZombieStairs
{
    public interface I2DMovement
    {
        void ChangeSpeed(float newWalkSpeed, float newRunSpeed);
        void ResetSpeeds();
        void EnterAimMode(InputAction.CallbackContext context);
        void ExitAimMode(InputAction.CallbackContext context);
    }
}