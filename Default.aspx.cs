using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    private static string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
    private static MySqlConnection conn = new MySqlConnection(connStr);

    private string sql;
    private MySqlCommand zapytanie;

    private DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    protected string date0(int x)
    {
        return (x > 9) ? x.ToString() : ("0" + x);
    }
    protected void aktualnosci()
    {
        string txt = "";
        try
        {
            conn.Open();

            sql = "SELECT * FROM aktualnosci ORDER BY `data` DESC LIMIT 4;";
            zapytanie = new MySqlCommand(sql, conn);

            MySqlDataReader reader = zapytanie.ExecuteReader();

            while (reader.Read())
            {
                DateTime newsData = unix.AddSeconds(Convert.ToInt32(reader[3]));
                txt += "<p>" + date0(newsData.Day) + "-" + date0(newsData.Month) + "-" + newsData.Year + " <a href=\"./Aktualnosci.aspx?a=" + reader[0].ToString() + "\">" + reader[1].ToString() + "</a></p>";
            }

            conn.Close();
        }
        catch (MySqlException ex)
        {
            InformacjeFull.InnerHtml = ex.ToString();
        }

        inf1.InnerHtml = "<h2>Aktualności</h2>";
        inf1.InnerHtml += txt;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        aktualnosci();
    }
}