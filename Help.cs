
namespace rbx{
    public static class HelpModule {
        public static bool HelpActive { get; private set; } = false;
        public static void ToggleHelp() {
            if (HelpActive) {
                HelpActive = false;
                return;
            }
            HelpActive = true;
        }
    }
}
