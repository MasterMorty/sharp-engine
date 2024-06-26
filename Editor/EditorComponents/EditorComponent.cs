using SFML.System;

namespace SHARP.Editor.EditorComponents;

public abstract class EditorComponent(EditorObject editorObject)
{
    protected readonly EditorObject EditorObject = editorObject;
    public virtual bool Initialize() => true;
    public virtual void Update() {}
    public virtual void UpdatePosition(Vector2f position) {}
    
    public EditorObject GetEditorObject() => EditorObject;
}