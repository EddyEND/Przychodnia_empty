using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Text;
using System.Security.Cryptography;

public partial class Logowanie : System.Web.UI.Page
{
    public string gwiazdka { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        gwiazdka = "<span class=\"gwiazdka\">*</span>";

        if (Page.IsPostBack)
        { // Validacje
            List<string> bledy = new List<string>();

            if (NazwaInput.Value == "")
                bledy.Add("Nie podano nazwy użytkownika");
            if (HasloInput.Value == "")
                bledy.Add("Nie podano hasła");

            if (bledy.Count == 0)
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
                MySqlConnection conn = new MySqlConnection(connStr);

                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM users WHERE nazwa=@Nazwa LIMIT 1;";
                    MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Nazwa", NazwaInput.Value));
                    MySqlDataReader wynik = zapytanie.ExecuteReader();

                    if (wynik.Read())
                    {
                        // nazwa uzytkownika sie zgadza, sprawdzam haslo


                        SHA512 alg = SHA512.Create();
                        byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(HasloInput.Value));

                        StringBuilder hash = new StringBuilder();
                        foreach (byte b in result)
                            hash.AppendFormat("{0:x2}", b);


                        if (String.Compare(wynik[5].ToString(), hash.ToString(), false) == 0)
                        {
                            Session["zalogowany"] = (int)1;
                            Session["nazwa"] = NazwaInput.Value;
                            Server.Transfer("Redirect.aspx?a=login", true);
                        }
                        else
                        {
                            bledy.Add("Nieprawidłowe hasło");
                        }
                    }
                    else bledy.Add("Nie ma takiego użytkownika");

                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    Blad.Text = ex.ToString();
                }
            }


            if (bledy.Count > 0)
            {
                string bledyHTML = "<ul>";
                foreach (string val in bledy)
                {
                    bledyHTML += "<li>" + val + "</li>";
                }
                bledyHTML += "</ul>";
                Blad.Text = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
            }


        }
    }
}