using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using UserAPI.Infrastructure;

namespace UserApiTest
{
    public class DataCheckerTests
    {
        [Fact]
        public void IsEmailTrueResultTest()
        {
            // Arrange
            string email = "user@yandex.ru";

            // Assert
            Assert.True(DataChecker.IsEmail(email));
        }

        [Fact]
        public void IsEmailFalseResultTest()
        {
            // Arrange
            string email = "user#yandex.ru";

            // Assert
            Assert.False(DataChecker.IsEmail(email));
        }
        [Fact]
        public void CheckPasswordTrueResultTest()
        {
            // Arrange
            string password = "Hello1111";

            // Assert
            Assert.True(DataChecker.CheckPassword(password));
        }

        [Fact]
        public void CheckPasswordSizeFalseResultTest()
        {
            // Arrange
            string password = "Hello1";

            // Assert
            Assert.False(DataChecker.CheckPassword(password));
        }
        [Fact]
        public void CheckPasswordWithOutUpperFalseResultTest()
        {
            // Arrange
            string password = "hello1111";

            // Assert
            Assert.False(DataChecker.CheckPassword(password));
        }
        [Fact]
        public void CheckPasswordWithOutNumberFalseResultTest()
        {
            // Arrange
            string password = "Helloooooo";

            // Assert
            Assert.False(DataChecker.CheckPassword(password));
        }
    }
}
