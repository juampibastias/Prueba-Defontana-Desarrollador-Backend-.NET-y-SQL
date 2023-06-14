using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PruebaTecnicaDefontana
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=lab-defontana.caporvnn6sbh.us-east-1.rds.amazonaws.com,1433;Database=Prueba;User Id=ReadOnly;Password=d*3PSf2MmRX9vJtA5sgwSphCVQ26*T53uU;";

            var query = "SELECT * FROM Venta WHERE Fecha >= DATEADD(day, -30, GETDATE())";

            var ventas = new List<Venta>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var schemaTable = reader.GetSchemaTable();
                        var columnNames = schemaTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

                        while (reader.Read())
                        {
                            var venta = new Venta();

                            if (columnNames.Contains("ID_Venta"))
                                venta.ID_Venta = Convert.ToInt32(reader["ID_Venta"]);

                            if (columnNames.Contains("TotalPesos"))
                                venta.TotalPesos = Convert.ToDecimal(reader["TotalPesos"]);

                            if (columnNames.Contains("Fecha"))
                                venta.Fecha = Convert.ToDateTime(reader["Fecha"]);

                            if (columnNames.Contains("ID_Local"))
                                venta.ID_Local = Convert.ToInt32(reader["ID_Local"]);

                            if (columnNames.Contains("ID_Detalle"))
                                venta.ID_Detalle = Convert.ToInt32(reader["ID_Detalle"]);

                            ventas.Add(venta);
                        }
                    }
                }
            }

            var totalVentas = ventas.Sum(v => v.TotalPesos);
            var cantidadVentas = ventas.Count;

            Console.WriteLine("Total de ventas: {0}", totalVentas);
            Console.WriteLine("Cantidad de ventas: {0}", cantidadVentas);

            var ventaMasAlta = ventas.OrderByDescending(v => v.TotalPesos).FirstOrDefault();
            if (ventaMasAlta != null)
            {
                Console.WriteLine("Venta más alta: {0} (Fecha: {1})", ventaMasAlta.TotalPesos, ventaMasAlta.Fecha);
            }
            else
            {
                Console.WriteLine("No se encontraron ventas.");
            }

            var productoConMayorMontoVentas = ventas.GroupBy(v => v.ID_Detalle)
                                                    .Select(g => new { ID_Detalle = g.Key, MontoTotalVentas = g.Sum(v => v.TotalPesos) })
                                                    .OrderByDescending(p => p.MontoTotalVentas)
                                                    .FirstOrDefault();
            if (productoConMayorMontoVentas != null)
            {
                Console.WriteLine("Producto con mayor monto de ventas: {0}", productoConMayorMontoVentas.ID_Detalle);
            }
            else
            {
                Console.WriteLine("No se encontraron ventas.");
            }

            Console.ReadLine();
        }
    }

    class Venta
    {
        public int ID_Venta { get; set; }
        public decimal TotalPesos { get; set; }
        public DateTime Fecha { get; set; }
        public int ID_Local { get; set; }
        public int ID_Detalle { get; set; }
    }
}
