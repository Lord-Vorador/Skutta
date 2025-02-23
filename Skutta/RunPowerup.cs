using Skutta.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skutta
{
    public class RunPowerup : Pickuppable
    {
        public RunPowerup(int xPos = 500, int yPos = 400) : base(xPos, yPos)
        {
            _soundEffectName = "run-powerup";
        }

        protected override void OnCollictionOccured(Player player)
        {
            player.MovePowerup();
        }
    }
}
