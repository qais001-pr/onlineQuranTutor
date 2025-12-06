using System;
using System.Collections.Generic;

namespace onlineQuranTutor.Models;

public partial class Juz
{
    public int JuzId { get; set; }

    public int SurahId { get; set; }

    public int StartingVerseId { get; set; }

    public string? ArbabicStartWord { get; set; }

    public virtual Quran StartingVerse { get; set; } = null!;

    public virtual Surah Surah { get; set; } = null!;
}
