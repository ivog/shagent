using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace SHAgent.Tests.StartCommand
{
    [TestClass]
    public class GivenStartCommandWithCorrectParametersAndNoProcessRunning
    {
        [TestMethod]
        public void ShouldStartProcess()
        {
            var processManager = Substitute.For<IProcessManager>();
            var shConfigManager = Substitute.For<IConfigurationManager>();
            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.FileExists(Arg.Any<string>()).Returns(true);

            var commandHandler = new CommandHandler(processManager, shConfigManager, Substitute.For<IMessenger>(), fileSystem);

            commandHandler.ExecuteCommand(Action.Parse("START;username;password", shConfigManager));

            processManager.Received().StartProcess(Arg.Any<Action>());
        }
    }

    [TestClass]
    public class GivenStartCommandWithProcessRunning
    {
        [TestMethod]
        public void ShouldNotStartProcess()
        {
            var processManager = Substitute.For<IProcessManager>();
            processManager.IsProcessRunning(Arg.Any<Action>()).Returns(true);

            var shConfigManager = Substitute.For<IConfigurationManager>();
            shConfigManager.ExpectedSourceIpAddress.Returns("127.0.0.1");
            shConfigManager.ExpectedUserName.Returns("username");
            shConfigManager.ExpectedPassword.Returns("password");

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.FileExists(Arg.Any<string>()).Returns(true);

            var commandHandler = new CommandHandler(processManager, shConfigManager, Substitute.For<IMessenger>(), fileSystem);

            try
            {
                commandHandler.ExecuteCommand(Action.Parse("START;username;password", shConfigManager));
            }
            catch (Exception e)
            {
                e.Message.Should().Be("Process is allready running.");
            }
            processManager.DidNotReceive().StartProcess(Arg.Any<Action>());
        }
    }
}
