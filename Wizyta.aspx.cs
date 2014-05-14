using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Wizyta : System.Web.UI.Page
{
    private static string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
    private static MySqlConnection conn = new MySqlConnection(connStr);

    protected void specjalistaFill()
    {
        SpecjalistaInput.Items.Add(new ListItem("Brak specjalistów", ""));
        try
        {
            conn.Open();

            string sql = "SELECT s.id, u.imie, u.nazwisko, s.specjalizacja FROM users u JOIN specjalisci s ON u.id=s.uid;";
            MySqlCommand zapytanie = new MySqlCommand(sql, conn);

            MySqlDataReader wynik = zapytanie.ExecuteReader();

            if (wynik.Read())
            {
                SpecjalistaInput.Items.Clear();
                SpecjalistaInput.Items.Add(new ListItem("Wybierz specjalistę...", ""));
                SpecjalistaInput.Items.Add(new ListItem("Dr " + wynik[1].ToString() + " " + wynik[2].ToString() + " - " + wynik[3].ToString(), wynik[0].ToString()));
            }
            while (wynik.Read())
            {
                SpecjalistaInput.Items.Add(new ListItem("Dr " + wynik[1].ToString() + " " + wynik[2].ToString() + " - " + wynik[3].ToString(), wynik[0].ToString()));
            }


            conn.Close();
        }
        catch (MySqlException ex)
        {
            blad.InnerHtml = ex.ToString();
        }
    }
    protected void dataPicker()
    {
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["zalogowany"] == null)
            Server.Transfer("Redirect.aspx?a=permission", true);

        specjalistaFill();
    }
}