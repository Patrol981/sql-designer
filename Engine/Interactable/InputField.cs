using SFML.Graphics;
using SFML.System;

using SQL_Designer.Gui;

namespace SQL_Designer.Interactable;

public class InputField: IInput {
  private string _content = "";
  private string _placeholder = "Enter field name...";
  private bool _editable = false;
  private RectangleShape _rect = new RectangleShape(new Vector2f(0, 0));
  private GuiText _displayText = new GuiText();
  private Guid _guid = Guid.NewGuid();
  public InputField() {
    _rect.FillColor = GuiTextData.GetInputStandardColor();
  }

  public InputField(string content) {
    _rect.FillColor = GuiTextData.GetInputStandardColor();
    _content = content;
  }

  public string GetContent() {
    return _content;
  }

  public Vector2f GetInputPositon() {
    return _rect.Position;
  }

  public GuiText GetGuiText() {
    return _displayText;
  }

  public RectangleShape GetRectangle() {
    return _rect;
  }

  public bool GetEditable() {
    return _editable;
  }

  public Guid GetGuid() {
    return _guid;
  }

  public void SetContent(string content) {
    _content = content;
  }

  public void SetInputPosition(Vector2f position) {
    _rect.Position = position;
    _displayText.SetPosition(_rect.Position);
  }

  public void SetResponsive(RectangleShape parent) {
    _rect.Size = new Vector2f(parent.Size.X - 30, 35);
    uint fontSize = (uint)(_rect.Size.X / 10);
    _displayText.SetFontSize(fontSize);
    _displayText.GetTextObject().Scale = new Vector2f(0.5f, 0.5f);
    _displayText.SetPosition(new Vector2f(_displayText.GetPositon().X + 5, _displayText.GetPositon().Y + 15));
  }

  public void SetEditable(bool editable) {
    _editable = editable;
  }

  public void Update() {
    if(_content != "") {
      _displayText.SetText(_content);
    } else {
      _displayText.SetText(_placeholder);
    }
    if(_editable) {
      _rect.FillColor = GuiTextData.GetSelectedColor();
    } else {
      _rect.FillColor = GuiTextData.GetInputStandardColor();
    }
  }
}