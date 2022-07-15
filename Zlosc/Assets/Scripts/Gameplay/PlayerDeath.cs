using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Gameplay;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Unity.Services.Analytics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player.health.IsAlive)
            {
                player.health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.controlEnabled = false;

                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
                var enemies = GameObject.FindObjectsOfType<EnemyController>();
                foreach(var enemy in enemies)
                {
                    if (!enemy._collider.enabled)
                    {
                        var ev = Simulation.Schedule<EnemyRespawn>(1);
                        ev.enemy = enemy;
                    }
                }
                var tokens = GameObject.FindObjectsOfType<TokenInstance>(true);
                foreach(var token in tokens)
                {
                    if (token.collected)
                    {
                        var ev = Simulation.Schedule<TokenRespawn>(1);
                        ev.token = token;
                    }
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                    {
                    };
                // The ‘myEvent’ event will get queued up and sent every minute
                Events.CustomData("playerDeath", parameters);
                Events.Flush();
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }
    }
}