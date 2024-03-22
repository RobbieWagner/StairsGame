using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RobbieWagnerGames.ZombieStairs
{
    public enum AimDirection
    {
        NONE = -1,
        ONE_OCLOCK = 1,
        TWO_OCLOCK = 2,
        THREE_OCLOCK = 3,
        FOUR_OCLOCK = 4,
        FIVE_OCLOCK = 5,
        SIX_OCLOCK = 6,
        SEVEN_OCLOCK = 7,
        EIGHT_OCLOCK = 8,
        NINE_OCLOCK = 9,
        TEN_OCLOCK = 10,
        ELEVEN_OCLOCK = 11,
        NOON = 12
    }

    public enum GunType
    {
        NONE = -1,
        PISTOL = 0,
        SHOTGUN = 1,
        FLAMETHROWER = 2
    }

    public class Gun : MonoBehaviour, IGun
    {
        public GunType type;
        [SerializedDictionary("Aim Direction","Sprite")] 
        public SerializedDictionary<AimDirection, Sprite> GunSprites;
        public int damage = 1;
        public float shotsPerSecond = 1;
    }
}
