using Core.Enums;

namespace Core.Interfaces
{
    public interface IEnemyFactory
    {
        IEnemy Create(EnemyType type);
    }
}
