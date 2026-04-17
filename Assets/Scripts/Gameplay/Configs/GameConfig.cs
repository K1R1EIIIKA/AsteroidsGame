using Gameplay.Enemies;
using Gameplay.PlayerLogic;
using Gameplay.Weapons.BulletWeapon;
using Gameplay.Weapons.LaserWeapon;
using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Asteroids/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Ship")]
        public ShipView ShipPrefab;

        [Header("Weapons")]
        public BulletView BulletPrefab;
        public LaserView LaserPrefab;

        [Header("Enemies")]
        public EnemyView AsteroidPrefab;
        public EnemyView FragmentPrefab;
        public EnemyView SmallFragmentPrefab;
        public EnemyView SaucerPrefab;
        
        [Header("Ship Sprites")]
        public Sprite ShipIdle;    
        public Sprite ShipThrust;
    }
}
