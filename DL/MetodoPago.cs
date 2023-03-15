using System;
using System.Collections.Generic;

namespace DL;

public partial class MetodoPago
{
    public int IdMetodoPago { get; set; }

    public string Metodo { get; set; } = null!;

    public virtual ICollection<Ventum> Venta { get; } = new List<Ventum>();
}
