using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Model;
using MyGame.View;

public class EnemyVisual
{
    private readonly Texture2D texture;
    private readonly Entity entity;
    private Color currentColor;

    public EnemyVisual(Texture2D texture, Entity entity)
    {
        this.texture = texture;
        this.entity = entity;
        currentColor = Color.White;
    }
    public void SetDamageEffect(bool isDamaged)
    {
        currentColor = isDamaged ? Color.Red : Color.White;
        entity.isDamaged = false;
    }

    // Теперь Draw принимает камеру, чтобы позицию пересчитать
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        SetDamageEffect(entity.isDamaged);
        var scale = entity.size / new Vector2(texture.Width, texture.Height);

        // Позиция на экране с учётом смещения камеры
        var screenPos = new Vector2(entity.position.X - camera.offsetX, entity.position.Y - camera.offsetY);
        var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

        spriteBatch.Draw(
            texture,
            screenPos,
            null,
            currentColor,
            0f,
            origin,
            scale,
            SpriteEffects.None,
            0f
        );
    }
}
