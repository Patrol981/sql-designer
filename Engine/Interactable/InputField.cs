using System.Net.NetworkInformation;

using SFML.Graphics;
using SFML.System;

using SQL_Designer.Gui;

namespace SQL_Designer.Interactable;

public enum FieldType {
  Char,
  VarChar,
  Text,
  TinyText,
  MediumText,
  LongText,
  Boolean,
  Integer,
  Float,
  Double,
  Decimal
}

public class FieldInfo {
  public bool AutoIncrement { get; set; }
  public bool PrimaryKey { get; set; }
  public bool NotNull { get; set; }
}

public class InputField : IInput {
  private string _content = "";
  private string _placeholder = "Enter field name...";
  private FieldType _fieldType = FieldType.Text;
  private int _fieldLength = 255;
  private bool _editable = false;
  private bool _showFieldTpyeInfo = false;
  private FieldInfo _fieldOptions = new();
  private RectangleShape _inputFieldRect = new RectangleShape(new Vector2f(0, 0));
  private RectangleShape _fieldTypeRect = new RectangleShape(new Vector2f(0, 0));
  private RectangleShape _fieldInfoRect = new RectangleShape(new Vector2f(0, 0));
  private List<GuiText> _fieldInfoOptions = new List<GuiText>();
  private GuiText _displayText = new GuiText();
  private GuiText _fieldTypeText = new GuiText();
  private Guid _guid = Guid.NewGuid();
  public InputField() {
    _inputFieldRect.FillColor = GuiTextData.GetInputStandardColor();
    _fieldTypeRect.FillColor = GuiTextData.GetInputStandardColor();
    _fieldInfoRect.FillColor = GuiTextData.GetInputStandardColor();

    InitInfoOptions();
  }

  public InputField(string content) {
    _inputFieldRect.FillColor = GuiTextData.GetInputStandardColor();
    _fieldTypeRect.FillColor = GuiTextData.GetInputStandardColor();
    _fieldInfoRect.FillColor = GuiTextData.GetInputStandardColor();
    _content = content;

    InitInfoOptions();
  }

  private void InitInfoOptions() {
    var autoIncrement = new GuiText();
    var primaryKey = new GuiText();
    var notNull = new GuiText();

    autoIncrement.SetText("I");
    primaryKey.SetText("P");
    notNull.SetText("N");

    _fieldInfoOptions.Add(autoIncrement);
    _fieldInfoOptions.Add(primaryKey);
    _fieldInfoOptions.Add(notNull);
  }

  public string GetContent() {
    return _content;
  }

  public Vector2f GetInputPositon() {
    return _inputFieldRect.Position;
  }

  public GuiText GetInputText() {
    return _displayText;
  }

  public GuiText GetFieldTypeText() {
    return _fieldTypeText;
  }

  public RectangleShape GetInputRectangle() {
    return _inputFieldRect;
  }

  public RectangleShape GetFieldTypeRectangle() {
    return _fieldTypeRect;
  }

  public RectangleShape GetFieldInfoRectangle() {
    return _fieldInfoRect;
  }

  public bool GetEditable() {
    return _editable;
  }

  public int GetFieldLength() {
    return _fieldLength;
  }

  public bool GetShowFieldTypeInfo() {
    return _showFieldTpyeInfo;
  }

  public Guid GetGuid() {
    return _guid;
  }

  public void SetContent(string content) {
    _content = content;
  }

  public void SetInputPosition(Vector2f position) {
    _inputFieldRect.Position = position;
    // _fieldTypeRect.Position = new Vector2f(position.X + 35, position.Y);
    _displayText.SetPosition(_inputFieldRect.Position);
    _fieldTypeText.SetPosition(_fieldTypeRect.Position);
  }

  public void SetFieldTypePosition(Vector2f position) {
    _fieldTypeRect.Position = position;
  }

  public void SetResponsive(RectangleShape parent) {
    _inputFieldRect.Size = new Vector2f(parent.Size.X - 30, (parent.Size.Y / 10));
    _fieldInfoRect.Size = new Vector2f(parent.Size.X - 175, (parent.Size.Y / 10));

    _fieldTypeRect.Position = new Vector2f(
      _inputFieldRect.Position.X +
      _inputFieldRect.Size.X + 15,
      _inputFieldRect.Position.Y
    );

    _fieldInfoRect.Position = new Vector2f(
      _inputFieldRect.Position.X -
      _inputFieldRect.Size.X + 130,

      _inputFieldRect.Position.Y
    );

    for (int i = 0; i < _fieldInfoOptions.Count; i++) {
      var targetPos = new Vector2f(0, 0);
      var offsetChar = _fieldInfoOptions[i].GetFontSize() * 1.25f;
      if (i % 2 == 0) {
        targetPos.X = (_inputFieldRect.Position.X - _inputFieldRect.Size.X + 140) + offsetChar * i;
        targetPos.Y = _inputFieldRect.Position.Y + 5;
      } else {
        targetPos.X = _inputFieldRect.Position.X - _inputFieldRect.Size.X + 140;
        targetPos.Y = _inputFieldRect.Position.Y + offsetChar + 10;
      }

      _fieldInfoOptions[i].SetPosition(targetPos);
    }

    uint fontSize = (uint)(_inputFieldRect.Size.X / 10);

    _displayText.SetFontSize(fontSize);
    _displayText.GetTextObject().Scale = new Vector2f(0.5f, 0.5f);
    _displayText.SetPosition(new Vector2f(_displayText.GetPositon().X + 5, _displayText.GetPositon().Y + 15));

    _fieldTypeText.SetFontSize(fontSize);
    _fieldTypeText.GetTextObject().Scale = new Vector2f(0.5f, 0.5f);
    _fieldTypeText.SetPosition(new Vector2f(_fieldTypeText.GetPositon().X + 5, _fieldTypeText.GetPositon().Y + 15));
  }

  public void SetEditable(bool editable) {
    _editable = editable;
  }

  public void Update() {
    if (_content != "") {
      _displayText.SetText(_content);
    } else {
      _displayText.SetText(_placeholder);
    }
    if (_editable) {
      _fieldTypeRect.Size = new Vector2f(_inputFieldRect.Size.X / 2, 35);
      _fieldTypeText.SetText(_fieldType.ToString());
      _inputFieldRect.FillColor = GuiTextData.GetSelectedColor();
    } else {
      _fieldTypeRect.Size = new Vector2f(_inputFieldRect.Size.X / 6, 35);
      _fieldTypeText.SetText(_fieldType.ToString().ElementAt(0).ToString());
      _inputFieldRect.FillColor = GuiTextData.GetInputStandardColor();
    }
  }

  public FieldType FieldType {
    get { return _fieldType; }
    set { _fieldType = value; }
  }

  public List<GuiText> FieldInfoOptions => _fieldInfoOptions;
  public FieldInfo FieldInfo => _fieldOptions;
}