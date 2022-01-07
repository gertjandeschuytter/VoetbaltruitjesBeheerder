using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.DATALAYER.Exceptions;
using BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER
{
    public class BestellingRepositoryADO : IBestellingRepository
    {
        #region Properties
        private readonly string _connectionString;
        #endregion

        #region Constructors
        public BestellingRepositoryADO(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new(_connectionString);
            return connection;
        }
        public Bestelling GeefBestelling(int BestellingId)
        {
            SqlConnection conn = GetConnection();
            string sql = "SELECT * FROM klant k INNER JOIN Bestelling  b  ON k.KlantId = b.KlantId WHERE b.BestellingId = @BestellingId";
            Klant k = null;
            using (SqlCommand cmd = new(sql, conn))
            {
                try
                {
                    conn.Open();
                    cmd.Parameters.Add("@BestellingId", SqlDbType.Int);
                    cmd.Parameters["@BestellingId"].Value = BestellingId;
                    IDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (k == null)
                    {
                        k = new Klant((int)reader["KlantId"], (string)reader["Naam"], (string)reader["Adres"]);
                    }
                    Bestelling b = new Bestelling((int)reader["BestellingId"], k, (DateTime)reader["Tijdstip"], (double)reader["Prijs"],
                        (bool)reader["Betaald"], ProductenUitBestellingWeergeven(BestellingId));
                    reader.Close();
                    return b;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("BestellingRepository - " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public IEnumerable<Bestelling> GeefBestellingen(int? bestellingId, DateTime? startdatum, DateTime? einddatum, Klant? klant)
        {
            List<Bestelling> bestellingen = new();
            Klant klantDB = null;
            bool WHERE = true;
            bool AND = false;
            string sql = "SELECT b.*, k.KlantId AS KlantId, k.Naam, k.Adres FROM[dbo].Bestelling b INNER JOIN[dbo].[Klant] k ON b.KlantId = k.KlantId";
            if (bestellingId != null)
            {
                if (WHERE)
                {
                    sql += " WHERE ";
                    WHERE = false;
                }
                if (AND)
                {
                    sql += " AND ";
                }
                else
                {
                    AND = true;
                }
                sql += "b.BestellingId = @BestellingId";
            }
            if (klant != null)
            {
                if (WHERE)
                {
                    sql += " WHERE ";
                    WHERE = false;
                }
                if (AND)
                {
                    sql += " AND ";
                }
                else
                {
                    AND = true;
                }
                sql += "b.KlantId = @KlantId";
            }
            if (startdatum != null)
            {
                if (WHERE)
                {
                    sql += " WHERE ";
                    WHERE = false;
                }
                if (AND)
                {
                    sql += " AND ";
                }
                else
                {
                    AND = false;
                }
                sql += "b.Tijdstip >= @startdatum";
            }
            if (einddatum != null)
            {
                if (WHERE)
                {
                    sql += " WHERE ";
                }
                if (AND)
                {
                    sql += " AND ";
                }
                sql += "b.Tijdstip <= @einddatum";
            }
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                try
                {
                    conn.Open();
                    if (bestellingId > 0)
                    {
                        cmd.Parameters.AddWithValue("@BestellingId", bestellingId);
                    }
                    if (klant != null)
                    {
                        cmd.Parameters.AddWithValue("@KlantId", klant.KlantId);
                    }
                    if (startdatum != null)
                    {
                        cmd.Parameters.AddWithValue("@startdatum", startdatum);
                    }
                    if (einddatum != null)
                    {
                        cmd.Parameters.AddWithValue("@einddatum", einddatum);
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        klantDB = new((int)reader["KlantId"], (string)reader["Naam"], (string)reader["Adres"]);
                        int bestellingIdDB = (int)reader["BestellingId"];
                        var tijdstip = (DateTime)reader["Tijdstip"];
                        var prijs = (double)reader["Prijs"];
                        var betaald = (bool)reader["Betaald"];
                        Bestelling bestelling = new(bestellingIdDB, klantDB, tijdstip, prijs, betaald, ProductenUitBestellingWeergeven(bestellingIdDB));
                        bestellingen.Add(bestelling);
                    }
                    return bestellingen;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryADOExceptions("BestellingWeergeven - " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private Dictionary<Voetbaltruitje, int> ProductenUitBestellingWeergeven(int bestellingsId)
        {
            Dictionary<Voetbaltruitje, int> producten = new();
            bool thuis = false;
            string sql = "SELECT vbt.BestellingId, vbt.VoetbaltruitjeId, vbt.Aantal, t.* FROM [dbo].BestellingVoetbaltruitje vbt " +
                "INNER JOIN [dbo].Voetbaltruitje t ON vbt.VoetbaltruitjeId = t.VoetbaltruitjeId";
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (bestellingsId == (int)reader["BestellingId"])
                        {
                            Club club = new((string)reader["Competitie"], (string)reader["Club"]);
                            string UitThuis = (string)reader["UitThuis"];
                            if (UitThuis == "Thuis") thuis = true;
                            ClubSet clubSet = new(thuis, (int)reader["Versie"]);

                            Kledingmaat kledingmaat = (Kledingmaat)Enum.Parse(typeof(Kledingmaat), (string)reader["Kledingmaat"]);
                            Voetbaltruitje voetbaltruitje = new((int)reader["VoetbaltruitjeId"], club, (string)reader["Seizoen"], (double)reader["Prijs"], kledingmaat, clubSet);
                            int aantal = (int)reader["Aantal"];
                            producten.Add(voetbaltruitje, aantal);
                        }
                    }
                    return producten;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryADOExceptions("BestellingProductenWeergeven - error", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public void UpdateBestelling(Bestelling bestelling)
        {
            if (!BestaatBestelling(bestelling.BestellingId)) throw new BestellingRepositoryADOExceptions("BESTAAT NIET!");
            Bestelling huidigeBestelling = this.GeefBestelling(bestelling.BestellingId);
            if (bestelling.Equals(huidigeBestelling)) throw new BestellingRepositoryADOExceptions("ZIJN AL GELIJK!");


            var producten = bestelling.GeefProducten();
            string sqlOne = "UPDATE [dbo].bestelling SET Prijs = @Prijs, Betaald = @Betaald, KlantId = @KlantId WHERE BestellingId = @BestellingId";
            string sqlTwo = "INSERT INTO [dbo].bestellingVoetbaltruitje (BestellingId, VoetbaltruitjeId, Aantal) VALUES(@BestellingId, @VoetbaltruitjeId, @Aantal)";
            string sqlThree = "DELETE FROM BestellingVoetbaltruitje WHERE BestellingId = @BestellingId";


            SqlConnection conn = GetConnection();
            SqlCommand cmdOne = new(sqlOne, conn);
            SqlCommand cmdThree = new(sqlThree, conn);

            conn.Open();
            SqlTransaction sqltr = conn.BeginTransaction();
            cmdOne.Transaction = sqltr;
            cmdThree.Transaction = sqltr;

            try
            {
                cmdOne.Parameters.AddWithValue("@Prijs", bestelling.Prijs);
                cmdOne.Parameters.AddWithValue("@Betaald", bestelling.Betaald);
                cmdOne.Parameters.AddWithValue("@klantId", bestelling.Klant.KlantId);
                cmdOne.Parameters.AddWithValue("@BestellingId", bestelling.BestellingId);
                cmdOne.ExecuteNonQuery();
                cmdThree.Parameters.AddWithValue("@BestellingId", bestelling.BestellingId);
                cmdThree.ExecuteNonQuery();

                foreach (var r in producten)
                {
                    SqlCommand cmdTwo = new(sqlTwo, conn);
                    cmdTwo.Transaction = sqltr;
                    cmdTwo.Parameters.AddWithValue("@BestellingId", bestelling.BestellingId);
                    cmdTwo.Parameters.AddWithValue("@VoetbaltruitjeId", r.Key.Id);
                    cmdTwo.Parameters.AddWithValue("@Aantal", r.Value);
                    cmdTwo.ExecuteNonQuery();
                }

                sqltr.Commit();

            }
            catch (Exception ex)
            {
                sqltr.Rollback();
                throw new BestellingRepositoryADOExceptions("UpdateBestelling - " + ex.Message);
            }
            finally
            {
                conn.Close();
            }


        }
        public void VerwijderBestelling(Bestelling bestelling)
        {
            string sql = "DELETE FROM [dbo].BestellingVoetbaltruitje WHERE BestellingId = @BestellingId";
            string sql2 = "DELETE FROM [dbo].Bestelling WHERE BestellingId = @BestellingId";
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd1 = conn.CreateCommand())
            using (SqlCommand cmd2 = conn.CreateCommand())
            {
                conn.Open();
                SqlTransaction sqltr = conn.BeginTransaction();
                cmd1.Transaction = sqltr;
                cmd2.Transaction = sqltr;
                try
                {
                    cmd1.CommandText = sql;
                    cmd2.CommandText = sql2;
                    cmd1.Parameters.AddWithValue("@BestellingId", bestelling.BestellingId);
                    cmd1.ExecuteNonQuery();
                    cmd2.Parameters.AddWithValue("@BestellingId", bestelling.BestellingId);
                    cmd2.ExecuteNonQuery();
                    sqltr.Commit();
                }
                catch (Exception ex)
                {
                    sqltr.Rollback();
                    throw new BestellingRepositoryADOExceptions("BestellingVerwijderen - ", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public void VoegBestellingToe(Bestelling bestelling)
        {
            int bestellingsId;
            var producten = bestelling.GeefProducten();
            SqlTransaction trans = null;
            string sql = "INSERT INTO [dbo].Bestelling (Betaald, Prijs, Tijdstip, KlantId) OUTPUT INSERTED.BestellingId VALUES (@Betaald, @Prijs, @Tijdstip, @KlantId)";
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd2 = conn.CreateCommand())
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    cmd.CommandText = sql;
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@Betaald", bestelling.Betaald);
                    cmd.Parameters.AddWithValue("@Prijs", bestelling.Prijs);
                    cmd.Parameters.AddWithValue("@Tijdstip", bestelling.Tijdstip);
                    cmd.Parameters.AddWithValue("@KlantId", bestelling.Klant.KlantId);
                    bestellingsId = (int)cmd.ExecuteScalar();
                    foreach (var x in producten)
                    {
                        string sql2 = "INSERT INTO [dbo].BestellingVoetbaltruitje (BestellingId, VoetbaltruitjeId, Aantal) VALUES (@BestellingId, @VoetbaltruitjeId, @Aantal)";
                        cmd2.Parameters.Clear();
                        cmd2.Transaction = trans;
                        cmd2.CommandText = sql2;
                        cmd2.Parameters.AddWithValue("@BestellingId", bestellingsId);
                        cmd2.Parameters.AddWithValue("@VoetbaltruitjeId", x.Key.Id);
                        cmd2.Parameters.AddWithValue("@Aantal", x.Value);
                        cmd2.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new BestellingRepositoryADOExceptions("BestellingToevoegen - ", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public bool BestaatBestelling(int bestellingId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].Bestelling WHERE bestellingId = @bestellingId";
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@bestellingId", bestellingId);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryADOExceptions("BestaatBestelling - " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        #endregion
    }
}
