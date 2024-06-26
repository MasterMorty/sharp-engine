using SFML.Graphics;

namespace SHARP.Editor;

public class EditorAssetManager
{
    private static readonly Lazy<EditorAssetManager> _lazy = new(() => new EditorAssetManager());
    
    public static EditorAssetManager Instance => _lazy.Value;
    private EditorAssetManager() {}

    private readonly Dictionary<string, Font> _uiFonts = new();
    
    public Font GetFont(string fontName) => _uiFonts[fontName];
    
    public void Initialize()
    {
        LoadFonts();
    }
    
    // TODO: create enum for fonts
    private void LoadFonts()
    {
        foreach (var font in Directory.GetFiles("./Assets/Fonts/Poppins"))
        {
            var fontName = Path.GetFileNameWithoutExtension(font);
            _uiFonts[fontName] = new Font(font);
        }
    }
    
    // TODO: Load other assets
}