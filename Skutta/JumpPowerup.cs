using Skutta.GameLogic;

namespace Skutta
{
    public class JumpPowerup : Pickuppable
    {
        public JumpPowerup(int xPos = 500, int yPos = 400) : base(xPos, yPos)
        {
            _soundEffectName = "coin-pickup";
        }
        protected override void OnCollictionOccured(Player player)
        {
            player.JumpPowerup();
        }
    }
}
