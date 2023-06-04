namespace BlogBook.Service
{
	public class AuthMessageSenderOptions
	{
		public string? Email { get; set; }
		public string? EmailPassword { get; set; }

		public string? SendGridKey { get; set; }
	}
}
