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

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER {
    public class KlantRepositoryADO : IKlantRepository {
        private string connectionString;
        public KlantRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public bool bestaatKlant(Klant klant)
        {
            SqlConnection conn = GetConnection();
            string query = "SELECT count(*) FROM [dbo].Klant WHERE naam=@naam AND adres=@adres";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.NVarChar));
                    cmd.CommandText = query;
                    cmd.Parameters["@naam"].Value = klant.Naam;
                    cmd.Parameters["@adres"].Value = klant.Adres;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("bestaatKlant - foutmelding", ex);
                }
            }
        }
        public bool bestaatKlant(int klantId)
        {
            SqlConnection conn = GetConnection();
            string query = "SELECT count(*) FROM [dbo].Klant WHERE klantId=@klantId";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@klantId", SqlDbType.Int));
                    cmd.CommandText = query;
                    cmd.Parameters["@klantId"].Value = klantId;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("bestaatKlant - foutmelding", ex);
                }
            }
        }
        public void voegKlantToe(Klant klant)
        {
            SqlConnection conn = GetConnection();
            string query = "INSERT INTO [dbo].Klant (naam,adres) output INSERTED.klantId values(@naam,@adres)";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.NVarChar));
                    cmd.CommandText = query;
                    cmd.Parameters["@naam"].Value = klant.Naam;
                    cmd.Parameters["@adres"].Value = klant.Adres;
                    int id = (int)cmd.ExecuteScalar();
                    klant.ZetKlantId(id);
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("voegKlantToe - foutmelding", ex);
                }
            }
        }
        public IReadOnlyList<Klant> GeefKlanten(string naam, string adres)
        {
            var klanten = new List<Klant>();
            SqlConnection conn = GetConnection();
            string query = "SELECT * FROM [dbo].Klant";
            bool AND = false;
            bool WHERE = true;
            if (!string.IsNullOrWhiteSpace(naam))
            {
                if (WHERE) query += " WHERE";
                WHERE = false;
                AND = true;
                query += " naam=@naam";
            }
            if (!string.IsNullOrWhiteSpace(adres))
            {
                if (WHERE) query += " WHERE";
                WHERE = false;
                if (AND) query += " AND ";
                query += " adres=@adres";
            }
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    if (!string.IsNullOrWhiteSpace(naam))
                    {
                        cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                        cmd.Parameters["@naam"].Value = naam;
                    }
                    if (!string.IsNullOrWhiteSpace(adres))
                    {
                        cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.NVarChar));
                        cmd.Parameters["@adres"].Value = adres;
                    }
                    cmd.CommandText = query;
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        int KlantIdDB = (int)r["KlantId"];
                        string NaamDB = (string)r["Naam"];
                        string AdresDB = (string)r["Adres"];
                        var bestellingenKlant = GeefBestellingen(KlantIdDB).ToList();
                        Klant k = new Klant(KlantIdDB, NaamDB, AdresDB);
                        k.ZetBestellingen(bestellingenKlant);
                        klanten.Add(k);
                    }
                    r.Close();
                    return klanten;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("GeefKlanten", ex);
                }
            }
        }
        private IEnumerable<Bestelling> GeefBestellingen(int klantId)
        {
            List<Bestelling> bestellingen = new();
            Klant klantDB = null;
            string sql = "SELECT b.*, k.Naam, k.Adres FROM[dbo].Bestelling b INNER JOIN[dbo].[Klant] k ON b.KlantId = k.KlantId WHERE b.klantId=@klantId";
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                try
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@KlantId", klantId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        klantDB = new((int)reader["KlantId"], (string)reader["Naam"], (string)reader["Adres"]);
                        int bestellingId = (int)reader["BestellingId"];
                        var tijdstip = (DateTime)reader["Tijdstip"];
                        var prijs = (double)reader["Prijs"];
                        var betaald = (bool)reader["Betaald"];
                        Bestelling bestelling = new(bestellingId, klantDB, tijdstip, prijs, betaald, ProductenUitBestellingWeergeven(bestellingId));
                        bestellingen.Add(bestelling);
                    }

                    return bestellingen;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("BestellingWeergeven - " + ex.Message);
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
        public Klant GeefKlant(int klantId)
        {
            SqlConnection conn = GetConnection();
            string query =
            "SELECT t1.[KlantId], t1.[Naam], t1.[Adres], t2.[Betaald], t2.[Prijs] AS PrijsBestelling, t2.[Tijdstip], t2.[BestellingId] ,t3.[aantal], t4.[VoetbaltruitjeId], t4.[competitie], t4.[club]," +
            "t4.[seizoen],t4.[kledingmaat],t4.[versie],t4.[prijs],t4.[uitthuis]" +
            " FROM[dbo].[Klant] t1" +
            " left join[dbo].[bestelling] t2 on t1.[KlantId] = t2.[KlantId]" +
            " left join[dbo].[bestellingVoetbaltruitje] t3 on t3.[bestellingid] = t2.[bestellingid]" +
            " left join[dbo].[voetbaltruitje] t4 on t4.[VoetbaltruitjeId] = t3.[VoetbaltruitjeId]" +
            " WHERE t1.KlantId=@klantId" +
            " ORDER BY t2.[KlantId]";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@klantId", SqlDbType.Int));
                    cmd.CommandText = query;
                    cmd.Parameters["@klantId"].Value = klantId;

                    SqlDataReader datareader = cmd.ExecuteReader();
                    List<Bestelling> bestellingen = new List<Bestelling>();
                    int bestellingIdOld = 0;
                    int bestellingId = 0;
                    bool betaald = false;
                    double prijs = 0.0;
                    DateTime tijdstip = DateTime.Now;
                    bool first = true;
                    int aantal;
                    int voetbaltruitjeId;
                    string competitiedb;
                    string clubdb;
                    string seizoendb;
                    Kledingmaat km;
                    int versiedb;
                    double prijsdb;
                    string uitthuisdb;
                    bool thuisdb;
                    Dictionary<Voetbaltruitje, int> producten = new Dictionary<Voetbaltruitje, int>();
                    bool leesklant = true;
                    Klant k = null;
                    while (datareader.Read())
                    {
                        if (leesklant)
                        {
                            string naamKlant = (string)datareader["naam"];
                            string adresKlant = (string)datareader["adres"];
                            k = new Klant(klantId, naamKlant, adresKlant);
                            leesklant = false;
                        }
                        if (!datareader.IsDBNull(datareader.GetOrdinal("bestellingId")))
                        {
                            bestellingId = (int)datareader["bestellingId"];
                            if (bestellingId != bestellingIdOld)
                            {
                                if (bestellingIdOld > 0)
                                {
                                    Bestelling bestelling = new Bestelling(bestellingIdOld, k, tijdstip, prijs, betaald, producten);
                                    //producten.Clear();
                                    producten = new Dictionary<Voetbaltruitje, int>();
                                }
                                first = true;
                                bestellingIdOld = bestellingId;
                            }
                            if (first)
                            {
                                betaald = (Boolean)datareader["betaald"];
                                prijs = (double)datareader["PrijsBestelling"];
                                tijdstip = (DateTime)datareader["tijdstip"];
                                bestellingId = (int)datareader["bestellingId"];
                                first = false;
                            }
                            aantal = (int)datareader["aantal"];
                            voetbaltruitjeId = (int)datareader["voetbaltruitjeId"];
                            competitiedb = (string)datareader["competitie"];
                            clubdb = (string)datareader["club"];
                            seizoendb = (string)datareader["seizoen"];
                            km = (Kledingmaat)Enum.Parse(typeof(Kledingmaat), (string)datareader["kledingmaat"]);
                            versiedb = (int)datareader["versie"];
                            prijsdb = (double)datareader["prijs"];
                            uitthuisdb = (string)datareader["uitthuis"];
                            thuisdb = false;
                            if (uitthuisdb == "Thuis") thuisdb = true;
                            Voetbaltruitje trui = new Voetbaltruitje(voetbaltruitjeId, new Club(competitiedb, clubdb), seizoendb, prijsdb, km, new ClubSet(thuisdb, versiedb));
                            producten.Add(trui, aantal);
                            if (bestellingId > 0)
                            {
                                Bestelling b = new Bestelling(bestellingId, k, tijdstip, prijs, betaald, producten);
                                bestellingen.Add(b);
                                k.VoegToeBestelling(b);
                            }
                        }
                    }
                    return k;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("KlantRepository: GeefKlant(id) - gefaald!", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public void updateKlant(Klant klant)
        {
            SqlConnection connection = GetConnection();
            string query = "UPDATE [dbo].Klant SET naam=@naam, adres=@adres WHERE KlantId=@KlantId ";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    command.Parameters.Add(new SqlParameter("@KlantId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@Naam", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@Adres", SqlDbType.NVarChar));
                    command.CommandText = query;
                    command.Parameters["@KlantId"].Value = klant.KlantId;
                    command.Parameters["@Naam"].Value = klant.Naam;
                    command.Parameters["@Adres"].Value = klant.Adres;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("UpdateKlant", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void verwijderKlant(Klant klant)
        {
            SqlConnection conn = GetConnection();
            string query = "DELETE FROM [dbo].Klant WHERE KlantId=@KlantId";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@KlantId", SqlDbType.Int));
                    cmd.CommandText = query;
                    cmd.Parameters["@KlantId"].Value = klant.KlantId;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("Klant kan niet verwijderd worden omdat hij nog bestellingen heeft - " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public bool heeftKlantBestellingen(Klant klant)
        {
            SqlConnection conn = GetConnection();
            string query = "SELECT COUNT(*) FROM [dbo].bestelling WHERE KlantId=@KlantId";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@KlantId", SqlDbType.Int));
                    cmd.CommandText = query;
                    cmd.Parameters["@KlantId"].Value = klant.KlantId;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryADOExceptions("heeftKlantBestellingen - foutmelding", ex);
                }
            }
        }
    }
}
