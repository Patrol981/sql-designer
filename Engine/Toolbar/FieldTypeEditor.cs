using SQL_Designer.Gui;
using SQL_Designer.Interactable;

using SFML.Graphics;
using SFML.System;

namespace SQL_Designer.Toolbar;
public class FieldTypeEditor {
  private RectangleShape _rect = new RectangleShape(new Vector2f(250, 300));
  private bool _isVisible;
  private InputField? _referenceInputField;
  private List<GuiText> _fieldTypes = new List<GuiText>();

  public readonly Vector2f BaseSize = new Vector2f(250, 300);

  public FieldTypeEditor() {
    _rect.FillColor = new Color(120, 120, 120);

    for (int i = 0; i < Enum.GetNames(typeof(FieldType)).Length; i++) {
      var stringValue = Enum.GetName(typeof(FieldType), i);
      var guiText = new GuiText();
      guiText.SetText(stringValue!);
      _fieldTypes.Add(guiText);
    }
  }

  public bool GetVisible() {
    return _isVisible;
  }

  public InputField GetReferenceInputField() {
    if (_referenceInputField == null) {
      throw new Exception("Object reference not set to an instance of an object.");
    }
    return _referenceInputField;
  }

  public void SetVisible(bool visible) {
    _isVisible = visible;
  }

  public void SetReferenceInputHandler(InputField inputField) {
    _referenceInputField = inputField;
  }

  public void Spawn(Vector2f destination) {
    _rect.Position = new Vector2f(destination.X + 30, destination.Y);
  }

  public void Update(RenderWindow app) {
    if (_isVisible) {
      app.Draw(_rect);

      var offsetBase = _fieldTypes[0].GetTextObject().CharacterSize;
      var factor = (_rect.Size.Y / BaseSize.Y);
      var targetSize = (offsetBase * factor) + (_rect.Size.Y / 10) / 2.5f;

      for (int i = 0; i < _fieldTypes.Count; i++) {
        _fieldTypes[i].SetPosition(new Vector2f(
          _rect.Position.X,

          _rect.Position.Y + (targetSize * (i + 1))
        ));

        app.Draw(_fieldTypes[i].GetTextObject());
      }
    }
  }

  public List<GuiText> FieldTypes => _fieldTypes;

}