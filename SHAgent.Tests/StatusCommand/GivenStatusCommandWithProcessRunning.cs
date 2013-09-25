using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SHAgent.Tests.StatusCommand
{
    [TestClass]
    public class GivenStatusCommandWithProcessRunning
    {
        [TestMethod]
        public void ShouldSendBusyMessage()
        {
            var processManager = Substitute.For<IProcessManager>();
            processManager.IsProcessRunning(Arg.Any<Action>()).Returns(true);
            
            var shConfigManager = Substitute.For<IConfigurationManager>();
            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");

            var messenger = Substitute.For<IMessenger>();
            
            var commandHandler = new CommandHandler(processManager, shConfigManager, messenger);

            commandHandler.ExecuteCommand(Action.Parse("STATUS;username;password", shConfigManager));

            messenger.Received().SendMessage("BUSY");
        }
    }
}
