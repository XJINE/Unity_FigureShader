using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class Draw2DShapeEffect : ImageEffectBase
{
    #region Enum

    public enum Shape : int
    {
        Circle = 0,
        Ring   = 1,
        Sqare  = 2,
        Rect   = 3,
    }

    #endregion Enum

    #region Struct

    [System.Serializable]
    public struct ShapeData
    {
        public Shape   shape;
        public Vector4 position;
        public Vector4 parameter;
        public Color   color;
    }

    #endregion Struct

    #region Field

    private new Camera camera;

    private ComputeBuffer shapeBuffer;

    // NOTE:
    // shapes must be initialized in Field.
    // Because shapes is not initialized in sometimes when initialized in Start() or Awake().

    private readonly SortedList<int, List<ShapeData>> shapes = new SortedList<int, List<ShapeData>>();

    private int shapeBufferID;

    #endregion Field

    #region Method

    protected override void Start()
    {
        base.Start();
        this.camera = base.GetComponent<Camera>();
        this.shapeBufferID = Shader.PropertyToID("_ShapeBuffer");
    }

    protected override void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (this.shapes.Count != 0)
        {
            List<ShapeData> shapes = new List<ShapeData>();

            foreach (var data in this.shapes)
            {
                shapes.AddRange(data.Value);
            }

            this.shapeBuffer = new ComputeBuffer(shapes.Count, Marshal.SizeOf(typeof(ShapeData)));
            this.shapeBuffer.SetData(shapes);

            base.material.SetBuffer(this.shapeBufferID, this.shapeBuffer);
        }

        base.OnRenderImage(src, dest);

        if (this.shapeBuffer != null)
        {
            this.shapeBuffer.Release();
        }
    }

    protected void OnDestroy()
    {
        if (this.shapeBuffer != null)
        {
            this.shapeBuffer.Release();
            this.shapeBuffer.Dispose();
            this.shapeBuffer = null;
        }
    }

    public void Clear()
    {
        if (this.shapes != null)
        {
            this.shapes.Clear();
        }
    }

    public void DrawCircle(Vector3 posInWorld, Color color, float radius, int index = -1)
    {
        DrawCircle((Vector2)this.camera.WorldToViewportPoint(posInWorld), color, radius, index);
    }

    public void DrawCircle(Vector2 posInScreen, Color color, float radius, int index = -1)
    {
        ShapeData shape = new ShapeData()
        {
            shape     = Shape.Circle,
            position  = new Vector4(posInScreen.x, posInScreen.y, 0, 0),
            parameter = new Vector4(radius, 0, 0, 0),
            color     = color
        };

        DrawShape(shape, index);
    }

    public void DrawRing(Vector3 posInWorld, Color color, float innerRadius, float outerRadius, int index = -1)
    {
        DrawRing((Vector2)this.camera.WorldToViewportPoint(posInWorld), color, innerRadius, outerRadius, index);
    }

    public void DrawRing(Vector2 posInScreen, Color color, float innerRadius, float outerRadius, int index = -1)
    {
        ShapeData shape = new ShapeData()
        {
            shape     = Shape.Ring,
            position  = new Vector4(posInScreen.x, posInScreen.y, 0, 0),
            parameter = new Vector4(innerRadius, outerRadius, 0, 0),
            color = color
        };

        DrawShape(shape, index);
    }

    public void DrawSqare(Vector3 posInWorld, Color color, float size, int index = -1)
    {
        DrawSqare((Vector2)this.camera.WorldToViewportPoint(posInWorld), color, size, index);
    }

    public void DrawSqare(Vector2 posInScreen, Color color, float size, int index = -1)
    {
        ShapeData shape = new ShapeData()
        {
            shape     = Shape.Sqare,
            position  = new Vector4(posInScreen.x, posInScreen.y, 0, 0),
            parameter = new Vector4(size, 0, 0, 0),
            color     = color
        };

        DrawShape(shape, index);
    }

    public void DrawRect(Vector3 posInWorld1, Vector3 posInWorld2, Color color, int index = -1)
    {
        DrawRect((Vector2)this.camera.WorldToViewportPoint(posInWorld1),
                 (Vector2)this.camera.WorldToViewportPoint(posInWorld2),
                 color,
                 index);
    }

    public void DrawRect(Vector2 posInScreen1, Vector2 posInScreen2, Color color, int index = -1)
    {
        ShapeData shape = new ShapeData()
        {
            shape     = Shape.Rect,
            position  = new Vector4(posInScreen1.x, posInScreen1.y, posInScreen2.x, posInScreen2.y),
            parameter = new Vector4(0, 0, 0, 0),
            color     = color
        };

        DrawShape(shape, index);
    }

    public void DrawShape(ShapeData shape, int index = 0)
    {
        if (this.shapes.ContainsKey(index))
        {
            this.shapes[index].Add(shape);
        }
        else
        {
            this.shapes.Add(index, new List<ShapeData>(){ shape });
        }
    }

    #endregion Method
}