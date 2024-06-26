using SFML.Graphics;
using SFML.System;
using SHARP.Editor.Manager;

namespace SHARP.Editor.EditorComponents;

public class TextRenderComponent(EditorObject editorObject, RenderWindow window) : RenderComponent(editorObject, window)
{
    private Text _textComponent = new();
    private EditorAssetManager _assetManager = EditorAssetManager.Instance;
    
    private string _text = "Default Text";
    private string _type = "Poppins-Regular";
    private uint _size = 16;
    private Vector2f _localPosition = new(0, 0);
    private Vector2f _globalPosition = new(0, 0);
    private Color _color = Color.White;

    // TODO: Find something better
    public void SetText(string text)
    {
        _text = text;
        _textComponent.DisplayedString = _text;
    }
    public void SetType(string type) => _type = type;
    public void SetSize(uint size) => _size = size;
    public void SetLocalPosition(Vector2f localPosition) => _localPosition = localPosition;
    public void SetColor(Color color) => _color = color;
    
    public Text GetText() => _textComponent;
    public override bool Initialize()
    {
        _globalPosition = GetEditorObject().Position + _localPosition;
        
        _textComponent = new Text(_text, _assetManager.GetFont(_type), _size);
        _textComponent.Position = _globalPosition;
        _textComponent.FillColor = _color;

        EditorRenderManager.Instance.AddRenderComponent(this);
        
        return true;
    }
    
    public override void Draw()
    {
        Window.Draw(_textComponent);
    }
    public override void Update()
    {
    }
    
    public override void UpdatePosition(Vector2f globalPosition)
    {
        _globalPosition = globalPosition + _localPosition;
        _textComponent.Position = _globalPosition;
    }
}