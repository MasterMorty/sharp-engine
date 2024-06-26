using SFML.Graphics;
using SFML.System;
using SHARP.Editor.Manager;

namespace SHARP.Editor.EditorComponents;

public class FillRenderComponent(EditorObject editorObject, RenderWindow window, Vector2f size) : RenderComponent(editorObject, window)
{
    private RectangleShape _rectangleShape;
    private Vector2f _localPosition = new(0, 0);
    private Vector2f _globalPosition = new(0, 0);
    private Color _fillColor = Color.White;
    private Color _outlineColor = Color.Transparent;
    private float _outlineThickness = 0;
    
    public void SetSize(Vector2f newSize) => size = newSize;
    public void SetLocalPosition(Vector2f localPosition) => _localPosition = localPosition;
    public void SetFillColor(Color fillColor) => _fillColor = fillColor;
    public void SetOutlineColor(Color outlineColor) => _outlineColor = outlineColor;
    public void SetOutlineThickness(float outlineThickness) => _outlineThickness = outlineThickness;
    
    public Vector2f GetSize() => size;
    public Vector2f GetLocalPosition() => _localPosition;
    public Vector2f GetGlobalPosition() => _globalPosition;
    public RectangleShape GetRectangleShape() => _rectangleShape;

    public override bool Initialize()
    {
        _rectangleShape = new RectangleShape(size)
        {
            Position = GetEditorObject().Position,
            FillColor = _fillColor,
            OutlineColor = _outlineColor,
            OutlineThickness = _outlineThickness
        };

        EditorRenderManager.Instance.AddRenderComponent(this);
        
        return true;
    }

    public override void Draw()
    {
        Window.Draw(_rectangleShape);
    }
    
    public override void UpdatePosition(Vector2f globalPosition)
    {
        _globalPosition = globalPosition + _localPosition;
        _rectangleShape.Position = _globalPosition;
    }
}