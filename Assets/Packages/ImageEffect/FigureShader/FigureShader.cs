using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class FigureShader : ImageEffectBase
{
    #region Field

    protected static int FigureDataBufferID = -1;

    public enum Figure : int
    {
        Circle = 0,
        Ring   = 1,
        Square = 2,
        Rect   = 3,
    }

    [System.Serializable]
    public struct FigureData
    {
        public Figure  figure;
        public Vector4 parameter;
        public Color   color;
    }

    private ComputeBuffer figureDataBuffer;

    private readonly SortedList<int, List<FigureData>> figureDataList = new SortedList<int, List<FigureData>>();

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

    public void DrawCircle(Vector2 pos, float radius, Color color, int index = -1)
    {
        DrawCircle(pos.x, pos.y, radius, color, index);
    }

    public void DrawCircle(float posX, float posY, float radius, Color color, int index = -1)
    {
        DrawFigure(new FigureData()
        {
            figure    = Figure.Circle,
            parameter = new Vector4(posX, posY, radius, 0),
            color     = color
        },
        index);
    }

    public void DrawRing(Vector2 pos, float inRad, float outRad, Color color, int index = -1)
    {
        DrawRing(pos.x, pos.y, inRad, outRad, color, index);
    }

    public void DrawRing(float posX, float posY, float inRad, float outRad, Color color, int index = -1)
    {
        DrawFigure(new FigureData()
        {
            figure    = Figure.Ring,
            parameter = new Vector4(posX, posY, inRad, outRad),
            color     = color
        },
        index);
    }

    public void DrawSquare(Vector2 pos, float size, Color color, int index = -1)
    {
        DrawSquare(pos.x, pos.y, size, color, index);
    }

    public void DrawSquare(float posX, float posY, float size, Color color, int index = -1)
    {
        DrawFigure(new FigureData()
        {
            figure    = Figure.Square,
            parameter = new Vector4(posX, posY, size, 0),
            color     = color
        },
        index);
    }

    public void DrawRect(Vector2 min, Vector2 max, Color color, int index = -1)
    {
        DrawRect(min.x, min.y, max.x, max.y, color, index);
    }

    public void DrawRect(float minX, float minY, float maxX, float maxY, Color color, int index = -1)
    {
        DrawFigure(new FigureData()
        {
            figure    = Figure.Rect,
            parameter = new Vector4(minX, minY, maxX, maxY),
            color     = color
        },
        index);
    }

    public void DrawFigure(FigureData figureData, int index = 0)
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