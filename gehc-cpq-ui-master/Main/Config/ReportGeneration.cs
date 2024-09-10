using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;

namespace cpq_ui_master.Main.Config
{
    [SetUpFixture]
    public class ReportsGenerationClass
    {
        private static ExtentReports extentReports;
        private static ExtentTest extentTest;

        public static ExtentReports StartReporting()
        {
            if (extentReports == null)
            {

                var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                var actualPath = path.Substring(0, path.LastIndexOf("bin"));
                var projectPath = new Uri(actualPath).LocalPath;
                var reportsDirectory = Path.Combine(projectPath, "Reports");
                if (!Directory.Exists(reportsDirectory))
                {
                    Directory.CreateDirectory(projectPath.ToString() + "Reports");
                }

                var reportPath = Path.Combine(reportsDirectory, "index.html");
                var htmlReporter = new ExtentSparkReporter(reportPath);
                extentReports = new ExtentReports();
                extentReports.AttachReporter(htmlReporter);
            }
            return extentReports;
        }


        public static void EndReporting(ExtentReports extentReports)
        {
            if (extentReports != null)
            {
                extentReports.Flush();
            }
        }

        public static void LogInfo(ExtentTest extentTest, string info)
        {
            extentTest.Info(info);
        }

        public static void LogPass(ExtentTest extentTest, string info)
        {
            extentTest.Pass(info);
        }

        public static void LogFail(ExtentTest extentTest, string info)
        {
            extentTest.Fail(info);
        }

        public static void LogWarning(ExtentTest extentTest, string info)
        {
            extentTest.Warning(info);
        }

        public static void LogSkip(ExtentTest extentTest, string info)
        {
            extentTest.Skip(info);
        }
        public static void EndTest(ExtentTest test)
        {

            var status = TestContext.CurrentContext.Result.Outcome.Status;
            Console.WriteLine(status);
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    LogFail(test, "Test failed with message: " + TestContext.CurrentContext.Result.Message);
                    test.Log(Status.Fail, "Stack Trace: " + stacktrace);
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    LogWarning(test, "Test skipped:");
                    test.Log(Status.Warning, "Test inconclusive");
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    LogSkip(test, "Test skipped:");
                    test.Log(Status.Skip, "Test skipped");
                    break;
                default:
                    logstatus = Status.Pass;
                    LogPass(test, "Test Passed:");
                    break;
            }
            LogInfo(test, "Test ended");

        }
    }
}
