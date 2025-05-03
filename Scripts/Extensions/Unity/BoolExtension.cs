namespace TinyUtilities.Extensions.Unity {
    public static class BoolExtension {
        public static string ToText(this bool value) => value ? "True" : "False";
    }
}