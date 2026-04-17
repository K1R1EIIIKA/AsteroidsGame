using Infrastructure.Data.Configs;

namespace Infrastructure.Data
{
    public class ConfigService
    {
        public ShipConfig Ship { get; private set; }
        public EnemyConfig Enemy { get; private set; }
        public WorldConfig World { get; private set; }

        public void Initialize(ShipConfig ship, EnemyConfig enemy, WorldConfig world)
        {
            Ship = ship;
            Enemy = enemy;
            World = world;
        }
    }
}
