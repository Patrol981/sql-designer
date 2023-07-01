using System;

using SFML.System;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;

using SQL_Designer.Gui;
using SQL_Designer.Toolbar;
using SQL_Designer.Renderer;
using SQL_DESIGNER;

namespace SQL_Designer.Interactable;

internal class ClickData {
  internal IPanel? Panel;
  internal InputField? InputField;
  internal GuiText? InputType;
  internal GuiText? InputInfo;
  internal TypeClicked TypeClicked;
  internal MouseButtonEventArgs? MouseButtonEventArgs;
}

public class Mouse {
  private readonly App _app;

  public Mouse(App app) {
    _app = app;
  }

  internal ClickData DefineWhatWasClicked(object sender, EventArgs args) {
    ClickData clickData = new ClickData();
    clickData.MouseButtonEventArgs = (MouseButtonEventArgs)args;
    var clickPanelX = clickData.MouseButtonEventArgs.X - (int)_app.CameraPos.X;
    var clickPanelY = clickData.MouseButtonEventArgs.Y - (int)_app.CameraPos.Y;
    var panels = _app.Panels;

    var typesField = _app.Renderer.FieldTypeEditor;
    for (short i = 0; i < typesField.FieldTypes.Count; i++) {
      var bounds = typesField.FieldTypes[i].GetTextObject().GetGlobalBounds();
      if (bounds.Contains(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y)) {
        clickData.TypeClicked = TypeClicked.InputType;
        clickData.InputType = typesField.FieldTypes[i];
      }
    }

    for (short i = 0; i < panels.Count; i++) {
      if (clickData.TypeClicked != TypeClicked.None) break;
      var inputFields = panels[i].GetInputFields();
      for (short j = 0; j < inputFields.Count; j++) {
        var inputBounds = inputFields[j].GetInputRectangle().GetGlobalBounds();
        var fieldTypeBounds = inputFields[j].GetFieldTypeRectangle().GetGlobalBounds();

        var fieldInfos = inputFields[j].FieldInfoOptions;
        foreach (var info in fieldInfos) {
          var infoBounds = info.GetTextObject().GetGlobalBounds();
          if (infoBounds.Contains(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y)) {
            clickData.TypeClicked = TypeClicked.FieldInfo;
            clickData.InputField = inputFields[j];
            clickData.InputInfo = info;
            break;
          }
        }

        if (inputBounds.Contains(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y)) {
          clickData.InputField = inputFields[j];
          clickData.TypeClicked = TypeClicked.InputField;
          break;
        } else if (fieldTypeBounds.Contains(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y)) {
          clickData.InputField = inputFields[j];
          clickData.TypeClicked = TypeClicked.TypeField;
          break;
        }
      }
      if (clickData.TypeClicked != TypeClicked.None) break;
      var bounds = panels[i].GetRectangle().GetGlobalBounds();
      if (bounds.Contains(clickPanelX, clickPanelY)) {
        clickData.Panel = panels[i];
        clickData.TypeClicked = TypeClicked.Panel;
        break;
      }
    }
    return clickData;
  }

  internal static void SwitchDraggableStateFromPanel(IPanel panel) {
    if (panel.GetDraggable()) {
      panel.SetDraggable(false);
    } else {
      panel.SetDraggable(true);
    }
  }

  internal void SetDraggableStateInPanels(bool value) {
    foreach (var panel in _app.Panels) {
      panel.SetDraggable(value);
    }
  }

  internal static void SwitchResizableStateFromPanel(IPanel panel) {
    if (panel.GetResizable()) {
      panel.SetResizable(false);
    } else {
      panel.SetResizable(true);
    }
  }

  internal void SetResizableStateInPanels(bool value) {
    foreach (var panel in _app.Panels) {
      panel.SetResizable(value);
    }
  }

  internal static void PanelLeftClick(IPanel panel) {
    panel.SetEditable(!panel.GetEditable());
    Console.WriteLine(panel.GetGuid());
  }

  internal static void InputFieldLeftClick(InputField inputField) {
    Console.WriteLine(inputField.GetGuid());
    // set the input string editable
    if (inputField.GetEditable()) {
      inputField.SetEditable(false);
    } else {
      inputField.SetEditable(true);
    }
  }

  internal static void HandleFieldInfo(InputField inputField, GuiText option) {
    var optionValue = option.GetText();

    switch (optionValue) {
      case "I":
        inputField.FieldInfo.AutoIncrement = !inputField.FieldInfo.AutoIncrement;
        if (inputField.FieldInfo.AutoIncrement) {
          option.SetColor(GuiTextData.GetSelectedColor());
        } else {
          option.SetColor(Color.White);
        }
        break;
      case "N":
        inputField.FieldInfo.NotNull = !inputField.FieldInfo.NotNull;
        if (inputField.FieldInfo.NotNull) {
          option.SetColor(GuiTextData.GetSelectedColor());
        } else {
          option.SetColor(Color.White);
        }
        break;
      case "P":
        inputField.FieldInfo.PrimaryKey = !inputField.FieldInfo.PrimaryKey;
        if (inputField.FieldInfo.PrimaryKey) {
          option.SetColor(GuiTextData.GetSelectedColor());
        } else {
          option.SetColor(Color.White);
        }
        break;
    }
  }

  internal void SetFieldTypeEditorVisibility(Vector2f mousePos, InputField inputField) {
    if (_app.Renderer.FieldTypeEditor.GetVisible()) {
      _app.Renderer.FieldTypeEditor.SetVisible(false);
    } else {
      _app.Renderer.FieldTypeEditor.SetVisible(true);
      _app.Renderer.FieldTypeEditor.Spawn(mousePos);
      _app.Renderer.FieldTypeEditor.SetReferenceInputHandler(inputField);
    }
  }
  public void OnClick(object sender, EventArgs args) {
    ClickData clickData = DefineWhatWasClicked(sender, args);
    Console.WriteLine(clickData.TypeClicked);
    switch (clickData.TypeClicked) {
      case TypeClicked.Panel:
        switch (clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Middle:
            SwitchResizableStateFromPanel(clickData.Panel!);
            break;
          case SFML.Window.Mouse.Button.Left:
            PanelLeftClick(clickData.Panel!);
            break;
          case SFML.Window.Mouse.Button.Right:
            SwitchDraggableStateFromPanel(clickData.Panel!);
            break;
        }
        break;
      case TypeClicked.InputField:
        switch (clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Middle:
            break;
          case SFML.Window.Mouse.Button.Left:
            InputFieldLeftClick(clickData.InputField!);
            break;
          case SFML.Window.Mouse.Button.Right:
            break;
        }
        break;
      case TypeClicked.TypeField:
        switch (clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Middle:
            break;
          case SFML.Window.Mouse.Button.Left:
            SetFieldTypeEditorVisibility(
              new Vector2f(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y),
              clickData.InputField!);
            break;
          case SFML.Window.Mouse.Button.Right:
            break;
        }
        break;
      case TypeClicked.None:
        switch (clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Right:
            _app.MousePos = new Vector2i(clickData.MouseButtonEventArgs.X, clickData.MouseButtonEventArgs.Y);
            SetDraggableStateInPanels(false);
            break;
          case SFML.Window.Mouse.Button.Middle:
            SetResizableStateInPanels(false);
            break;
        }
        break;

      case TypeClicked.InputType:
        switch (clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Left:
            var targetText = clickData.InputType!.GetText();
            Enum.TryParse(targetText, false, out FieldType enumType);
            _app.Renderer.FieldTypeEditor.GetReferenceInputField().FieldType = enumType;
            _app.Renderer.FieldTypeEditor.SetVisible(false);
            break;
        }
        break;

      case TypeClicked.FieldInfo:
        switch (clickData.MouseButtonEventArgs!.Button) {
          case SFML.Window.Mouse.Button.Left:
            HandleFieldInfo(clickData.InputField!, clickData.InputInfo!);
            break;
        }
        break;
    }
  }
}