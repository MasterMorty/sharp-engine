using SFML.System;

namespace SHARP.Editor;

public static class SharpTime
{
    // Time
    private static Clock _time = new Clock();
    private static float _deltaTime;
    private static float _totalTime;
    
    public static float DeltaTime => _deltaTime;
    public static float TotalTime => _totalTime;
    
    // FPS
    private static float _fpsAccumulator;
    private static int _frames;
    private static int _fps;

    public static void Restart()
    {
        _time.Restart();
        _deltaTime = 0;
        _totalTime = 0;
    }
    
    public static void Update()
    {
        // Time
        _deltaTime = _time.Restart().AsSeconds();
        _totalTime += _deltaTime;
        
        // FPS
        _fpsAccumulator += _deltaTime;
        
        if (_fpsAccumulator >= 1.0f)
        {
            _fps = _frames;
            _frames = 0;
            _fpsAccumulator -= 1.0f;
        }
        _frames++;
    }

    public static int GetFps()
    {
        return _fps;
    }
}