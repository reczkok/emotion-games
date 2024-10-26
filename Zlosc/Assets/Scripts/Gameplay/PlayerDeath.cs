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
        private static readonly int Hurt = Animator.StringToHash("hurt");
        private static readonly int Dead = Animator.StringToHash("dead");
        private readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (!player) return;
            if (!player.health.IsAlive) return;
            
            player.health.Die();
            // player.collider.enabled = false;
            player.controlEnabled = false;

            if (player.audioSource && player.ouchAudio)
                player.audioSource.PlayOneShot(player.ouchAudio);
            player.animator.SetTrigger(Hurt);
            player.animator.SetBool(Dead, true);
            var enemies = Object.FindObjectsByType<EnemyController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach(var enemy in enemies)
            {
                if (enemy._collider.enabled) continue;
                var ev = Simulation.Schedule<EnemyRespawn>(1);
                ev.enemy = enemy;
            }

            var tokens =
                Object.FindObjectsByType<TokenInstance>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach(var token in tokens)
            {
                if (!token.collected) continue;
                var ev = Simulation.Schedule<TokenRespawn>(1);
                ev.token = token;
            }
            var parameters = new Dictionary<string, object>();
            // The ‘myEvent’ event will get queued up and sent every minute
            //Events.CustomData("playerDeath", parameters);
            //Events.Flush();
            Simulation.Schedule<PlayerSpawn>(2);
        }
    }
}