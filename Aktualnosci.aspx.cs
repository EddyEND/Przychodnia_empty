using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Aktualnosci : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["a"] != null)
        {
            int a = 0;
            Int32.TryParse(Request.QueryString["a"].Trim(), out a);

            if (a > 0)
            {
                string txt = "";
                try
                {
                    conn.Open();

                    sql = "SELECT * FROM aktualnosci WHERE id=@Id;";
                    zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Id", a.ToString()));

                    MySqlDataReader reader = zapytanie.ExecuteReader();

                    while (reader.Read())
                    {
                        txt += "<h2>" + reader[1].ToString() + "</h2>";
                        DateTime newsData = unix.AddSeconds(Convert.ToInt32(reader[3]));
                        txt += "<div class=\"dataNews\">" + date0(newsData.Day) + "-" + date0(newsData.Month) + "-" + newsData.Year + " " + date0(newsData.Hour) + ":" + date0(newsData.Minute) + "</div>";
                        txt += "<div class=\"trescNews\">" + reader[2].ToString() + "</div>";

                    }

                    aktualnosci.InnerHtml = "<div>" + txt + "</div>";

                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    aktualnosci.InnerHtml = ex.ToString();
                }
            }
            else Response.Redirect("./");

        }
        else Response.Redirect("./");

    }
}