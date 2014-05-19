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

public partial class Panel : System.Web.UI.Page
{
    private static string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
    private static MySqlConnection conn = new MySqlConnection(connStr);

    protected bool sprHaslo(string haslo)
    {
        List<string> bledy = new List<string>();

        if (HasloInput.Value.Trim() == "")
            bledy.Add("Brak aktualnego hasła");
        else
        {
           
            try
            {
                conn.Open();

                string sql = "SELECT haslo FROM users WHERE id=@Id";
                MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));
                

                MySqlDataReader wynik = zapytanie.ExecuteReader();

                if (wynik.Read())
                {
                    SHA512 alg = SHA512.Create();
                    byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(HasloInput.Value.Trim()));

                    StringBuilder hash = new StringBuilder();
                    foreach (byte b in result)
                        hash.AppendFormat("{0:x2}", b);

                    if (String.Compare(wynik[0].ToString(), hash.ToString()) != 0)
                        bledy.Add("Aktualne hasło nieprawidłowe");
                }
                conn.Close();

            }
            catch (MySqlException ex)
            {
                blad0.InnerHtml = ex.ToString();
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
            blad0.InnerHtml = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
            return false;
        }
        else
        {
            blad0.InnerHtml = "";
            return true;
        }
    }
    protected bool validDane()
    {
        List<string> bledy = new List<string>();

        if (NewImieInput.Value.Trim() == "")
            bledy.Add("Brak imienia");

        if (NewNazwiskoInput.Value.Trim() == "")
            bledy.Add("Brak nazwiska");

        if (bledy.Count > 0)
        {
            string bledyHTML = "<ul>";
            foreach (string val in bledy)
            {
                bledyHTML += "<li>" + val + "</li>";
            }
            bledyHTML += "</ul>";
            blad1.InnerHtml = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
            return false;
        }
        else
        {
            blad1.InnerHtml = "";

            try
            {
                conn.Open();

                string sql = "UPDATE users SET imie=@Imie, nazwisko=@Nazwisko WHERE id=@id;";
                MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                zapytanie.Parameters.Add(new MySqlParameter("@Imie", NewImieInput.Value.Trim()));
                zapytanie.Parameters.Add(new MySqlParameter("@Nazwisko", NewNazwiskoInput.Value.Trim()));
                zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));

                int wynik = zapytanie.ExecuteNonQuery();

                if (wynik > 0)
                {
                    sql = "SELECT * FROM users WHERE id=@id;";
                    zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));

                    MySqlDataReader wyn = zapytanie.ExecuteReader();

                    if (wyn.Read())
                    {
                        Session["imie"] = wyn[2].ToString();
                        Session["nazwisko"] = wyn[3].ToString();
                    }
                    conn.Close();
                    return true;
                }
                else 
                {
                    conn.Close();
                    return false; 
                }
            }
            catch (MySqlException ex)
            {
                blad1.InnerHtml = ex.ToString();
                return false;
            }


        }
    }
    protected bool validEmail()
    {
        List<string> bledy = new List<string>();

        if (NewEmailInput.Value.Trim() == "")
            bledy.Add("Brak adresu E-mail");
        else
        {
            string emailReg = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            Regex reg = new Regex(emailReg);
            if (!reg.IsMatch(NewEmailInput.Value.Trim())) bledy.Add("Nieprawidłowy adres E-mail");

            else if (NewEmailInput2.Value.Trim() == "")
                bledy.Add("Brak powtórzenia adresu E-mail");
            else if (NewEmailInput.Value.Trim() != NewEmailInput2.Value.Trim())
                bledy.Add("Adresy E-mail nie są identyczne");
            else
            {
                try 
                {
                    conn.Open();
                
                    string sql = "SELECT id FROM users WHERE email=@Email AND id<>@Id;";
                    MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Email", NewEmailInput.Value.Trim()));
                    zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));

                    object wynik = zapytanie.ExecuteScalar();

                    if (wynik != null)
                    {
                        bledy.Add("Istnieje już użytkownik zarejestrowany na podany adres E-mail");
                    }
                    else
                    {

                        sql = "UPDATE users SET email=@Email WHERE id=@id;";
                        zapytanie = new MySqlCommand(sql, conn);
                        zapytanie.Parameters.Add(new MySqlParameter("@Email", NewEmailInput.Value.Trim()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));

                        int wyn = zapytanie.ExecuteNonQuery();

                        if (wyn > 0)
                        {
                            conn.Close();
                            return true;
                        }
                        else
                        { 
                            conn.Close();
                            return false; 
                        }

                    }
                    conn.Close();
                
                }
                catch (MySqlException ex)
                {
                    blad2.InnerHtml = ex.ToString();
                    return false;
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
            blad2.InnerHtml = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
            return false;
        }
        else 
        {
            blad1.InnerHtml = "";
            return true; 
        }

    }
    protected bool validHaslo()
    {
        List<string> bledy = new List<string>();

        if (NewHasloInput.Value.Trim() == "")
            bledy.Add("Brak hasła");
        else
        {
            if (NewHasloInput.Value.Trim().Length < 8)
                bledy.Add("Hasło ma mniej niż 8 znaków");
            else
            { // regexpy hasła
                string znakiSpecjalne = "` ~ ! @ # $ % ^ & * ( ) _ + \\- = \\[ \\] { } , . : ; ' \" | \\\\ / < > ?";
                Regex reg = new Regex("[a-ząćęłńóśźż]");
                if (!reg.IsMatch(NewHasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jedną małą literę");
                reg = new Regex("[A-ZĄĆĘŁŃÓŚŹŻ]");
                if (!reg.IsMatch(NewHasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jedną dużą literę");
                reg = new Regex("[0-9]");
                if (!reg.IsMatch(NewHasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jedną cefrę");
                reg = new Regex("[" + znakiSpecjalne + "]");
                if (!reg.IsMatch(NewHasloInput.Value.Trim())) bledy.Add("Hasło musi mieć przynajmniej jeden symbol:<br />` ~ ! @ # $ % ^ & * ( ) _ + - = [ ] { } , . : ; ' \" | \\ / < > ?");

                reg = new Regex("[^a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ0-9 " + znakiSpecjalne + "]");
                if (reg.IsMatch(NewHasloInput.Value.Trim()))
                {
                    string bledyReg = "";
                    foreach (Match match in reg.Matches(NewHasloInput.Value.Trim()))
                        bledyReg += " " + match.Value.Trim() + "[" + match.Index + "]";

                    bledy.Add("Hasło zawiera niedozwolony znak/i (znak [pozycja]):" + bledyReg);
                }
                if (bledy.Count == 0 && NewHasloInput2.Value.Trim() == "")
                    bledy.Add("Brak powtórzenia hasła");
                else if (bledy.Count == 0 && NewHasloInput.Value.Trim() != NewHasloInput2.Value.Trim())
                    bledy.Add("Hasła nie są identyczne");
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
            blad3.InnerHtml = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
            return false;
        }
        else
        {
            blad3.InnerHtml = "";

            try
            {
                conn.Open();

                string sql = "UPDATE users SET haslo=@Haslo WHERE id=@id;";
                MySqlCommand zapytanie = new MySqlCommand(sql, conn);

                SHA512 alg = SHA512.Create();
                byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(NewHasloInput.Value.Trim()));

                StringBuilder hash = new StringBuilder();
                foreach (byte b in result)
                    hash.AppendFormat("{0:x2}", b);

                zapytanie.Parameters.Add(new MySqlParameter("@Haslo", hash.ToString()));
                zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));

                int wynik = zapytanie.ExecuteNonQuery();

                if (wynik > 0)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                blad1.InnerHtml = ex.ToString();
                return false;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["zalogowany"] == null)
            Server.Transfer("Redirect.aspx?a=permission", true);
        
        HtmlGenericControl x1 = new HtmlGenericControl("div");

        x1.InnerHtml = "OK";

        if (Page.IsPostBack)
        {
            if (sprHaslo(HasloInput.Value.Trim()))
            {
                blad1.InnerHtml = blad2.InnerHtml = blad3.InnerHtml = "";
                if (Request.Form["dane"] != null)
                {
                    if (validDane())
                        Server.Transfer("Redirect.aspx?a=chdane&link=" + Session["PrevPage"], true);
                }
                else if (Request.Form["email"] != null)
                {
                    if (validEmail())
                        Server.Transfer("Redirect.aspx?a=chemail&link=" + Session["PrevPage"], true);
                }
                else if (Request.Form["pass"] != null)
                {
                    if (validHaslo())
                        Server.Transfer("Redirect.aspx?a=chhaslo", true);
                }
            }
        }
    }
}