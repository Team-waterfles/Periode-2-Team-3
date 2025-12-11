using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisJaar2.Models;

public class Level
{
    public int Id { get; set; }
    public string Naam { get; set; }
    public string Beschrijving { get; set; }

    // Voor UI
    public bool IsUnlocked { get; set; }
    public bool IsCompleted { get; set; }
}
