using Raylib_cs;
using static Raylib_cs.Raylib;

using System.Collections.Generic;

namespace hrv {
    public class Canvas {
        public Canvas(List<CanvasObject> items) : items(items) {}

        public void Draw() {
            foreach (CanvasObject item in items) {
                item.Update();
                item.Draw();
            }
        }

        public void CleanExpired() {
            items.RemoveAll(item => item.IsExpired());
        }

        private List<CanvasObject> items;
    }
}
