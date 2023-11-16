using System;
using System.Data;
using System.Data.SqlServerCe;
using NullPaper.Modelo;


namespace NullPaper.Banco
{
    class LeituraDAO
    {

        private static SqlCeConnection conn = new SqlCeConnection(@"Data Source=C:\Users\bruno\source\repos\NullPaper\NullPaper\Banco\BancoDB.sdf");


        public static DataTable pegarLeitura()
        {

            SqlCeDataAdapter data = new SqlCeDataAdapter("SELECT * FROM Leitura ORDER BY id DESC", conn);

            DataSet ds = new DataSet();
            data.Fill(ds);

            return ds.Tables[0];
        }

        public static bool gravarLeitura(Leitura leitura)
        {
            string sql = "INSERT INTO [Leitura] " +
                "(Computador, Impressora, Driver, JobId, Documento, Usuario, ColorMono, TamanhoPapel, DataImpressao, TotalPages, HashCode, PortaImpressora) " +
                "VALUES (@Computador, @Impressora, @Driver, @JobId, @Documento, @Usuario, @ColorMono, @TamanhoPapel, @DataImpressao, @TotalPages, @HashCode, @PortaImpressora)";
            SqlCeCommand command = new SqlCeCommand(sql, conn);

            command.Parameters.AddWithValue("@Computador", leitura.Computador);
            command.Parameters.AddWithValue("@Impressora", leitura.Impressora);
            command.Parameters.AddWithValue("@Driver", leitura.Driver);
            command.Parameters.AddWithValue("@JobId", leitura.JobId);
            command.Parameters.AddWithValue("@Documento", leitura.Documento);
            command.Parameters.AddWithValue("@Usuario", leitura.Usuario);
            command.Parameters.AddWithValue("@ColorMono", leitura.ColorMono);
            command.Parameters.AddWithValue("@TamanhoPapel", leitura.TamanhoPapel);
            command.Parameters.AddWithValue("@DataImpressao", leitura.DataImpressao);
            command.Parameters.AddWithValue("@TotalPages", leitura.TotalPages);
            command.Parameters.AddWithValue("@HashCode", leitura.HashCode);
            command.Parameters.AddWithValue("@PortaImpressora", leitura.PortaImpressora);

            try
            {
                if (conn.State.Equals(ConnectionState.Closed))
                {
                    conn.Open();
                }

                if (command.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally { conn.Close(); }
        }


        // inicio

        public static Leitura recuperarLeituraPersist(Leitura leitura)
        {
            string selectSql = "SELECT * FROM Leitura WHERE Computador = @Computador " +
                "AND Impressora = @Impressora AND Driver = @Driver " +
                "AND JobId = @JobId AND Documento = @Documento " +
                "AND Usuario = @Usuario AND ColorMono = @ColorMono " +
                "AND TamanhoPapel = @TamanhoPapel AND DataImpressao = @DataImpressao";

            SqlCeCommand selectCommand = new SqlCeCommand(selectSql, conn);

            selectCommand.Parameters.AddWithValue("@Computador", leitura.Computador);
            selectCommand.Parameters.AddWithValue("@Impressora", leitura.Impressora);
            selectCommand.Parameters.AddWithValue("@Driver", leitura.Driver);
            selectCommand.Parameters.AddWithValue("@JobId", leitura.JobId);
            selectCommand.Parameters.AddWithValue("@Documento", leitura.Documento);
            selectCommand.Parameters.AddWithValue("@Usuario", leitura.Usuario);
            selectCommand.Parameters.AddWithValue("@ColorMono", leitura.ColorMono);
            selectCommand.Parameters.AddWithValue("@TamanhoPapel", leitura.TamanhoPapel);
            selectCommand.Parameters.AddWithValue("@DataImpressao", leitura.DataImpressao);

            try
            {
                if (conn.State.Equals(ConnectionState.Closed))
                {
                    conn.Open();
                }

                SqlCeDataReader reader = selectCommand.ExecuteReader();

                {
                    if (reader.Read())
                    {
                        Leitura leituraPersist = new Leitura();
                        leituraPersist.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        leituraPersist.Computador = reader.GetString(reader.GetOrdinal("Computador"));
                        leituraPersist.Impressora = reader.GetString(reader.GetOrdinal("Impressora"));
                        leituraPersist.Driver = reader.GetString(reader.GetOrdinal("Driver"));
                        leituraPersist.JobId = reader.GetInt32(reader.GetOrdinal("JobId"));
                        leituraPersist.Documento = reader.GetString(reader.GetOrdinal("Documento"));
                        leituraPersist.Usuario = reader.GetString(reader.GetOrdinal("Usuario"));
                        leituraPersist.ColorMono = reader.GetString(reader.GetOrdinal("ColorMono"));
                        leituraPersist.TamanhoPapel = reader.GetString(reader.GetOrdinal("TamanhoPapel"));
                        leituraPersist.DataImpressao = reader.GetString(reader.GetOrdinal("DataImpressao"));
                        leituraPersist.TotalPages = reader.GetInt32(reader.GetOrdinal("TotalPages"));
                        return leituraPersist;

                    }
                    else
                    {
                        Console.WriteLine("Nenhum objeto inserido encontrado.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally { conn.Close(); }

        }
        //fim recupera persist


        // início update total pages
        public static void atualizaTotalPages(int idLeitura, int totalPages)
        {
            string sql = "UPDATE [Leitura] SET TotalPages = @TotalPages WHERE id = @idLeitura";
            SqlCeCommand command = new SqlCeCommand(sql, conn);

            command.Parameters.AddWithValue("@idLeitura", idLeitura);
            command.Parameters.AddWithValue("@TotalPages", totalPages);

            try
            {
                if (conn.State.Equals(ConnectionState.Closed))
                {
                    conn.Open();
                }

                command.ExecuteNonQueryAsync();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            finally { conn.Close(); }

        }
        // fim update total pages
    }
}
