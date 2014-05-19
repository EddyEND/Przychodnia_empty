using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

public partial class Rejestracja : System.Web.UI.Page
{
    protected void validacja(){
        List<string> bledy = new List<string>();

        string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
        MySqlConnection conn = new MySqlConnection(connStr);

        try {
            conn.Open();

         // Validacje
            if (ImieInput.Value.Trim() == "")
                bledy.Add("Brak imienia");

            if (NazwiskoInput.Value.Trim() == "")
                bledy.Add("Brak nazwiska");

            if (NazwaInput.Value.Trim() == "")
                bledy.Add("Brak nazwy użytkownika");
            else {
                if (NazwaInput.Value.Trim().Length < 4)
                    bledy.Add("Nazwa ma mniej niż 4 znaki");
                else {
                    string sql = "SELECT id FROM users WHERE nazwa=@Nazwa;";
                    MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Nazwa", NazwaInput.Value.Trim()));

                    object wynik = zapytanie.ExecuteScalar();

                    if (wynik != null)
                    {
                        bledy.Add("Podana nazwa użytkownika jest już zajęta");
                    }
                }
            }

            if (HasloInput.Value.Trim() == "")
                bledy.Add("Brak hasła");
            else {
                if (HasloInput.Value.Trim().Length < 8)
                    bledy.Add("Hasło ma mniej niż 8 znaków");
                else { // regexpy hasła
                    string znakiSpecjalne = "` ~ ! @ # $ % ^ & * ( ) _ + \\- = \\[ \\] { } , . : ; ' \" | \\\\ / < > ?";
                    Regex reg = new Regex("[a-ząćęłńóśźż]");
                    if (!reg.IsMatch(HasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jedną małą literę");
                    reg = new Regex("[A-ZĄĆĘŁŃÓŚŹŻ]");
                    if (!reg.IsMatch(HasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jedną dużą literę");
                    reg = new Regex("[0-9]");
                    if (!reg.IsMatch(HasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jedną cefrę");
                    reg = new Regex("[" + znakiSpecjalne + "]");
                    if (!reg.IsMatch(HasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jeden symbol:<br />` ~ ! @ # $ % ^ & * ( ) _ + - = [ ] { } , . : ; ' \" | \\ / < > ?");

                    reg = new Regex("[^a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ0-9 " + znakiSpecjalne + "]");
                    if (reg.IsMatch(HasloInput.Value.Trim()))
                    {
                        string bledyReg = "";
                        foreach (Match match in reg.Matches(HasloInput.Value.Trim()))
                            bledyReg += " " + match.Value.Trim() + "[" + match.Index + "]";

                        bledy.Add("Hasło zawiera niedozwolony znak/i (znak [pozycja]):" + bledyReg); 
                    }
                    if (HasloInput2.Value.Trim() == "")
                        bledy.Add("Brak powtórzenia hasła");
                    else if (HasloInput.Value.Trim() != HasloInput2.Value.Trim())
                        bledy.Add("Hasła nie są identyczne");
                }
            }

            if (EmailInput.Value.Trim() == "")
                bledy.Add("Brak adresu E-mail");
            else {
                string emailReg = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
                Regex reg = new Regex(emailReg);
                if (!reg.IsMatch(EmailInput.Value.Trim())) bledy.Add("Nieprawidłowy adres E-mail");

                else if (EmailInput2.Value.Trim() == "")
                    bledy.Add("Brak powtórzenia adresu E-mail");
                else if (EmailInput.Value.Trim() != EmailInput2.Value.Trim())
                        bledy.Add("Adresy E-mail nie są identyczne");
                else
                {
                    string sql = "SELECT id FROM users WHERE email=@Email;";
                    MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Email", EmailInput.Value.Trim()));

                    object wynik = zapytanie.ExecuteScalar();

                    if (wynik != null)
                    {
                        bledy.Add("Istnieje już użytkownik zarejestrowany na podany adres E-mail");
                    }
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
            else {
                Blad.InnerHtml = "";

                // =================================
            
                string sql = "INSERT INTO users VALUES (@Id, @Nazwa, @Imie, @Nazwisko, @Email, @Haslo, @DataRejestracji, @Aktywne, @Typ);";
                MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                zapytanie.Parameters.Add(new MySqlParameter("@Id", 0));
                zapytanie.Parameters.Add(new MySqlParameter("@Nazwa", NazwaInput.Value.Trim()));
                zapytanie.Parameters.Add(new MySqlParameter("@Imie", ImieInput.Value.Trim()));
                zapytanie.Parameters.Add(new MySqlParameter("@Nazwisko", NazwiskoInput.Value.Trim()));
                zapytanie.Parameters.Add(new MySqlParameter("@Email", EmailInput.Value.Trim()));

                SHA512 alg = SHA512.Create();
                byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(HasloInput.Value.Trim()));

                StringBuilder hash = new StringBuilder();
                foreach (byte b in result)
                hash.AppendFormat("{0:x2}", b);

                zapytanie.Parameters.Add(new MySqlParameter("@Haslo", hash.ToString()));

                DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan elapsedTime = DateTime.Now - unix;

                zapytanie.Parameters.Add(new MySqlParameter("@DataRejestracji", elapsedTime.TotalSeconds.ToString()));

                zapytanie.Parameters.Add(new MySqlParameter("@Aktywne", "0"));
                zapytanie.Parameters.Add(new MySqlParameter("@Typ", "P"));

                int wynik = zapytanie.ExecuteNonQuery();

                if (wynik > 0)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");

                    div.InnerHtml = "Dziękujemy za rejestrację, " + NazwaInput.Value.Trim() + ". Twoje konto jest gotowe do użycia. Teraz możesz się zalogować.";
                    Rejestracja_.Controls.Clear();
                    Rejestracja_.Controls.Add(div);
                }

            
                // ===================================

            }

            conn.Close();
        }
        catch (MySqlException ex) {
            Blad.InnerHtml = ex.ToString();
        }
    }

    public string gwiazdka { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["zalogowany"] != null)
            Response.Redirect("./", true);
        
        if (Page.IsPostBack)
            validacja();

    }
}