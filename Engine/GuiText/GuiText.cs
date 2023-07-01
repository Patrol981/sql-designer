
using SFML.Graphics;
using SFML.System;

namespace SQL_Designer.Gui;

public sealed class GuiTextData {
  private static GuiTextData? s_instance;
  private static Font s_font = new Font("./Resources/Fonts/PressStart2P-Regular.ttf");
  private static Color s_selectedColor = new Color(220, 20, 60);
  private static Color s_inputStandardColor = new Color(69, 69, 69); // nice

  public static Font GetFont() {
    return s_font;
  }

  public static Color GetSelectedColor() {
    return s_selectedColor;
  }

  public static Color GetInputStandardColor() {
    return s_inputStandardColor;
  }

  public static void SetFont(Font font) {
    s_font = font;
  }

  public static GuiTextData GetInstance() {
    Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
    if (s_instance == null) {
      s_instance = new GuiTextData();
    }
    return s_instance;
  }
}

public class GuiText : IGuiText {
  private Text _text = new Text();
  public readonly uint BaseCharacterSize = 14;
  public readonly uint MinCharacterSize = 10;
  public readonly uint MaxCharacterSize = 30;

  public GuiText() {
    _text.CharacterSize = BaseCharacterSize;
    _text.DisplayedString = "nothing to see here";
    _text.Position = new Vector2f(0, 0);
    _text.FillColor = Color.White;
    _text.Font = GuiTextData.GetFont();
    _text.Style = Text.Styles.Regular;
  }

  public Color GetColor() {
    return _text.FillColor;
  }

  public uint GetFontSize() {
    return _text.CharacterSize;
  }

  public Vector2f GetPositon() {
    return _text.Position;
  }

  public string GetText() {
    return _text.DisplayedString;
  }

  public void SetColor(Color color) {
    _text.FillColor = color;
  }

  public void SetFontSize(uint fontSize) {
    _text.CharacterSize = fontSize;
  }

  public void SetPosition(Vector2f position) {
    _text.Position = position;
  }

  public void SetText(string text) {
    _text.DisplayedString = text;
  }

  public Text GetTextObject() {
    return _text;
  }
}