using UnityEngine;

[ExecuteInEditMode]
public class Sample : MonoBehaviour
{
    public FigureShader figureShader;

    void Update()
    {
        // NOTE:
        // Use Clear when FigureShader.autoClear = false.

        this.figureShader.Clear();

        this.figureShader.DrawCircle(new Vector2(0, 0), Color.red, 1);

        this.figureShader.DrawRing(new Vector2(0.8f, 0.8f), Color.green, 0.1f, 0.15f);

        this.figureShader.DrawSquare(this.transform.position, Color.blue, 0.2f, 10);

        this.figureShader.DrawRect(new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Color(1, 1, 0, 0.5f));
    }
}