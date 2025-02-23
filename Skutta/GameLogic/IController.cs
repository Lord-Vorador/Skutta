using Microsoft.Xna.Framework;

namespace Skutta.GameLogic;

public interface IController
{
    string Name { get; set; }

    void SetPosition(Vector2 vector2);
    void Update(GameTime gameTime);
}