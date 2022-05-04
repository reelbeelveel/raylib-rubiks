using Raylib_cs;
using static Raylib_cs.Raylib;

namespace hrv {
    public static class ScreenShot {
        public static string Capture() {
            // TODO save somewhere useful
            string name = GetFileName();
            Raylib.TakeScreenshot(name);
            return name;
        }
        private static string GetFileName() {
            ShellProcess datetime = new ShellProcess("date", "+HRVScreenshot_%m%d%y_%I%M.png");
            string file = datetime.GetLine();
            datetime.Kill();
            return file;
        }
    }
}
