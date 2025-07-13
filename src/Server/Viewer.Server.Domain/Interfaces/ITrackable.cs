namespace Viewer.Server.Domain.Interfaces;

public interface ITrackable
{
	DateTimeOffset? CreatedAt { get; set; }
	DateTimeOffset? UpdatedAt { get; set; }
}