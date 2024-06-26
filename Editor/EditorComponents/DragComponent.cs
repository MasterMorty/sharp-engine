using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SHARP.Editor.EditorComponents;

public class DragComponent(EditorObject editorObject, RenderWindow window) : EditorComponent(editorObject)
{
    private bool _isDragging = false;
    private bool _firstClick = false;
    private Vector2f _offset;
    private Vector2i _mousePosition;

    private Cursor grabCursor = new Cursor(Cursor.CursorType.Hand);
    private Cursor normalCursor = new Cursor(Cursor.CursorType.Arrow);

    
    private RectangleShape _fillRenderComponentRectangle;
    public override bool Initialize()
    {
        var fillRenderComponent = editorObject.GetComponent<FillRenderComponent>();

        if (fillRenderComponent == null)
        {
            Console.WriteLine("Cannot init DragComponent without FillRenderComponent!");
            return false;
        }
        
        _fillRenderComponentRectangle = fillRenderComponent.GetRectangleShape();
        
        return true;
    }

    // TODO: keep the renderpriority in mind so that only first object is dragged
    public override void Update()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            _mousePosition = Mouse.GetPosition(window);
            if (IsMouseOver(_mousePosition))
            {
                _isDragging = true;
                window.SetMouseCursorGrabbed(true);
                window.SetMouseCursor(grabCursor);
            }
        }
        else
        {
            _isDragging = false;
            _firstClick = false;
            window.SetMouseCursorGrabbed(false);
            window.SetMouseCursor(normalCursor);
        }
        
        if (_isDragging)
        {
            if (!_firstClick)
            {
                _firstClick = true;
                _offset = (Vector2f)_mousePosition - _fillRenderComponentRectangle.Position;
            }
            
            GetEditorObject().WorldPosition = (Vector2f)_mousePosition - _offset;
        }
    }
    
    private bool IsMouseOver(Vector2i mousePosition)
    {
        return _fillRenderComponentRectangle.GetGlobalBounds().Contains(mousePosition);
    }
}