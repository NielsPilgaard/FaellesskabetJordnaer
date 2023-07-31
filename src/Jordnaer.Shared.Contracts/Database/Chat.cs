using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jordnaer.Shared.Contracts;

[Index(nameof(LastMessageSentUtc))]
public class Chat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    /// <summary>
    /// The display name of the chat.
    /// <para>
    /// If set to <c>null</c>, this defaults to a concatenated string of recipient names.
    /// </para>
    /// </summary>
    public string? DisplayName { get; set; }

    public IList<ChatMessage> Messages { get; set; } = Array.Empty<ChatMessage>();
    public IList<UserProfile> Recipients { get; set; } = Array.Empty<UserProfile>();

    public DateTime LastMessageSentUtc { get; set; }
    public DateTime StartedUtc { get; set; } = DateTime.UtcNow;
}

