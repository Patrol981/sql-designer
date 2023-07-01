using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

using SFML.Window;

namespace SQL_DESIGNER.Globals;
public sealed class RendererData {
  private static RendererData? s_instance;
  private static Color s_color;
  private static bool s_fullscreen = false;
  private static VideoMode s_desktopResolution = VideoMode.DesktopMode;
  private static VideoMode s_widnowedResolution = new VideoMode(1200, 700);

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
    if (s_instance == null) {
      s_instance = new RendererData();
    }
    return s_instance;
  }
}
