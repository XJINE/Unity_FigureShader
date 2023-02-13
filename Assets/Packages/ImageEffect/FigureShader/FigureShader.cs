using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteAlways]
public class FigureShader : ImageEffectBase
{
    #region Field

    protected static int FigureDataBufferID = -1;

    public enum Figure
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

    private          ComputeBuffer                     _figureDataBuffer;
    private          List<FigureData>                  _figureDataList;
    private readonly SortedList<int, List<FigureData>> _sortedFigureDataList = new ();

    public bool autoClear = true;

    #endregion Field

    #region Method

    protected virtual void Awake()
    {
        if (FigureDataBufferID < 0)
        {
            FigureDataBufferID = Shader.PropertyToID("_FigureDataBuffer");
        }

        // NOTE:
        // 1000 means enough size to use.
        _figureDataList = new List<FigureData>(1000);
    }

    protected override void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_sortedFigureDataList.Count != 0)
        {
            foreach (var data in _sortedFigureDataList)
            {
                _figureDataList.AddRange(data.Value);
            }

            _figureDataBuffer = new ComputeBuffer(_figureDataList.Count, Marshal.SizeOf(typeof(FigureData)));
            _figureDataBuffer.SetData(_figureDataList);

            material.SetBuffer(FigureDataBufferID, _figureDataBuffer);
        }

        base.OnRenderImage(src, dest);

        if (_figureDataBuffer != null) { _figureDataBuffer    .Release(); }
        if (_figureDataList   != null) { _figureDataList      .Clear();   }
        if (autoClear)                 { _sortedFigureDataList.Clear();   }
    }

    protected void OnDestroy()
    {
        if (_figureDataBuffer != null)
        {
            _figureDataBuffer.Release();
            _figureDataBuffer = null;
        }
    }

    public void Clear()
    {
        if (_sortedFigureDataList != null)
        {
            _sortedFigureDataList.Clear();
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
        if (_sortedFigureDataList.ContainsKey(index))
        {
            _sortedFigureDataList[index].Add(figureData);
        }
        else
        {
            _sortedFigureDataList.Add(index, new List<FigureData>(){ figureData });
        }
    }

    #endregion Method
}