using BusinessLayer_VoetbaltruitjesWinkel.Repo_Interfaces;
using BusinessLayer_VoetbaltruitjesWinkel.DATALAYER.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER {
    public class ClubRepositoryADO : IClubRepository {
        private string connectionString;
        public ClubRepositoryADO(string connectionString) {
            this.connectionString = connectionString;
        }
        private SqlConnection GetConnection() {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public IReadOnlyList<string> GeefCompetities() {
            SqlConnection conn = GetConnection();
            string query = "SELECT DISTINCT competitie FROM [dbo].clubcompetitie";
            var lijst = new List<string>();
            using (SqlCommand cmd = conn.CreateCommand()) {
                cmd.CommandText = query;
                conn.Open();
                try {
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        lijst.Add(r["competitie"].ToString());
                    }
                    return lijst.AsReadOnly();
                } catch (Exception ex) {
                    throw new ClubRepositoryADOExceptions("Clubrepository: geefcompetities - gefaald", ex);
                } finally {
                    conn.Close();
                }
            }
        }
        public IReadOnlyList<string> GeefClubs(string competitie) {
            SqlConnection conn = GetConnection();
            string query = "SELECT * FROM [dbo].clubcompetitie WHERE competitie = @competitie";
            var lijst = new List<string>();
            using (SqlCommand cmd = conn.CreateCommand()) {
                cmd.CommandText = query;
                cmd.Parameters.Add("@competitie", SqlDbType.NVarChar);
                cmd.Parameters["@competitie"].Value = competitie;
                conn.Open();
                try {
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        lijst.Add((r["club"].ToString()));
                    }
                    return lijst.AsReadOnly();
                } catch (Exception ex) {
                    throw new ClubRepositoryADOExceptions("Clubrepository - geefclubs - gefaald", ex);
                } finally {
                    conn.Close();
                }
            }
        }
        public bool BestaatCompetitie(string competitie) {
            SqlConnection conn = GetConnection();
            string query = "SELECT COUNT(1) FROM [dbo].clubcompetitie WHERE competitie = @competitie";
            using (SqlCommand cmd = conn.CreateCommand()) {
                conn.Open();
                try {
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@competitie", SqlDbType.NVarChar));
                    cmd.Parameters["@competitie"].Value = competitie;
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
    }
}
