namespace SHARP.Editor.Manager;

public class EditorObjectManager
{
    private static readonly Lazy<EditorObjectManager> _lazy = new(() => new EditorObjectManager());
    public static EditorObjectManager Instance => _lazy.Value;
    private EditorObjectManager() {}
    
    private List<EditorObject> _objects = new();
    
    public List<EditorObject> GetObjects() => _objects;
    public EditorObject GetObjectById(string id) => _objects.Find(obj => obj.GetName() == id) ?? null;
    
    public void AddObject(EditorObject editorObject)
    {
        _objects.Add(editorObject);
    }
    
    public void RemoveObject(EditorObject editorObject)
    {
        _objects.Remove(editorObject);
    }
    
    public bool Initialize()
    {
        foreach (EditorObject editorObject in _objects)
        {
            if (!editorObject.Initialize())
            {
                return false;
            }
        }
        return true;
    }
    
    public void Update()
    {
        foreach (EditorObject editorObject in _objects)
        {
            editorObject.Update();
        }
    }
}