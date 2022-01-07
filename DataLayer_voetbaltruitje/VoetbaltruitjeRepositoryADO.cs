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
    public class VoetbaltruitjeRepositoryADO : IVoetbaltruitjeRepository {
        //fields
        private readonly string connectionString;
        //ctor
        public VoetbaltruitjeRepositoryADO(string connectionString) {
            this.connectionString = connectionString;
        }
        private SqlConnection GetConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        //done
        public Voetbaltruitje GeefVoetbaltruitje(int id) {
            SqlConnection connection = GetConnection();
            string query = "SELECT * FROM [dbo].Voetbaltruitje WHERE voetbaltruitjeId=@voetbaltruitjeid";
            using (SqlCommand command = connection.CreateCommand()) {
                connection.Open();
                try {
                    command.Parameters.Add(new SqlParameter("@voetbaltruitjeid", SqlDbType.Int));
                    command.Parameters["@voetbaltruitjeid"].Value = id;
                    command.CommandText = query;

                    SqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    int voetbaltruitjeId = (int)dataReader["voetbaltruitjeId"];
                    string competitieDB = (string)dataReader["competitie"];
                    string ploegDB = (string)dataReader["club"];
                    string seizoenDB = (string)dataReader["seizoen"];
                    Kledingmaat km = (Kledingmaat)Enum.Parse(typeof(Kledingmaat), (string)dataReader["kledingmaat"]);
                    int versieDB = (int)dataReader["versie"];
                    double prijsDB = (double)dataReader["prijs"];
                    string thuisUitDB = (string)dataReader["UitThuis"];
                    bool thuisDB = false;
                    if (thuisUitDB == "Thuis") thuisDB = true;
                    else thuisDB = false;
                    Club club = new Club(competitieDB, ploegDB);
                    ClubSet clubSet = new ClubSet(thuisDB, versieDB);
                    Voetbaltruitje truitje = new Voetbaltruitje(voetbaltruitjeId, club, seizoenDB, prijsDB, km, clubSet);

                    dataReader.Close();

                    return truitje;
                } catch (Exception ex) {
                    throw new VoetbaltruitjeRepositoryADOExceptions("GeefVoetbaltruitje", ex);
                } finally {
                    connection.Close();
                }
            }
        }
        public bool BestaatVoetbaltruitje(int voetbaltruitjeId) {
            SqlConnection conn = GetConnection();
            string query = "SELECT COUNT(1) FROM [dbo].voetbaltruitje WHERE voetbaltruitjeid = @voetbaltruitjeid";
            using (SqlCommand cmd = conn.CreateCommand()) {
                conn.Open();
                try {
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@voetbaltruitjeid", SqlDbType.Int));
                    cmd.Parameters["@voetbaltruitjeid"].Value = voetbaltruitjeId;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                } catch (Exception ex) {
                    throw new ClubRepositoryADOExceptions("Clubrepository, bestaatCompetitie - gefaald", ex);
                } finally {
                    conn.Close();
                }
            }
        }
        public bool BestaatVoetbaltruitje(Voetbaltruitje truitje) {
            SqlConnection connection = GetConnection();
            string query =
                "SELECT Count (*) FROM [dbo].Voetbaltruitje"
                + " WHERE club=@club"
                + " AND UitThuis=@uitthuis"
                + " AND competitie=@competitie"
                + " AND kledingmaat=@kledingmaat"
                + " AND prijs=@prijs"
                + " AND versie=@versie"
                + " AND seizoen=@seizoen";
            using (SqlCommand command = connection.CreateCommand()) {
                connection.Open();
                try {
                    command.Parameters.Add(new SqlParameter("@club", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@competitie", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@UitThuis", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@kledingmaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@seizoen", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@versie", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Float));
                    command.CommandText = query;
                    command.Parameters["@club"].Value = truitje.Club.Ploeg;
                    command.Parameters["@prijs"].Value = truitje.Prijs;
                    command.Parameters["@competitie"].Value = truitje.Club.Competitie;
                    if (truitje.ClubSet.Thuis) command.Parameters["@uitthuis"].Value = "Thuis";
                    else command.Parameters["@uitthuis"].Value = "Uit";
                    command.Parameters["@kledingmaat"].Value = Enum.GetName(typeof(Kledingmaat), truitje.Kledingmaat);
                    command.Parameters["@versie"].Value = truitje.ClubSet.Versie;
                    command.Parameters["@seizoen"].Value = truitje.Seizoen;
                    int n = (int)command.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;
                } catch (Exception ex) {
                    throw new VoetbaltruitjeRepositoryADOExceptions("BestaatVoetbaltruitje", ex);
                } finally {
                    connection.Close();
                }
            }
        }
        public void VoegVoetbaltruitjeToe(Voetbaltruitje truitje) {
            SqlConnection conn = GetConnection();
            string query = "INSERT INTO [dbo].voetbaltruitje (competitie,club,seizoen,prijs,kledingmaat,uitthuis,versie) output INSERTED.voetbaltruitjeId" +
                " VALUES (@competitie,@club,@seizoen,@prijs,@kledingmaat,@uitthuis,@versie)";
            using (SqlCommand cmd = conn.CreateCommand()) {
                conn.Open();
                try {
                    cmd.Parameters.Add(new SqlParameter("@competitie", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@club", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@seizoen", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Float));
                    cmd.Parameters.Add(new SqlParameter("@kledingmaat", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@uitthuis", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@versie", SqlDbType.Int));

                    cmd.CommandText = query;

                    cmd.Parameters["@competitie"].Value = truitje.Club.Competitie;
                    cmd.Parameters["@club"].Value = truitje.Club.Ploeg;
                    if (truitje.ClubSet.Thuis) cmd.Parameters["@uitthuis"].Value = "Thuis";
                    else cmd.Parameters["@uitthuis"].Value = "Uit";
                    cmd.Parameters["@kledingmaat"].Value = Enum.GetName(typeof(Kledingmaat), truitje.Kledingmaat);
                    cmd.Parameters["@versie"].Value = truitje.ClubSet.Versie;
                    cmd.Parameters["@seizoen"].Value = truitje.Seizoen;
                    cmd.Parameters["@prijs"].Value = truitje.Prijs;
                    int id = (int)cmd.ExecuteScalar();
                    truitje.ZetId(id);
                } catch (Exception ex) {
                    throw new VoetbaltruitjeRepositoryADOExceptions("VoegTruitjeToe", ex);
                } finally {
                    conn.Close();
                }
            }
        }
        public void VerwijderVoetbaltruitje(Voetbaltruitje voetbaltruitje) {
            SqlConnection conn = GetConnection();
            string query = "DELETE FROM [dbo].voetbaltruitje WHERE voetbaltruitjeId=@voetbaltruitjeId";
            using (SqlCommand cmd = conn.CreateCommand()) {
                conn.Open();
                try {
                    cmd.Parameters.Add(new SqlParameter("@voetbaltruitjeId", SqlDbType.Int));
                    cmd.CommandText = query;
                    cmd.Parameters["@voetbaltruitjeId"].Value = voetbaltruitje.Id;
                    cmd.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new VoetbaltruitjeRepositoryADOExceptions("VerwijderVoetbaltruitje", ex);
                } finally {
                    conn.Close();
                }
            }
        }
        public void UpdateVoetbaltruitje(Voetbaltruitje truitje) {
            SqlConnection connection = GetConnection();
            string query = "UPDATE [dbo].Voetbaltruitje SET competitie=@competitie, club=@club, seizoen=@seizoen, prijs=@prijs, kledingmaat=@kledingmaat, uitthuis=@uitthuis, versie=@versie WHERE voetbaltruitjeId=@voetbaltruitjeId ";
            using (SqlCommand command = connection.CreateCommand()) {
                connection.Open();
                try {
                    command.Parameters.Add(new SqlParameter("@competitie", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@club", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@voetbaltruitjeId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@seizoen", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Float));
                    command.Parameters.Add(new SqlParameter("@kledingmaat", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@uitthuis", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@versie", SqlDbType.Int));


                    command.CommandText = query;
                    command.Parameters["@competitie"].Value = truitje.Club.Competitie;
                    command.Parameters["@voetbaltruitjeId"].Value = truitje.Id;
                    command.Parameters["@club"].Value = truitje.Club.Ploeg;
                    command.Parameters["@seizoen"].Value = truitje.Seizoen;
                    command.Parameters["@prijs"].Value = truitje.Prijs;
                    command.Parameters["@kledingmaat"].Value = Enum.GetName(typeof(Kledingmaat), truitje.Kledingmaat);
                    command.Parameters["@versie"].Value = truitje.ClubSet.Versie;
                    if (truitje.ClubSet.Thuis) command.Parameters["@uitthuis"].Value = "Thuis";
                    else command.Parameters["@uitthuis"].Value = "Uit";
                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    throw new VoetbaltruitjeRepositoryADOExceptions("UpdateVoetbalTruitje", ex);
                } finally {
                    connection.Close();
                }
            }
        }
        public IEnumerable<Voetbaltruitje> GeefVoetbaltruitjes(string competitie, string club, string seizoen, string kledingmaat, int? versie, bool? thuis, double? prijs) {
            var truitjes = new List<Voetbaltruitje>();
            SqlConnection conn = GetConnection();
            string query = "SELECT * FROM [dbo].voetbaltruitje";
            bool AND = false;
            bool WHERE = true;
            if (!string.IsNullOrWhiteSpace(competitie)) {
                WHERE = false;
                query += " WHERE";
                AND = true;
                query += " competitie=@competitie";
            }
            if (!string.IsNullOrWhiteSpace(club)) {
                if (WHERE)query += " WHERE";
                if (AND) query += " AND ";
                else AND = true;
                query += " club=@club";
            }
            if (!string.IsNullOrWhiteSpace(seizoen)) {
                if (WHERE) query += " WHERE";
                if (AND) query += " AND ";
                else AND = true;
                query += " seizoen=@seizoen";
            }
            if (!string.IsNullOrWhiteSpace(kledingmaat)) {
                if (WHERE) query += " WHERE";
                if (AND) query += " AND ";
                else AND = true;
                query += " kledingmaat=@kledingmaat";
            }
            if (versie.HasValue) {
                if (WHERE) query += " WHERE";
                if (AND) query += " AND ";
                else AND = true;
                query += " versie=@versie";
            }
            if (thuis != null) {
                if (WHERE) query += " WHERE";
                if (AND) query += " AND ";
                else AND = true;
                query += " UitThuis=@UitThuis";
            }
            if (prijs != null) {
                if (WHERE) query += " WHERE";
                if (AND) query += " AND ";
                else AND = true;
                query += " prijs=@prijs";
            }
            using (SqlCommand cmd = conn.CreateCommand()) {
                conn.Open();
                try {
                    if (!string.IsNullOrWhiteSpace(competitie)) {
                        cmd.Parameters.Add(new SqlParameter("@competitie", SqlDbType.NVarChar));
                        cmd.Parameters["@competitie"].Value = competitie;
                    }
                    if (!string.IsNullOrWhiteSpace(club)) {
                        cmd.Parameters.Add(new SqlParameter("@club", SqlDbType.NVarChar));
                        cmd.Parameters["@club"].Value = club;
                    }
                    if (!string.IsNullOrWhiteSpace(seizoen)) {
                        cmd.Parameters.Add(new SqlParameter("@seizoen", SqlDbType.NVarChar));
                        cmd.Parameters["@seizoen"].Value = seizoen;
                    }
                    if (!string.IsNullOrWhiteSpace(kledingmaat)) {
                        cmd.Parameters.Add(new SqlParameter("@kledingmaat", SqlDbType.NVarChar));
                        cmd.Parameters["@kledingmaat"].Value = kledingmaat;
                    }
                    if (versie != null) {
                        cmd.Parameters.Add(new SqlParameter("@versie", SqlDbType.Int));
                        cmd.Parameters["@versie"].Value = versie;
                    }
                    if (thuis != null) {
                        cmd.Parameters.Add(new SqlParameter("@UitThuis", SqlDbType.NVarChar));
                        if ((bool)thuis) cmd.Parameters["@UitThuis"].Value = "Thuis";
                        else cmd.Parameters["@UitThuis"].Value = "Uit";
                    }
                    if (prijs != null) {
                        cmd.Parameters.Add(new SqlParameter("@prijs", SqlDbType.Float));
                        cmd.Parameters["@prijs"].Value = prijs;
                    }
                    cmd.CommandText = query;
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        int voetbaltruitjeId = (int)r["voetbaltruitjeId"];
                        string competitieDB = (string)r["competitie"];
                        string clubDB = (string)r["club"];
                        string seizoenDB = (string)r["seizoen"];
                        string kledingmaatDB = (string)r["kledingmaat"];
                        int versieDB = (int)r["versie"];
                        double prijsDB = (double)r["prijs"];
                        string uitthuisDB = (string)r["uitthuis"];
                        bool thuisDB = false;
                        if (uitthuisDB == "Thuis") thuisDB = true;
                        Kledingmaat kledingmaatInEnum = (Kledingmaat)Enum.Parse(typeof(Kledingmaat), kledingmaatDB);
                        Voetbaltruitje truitje = new Voetbaltruitje(voetbaltruitjeId, new Club(competitieDB, clubDB), seizoenDB, prijsDB, kledingmaatInEnum, new ClubSet(thuisDB, versieDB));
                        truitjes.Add(truitje);
                    }
                    r.Close();
                    return truitjes;
                } catch (Exception ex) {
                    throw new VoetbaltruitjeRepositoryADOExceptions("GeefVoetbalTruitjes", ex);
                } finally {
                    conn.Close();
                }
            }
        }
    }
}

