using System;

using SFML.System;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;

using SQL_Designer.Gui;

namespace SQL_Designer.Interactable;

internal class ClickData {
  internal IPanel? Panel;
  internal InputField? InputField;
  internal TypeClicked TypeClicked;
  internal MouseButtonEventArgs? MouseButtonEventArgs;
}

public static class Mouse {
  internal static ClickData DefineWhatWasClicked(object sender, EventArgs args) {
    ClickData clickData = new ClickData();
    clickData.MouseButtonEventArgs = (MouseButtonEventArgs)args;
    var panels = Panels.GetPanels();
    for(short i=0; i<panels.Count; i++) {
      if(clickData.TypeClicked != TypeClicked.None) break;
      var inputFields = panels[i].GetInputFields();
      for(short j=0; j<inputFields.Count; j++) {
        var inputBounds = inputFields[j].GetRectangle().GetGlobalBounds();
        if(inputBounds.Contains(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y)) {
          clickData.InputField = inputFields[j];
          clickData.TypeClicked = TypeClicked.InputField;
          break;
        }
      }
      if(clickData.TypeClicked != TypeClicked.None) break;
      var bounds = panels[i].GetRectangle().GetGlobalBounds();
      if(bounds.Contains(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y)) {
        clickData.Panel = panels[i];
        clickData.TypeClicked = TypeClicked.Panel;
        break;
      }
    }
    return clickData;
  }

  internal static void SwitchDraggableStateFromPanel(IPanel panel) {
    if(panel.GetDraggable()) {
      panel.SetDraggable(false);
    } else {
      panel.SetDraggable(true);
    }
  }

  internal static void SwitchResizableStateFromPanel(IPanel panel) {
    if(panel.GetResizable()) {
      panel.SetResizable(false);
    } else {
      panel.SetResizable(true);
    }
  }

  internal static void PanelLeftClick(IPanel panel) {
    Console.WriteLine(panel.GetGuid());
  }

  internal static void InputFieldLeftClick(InputField inputField) {
    Console.WriteLine(inputField.GetGuid());
    // set the input string editable
    if(inputField.GetEditable()) {
      inputField.SetEditable(false);
    } else {
      inputField.SetEditable(true);
    }
  }
  public static void OnClick(object sender, EventArgs args) {
    ClickData clickData = DefineWhatWasClicked(sender, args);
    switch(clickData.TypeClicked) {
      case TypeClicked.Panel:
        switch(clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Middle:
            SwitchDraggableStateFromPanel(clickData.Panel!);
            break;
          case SFML.Window.Mouse.Button.Left:
            PanelLeftClick(clickData.Panel!);
            break;
          case SFML.Window.Mouse.Button.Right:
            SwitchResizableStateFromPanel(clickData.Panel!);
            break;
        }
        break;
      case TypeClicked.InputField:
        switch(clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Middle:
            break;
          case SFML.Window.Mouse.Button.Left:
            InputFieldLeftClick(clickData.InputField!);
            break;
          case SFML.Window.Mouse.Button.Right:
            break;
        }
        break;
    }
  }
}