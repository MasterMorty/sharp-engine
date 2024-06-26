using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SHARP.Editor.EditorComponents;
using SHARP.Editor.Manager;

namespace SHARP.Editor;

public class Editor
{
    private readonly RenderWindow _window;
    
    private EditorObjectManager _editorObjectManager = EditorObjectManager.Instance;
    private EditorRenderManager _editorRenderManager = EditorRenderManager.Instance;
    public Editor()
    {
        InitFolders();
        
        VideoMode videoMode = new(1920, 1080);
        const string title = "SHARP Editor";
        _window = new RenderWindow(videoMode, title);
        _window.SetVerticalSyncEnabled(false);

        _window.Closed += CloseEditor;
        _window.KeyPressed += KeyPressedHandler;
        _window.Resized += ResizeWindow;

        // View
        View view = new();
        view.Size = new Vector2f(_window.Size.X, _window.Size.Y);
        view.Center = new Vector2f(_window.Size.X / 2, _window.Size.Y / 2);
        _window.SetView(view);

        EditorAssetManager.Instance.Initialize();
    }

    public void Run()
    {
        if (!Initialize()) return;
        
        while (_window.IsOpen)
        {
            _window.DispatchEvents();
            SharpTime.Update();

            Update();
            Draw();
        }
    }
    
    private bool Initialize()
    {
        InitObjects();
        if (!_editorObjectManager.Initialize()) return false;
        
        return true;
    }
    
    private void InitFolders()
    {
        CopyDirectory("../../../Editor/Assets", "./Assets");
    }

    private void InitObjects()
    {
        //FPS
        EditorObject fps = new("FPS");
        
        //cheap right align TODO: Remove later
        fps.WorldPosition = new Vector2f(_window.Size.X - 120, 0);
        
        var fpsTextRenderComponent = fps.AddComponent<TextRenderComponent>(fps, _window);
        fpsTextRenderComponent.SetRenderPriority(1);
        fpsTextRenderComponent.SetLocalPosition(new Vector2f(10, 10));
        fpsTextRenderComponent.SetSize(20);
        
        var fpsFillRenderComponent = fps.AddComponent<FillRenderComponent>(fps, _window, new Vector2f(120, 40));
        fpsFillRenderComponent.SetFillColor(new Color(23, 23, 23));
        
    }

    private void Draw()
    {
        _window.Clear();
        _editorRenderManager.Draw();
        _window.Display();
    }

    private void Update()
    {
        _editorObjectManager.Update();
        
        // Only for testing
        _editorObjectManager.GetObjectById("FPS").GetComponent<TextRenderComponent>()?.SetText($"FPS: {SharpTime.GetFps()}");
    }
    
    private void ResizeWindow(object? sender, SizeEventArgs e)
    {
        View view = _window.GetView();
        view.Size = new Vector2f(e.Width, e.Height);
        view.Center = new Vector2f(e.Width / 2, e.Height / 2);
        _window.SetView(view);
    }

    private void KeyPressedHandler(object? sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.Escape)
        {
            _window.Close();
        }
    }

    private void CloseEditor(object? sender, EventArgs e)
    {
        _window.Close();
    }

    private static void CopyDirectory(string sourceDir, string destDir, bool overwrite = true)
    {
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
        }

        if (Directory.Exists(destDir) && overwrite)
        {
            Directory.Delete(destDir, true);
        }

        Directory.CreateDirectory(destDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFileName = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFileName, true);
        }

        foreach (var subDir in Directory.GetDirectories(sourceDir))
        {
            var destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
            CopyDirectory(subDir, destSubDir, overwrite);
        }
    }
}