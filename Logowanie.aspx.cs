using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

public partial class Logowanie : System.Web.UI.Page
{
    protected void logowanie()
    {
        List<string> bledy = new List<string>();

        if (NazwaInput.Value.Trim() == "")
            bledy.Add("Nie podano nazwy użytkownika");
        if (HasloInput.Value.Trim() == "")
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
                zapytanie.Parameters.Add(new MySqlParameter("@Nazwa", NazwaInput.Value.Trim()));
                MySqlDataReader wynik = zapytanie.ExecuteReader();

                if (wynik.Read())
                {
                    // nazwa uzytkownika sie zgadza, sprawdzam haslo


                    SHA512 alg = SHA512.Create();
                    byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(HasloInput.Value.Trim()));

                    StringBuilder hash = new StringBuilder();
                    foreach (byte b in result)
                    hash.AppendFormat("{0:x2}", b);


                    if (String.Compare(wynik[5].ToString(), hash.ToString(), false) == 0)
                    {
                        Session["zalogowany"] = (int)1;
                        Session["id"] = wynik[0].ToString();
                        Session["nazwa"] = wynik[1].ToString();
                        Session["imie"] = wynik[2].ToString();
                        Session["nazwisko"] = wynik[3].ToString();
                        Session["typ"] = wynik[8].ToString();

                        // Server.Transfer("Redirect.aspx?a=login", true);
                        Server.Transfer("Redirect.aspx?a=login&link=" + Session["PrevPage"], true);
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
                Blad.InnerHtml = ex.ToString();
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
            Blad.InnerHtml = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
        }

    }
    public string gwiazdka { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        gwiazdka = "<span class=\"gwiazdka\">*</span>";

        if (Session["zalogowany"] != null)
            Response.Redirect("./", true);

        if (Page.IsPostBack)
            logowanie();
    }
}