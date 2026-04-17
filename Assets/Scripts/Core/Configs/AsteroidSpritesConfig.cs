using Core.Enums;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = "AsteroidSpritesConfig", menuName = "Asteroids/AsteroidSpritesConfig")]
    public class AsteroidSpritesConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite[] BigAsteroids { get; private set; }
        [field: SerializeField] public Sprite[] MediumAsteroids { get; private set; }
        [field: SerializeField] public Sprite[] SmallAsteroids { get; private set; }

        public Sprite GetRandom(EnemyType type)
        {
            var sprites = type switch
                {
                    EnemyType.Asteroid => BigAsteroids,
                    EnemyType.Fragment => MediumAsteroids,
                    EnemyType.SmallFragment => SmallAsteroids,
                    EnemyType.Saucer => null,
                    _ => null
                };

            if (sprites == null || sprites.Length == 0) return null;
            return sprites[Random.Range(0, sprites.Length)];
        }
    }
}
