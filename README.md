# Unity_FigureShader

<img src="https://github.com/XJINE/Unity_FigureShader/blob/main/Screenshot.png" width="100%" height="auto" />

Draw some 2D shapes with FragmentShader. This effect should be used for a debug.

## Importing

You can use Package Manager or import it directly.

```
https://github.com/XJINE/Unity_FigureShader.git?path=Assets/Packages/FigureShader
```

### Dependencies

This project use following resources.

- https://github.com/XJINE/Unity_ImageEffectBase


## How to use

You can draw some Circle, Ring, Square and Rect into screen space.
Set a position, color, size and any other parameters into these functions.

``` csharp
figureShader.DrawCircle(0, 0, 1, Color.red);

figureShader.DrawRing(0.8f, 0.8f, 0.1f, 0.15f, Color.green);

figureShader.DrawSquare
(Camera.main.WorldToViewportPoint(this.transform.position), 0.2f, Color.blue, 10);

figureShader.DrawRect
(new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Color(1, 1, 0, 0.5f));
```

### Drawing Order

Args ``index`` means the order of the rendering.
Usually, the order is same as call order.　In such a case, the index value is 0.

However, if you set index 1, the shape will be drawn after the others.
Smaller index shape will be drawn much faster than the others.
