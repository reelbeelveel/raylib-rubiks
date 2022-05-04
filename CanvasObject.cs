using Raylib_cs;
using static Raylib_cs.Raylib;

namespace hrv.Canvas.CanvasObject {
    public class CanvasObject {
        public CanvasObject(double x = 0, double y = 0) : x(x), y(y) {
            this.id = numObjects++;
            this.birthTime = Raylib.GetTime();
        }
        public void Update() { 
            if(isExpired) return;
            if(canExpire) {
                if(Raylib.GetTime() - birthTime > lifeTime) {
                    isExpired = true;
                    return;
                }
            }
            _update();
        }

        public void Draw() {
            if(isExpired) return;
            if(isVisible) _draw();
        }

        public virtual ScaleUp(double x, double y) {
            _scaleUp(x, y);
        }

        protected virtual void _update() {}
        protected virtual void _draw() {}
        protected virtual void _scaleUp(double x, double y) {}

        public string type = "CanvasObject";
        public string name = "null";
        public static ulong numObjects = 0;
        public ulong id;

        protected double birthTime;
        protected bool   canExpire = true;
        protected double lifetime  = -1;
        protected double x, y;

        public bool   isVisible = false;
        public bool   isExpired = true;
    }
}
