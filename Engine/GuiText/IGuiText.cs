using SFML.Graphics;
using SFML.System;

namespace SQL_Designer.Gui;

public interface IGuiText {
  public uint GetFontSize();
  public void SetFontSize(uint fontSize);
  public string GetText();
  public void SetText(string text);
  public Vector2f GetPositon();
  public void SetPosition(Vector2f position);
  public Color GetColor();
  public void SetColor(Color color);
  public Text GetTextObject();
}