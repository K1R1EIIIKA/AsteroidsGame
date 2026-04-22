using Infrastructure.Data;
using UnityEngine;

namespace Infrastructure.Physics
{
    public class BoundaryWrapper
    {
        private readonly ConfigService _config;

        public BoundaryWrapper(ConfigService config)
        {
            _config = config;
        }

        public Vector2 Wrap(Vector2 position)
        {
            var hw = _config.World.WorldWidth  * 0.5f;
            var hh = _config.World.WorldHeight * 0.5f;

            if (position.x >  hw) position.x = -hw;
            if (position.x < -hw) position.x =  hw;
            if (position.y >  hh) position.y = -hh;
            if (position.y < -hh) position.y =  hh;

            return position;
        }
    }
}
