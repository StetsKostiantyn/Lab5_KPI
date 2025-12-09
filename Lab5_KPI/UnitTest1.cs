using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework.Constraints;

namespace Lab5_KPI
{
    public class Tests
    {
        IWebDriver driver;
        const int Delay = 3000;
        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();

            driver.Manage().Window.Maximize();
        }

        [Test]
        public void AddRemoveElements_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/add_remove_elements/");
            Thread.Sleep(Delay);

            var addButton = driver.FindElement(By.XPath("//button[text()='Add Element']"));
            var deleteButtonsBefore = driver.FindElements(By.XPath("//button[@class='added-manually']"));
            Assert.That(0, Is.EqualTo(deleteButtonsBefore.Count));

            addButton.Click();
            addButton.Click();
            var deleteButtonAfter = driver.FindElement(By.XPath("//button[@class='added-manually']"));
            Assert.That(deleteButtonAfter.Displayed);

            deleteButtonAfter.Click();
            var deleteButtonsAfterClick = driver.FindElements(By.XPath("//button[@class='added-manually']"));
            Assert.That(1, Is.EqualTo(deleteButtonsAfterClick.Count));
        }
        [Test]
        public void Checkboxes_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/checkboxes");
            Thread.Sleep(Delay);

            var checkboxes = driver.FindElements(By.CssSelector("form#checkboxes input[type='checkbox']"));

            Assert.That(checkboxes.Count, Is.EqualTo(2));

            var checkbox1 = checkboxes[0];
            var checkbox2 = checkboxes[1];

            Assert.That(checkbox1.Selected, Is.False);
            Assert.That(checkbox2.Selected, Is.True);

            checkbox1.Click();
            checkbox2.Click();
            Assert.That(checkbox1.Selected, Is.True);
            Assert.That(checkbox2.Selected, Is.False);
        }

        [Test]
        public void Dropdown_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");
            Thread.Sleep(Delay);

            var dropdownElement = driver.FindElement(By.Id("dropdown"));

            var selectElement = new SelectElement(dropdownElement);

            Assert.That(selectElement.SelectedOption.Text, Is.Not.EqualTo("Option 1"));

            selectElement.SelectByText("Option 1");

            Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 1"));
            selectElement.SelectByValue("2");

            Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 2"));
        }

        [Test]
        public void Inputs_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/inputs");
            Thread.Sleep(Delay);

            var inputField = driver.FindElement(By.TagName("input"));

            string testNumber = "123";
            inputField.SendKeys(testNumber);

            Assert.That(inputField.GetAttribute("value"), Is.EqualTo(testNumber));
            inputField.Clear();

            string testString = "abc";
            inputField.SendKeys(testString);

            Assert.That(inputField.GetAttribute("value"), Is.EqualTo(string.Empty));
        }

        [Test]
        public void StatusCodes_Test_Sequential()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/status_codes");
            Thread.Sleep(Delay);

            driver.FindElement(By.CssSelector("a[href='status_codes/200']")).Click();
            Thread.Sleep(500);
            var contentElement200 = driver.FindElement(By.CssSelector("div.example > p"));
            string expectedText200 = "This page returned a 200 status code.";
            Assert.That(contentElement200.Text.Contains(expectedText200), Is.True);
            driver.Navigate().Back();
            Thread.Sleep(500);

            driver.FindElement(By.CssSelector("a[href='status_codes/301']")).Click();
            Thread.Sleep(500);
            var contentElement301 = driver.FindElement(By.CssSelector("div.example > p"));
            string expectedText301 = "This page returned a 301 status code.";
            Assert.That(contentElement301.Text.Contains(expectedText301), Is.True);
            driver.Navigate().Back();
            Thread.Sleep(500);

            driver.FindElement(By.CssSelector("a[href='status_codes/404']")).Click();
            Thread.Sleep(500);
            var contentElement404 = driver.FindElement(By.CssSelector("div.example > p"));
            string expectedText404 = "This page returned a 404 status code.";
            Assert.That(contentElement404.Text.Contains(expectedText404), Is.True);
            driver.Navigate().Back();
            Thread.Sleep(500);

            driver.FindElement(By.CssSelector("a[href='status_codes/500']")).Click();
            Thread.Sleep(500);
            var contentElement500 = driver.FindElement(By.CssSelector("div.example > p"));
            string expectedText500 = "This page returned a 500 status code.";
            Assert.That(contentElement500.Text.Contains(expectedText500), Is.True);
        }
        [Test]
        public void DynamicControls_CheckboxTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_controls");
            Thread.Sleep(Delay);

            var checkbox = driver.FindElement(By.Id("checkbox"));
            var removeButton = driver.FindElement(By.CssSelector("#checkbox-example button"));

            Assert.That(checkbox.Displayed, Is.True);

            removeButton.Click();

            Thread.Sleep(6000);

            var message = driver.FindElement(By.Id("message"));
            var checkboxesAfterRemove = driver.FindElements(By.Id("checkbox"));
            Assert.That(checkboxesAfterRemove.Count, Is.EqualTo(0));

            var addButton = driver.FindElement(By.CssSelector("#checkbox-example button"));
            addButton.Click();

            Thread.Sleep(4000);

            message = driver.FindElement(By.Id("message"));
            Assert.That(message.Text, Is.EqualTo("It's back!"));
            checkbox = driver.FindElement(By.Id("checkbox"));
            Assert.That(checkbox.Displayed, Is.True);
        }
        [Test]
        public void DynamicControls_InputTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_controls");
            Thread.Sleep(Delay);

            var inputField = driver.FindElement(By.CssSelector("#input-example input"));
            var enableButton = driver.FindElement(By.CssSelector("#input-example button"));

            Assert.That(inputField.Enabled, Is.False);

            enableButton.Click();

            Thread.Sleep(4000);

            var message = driver.FindElement(By.Id("message"));
            Assert.That(message.Text, Is.EqualTo("It's enabled!"));

            Assert.That(inputField.Enabled, Is.True);

            var disableButton = driver.FindElement(By.CssSelector("#input-example button"));
            disableButton.Click();

            Thread.Sleep(4000);

            message = driver.FindElement(By.Id("message"));
            Assert.That(message.Text, Is.EqualTo("It's disabled!"));

            Assert.That(inputField.Enabled, Is.False);
        }
        [Test]
        public void FileUpload_Test()
        {
            string filePath = Path.Combine(Path.GetTempPath(), "test_upload.txt");

            File.WriteAllText(filePath, "test");

            try
            {
                driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/upload");
                Thread.Sleep(Delay);

                var fileInput = driver.FindElement(By.Id("file-upload"));

                var uploadButton = driver.FindElement(By.Id("file-submit"));

                fileInput.SendKeys(filePath);

                uploadButton.Click();

                Thread.Sleep(1000);

                var header = driver.FindElement(By.TagName("h3"));
                Assert.That(header.Text, Is.EqualTo("File Uploaded!"));

                var uploadedFileName = driver.FindElement(By.Id("uploaded-files"));
                Assert.That(uploadedFileName.Text, Is.EqualTo("test_upload.txt"));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
        [Test]
        public void JavaScriptAlerts_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            Thread.Sleep(Delay);

            var resultText = driver.FindElement(By.Id("result"));

            var jsAlertButton = driver.FindElement(By.CssSelector("button[onclick='jsAlert()']"));
            jsAlertButton.Click();
            Thread.Sleep(500);

            var alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"));
            alert.Accept();
            Assert.That(resultText.Text, Is.EqualTo("You successfully clicked an alert"));

            var jsConfirmButton = driver.FindElement(By.CssSelector("button[onclick='jsConfirm()']"));
            jsConfirmButton.Click();
            Thread.Sleep(500);

            alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"));
            alert.Dismiss();
            Assert.That(resultText.Text, Is.EqualTo("You clicked: Cancel"));

            var jsPromptButton = driver.FindElement(By.CssSelector("button[onclick='jsPrompt()']"));
            jsPromptButton.Click();
            Thread.Sleep(500);

            alert = driver.SwitchTo().Alert();
            string testText = "test";
            alert.SendKeys(testText);
            Thread.Sleep(2000);
            alert.Accept();
            Assert.That(resultText.Text, Is.EqualTo("You entered: " + testText));
        }

        [Test]
        public void MultipleWindows_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/windows");
            Thread.Sleep(Delay);

            string originalWindowHandle = driver.CurrentWindowHandle;

            driver.FindElement(By.CssSelector("a[href='/windows/new']")).Click();
            Thread.Sleep(1000);

            var allWindowHandles = driver.WindowHandles;
            string newWindowHandle = allWindowHandles.First(h => h != originalWindowHandle);

            driver.SwitchTo().Window(newWindowHandle);

            var newWindowHeader = driver.FindElement(By.TagName("h3"));
            Assert.That(newWindowHeader.Text, Is.EqualTo("New Window"));

            driver.Close();
            Thread.Sleep(500);

            driver.SwitchTo().Window(originalWindowHandle);

            var originalWindowHeader = driver.FindElement(By.TagName("h3"));
            Assert.That(originalWindowHeader.Text, Is.EqualTo("Opening a new window"));
        }
        [Test]
        public void NotificationMessages_Test()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/notification_message_rendered");
            Thread.Sleep(Delay);

            var clickLink = driver.FindElement(By.CssSelector("a[href='/notification_message']"));

            clickLink.Click();
            Thread.Sleep(500);

            var notification = driver.FindElement(By.Id("flash"));

            Assert.That(notification.Displayed, Is.True);
            Assert.That(notification.Text.Contains("Action"), Is.True);

            var closeButton = driver.FindElement(By.CssSelector("a.close"));
            closeButton.Click();
            Thread.Sleep(500);

            var notificationsAfterClose = driver.FindElements(By.Id("flash"));
            Assert.That(notificationsAfterClose.Count, Is.EqualTo(0));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Dispose();
        }
    }
}