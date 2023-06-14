﻿using System;
using System.Collections.Generic;

namespace Prueba_Defontana___Desarrollador_Backend_.NET_y_SQL.Models;

public partial class VentaDetalle
{
    public long IdVentaDetalle { get; set; }

    public long IdVenta { get; set; }

    public int PrecioUnitario { get; set; }

    public int Cantidad { get; set; }

    public int TotalLinea { get; set; }

    public long IdProducto { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Ventum IdVentaNavigation { get; set; } = null!;
}
