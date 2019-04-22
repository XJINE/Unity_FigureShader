using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class FigureShader : ImageEffectBase
{
    #region Enum

    public enum Shape : int
    {
        Circle = 0,
        Ring   = 1,
        Square = 2,
        Rect   = 3,
    }

    #endregion Enum

    #region Struct

    [System.Serializable]
    public struct FigureData
    {
        public Shape   shape;
        public Vector4 position;
        public Vector4 parameter;
        public Color   color;
    }

    #endregion Struct

    #region Field

    private new Camera camera;

    private ComputeBuffer figureDataBuffer;

    private readonly SortedList<int, List<FigureData>> figureDataList = new SortedList<int, List<FigureData>>();

    protected static int FigureDataBufferID = -1;

    public bool autoClear = true;

    #endregion Field

    #region Method

    protected virtual void Awake()
    {
        if (FigureShader.FigureDataBufferID < 0)
        {
            FigureShader.FigureDataBufferID = Shader.PropertyToID("_FigureDataBuffer");
        }
    }

    protected override void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (this.figureDataList.Count != 0)
        {
            List<FigureData> figureDataList = new List<FigureData>();

            foreach (var data in this.figureDataList)
            {
                figureDataList.AddRange(data.Value);
            }

            this.figureDataBuffer = new ComputeBuffer(figureDataList.Count, Marshal.SizeOf(typeof(FigureData)));
            this.figureDataBuffer.SetData(figureDataList);

            base.material.SetBuffer(FigureShader.FigureDataBufferID, this.figureDataBuffer);
        }

        base.OnRenderImage(src, dest);

        if (this.figureDataBuffer != null)
        {
            this.figureDataBuffer.Release();
        }

        if (this.autoClear)
        {
            this.figureDataList.Clear();
        }
    }

    protected void OnDestroy()
    {
        if (this.figureDataBuffer != null)
        {
            this.figureDataBuffer.Release();
            this.figureDataBuffer = null;
        }
    }

    public void Clear()
    {
        if (this.figureDataList != null)
        {
            this.figureDataList.Clear();
        }
    }

    public void DrawCircle(Vector3 posInWorld, Color color, float radius, int index = -1)
    {
        DrawCircle((Vector2)this.camera.WorldToViewportPoint(posInWorld), color, radius, index);
    }

    public void DrawCircle(Vector2 posInScreen, Color color, float radius, int index = -1)
    {
        FigureData shape = new FigureData()
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
        FigureData shape = new FigureData()
        {
            shape     = Shape.Ring,
            position  = new Vector4(posInScreen.x, posInScreen.y, 0, 0),
            parameter = new Vector4(innerRadius, outerRadius, 0, 0),
            color = color
        };

        DrawShape(shape, index);
    }

    public void DrawSquare(Vector3 posInWorld, Color color, float size, int index = -1)
    {
        DrawSquare((Vector2)this.camera.WorldToViewportPoint(posInWorld), color, size, index);
    }

    public void DrawSquare(Vector2 posInScreen, Color color, float size, int index = -1)
    {
        FigureData shape = new FigureData()
        {
            shape     = Shape.Square,
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
        FigureData shape = new FigureData()
        {
            shape     = Shape.Rect,
            position  = new Vector4(posInScreen1.x, posInScreen1.y, posInScreen2.x, posInScreen2.y),
            parameter = new Vector4(0, 0, 0, 0),
            color     = color
        };

        DrawShape(shape, index);
    }

    public void DrawShape(FigureData figureData, int index = 0)
    {
        if (this.figureDataList.ContainsKey(index))
        {
            this.figureDataList[index].Add(figureData);
        }
        else
        {
            this.figureDataList.Add(index, new List<FigureData>(){ figureData });
        }
    }

    #endregion Method
}