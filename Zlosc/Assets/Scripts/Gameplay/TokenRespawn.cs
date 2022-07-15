using Platformer.Core;
using Platformer.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gameplay
{
    public class TokenRespawn : Simulation.Event<TokenRespawn>
    {
        public TokenInstance token;

        public override void Execute()
        {
            token.Respawn();
        }
    }
}
