using SFML.Graphics;
using SFML.System;
using SFML.Window;

using System.Threading;

using SQL_Designer.Interactable;

namespace SQL_Designer.Gui;

public sealed class Panels {
  private static Panels? s_instance;
  private static List<IPanel> s_panelList = new List<IPanel>();

  public static List<IPanel> GetPanels() {
    return s_panelList;
  }

  public static IPanel GetPanel(short index) {
    return s_panelList[index];
  }

  public static void SetPanels(List<IPanel> panelList) {
    s_panelList = panelList;
  }

  public static void AddPanel(IPanel panel) {
    if(!s_panelList.Exists(x => x.GetGuid() == panel.GetGuid())) {
      s_panelList.Add(panel);
    }
  }

  public static void Update(RenderWindow app) {
    for(short i=0; i<s_panelList.Count; i++) {
      s_panelList[i].Update(app);
    }
  }

  public static Panels GetInstance() {
    if(s_instance == null) {
      s_instance = new Panels();
    }
    return s_instance;
  }
}

public class Panel: IPanel {
  private RectangleShape _rect = new RectangleShape(new Vector2f(30,30));
  private GuiText _title = new GuiText();
  private List<InputField> _inputFields = new List<InputField>();
  private Guid _guid = Guid.NewGuid();
  private bool _draggable = false;
  private bool _resizable = false;
  public readonly float MinX = 150;
  public readonly float MinY = 75;

  public Panel() {
    _rect.FillColor = new Color(120,120,120);
    _rect.Position = new Vector2f(30,30);
    Panels.AddPanel(this);
    _inputFields.Add(new InputField());
  }

  public Panel(Vector2f rectangleSize) {
    _rect.FillColor = new Color(120,120,120);
    _rect.Size = rectangleSize;
    _rect.Position = new Vector2f(30,30);
    Panels.AddPanel(this);
    _inputFields.Add(new InputField());
  }

  public Panel(Vector2f rectangleSize, string panelName) {
    _rect.FillColor = new Color(120,120,120);
    _rect.Size = rectangleSize;
    _title.SetText(panelName);
    _rect.Position = new Vector2f(30,30);
    _title.SetPosition(_rect.Position);
    Panels.AddPanel(this);
    _inputFields.Add(new InputField());
    _inputFields.Add(new InputField());
    _inputFields.Add(new InputField());
  }

  public void Update(RenderWindow app) {

    _title.SetPosition(new Vector2f(_rect.Position.X + 5f, _rect.Position.Y + 5f));

    if(_draggable) {
      var mousePos = app.MapPixelToCoords(SFML.Window.Mouse.GetPosition(app));
      _rect.Position = new Vector2f(mousePos.X - 15, mousePos.Y - 15);
    }

    if(_resizable) {
      var mousePos = app.MapPixelToCoords(SFML.Window.Mouse.GetPosition(app));
      _rect.Size = new Vector2f(mousePos.X + 15, mousePos.Y + 15);
    }

    if(_rect.Size.X < MinX) {
      _rect.Size = new Vector2f(MinX, _rect.Size.Y);
    }

    if(_rect.Size.Y < MinY) {
      _rect.Size = new Vector2f(_rect.Size.X, MinY);
    }

    app.Draw(_rect);
    app.Draw(_title.GetTextObject());

    for(short i=0; i<_inputFields.Count; i++) {
      _inputFields[i].Update();
      _inputFields[i].SetInputPosition(new Vector2f(_rect.Position.X + 15, _rect.Position.Y + (45 * (i+1))));
      _inputFields[i].SetResponsive(_rect);
      app.Draw(_inputFields[i].GetRectangle());
      app.Draw(_inputFields[i].GetGuiText().GetTextObject());
    }
  }

  public void AddInputField(InputField inputField) {
    if(!_inputFields.Exists(x => x.GetGuid() == inputField.GetGuid())) {
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

  public List<InputField> GetInputFields() {
    return _inputFields;
  }

  public void SetRectangleSize(Vector2f size) {
    _rect.Size = size;
  }

  public void SetRectanglePosition(Vector2f position) {
    _rect.Position = position;
    if(_title != null) {
      _title.SetPosition(position);
    }
  }

  public void SetDraggable(bool draggable) {
    _draggable = draggable;
  }

  public void SetResizable(bool resizable) {
    _resizable = resizable;
  }
}