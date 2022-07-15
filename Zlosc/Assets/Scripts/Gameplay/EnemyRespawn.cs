using Platformer.Core;
using Platformer.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gameplay
{
    public class EnemyRespawn : Simulation.Event<EnemyRespawn>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            enemy._collider.enabled = true;
            enemy.control.enabled = false;
            enemy.Teleport(enemy.startPos);
        }
    }
}
