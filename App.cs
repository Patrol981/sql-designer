using SFML.Graphics;
using SFML.Window;

using SQL_Designer.Renderer;
using SQL_Designer.Interactable;
using SQL_DESIGNER.Globals;
using SQL_Designer.Gui;
using SFML.System;

namespace SQL_DESIGNER;
public class App {
  private readonly IRenderer _renderer = null!;
  private readonly RenderWindow _renderWindow = null!;
  private readonly SQL_Designer.Interactable.Mouse _mouse = null!;
  private readonly SQL_Designer.Interactable.Keyboard _keyboard = null!;

  private Vector2u _currentScreenSize = new Vector2u(0, 0);
  private Vector2u _baseScreenSize = new Vector2u(0, 0);
  private Vector2i _mousePos = new Vector2i(0, 0);
  private Vector2f _cameraPos = new Vector2f(0, 0);
  private List<IPanel> _panels = new List<IPanel>();

  public App() {
    _renderWindow = new RenderWindow(RendererData.GetWindowedWindow(), "SQL Designer");
    _renderWindow.SetFramerateLimit(0); // SFML.NET drops performance without it
    _renderWindow.SetVerticalSyncEnabled(true); // SFML.NET drops performance without it
    _renderWindow.SetIcon(512, 512, ImageToByteArray("./Resources/Icons/logo.png"));

    _renderer = new Renderer(_renderWindow);
    _mouse = new SQL_Designer.Interactable.Mouse(this);
    _keyboard = new SQL_Designer.Interactable.Keyboard(this);

    _renderWindow.Closed += new EventHandler(OnClose!);
    _renderWindow.MouseButtonPressed += new EventHandler<SFML.Window.MouseButtonEventArgs>(_mouse.OnClick!);
    _renderWindow.KeyPressed += new EventHandler<SFML.Window.KeyEventArgs>(_keyboard.OnKeyPressed!);
    _renderWindow.Resized += new EventHandler<SFML.Window.SizeEventArgs>(OnResize!);

    _baseScreenSize = _renderWindow.Size;
  }

  public void Run() {
    while (_renderWindow.IsOpen) {
      _renderWindow.DispatchEvents();
      _renderer.Update(_panels);
      _renderWindow.Display();
    }
  }

  private void OnResize(object sender, EventArgs e) {
    var args = (SizeEventArgs)e;
    _currentScreenSize = new Vector2u(args.Width, args.Height);
  }

  public void AddPanel(IPanel panel) {
    var canBeAdded = _panels.Where(x => x.GetGuid() == panel.GetGuid()).FirstOrDefault();
    if (canBeAdded != null) return;
    _panels.Add(panel);
  }

  public List<IPanel> Panels {
    get { return _panels; }
  }

  public Vector2i MousePos {
    get { return _mousePos; }
    set { _mousePos = value; }
  }

  public Vector2f CameraPos {
    get { return _cameraPos; }
    set { _cameraPos = value; }
  }

  public IRenderer Renderer => _renderer;
  public RenderWindow RenderWindow => _renderWindow;
  public Vector2u ScreenSize => _currentScreenSize;
  public Vector2u BaseSize => _baseScreenSize;

  static void OnClose(object sender, EventArgs e) {
    RenderWindow window = (RenderWindow)sender;
    window.Close();
  }

  static Byte[] ImageToByteArray(string path) {
    Texture texture = new Texture(path);
    Image image = texture.CopyToImage();
    return image.Pixels;
  }
}