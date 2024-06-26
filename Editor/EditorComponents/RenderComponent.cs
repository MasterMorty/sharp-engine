using SFML.Graphics;
using SHARP.Editor.Manager;

namespace SHARP.Editor.EditorComponents;

public abstract class RenderComponent(EditorObject editorObject, RenderWindow window) : EditorComponent(editorObject)
{
    protected readonly RenderWindow Window = window;
    private int _renderPriority = 0;
    public void SetRenderPriority(int renderPriority) => _renderPriority = renderPriority;
    public int GetRenderPriority() => _renderPriority;
    public abstract void Draw();
}