using UnityEngine;

[ExecuteAlways]
public class Sample : MonoBehaviour
{
    private FigureShader _figureShader;

    private FigureShader _FigureShader
    {
        get
        {
            if (_figureShader == null)
            {
                _figureShader = GetComponent<FigureShader>();
            }

            return _figureShader;
        }
    }

    private void Update()
    {
        // NOTE:
        // Use Clear when FigureShader.autoClear = false.

        _FigureShader.Clear();

        _FigureShader.DrawCircle(0, 0, 1, Color.red);

        _FigureShader.DrawRing(0.8f, 0.8f, 0.1f, 0.15f, Color.green);

        _FigureShader.DrawSquare(Camera.main.WorldToViewportPoint(transform.position), 0.2f, Color.blue, 10);

        _FigureShader.DrawRect(new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Color(1, 1, 0, 0.5f));
    }
}