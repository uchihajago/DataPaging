using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPaging
{
    class Program
    {
        static void Main(string[] args)
        {
            var localQueryResult = new DataTable();
            var random = new Random();
            localQueryResult.Columns.Add("ID", typeof(Int32));
            localQueryResult.Columns.Add("Content");
            var start = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                var rw = localQueryResult.NewRow();
                rw["ID"] = i;
                rw["Content"] = "Contenido #" + (random.NextDouble() * (9999 - 10) + 10).ToString();
                localQueryResult.Rows.Add(rw);
            }
            Console.WriteLine("Tiempo de llenado " + (DateTime.Now - start).ToString());
            Console.WriteLine(localQueryResult.Rows.Count.ToString());
            Console.WriteLine("");
            var localCurrentPage = 0;
            int localPageSize = 10;
            do
            {
                start = DateTime.Now;
                var tp = int.Parse((localQueryResult.Rows.Count / localPageSize).ToString());
                var queryResultPage = localQueryResult.AsEnumerable().Take(1);
                if (localCurrentPage == 0)
                    queryResultPage = localQueryResult.AsEnumerable().Take(10);
                else if (tp == localCurrentPage)
                    queryResultPage = localQueryResult.AsEnumerable().Skip(Math.Max(0, localQueryResult.Rows.Count - localPageSize));
                else
                    queryResultPage = localQueryResult.AsEnumerable().Skip(localPageSize * localCurrentPage).Take(localPageSize);

                Console.WriteLine(localQueryResult.Columns[0].ColumnName + "     " + localQueryResult.Columns[1].ColumnName);
                if (queryResultPage != null)
                {
                    foreach (DataRow item in (queryResultPage.CopyToDataTable()).Rows)
                    {
                        Console.WriteLine(((int)item[0]).ToString("0000") + "   " + item[1].ToString());
                    }
                    Console.WriteLine("Tiempo de Impresion " + (DateTime.Now - start).ToString());
                }
                else
                {
                    Console.WriteLine("Error en paginacion");
                }
                Console.WriteLine("Paguina = " + localCurrentPage.ToString());
                Console.WriteLine("Total de paguinas = " + (localQueryResult.Rows.Count / localPageSize).ToString());
                Console.WriteLine("---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ");
                Console.WriteLine("");
                Console.Write("Siguiente paguina = ");
                var val = Console.ReadLine();
                if (!(int.TryParse(val, out localCurrentPage)))
                {
                    if (val.ToString().Trim().ToUpper() == "CLS")
                    {
                        Console.Clear();
                    }
                    else if (val.ToString().Trim().ToUpper() == "EXIT")
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("Comando no soportado.");
                    }
                }
            } while (localCurrentPage <= (localQueryResult.Rows.Count / localPageSize));
            Console.WriteLine("Finalizado");
            Console.ReadLine();
        }
    }
}