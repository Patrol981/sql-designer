using SFML.Graphics;
using SFML;
using SFML.System;
using SFML.Window;

using SQL_Designer.Gui;
using SQL_Designer.Renderer;

namespace SQL_Designer.Interactable;

public static class Keyboard {
  internal static string RemoveLastCharacter(string text) {
    if(text.Length < 1) {
      return text;
    }
    return text.Remove(text.Length - 1, 1);
  }

  internal static void HandleInputFields(object sender, KeyEventArgs args) {
    var panels = Panels.GetPanels();
    for(short i=0; i<panels.Count; i++) {
      var inputFields = panels[i].GetInputFields();
      for(short j=0; j<inputFields.Count; j++) {
        if(inputFields[j].GetEditable()) {
          switch(args.Code) {
            case SFML.Window.Keyboard.Key.Backspace:
              inputFields[j].SetContent(RemoveLastCharacter(inputFields[j].GetContent()));
              break;
            case SFML.Window.Keyboard.Key.Space:
              inputFields[j].SetContent(inputFields[j].GetContent() + " ");
              break;
            default:
              inputFields[j].SetContent(inputFields[j].GetContent() + args.Code);
              break;
          }
        }
      }
    }
  }

  internal static void HandleInAppShortcuts(object sender, KeyEventArgs args) {
    if(args.Alt) {
      switch(args.Code) {
        case SFML.Window.Keyboard.Key.F:
          var app = (RenderWindow)sender;
          Console.WriteLine(app.GetView());
          // app.Close();
          if(RendererData.GetFullscreen()) {
            // app = new RenderWindow(RendererData.GetWindowedWindow(), "SQL Designer");
          } else {
           //  app = new RenderWindow(RendererData.GetFullscreenWindow(), "SQL Designer");
          }
          break;
        case SFML.Window.Keyboard.Key.P:
          Console.WriteLine("test");
          Panels.AddPanel(new Panel(new Vector2f(250, 500), "New Panel"));
          break;
      }
    }
  }
  public static void OnKeyPressed(object sender, KeyEventArgs args) {
    HandleInputFields(sender, args);
    HandleInAppShortcuts(sender, args);
  }
}