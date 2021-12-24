using SFML.Graphics;
using SFML.System;

using SQL_Designer.Interactable;

namespace SQL_Designer.Gui;

public interface IPanel {
  public void Update(RenderWindow app);
  public void AddInputField(InputField inputField);
  public void RemoveInputField(short index);
  public Vector2f GetRectangleSize();
  public Vector2f GetRectanglePosition();
  public RectangleShape GetRectangle();
  public Guid GetGuid();
  public bool GetDraggable();
  public bool GetResizable();
  public List<InputField> GetInputFields();
  public void SetRectangleSize(Vector2f size);
  public void SetRectanglePosition(Vector2f position);
  public void SetDraggable(bool draggable);
  public void SetResizable(bool resizable);
}