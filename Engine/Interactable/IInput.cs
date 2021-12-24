namespace SQL_Designer.Interactable;

public interface IInput {
  public void Update();
  public string GetContent();
  public Guid GetGuid();
  public void SetContent(string content);
}