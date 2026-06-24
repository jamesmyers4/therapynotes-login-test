namespace TherapyNotesUITests
{
    public static class TestConfig
    {
        public static readonly string BaseUrl = "https://www.therapynotes.com";
        public static readonly string LoginPath = "/app/login/";
        public static readonly string DashboardPath = "/app/";
        public static readonly string PracticeCode =
            Environment.GetEnvironmentVariable("TN_PRACTICE_CODE") ?? "QAInterviewPractice";
        public static readonly string Username =
            Environment.GetEnvironmentVariable("TN_USERNAME") ?? "TestUser";
        public static readonly string Password =
            Environment.GetEnvironmentVariable("TN_PASSWORD") ?? "";
        public static readonly bool Headless =
            Environment.GetEnvironmentVariable("TN_HEADLESS") is string h
                ? bool.Parse(h)
                : false;
    }
}