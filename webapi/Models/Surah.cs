using System;
using System.Collections.Generic;

namespace onlineQuranTutor.Models;

public partial class Surah
{
    public int Id { get; set; }

    public string? SurahNames { get; set; }

    public string? SurahUrduNames { get; set; }

    public virtual ICollection<Juz> Juzs { get; set; } = new List<Juz>();

    public virtual ICollection<Quran> Qurans { get; set; } = new List<Quran>();
}
