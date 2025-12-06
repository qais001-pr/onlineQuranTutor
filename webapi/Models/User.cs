using System;
using System.Collections.Generic;

namespace onlineQuranTutor.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string UserType { get; set; } = null!;

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Timezone { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public string? PictureType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Guardian> Guardians { get; set; } = new List<Guardian>();
}
