using SFML.Graphics;
using SFML.System;
using SFML.Window;

using SQL_Designer.Gui;
using SQL_Designer.Interactable;

namespace SQL_Designer.Renderer;

public sealed class RendererData {
  private static RendererData? s_instance;
  private static Color s_color;
  private static bool s_fullscreen = false;
  private static VideoMode s_desktopResolution = VideoMode.DesktopMode;
  private static VideoMode s_widnowedResolution = new VideoMode(1200,700);

  public static Color GetColor() {
    return s_color;
  }

  public static bool GetFullscreen() {
    return s_fullscreen;
  }

  public static VideoMode GetWindowedWindow() {
    return s_widnowedResolution;
  }

  public static VideoMode GetFullscreenWindow() {
    return s_desktopResolution;
  }

  public static void SetColor(Color color) {
    s_color = color;
  }

  public static void SetFullscreen(bool fullscreen) {
    s_fullscreen = fullscreen;
  }

  public static RendererData GetInstance() {
    if(s_instance == null) {
      s_instance = new RendererData();
    }
    return s_instance;
  }
}

public class Renderer: IRenderer {
  private readonly RenderWindow _app;

  // IPanel _panel = new Panel(new Vector2f(250,500), "Test");
  public Renderer(RenderWindow app) {
    _app = app;

    RendererData.GetInstance();
    RendererData.SetColor(new Color(35,35,35));

    GuiTextData.GetInstance();
  }
  public void Update() {
    _app.Clear(RendererData.GetColor());
    Panels.Update(_app);
  }
}