namespace Viewer.Server.Domain;

public interface ITrackable
{
	DateTimeOffset? CreatedAt { get; set; }
	DateTimeOffset? UpdatedAt { get; set; }
}