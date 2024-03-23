using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.Managers
{
    public interface IInputManager
    {
        static IInputManager Instance;
        bool RegisterActionCollection(IInputActionCollection actionCollection);
        bool DeregisterActionCollection(IInputActionCollection actionCollection);
        bool RegisterActionMap(InputActionMap map);
        bool DeregisterActionMap(InputActionMap map);
        void DisableActions();
        void ReenableActions();
    }
}