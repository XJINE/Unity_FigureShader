using UnityEngine;

[ExecuteInEditMode]
public class Sample : MonoBehaviour
{
    public Draw2DShapeEffect effect;

    void Update()
    {
        this.effect.Clear();

        this.effect.DrawCircle(new Vector2(0, 0), Color.red, 1);

        this.effect.DrawRing  (new Vector2(0.8f, 0.8f), Color.green, 0.1f, 0.15f);

        this.effect.DrawSqare (this.transform.position, Color.blue, 0.2f, 10);

        this.effect.DrawRect(new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Color(1, 1, 0, 0.5f));
    }
}