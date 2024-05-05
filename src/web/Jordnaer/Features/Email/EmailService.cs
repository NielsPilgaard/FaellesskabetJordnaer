using Jordnaer.Consumers;
using Jordnaer.Shared;
using MassTransit;
using SendGrid.Helpers.Mail;

namespace Jordnaer.Features.Email;

public interface IEmailService
{
	Task<bool> SendEmailFromContactForm(ContactForm contactForm, CancellationToken cancellationToken = default);
}

public sealed class EmailService(IPublishEndpoint publishEndpoint) : IEmailService
{
	public async Task<bool> SendEmailFromContactForm(
		ContactForm contactForm,
		CancellationToken cancellationToken = default)
	{
		var replyTo = new EmailAddress(contactForm.Email, contactForm.Name);

		var subject = contactForm.Name is null
						  ? "Kontaktformular"
						  : $"Kontaktformular besked fra {contactForm.Name}";

		var email = new SendEmail
		{
			Subject = subject,
			ReplyTo = replyTo,
			HtmlContent = contactForm.Message,
			To = EmailConstants.ContactEmail
		};

		await publishEndpoint.Publish(email, cancellationToken);

		return true;
	}
}