using SQL_Designer.Gui;
using SQL_Designer.Toolbar;

namespace SQL_Designer.Renderer;

public interface IRenderer {
  public void Update(List<IPanel> panels);
  public FieldTypeEditor FieldTypeEditor { get; }
}