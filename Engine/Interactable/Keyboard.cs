using SFML.Graphics;
using SFML;
using SFML.System;
using SFML.Window;

using SQL_Designer.Gui;
using SQL_Designer.Renderer;
using SQL_DESIGNER.Globals;
using SQL_DESIGNER;
using SQL_DESIGNER.SaveSystem;

namespace SQL_Designer.Interactable;

public class Keyboard {
  private readonly App _app;

  public Keyboard(App app) {
    _app = app;
  }

  internal static string RemoveLastCharacter(string text) {
    if (text.Length < 1) {
      return text;
    }
    return text.Remove(text.Length - 1, 1);
  }

  internal static string ToLowerCase(string text) {
    return text.ToLower();
  }

  internal static string ToUpperCase(string text) {
    return text.ToUpper();
  }

  internal void HandleInputFields(object sender, KeyEventArgs args) {
    var panels = _app.Panels;
    var panelsEditable = _app.Panels.Where(x => x.GetEditable() == true);
    foreach (var p in panelsEditable) {
      switch (args.Code) {
        case SFML.Window.Keyboard.Key.Backspace:
          p.SetTitle(RemoveLastCharacter(p.GetTitle()));
          break;
        case SFML.Window.Keyboard.Key.RShift:
          break;
        case SFML.Window.Keyboard.Key.LShift:
          break;
        case SFML.Window.Keyboard.Key.Space:
          p.SetTitle(p.GetTitle() + " ");
          break;
        default:
          if (args.Control || args.Alt) break;
          if (args.Shift) {
            p.SetTitle(p.GetTitle() + ToUpperCase(args.Code.ToString()));
          } else {
            p.SetTitle(p.GetTitle() + ToLowerCase(args.Code.ToString()));
          }
          break;
      }
    }

    for (short i = 0; i < panels.Count; i++) {
      var inputFields = panels[i].GetInputFields();
      for (short j = 0; j < inputFields.Count; j++) {
        if (inputFields[j].GetEditable()) {
          switch (args.Code) {
            case SFML.Window.Keyboard.Key.Backspace:
              inputFields[j].SetContent(RemoveLastCharacter(inputFields[j].GetContent()));
              break;
            case SFML.Window.Keyboard.Key.Space:
              inputFields[j].SetContent(inputFields[j].GetContent() + " ");
              break;
            case SFML.Window.Keyboard.Key.RShift:
              break;
            case SFML.Window.Keyboard.Key.LShift:
              break;
            default:
              if (args.Shift) {
                inputFields[j].SetContent(inputFields[j].GetContent() + ToUpperCase(args.Code.ToString()));
              } else {
                inputFields[j].SetContent(inputFields[j].GetContent() + ToLowerCase(args.Code.ToString()));
              }

              break;
          }
          break;
        }
      }
    }
  }

  internal void HandleInAppShortcuts(object sender, KeyEventArgs args) {
    if (args.Control) {
      switch (args.Code) {
        case SFML.Window.Keyboard.Key.E:
          SqlExporter.ExportToSql(_app);
          break;
        default:
          break;
      }
    }
    if (args.Alt) {
      if (args.Code == SFML.Window.Keyboard.Key.A) {
        _app.CameraPos = new Vector2f(_app.CameraPos.X - 15f, _app.CameraPos.Y);
      }
      if (args.Code == SFML.Window.Keyboard.Key.D) {
        _app.CameraPos = new Vector2f(_app.CameraPos.X + 15f, _app.CameraPos.Y);
      }
      if (args.Code == SFML.Window.Keyboard.Key.W) {
        _app.CameraPos = new Vector2f(_app.CameraPos.X, _app.CameraPos.Y - 15f);
      }
      if (args.Code == SFML.Window.Keyboard.Key.S) {
        _app.CameraPos = new Vector2f(_app.CameraPos.X, _app.CameraPos.Y + 15f);
      }
      switch (args.Code) {
        case SFML.Window.Keyboard.Key.F:
          // var app = (RenderWindow)sender;
          // Console.WriteLine(app.GetView());
          // app.Close();
          var panels = _app.Panels.Where(x => x.GetEditable() == true);
          foreach (var p in panels) {
            p.AddInputField();
          }
          break;
        case SFML.Window.Keyboard.Key.R:
          var panelsToRemove = _app.Panels.Where(x => x.GetEditable() == true);
          foreach (var p in panelsToRemove) {
            p.RemoveLastInputField();
          }
          break;
        case SFML.Window.Keyboard.Key.P:
          _app.AddPanel(new Panel(_app, new Vector2f(250, 500), "New Panel"));
          break;
      }
    }
  }
  public void OnKeyPressed(object sender, KeyEventArgs args) {
    HandleInputFields(sender, args);
    HandleInAppShortcuts(sender, args);
  }
}