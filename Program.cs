using System;

using SFML.Graphics;
using SFML.Window;

using SQL_Designer.Renderer;
using SQL_Designer.Interactable;

RendererData.GetInstance();

// RenderWindow _app = new RenderWindow(RendererData.GetWindowedWindow(), "SQL Designer");
RenderWindow _app = new RenderWindow(RendererData.GetFullscreenWindow(), "SQL Designer");
_app.SetFramerateLimit(0); // SFML.NET drops performance without it
_app.SetVerticalSyncEnabled(true); // SFML.NET drops performance without it
_app.Closed += new EventHandler(OnClose!);
_app.MouseButtonPressed += new EventHandler<SFML.Window.MouseButtonEventArgs>(SQL_Designer.Interactable.Mouse.OnClick!);
_app.KeyPressed += new EventHandler<SFML.Window.KeyEventArgs>(SQL_Designer.Interactable.Keyboard.OnKeyPressed!);
IRenderer _renderer = new Renderer(_app);

while(_app.IsOpen) {
  _app.DispatchEvents();
  _renderer.Update();
  _app.Display();
}

static void OnClose(object sender, EventArgs e) {
  RenderWindow window = (RenderWindow)sender;
  window.Close();
}

