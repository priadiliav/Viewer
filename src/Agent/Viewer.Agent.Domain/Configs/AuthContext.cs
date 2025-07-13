namespace Viewer.Agent.Domain.Configs;

public class AuthContext
{
	private string? _token;
	public string Token
	{
		get => _token ?? throw new InvalidOperationException("Token is not set.");
		set
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Token cannot be null or whitespace.", nameof(value));
			_token = value;
		}
	}
}