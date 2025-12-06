using System;
using System.Collections.Generic;

namespace onlineQuranTutor.Models;

public partial class Guardian
{
    public int GuardianId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
