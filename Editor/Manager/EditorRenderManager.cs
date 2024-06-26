using SFML.Graphics;
using SHARP.Editor.EditorComponents;

namespace SHARP.Editor.Manager;

public class EditorRenderManager
{
    private static readonly Lazy<EditorRenderManager> _lazy = new(() => new EditorRenderManager());
    public static EditorRenderManager Instance => _lazy.Value;
    private EditorRenderManager() {}
    
    
    private List<RenderComponent> _renderComponents = new();

    public void AddRenderComponent(RenderComponent renderComponent)
    {
        _renderComponents.Add(renderComponent);
        _renderComponents = _renderComponents.OrderBy(component => component.GetRenderPriority()).ToList();
    }

    public void Draw()
    {
        foreach (RenderComponent renderComponent in _renderComponents)
        {
            renderComponent.Draw();
        }
    }
}