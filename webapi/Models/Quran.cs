using System;
using System.Collections.Generic;

namespace onlineQuranTutor.Models;

public partial class Quran
{
    public int Id { get; set; }

    public int SuraId { get; set; }

    public int VerseId { get; set; }

    public string AyahText { get; set; } = null!;

    public virtual ICollection<Juz> Juzs { get; set; } = new List<Juz>();

    public virtual Surah Sura { get; set; } = null!;
}
