using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.BusinessTest.ServiceTest
{
    public class EmailServiceTest
    {
        private readonly EmailService _emailService;
        private readonly SmtpSettings _smtpSettings;

        public EmailServiceTest()
        {
            var setting = EnviromentSetting.GetInstance();

            // SMTP Settings
            var smtpSettings = new SmtpSettings
            {
                Host = setting.GetSMTPHost(),
                Port = int.Parse(setting.GetSMTPPort()),
                Password = setting.GetSMTPPassword(),
                FromEmail = setting.GetSMTPFFromEmail(),
            };

            _emailService = new EmailService(_smtpSettings);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldSendEmail_WhenValidParameters()
        {
            // Arrange
            var to = "testemail@gmail.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act & Assert
            await _emailService.SendEmailAsync(to, subject, body);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowArgumentNullException_WhenToIsNull()
        {
            // Arrange
            var subject = "Test Subject";
            var body = "Test Body";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _emailService.SendEmailAsync(null, subject, body));
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowArgumentNullException_WhenSubjectIsNull()
        {
            // Arrange
            var to = "testemail@gmail.com";
            var body = "Test Body";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _emailService.SendEmailAsync(to, null, body));
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowArgumentNullException_WhenBodyIsNull()
        {
            // Arrange
            var to = "testemail@gmail.com";
            var subject = "Test Subject";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _emailService.SendEmailAsync(to, subject, null));
        }
    }
}
