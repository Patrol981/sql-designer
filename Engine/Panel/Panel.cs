using SFML.Graphics;
using SFML.System;
using SFML.Window;

using System.Threading;

using SQL_Designer.Interactable;
using SQL_DESIGNER;

namespace SQL_Designer.Gui;

public class Panel : IPanel {
  private readonly App _app;

  private RectangleShape _rect = new RectangleShape(new Vector2f(30, 30));
  private GuiText _title = new GuiText();
  private List<InputField> _inputFields = new List<InputField>();
  private Guid _guid = Guid.NewGuid();
  private bool _draggable = false;
  private bool _resizable = false;
  private bool _editable = false;
  private Vector2f _position = new Vector2f(0, 0);

  public readonly Vector2f BaseSize = new Vector2f(250, 500);
  public readonly Vector2f MinSize = new Vector2f(150, 75);
  public readonly Vector2f MaxSize = new Vector2f(500, 500);

  private float _minX = 150;
  private float _minY = 75;

  private readonly float _startMinX = 150;
  private readonly float _startMinY = 75;

  public Panel(App app) {
    _app = app;

    _rect.FillColor = new Color(120, 120, 120);
    _rect.Position = new Vector2f(30, 30);
    _app.Panels.Add(this);
    _inputFields.Add(new InputField());

    Resize(_app.RenderWindow.Size.X, _app.RenderWindow.Size.Y);
  }

  public Panel(App app, Vector2f rectangleSize) {
    _app = app;

    _rect.FillColor = new Color(120, 120, 120);
    _rect.Size = rectangleSize;
    _rect.Position = new Vector2f(30, 30);
    _app.Panels.Add(this);
    _inputFields.Add(new InputField());

    Resize(_app.RenderWindow.Size.X, _app.RenderWindow.Size.Y);
  }

  public Panel(App app, Vector2f rectangleSize, string panelName) {
    _app = app;

    _rect.FillColor = new Color(120, 120, 120);
    _rect.Size = rectangleSize;
    _title.SetText(panelName);
    _rect.Position = new Vector2f(30, 30);
    _title.SetPosition(_rect.Position);
    _app.Panels.Add(this);
    _inputFields.Add(new InputField());
    _inputFields.Add(new InputField());
    _inputFields.Add(new InputField());

    Resize(_app.RenderWindow.Size.X, _app.RenderWindow.Size.Y);
  }

  public void Update(RenderWindow app) {
    if (_draggable) {
      var mousePos = app.MapPixelToCoords(SFML.Window.Mouse.GetPosition(app));
      var offset = _app.CameraPos;
      mousePos.X -= (int)offset.X;
      mousePos.Y -= (int)offset.Y;
      _rect.Position = new Vector2f(mousePos.X - 15, mousePos.Y - 15);
    }

    if (_resizable) {
      var mousePos = app.MapPixelToCoords(SFML.Window.Mouse.GetPosition(app));
      _rect.Size = new Vector2f(mousePos.X + 15, mousePos.Y + 15);
    }

    if (_editable) {
      _rect.FillColor = GuiTextData.GetSelectedColor();
    } else {
      _rect.FillColor = new Color(120, 120, 120);
    }

    if (_rect.Size.X < _minX) {
      _rect.Size = new Vector2f(_minX, _rect.Size.Y);
    }

    if (_rect.Size.Y < _minY) {
      _rect.Size = new Vector2f(_rect.Size.X, _minY);
    }

    _position = _rect.Position;
    _rect.Position = _position + _app.CameraPos;

    _title.SetPosition(new Vector2f(_rect.Position.X + 5f, _rect.Position.Y + 5f));

    SetResponsive();

    app.Draw(_rect);
    app.Draw(_title.GetTextObject());

    for (short i = 0; i < _inputFields.Count; i++) {
      var offsetBase = 45.0f;
      var factor = _rect.Size.Y / BaseSize.Y;
      var targetSize = (offsetBase * factor) + (_rect.Size.Y / 10) / 2;
      _inputFields[i].Update();
      _inputFields[i].SetInputPosition(new Vector2f(
        _rect.Position.X + 15,
        (_rect.Position.Y + (targetSize * (i + 1)))
      ));
      _inputFields[i].SetResponsive(_rect);
      app.Draw(_inputFields[i].GetInputRectangle());
      app.Draw(_inputFields[i].GetFieldTypeRectangle());
      app.Draw(_inputFields[i].GetInputText().GetTextObject());
      app.Draw(_inputFields[i].GetFieldTypeText().GetTextObject());
      app.Draw(_inputFields[i].GetFieldInfoRectangle());
      for (short x = 0; x < _inputFields[i].FieldInfoOptions.Count; x++) {
        app.Draw(_inputFields[i].FieldInfoOptions[x].GetTextObject());
      }
    }

    _rect.Position = _position;
  }

  private void SetResponsive() {
    float factor = _rect.Size.X / BaseSize.X;
    float size = _title.BaseCharacterSize * factor;

    _title.GetTextObject().CharacterSize = (uint)Math.Clamp(size, (float)_title.MinCharacterSize, (float)_title.MaxCharacterSize);
  }

  public void Resize(float x, float y) {
    _minX = _startMinX / x;
    _minY = _startMinY / y;
  }

  public void AddInputField(InputField inputField) {
    if (!_inputFields.Exists(x => x.GetGuid() == inputField.GetGuid())) {
      _inputFields.Add(inputField);
    } else {
      throw new Exception("this object already exists in the list.");
    }
  }

  public void RemoveInputField(short index) {
    _inputFields.Remove(_inputFields[index]);
  }

  public Vector2f GetRectangleSize() {
    return _rect.Size;
  }

  public Vector2f GetRectanglePosition() {
    return _rect.Position;
  }

  public RectangleShape GetRectangle() {
    return _rect;
  }

  public Guid GetGuid() {
    return _guid;
  }

  public bool GetDraggable() {
    return _draggable;
  }

  public bool GetResizable() {
    return _resizable;
  }

  public bool GetEditable() {
    return _editable;
  }

  public string GetTitle() {
    return _title.GetText();
  }

  public List<InputField> GetInputFields() {
    return _inputFields;
  }

  public void AddInputField() {
    _inputFields.Add(new InputField());
  }

  public void RemoveLastInputField() {
    _inputFields.RemoveAt(_inputFields.Count - 1);
  }

  public void SetRectangleSize(Vector2f size) {
    _rect.Size = size;
  }

  public void SetRectanglePosition(Vector2f position) {
    _rect.Position = position;
    if (_title != null) {
      _title.SetPosition(position);
    }
  }

  public void SetDraggable(bool draggable) {
    _draggable = draggable;
  }

  public void SetResizable(bool resizable) {
    _resizable = resizable;
  }

  public void SetEditable(bool editable) {
    _editable = editable;
  }

  public void SetTitle(string title) {
    _title.SetText(title);
  }
}