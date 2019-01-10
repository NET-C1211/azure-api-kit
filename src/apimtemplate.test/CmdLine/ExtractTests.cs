using System;
using Xunit;
using McMaster.Extensions.CommandLineUtils;


namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates
{
    public class ExtractTests
    {
        [Fact]
        public void ShouldFailWithUnknownCommand()
        {
            //arrange - do all your instatiation and configuration

            //act - execute

            //assert
            var createCommand = new ExtractCommand();

            var ex = Assert.ThrowsAny<CommandParsingException>(() => createCommand.Execute("test"));
            //Console.WriteLine(ex.Message);
            Assert.Contains("Unrecognized command or argument 'test'", ex.Message);
        }
    }
}
