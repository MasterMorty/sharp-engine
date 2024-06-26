using System.ComponentModel;
using SFML.Graphics;
using SFML.System;
using SHARP.Editor.EditorComponents;
using SHARP.Editor.Manager;

namespace SHARP.Editor;

public class EditorObject : Transformable
{
    public EditorObject(string name)
    {
        this.name = name;
        EditorObjectManager.Instance.AddObject(this);
    }
    
    private string name;
    
    public string GetName() => name;
    public string SetName(string newName) => name = newName;
    
    private List<EditorComponent> _components = new();
    private List<EditorObject> _children = new();
    private EditorObject? _parent = null;
    
    private Vector2f _localPosition = new(0,0);
    private Vector2f _worldPosition = new(0,0);
    
    public Vector2f LocalPosition
    {
        get => _localPosition;
        set => _localPosition = value;
    }
    public Vector2f WorldPosition
    {
        get => _worldPosition;
        set
        {
            if (_parent != null)
            {
                _localPosition = value - _parent._worldPosition;
                _worldPosition = _parent._worldPosition + _localPosition;
                Position = _worldPosition;
            }
            else
            {
                _worldPosition = value;
                Position = _worldPosition;
            }
        }
    }
    
    // maybe something better than this
    private Vector2f _lastPosition = new(0,0);
    
    public T AddComponent<T>(params object[] args) where T : class
    {
        var obj = (T)Activator.CreateInstance(typeof(T), args);
        
        if (obj is EditorComponent component)
        {
            _components.Add(component);
            return obj;
        }
        
        return null;
    }
    
    public T? GetComponent<T>() where T : class
    {
        foreach (EditorComponent component in _components)
        {
            if (component is T obj)
            {
                return obj;
            }
        }
        return null;
    }
    
    public void RemoveComponent<T>() where T : EditorComponent
    {
        _components.RemoveAll(component => component is T);
    }
    
    public void AddChild(EditorObject child)
    {
        child._parent = this;
        _children.Add(child);
    }
    
    public void RemoveChild(EditorObject child)
    {
        _children.Remove(child);
    }
    
    // maybe something better than this - complete structure that uses render and position nodes
    public void Update()
    {
        bool positionChanged = false;
        
        if (_lastPosition != _worldPosition)
        {
            _lastPosition = _worldPosition;
            positionChanged = true;
        }
        
        foreach (EditorComponent component in _components)
        {
            component.Update();
            if (positionChanged) component.UpdatePosition(_worldPosition);
        }

        if (positionChanged)
        {
            foreach (var child in _children)
            {
                child.WorldPosition = _worldPosition + child._localPosition;
            }
        }
    }
    
    public bool Initialize()
    {
        _worldPosition = Position + _localPosition;
        Position = _worldPosition;
        
        foreach (EditorComponent component in _components)
        {
            if (!component.Initialize())
            {
                return false;
            }
        }
        return true;
    }
}