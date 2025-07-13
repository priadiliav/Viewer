namespace Viewer.Agent.Domain.Configs;

public class ConnectionConfig
{
	public string Host { get; set; } = string.Empty;
	public string Port { get; set; } = "5000";
	public bool UseSsl { get; set; } = false;
	public string CertificatePath { get; set; } = string.Empty;
	public string CertificatePassword { get; set; } = string.Empty;
}