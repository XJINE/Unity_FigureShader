# Unity_Draw2DShapeEffect

<img src="https://github.com/XJINE/Unity_Draw2DShapeEffect/blob/master/Screenshot.png" width="100%" height="auto" />

Draw some 2D shapes with FragmentShader. This effect should be used for a debug.

## How to use

You can draw some Circle, Ring, Square and Rect into screen space.
Set a position, color, size and any other parameters into these functions.

``` csharp
this.effect.DrawCircle(new Vector2(0, 0), Color.red, 1);

this.effect.DrawRing(new Vector2(0.8f, 0.8f), Color.green, 0.1f, 0.15f);

this.effect.DrawSqare(this.transform.position, Color.blue, 0.2f, 10);

this.effect.DrawRect(new Vector2(0.5f, 0), new Vector2(1, 0.5f), new Color(1, 1, 0, 0.5f));
```

### Drawing Order

Args ``index`` means the order of the rendering.
Usually, the order is same as call order.　In such a case, the index value is 0.

However, if you set index 1, the shape will be drawn after the others.
Smaller index shape will be drawn much faster than the others.

## Import to Your Project

You can import this asset from UnityPackage.

- [Draw2DShapeEffect.unitypackage](https://github.com/XJINE/Unity_Draw2DShapeEffect/blob/master/Draw2DShapeEffect.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_ImageEffectBase](https://github.com/XJINE/Unity_ImageEffectBase)
