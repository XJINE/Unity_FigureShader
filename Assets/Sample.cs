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

        this.figureShader.DrawCircle(0, 0, 1, Color.red);

        this.figureShader.DrawRing(0.8f, 0.8f, 0.1f, 0.15f, Color.green);

        this.figureShader.DrawSquare
        (Camera.main.WorldToViewportPoint(this.transform.position), 0.2f, Color.blue, 10);

        this.figureShader.DrawRect
        (new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Color(1, 1, 0, 0.5f));
    }
}