using SFML.Graphics;
using SFML.System;
using SFML.Window;

using SQL_Designer.Gui;
using SQL_Designer.Interactable;
using SQL_Designer.Toolbar;

using SQL_DESIGNER.Globals;

namespace SQL_Designer.Renderer;

public class Renderer : IRenderer {
  private readonly RenderWindow _renderWindow;
  private FieldTypeEditor _fieldEditor;

  public Renderer(RenderWindow renderWindow) {
    _renderWindow = renderWindow;
    _fieldEditor = new FieldTypeEditor();

    RendererData.GetInstance();
    RendererData.SetColor(new Color(35, 35, 35));

    GuiTextData.GetInstance();
  }
  public void Update(List<IPanel> panels) {
    _renderWindow.Clear(RendererData.GetColor());
    for (short i = 0; i < panels.Count; i++) {
      panels[i]?.Update(_renderWindow);
    }
    _fieldEditor.Update(_renderWindow);
  }

  public FieldTypeEditor FieldTypeEditor => _fieldEditor;
}