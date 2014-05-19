using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

public partial class admin_Default : System.Web.UI.Page
{
    private static string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
    private static MySqlConnection conn = new MySqlConnection(connStr);

    private string sql;
    private MySqlCommand zapytanie;

    private DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private Dictionary<string, object> post = new Dictionary<string, object>();


    protected string date0(int x)
    {
        return (x > 9) ? x.ToString() : ("0" + x);
    }
    protected void start()
    {
        int pacjenci = 0, specjalisci = 0, wizyty = 0, aktualnosci = 0;
        try
        {
            conn.Open();

            sql = "SELECT COUNT(*) FROM users WHERE typ=@Typ;";
            zapytanie = new MySqlCommand(sql, conn);

            zapytanie.Parameters.Add(new MySqlParameter("@Typ", "P"));

            object wynik = zapytanie.ExecuteScalar();

            if (wynik != null)
                pacjenci = Convert.ToInt32(wynik);

            //==========================================
            sql = "SELECT COUNT(*) FROM specjalisci;";
            zapytanie = new MySqlCommand(sql, conn);

            wynik = zapytanie.ExecuteScalar();

            if (wynik != null)
                specjalisci = Convert.ToInt32(wynik);

            //==========================================
            sql = "SELECT COUNT(*) FROM wizyty WHERE `data` >= @Dzis;";
            zapytanie = new MySqlCommand(sql, conn);
            zapytanie.Parameters.Add(new MySqlParameter("@Dzis", (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc) - unix).TotalSeconds.ToString()));

            wynik = zapytanie.ExecuteScalar();
            
            if (wynik != null)
                wizyty = Convert.ToInt32(wynik);

            //==========================================
            sql = "SELECT COUNT(*) FROM aktualnosci;";
            zapytanie = new MySqlCommand(sql, conn);

            wynik = zapytanie.ExecuteScalar();

            if (wynik != null)
                aktualnosci = Convert.ToInt32(wynik);

            conn.Close();
        }
        catch (MySqlException ex)
        {
            Blad.InnerHtml = ex.ToString();
        }

        string txt = "";
        txt += "<div class=\"wiersz\">";
        txt += "<div class=\"pole\">Zarejestrowanych pacjentów: <span>" + pacjenci + "</span></div>";
        txt += "</div>";

        txt += "<div class=\"wiersz\">";
        txt += "<div class=\"pole\">Specjalistów: <span>" + specjalisci + "</span></div>";
        txt += "</div>";

        txt += "<div class=\"wiersz\">";
        txt += "<div class=\"pole\">Umówionych wizyt: <span>" + wizyty + "</span></div>";
        txt += "</div>";

        txt += "<div class=\"wiersz\">";
        txt += "<div class=\"pole\">Aktualności: <span>" + aktualnosci + "</span></div>";
        txt += "</div>";

        admin.InnerHtml = "<div class=\"naglowek\">Statystyki</div>" + txt;
    }
    protected void ustawienia()
    {
        
        
        admin.InnerHtml = "<div class=\"naglowek\">Ustawienia ogólne</div>";
    }
    protected void aktualnosci()
    {
        bool x = true;
        string txt = "", valid = "";
        int idn = 0, rows = 0;


        if (Page.IsPostBack)
        {
            if (post.ContainsKey("idn")) // validacja + update
            {
                Int32.TryParse(post["idn"].ToString(), out idn);

                List<string> bledy = new List<string>();

                if (post["TytulInput"].ToString() == "")
                    bledy.Add("Brak tytułu");

                if (bledy.Count > 0)
                {
                    string bledyHTML = "<ul>";
                    foreach (string val in bledy)
                    {
                        bledyHTML += "<li>" + val + "</li>";
                    }
                    bledyHTML += "</ul>";
                    valid = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
                    valid = "<div id=\"Blad\">" + valid + "</div>";
                }
                else // update
                {
                    try
                    {
                        conn.Open();

                        sql = "UPDATE aktualnosci SET tytul=@Tytul, tresc=@Tresc WHERE id=@Idn;";
                        zapytanie = new MySqlCommand(sql, conn);

                        zapytanie.Parameters.Add(new MySqlParameter("@Idn", idn.ToString()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Tytul", post["TytulInput"].ToString()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Tresc", post["TrescInput"].ToString()));

                        int wynik = zapytanie.ExecuteNonQuery();

                        if (wynik > 0)
                        {
                            conn.Close();
                            Server.Transfer("../Redirect.aspx?a=newsedit&link=" + Session["PrevPage"], true);
                        }

                        conn.Close();
                    }
                    catch (MySqlException ex)
                    {
                        Blad.InnerHtml = ex.ToString();
                    }
                }
            }
            
            // formularz edycji
            foreach (string key in post.Keys)
            {
                if (key.IndexOf("edit_") != -1)
                {
                    if (Int32.TryParse(key.Substring(5), out idn))
                        break;
                }
            }

            if (idn > 0)
            {
                try
                {
                    conn.Open();

                    sql = "SELECT * FROM aktualnosci WHERE id=@Idn;";
                    zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Idn", idn.ToString()));

                    MySqlDataReader reader = zapytanie.ExecuteReader();

                    while (reader.Read())
                    {
                        txt += "<div class=\"wiersz\">";
                        txt += "<div class=\"pole\"><label for=\"TytulInput\">Tytuł: </label><input id=\"TytulInput\" name=\"TytulInput\" type=\"text\" value=\"" + ((post.ContainsKey("TytulInput")) ? post["TytulInput"] : reader[1].ToString()) + "\" runat=\"server\" /></div>";
                        txt += "</div>";
                        txt += "<div class=\"wiersz\">";
                        txt += "<div class=\"pole\"><label for=\"TrescInput\">Treść: </label><textarea id=\"TrescInput\" name=\"TrescInput\" runat=\"server\">" + ((post.ContainsKey("TrescInput")) ? post["TrescInput"] : reader[2].ToString()) + "</textarea></div>";
                        txt += "</div>";
                        txt += "<div><input name=\"idn\" type=\"hidden\" value=\"" + idn + "\" /><input id=\"Submit1\" type=\"submit\" value=\"Edytuj\" /></div>";

                    }

                    txt = "<div>" + txt + "</div>";

                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    Blad.InnerHtml = ex.ToString();
                }
                admin.InnerHtml = "<div class=\"naglowek\">Edycja aktualności</div>" + valid + txt;
            }
                // =================== delete
            else if (post.ContainsKey("delete"))
            {
                List<int> delIds = new List<int>();
                foreach (string key in post.Keys)
                    if (key.IndexOf("del_") != -1)
                    {
                        int idDel;
                        if (Int32.TryParse(key.Substring(4), out idDel))
                            delIds.Add(idDel);
                    }

                if (delIds.Count > 0)
                {
                    string lista = String.Join(", ", delIds);

                    try
                    {
                        conn.Open();

                        sql = "DELETE FROM aktualnosci WHERE id in (" + lista + ");";
                        zapytanie = new MySqlCommand(sql, conn);

                        rows = zapytanie.ExecuteNonQuery();

                        conn.Close();

                        valid = "<div id=\"Blad\"><div class=\"wiersz ok\">Usunięto aktualności: " + rows + "</div></div>";
                    }
                    catch (MySqlException ex)
                    {
                        Blad.InnerHtml = ex.ToString();
                    }

                }
                else Response.Redirect("./Default.aspx?action=aktualnosci");
            }
            else if (post.ContainsKey("add")) //=============== add
            {
                
                if (post.ContainsKey("TytulInput") && post.ContainsKey("TrescInput"))
                {
                    List<string> bledy = new List<string>();

                    if (post["TytulInput"].ToString() == "")
                        bledy.Add("Brak tytułu");
                    
                    if (bledy.Count > 0)
                    {
                        string bledyHTML = "<ul>";
                        foreach (string val in bledy)
                        {
                            bledyHTML += "<li>" + val + "</li>";
                        }
                        bledyHTML += "</ul>";
                        valid = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
                        valid = "<div id=\"Blad\">" + valid + "</div>";
                    }
                    else // add
                    {
                        try
                        {
                            conn.Open();

                            sql = "INSERT INTO aktualnosci VALUES(@Id, @Tytul, @Tresc, @Data);";
                            zapytanie = new MySqlCommand(sql, conn);

                            zapytanie.Parameters.Add(new MySqlParameter("@Id", 0));
                            zapytanie.Parameters.Add(new MySqlParameter("@Tytul", post["TytulInput"].ToString()));
                            zapytanie.Parameters.Add(new MySqlParameter("@Tresc", post["TrescInput"].ToString()));

                            TimeSpan elapsedTime = DateTime.Now - unix;

                            zapytanie.Parameters.Add(new MySqlParameter("@Data", elapsedTime.TotalSeconds.ToString()));

                            int wynik = zapytanie.ExecuteNonQuery();

                            if (wynik > 0)
                            {
                                conn.Close();
                                Server.Transfer("../Redirect.aspx?a=newsadd&link=" + Session["PrevPage"], true);
                            }

                            conn.Close();
                        }
                        catch (MySqlException ex)
                        {
                            Blad.InnerHtml = ex.ToString();
                        }
                    }
                }
                
                txt += "<div class=\"wiersz\">";
                txt += "<div class=\"pole\"><label for=\"TytulInput\">Tytuł: </label><input id=\"TytulInput\" name=\"TytulInput\" type=\"text\" value=\"" + ((post.ContainsKey("TytulInput")) ? post["TytulInput"] : "") + "\" runat=\"server\" /></div>";
                txt += "</div>";
                txt += "<div class=\"wiersz\">";
                txt += "<div class=\"pole\"><label for=\"TrescInput\">Treść: </label><textarea id=\"TrescInput\" name=\"TrescInput\" runat=\"server\">" + ((post.ContainsKey("TrescInput")) ? post["TrescInput"] : "") + "</textarea></div>";
                txt += "</div>";
                txt += "<div><input type=\"hidden\" name=\"add\" /><input id=\"Submit1\" type=\"submit\" value=\"Dodaj\" /></div>";

                txt = "<div>" + txt + "</div>";

                admin.InnerHtml = "<div class=\"naglowek\">Dodaj aktualność</div>" + valid + txt;
            }
            else
            {
                Response.Redirect("./Default.aspx?action=aktualnosci");
            }
        }


        if (!Page.IsPostBack || rows > 0) // lista aktualnosci
        {
            string aktualnosciBrak = "<div id=\"Blad\"><div class=\"wiersz blad\">Brak aktualnosci</div></div>";
            try
            {
                conn.Open();

                sql = "SELECT * FROM aktualnosci ORDER BY `data` DESC;";
                zapytanie = new MySqlCommand(sql, conn);

                MySqlDataReader reader = zapytanie.ExecuteReader();

                while (reader.Read())
                {
                    if (x)
                    {
                        x = false;
                        aktualnosciBrak = "";
                        txt += "<div class=\"wiersz head\">";
                        txt += "<div class=\"komorka\">Tytuł</div>";
                        txt += "<div class=\"komorka\">Czas dodania</div>";
                        txt += "<div class=\"komorka\">Edycja</div>";
                        txt += "<div class=\"komorka\">Usuwanie <input id=\"delAll\" type=\"checkbox\" /></div>";
                        txt += "</div>";
                    }
                    txt += "<div class=\"wiersz\">";
                    txt += "<div class=\"komorka\">" + reader[1].ToString() + "</div>";
                    DateTime newsData = unix.AddSeconds(Convert.ToInt32(reader[3]));
                    txt += "<div class=\"komorka\">" + date0(newsData.Day) + "-" + date0(newsData.Month) + "-" + newsData.Year + " godz. " + date0(newsData.Hour) + ":" + date0(newsData.Minute) + "</div>";
                    txt += "<div class=\"komorka\"><input type=\"submit\" name=\"edit_" + reader[0].ToString() + "\" value=\"Edytuj\" /></div>";
                    txt += "<div class=\"komorka\"><input type=\"checkbox\" name=\"del_" + reader[0].ToString() + "\" /></div>";
                    txt += "</div>";
                }
                if (aktualnosciBrak == "")
                {
                    txt += "<div class=\"wiersz noborder\">";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\"><input id=\"deleteNews\" type=\"submit\" name=\"delete\" value=\"Usuń\" /></div>";
                    txt += "</div>";
                }


                txt = "<div id=\"tabela\" class=\"u\">" + txt + "</div>";

                conn.Close();
            }
            catch (MySqlException ex)
            {
                Blad.InnerHtml = ex.ToString();
            }
            admin.InnerHtml = "<div class=\"naglowek\">Aktualności</div><div class=\"dodaj\"><input type=\"submit\" name=\"add\" value=\"Dodaj\" /></div>" + valid + aktualnosciBrak + txt;
        }

        
    }
    protected void uzytkownicy()
    {
        string txt = "", uname = "", valid = "";
        bool x = true;
        int idu = 0, rows = 0;
        
        if (Page.IsPostBack)
        {
            if (post.ContainsKey("idu")) // validacja + update
            {
                Int32.TryParse(post["idu"].ToString(), out idu);

                List<string> bledy = new List<string>();

                string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
                MySqlConnection conn = new MySqlConnection(connStr);

                try {
                    conn.Open();

                    if (post["ImieInput"].ToString() == "")
                        bledy.Add("Brak imienia");
                    if (post["NazwiskoInput"].ToString() == "")
                        bledy.Add("Brak nazwiska");

                    if (post["NazwaInput"].ToString() == "")
                        bledy.Add("Brak nazwy użytkownika");
                    else
                    {
                        if (post["NazwaInput"].ToString().Length < 4)
                            bledy.Add("Nazwa ma mniej niż 4 znaki");
                        else
                        {
                            string sql = "SELECT id FROM users WHERE nazwa=@Nazwa AND id <> @Idu;";
                            MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                            zapytanie.Parameters.Add(new MySqlParameter("@Idu", idu));
                            zapytanie.Parameters.Add(new MySqlParameter("@Nazwa", post["NazwaInput"].ToString()));

                            object wynik = zapytanie.ExecuteScalar();

                            if (wynik != null)
                            {
                                bledy.Add("Podana nazwa użytkownika jest już zajęta");
                            }
                        }
                    }

                    if (post["EmailInput"].ToString() == "")
                        bledy.Add("Brak adresu E-mail");
                    else
                    {
                        string emailReg = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
                        Regex reg = new Regex(emailReg);
                        if (!reg.IsMatch(post["EmailInput"].ToString())) bledy.Add("Nieprawidłowy adres E-mail");

                        else
                        {
                            string sql = "SELECT id FROM users WHERE email=@Email AND id <> @Idu;";
                            MySqlCommand zapytanie = new MySqlCommand(sql, conn);
                            zapytanie.Parameters.Add(new MySqlParameter("@Idu", idu));
                            zapytanie.Parameters.Add(new MySqlParameter("@Email", post["EmailInput"].ToString()));

                            object wynik = zapytanie.ExecuteScalar();

                            if (wynik != null)
                            {
                                bledy.Add("Istnieje już użytkownik zarejestrowany na podany adres E-mail");
                            }
                        }
                    }

                    if (post["HasloInput"].ToString() != "")
                    {
                        string znakiSpecjalne = "` ~ ! @ # $ % ^ & * ( ) _ + \\- = \\[ \\] { } , . : ; ' \" | \\\\ / < > ?";
                        
                        Regex reg = new Regex("[^a-ząćęłńóśźżA-ZĄĆĘŁŃÓŚŹŻ0-9 " + znakiSpecjalne + "]");
                        if (reg.IsMatch(post["HasloInput"].ToString()))
                        {
                            string bledyReg = "";
                            foreach (Match match in reg.Matches(post["HasloInput"].ToString()))
                                bledyReg += " " + match.Value + "[" + match.Index + "]";

                            bledy.Add("Hasło zawiera niedozwolony znak/i (znak [pozycja]):" + bledyReg);
                        }
                        if (post["HasloInput2"].ToString() == "")
                            bledy.Add("Brak powtórzenia hasła");
                        else if (post["HasloInput"].ToString() != post["HasloInput2"].ToString())
                            bledy.Add("Hasła nie są identyczne");
                    }
                    //=============================

                    if (bledy.Count > 0)
                    {
                        string bledyHTML = "<ul>";
                        foreach (string val in bledy)
                        {
                            bledyHTML += "<li>" + val + "</li>";
                        }
                        bledyHTML += "</ul>";
                        valid = "<div class=\"wiersz blad\">Formularz zawiera błędy:<br />" + bledyHTML + "</div>";
                    }
                    else // update
                    {
                        sql = "UPDATE users SET imie=@Imie, nazwisko=@Nazwisko, nazwa=@Nazwa, email=@Email WHERE id=@Idu;";
                        zapytanie = new MySqlCommand(sql, conn);

                        if (post["HasloInput"].ToString() != "")
                        {
                            sql = "UPDATE users SET imie=@Imie, nazwisko=@Nazwisko, nazwa=@Nazwa, email=@Email, haslo=@Haslo WHERE id=@Idu;";
                            zapytanie = new MySqlCommand(sql, conn);

                            SHA512 alg = SHA512.Create();
                            byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(post["HasloInput"].ToString()));

                            StringBuilder hash = new StringBuilder();
                            foreach (byte b in result)
                                hash.AppendFormat("{0:x2}", b);

                            zapytanie.Parameters.Add(new MySqlParameter("@Haslo", hash.ToString()));
                        }
                        
                        zapytanie.Parameters.Add(new MySqlParameter("@Idu", idu.ToString()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Imie", post["ImieInput"].ToString()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Nazwisko", post["NazwiskoInput"].ToString()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Nazwa", post["NazwaInput"].ToString()));
                        zapytanie.Parameters.Add(new MySqlParameter("@Email", post["EmailInput"].ToString()));

                        int wynik = zapytanie.ExecuteNonQuery();

                        if (wynik > 0)
                        {
                            conn.Close();
                            Server.Transfer("../Redirect.aspx?a=useredit&link=" + Session["PrevPage"], true);
                        }
                    }
                    
                    valid = "<div id=\"Blad\">" + valid + "</div>";

                    conn.Close();
                }
                catch (MySqlException ex) {
                    Blad.InnerHtml = ex.ToString();
                }
            }
            
            // formularz edycji
            foreach (string key in post.Keys)
            {
                if (key.IndexOf("edit_") != -1)
                {
                    if (Int32.TryParse(key.Substring(5), out idu))
                        break;
                }
            }

            if (idu > 0)
            {
                try
                {
                    conn.Open();

                    sql = "SELECT * FROM users WHERE id=@Idu;";
                    zapytanie = new MySqlCommand(sql, conn);
                    zapytanie.Parameters.Add(new MySqlParameter("@Idu", idu.ToString()));

                    MySqlDataReader reader = zapytanie.ExecuteReader();

                    while (reader.Read())
                    {
                        txt += "<div class=\"wiersz\">";
                        txt += "<div class=\"pole\"><label for=\"ImieInput\">Imię: </label><input id=\"ImieInput\" name=\"ImieInput\" type=\"text\" value=\"" + ((post.ContainsKey("ImieInput")) ? post["ImieInput"] : reader[2].ToString()) + "\" runat=\"server\" /></div>";
                        txt += "<div class=\"pole\"><label for=\"NazwiskoInput\">Nazwisko: </label><input id=\"NazwiskoInput\" name=\"NazwiskoInput\" type=\"text\" value=\"" + ((post.ContainsKey("NazwiskoInput")) ? post["NazwiskoInput"] : reader[3].ToString()) + "\" runat=\"server\" /></div>";
                        txt += "</div>";
                        txt += "<div class=\"wiersz\">";
                        txt += "<div class=\"pole\"><label for=\"NazwaInput\">Nazwa użytkownika: </label><input id=\"NazwaInput\" name=\"NazwaInput\" type=\"text\" value=\"" + ((post.ContainsKey("NazwaInput")) ? post["NazwaInput"] : reader[1].ToString()) + "\" runat=\"server\" /></div>";
                        txt += "<div class=\"pole\"><label for=\"EmailInput\">Adres E-mail: </label><input id=\"EmailInput\" name=\"EmailInput\" type=\"text\" value=\"" + ((post.ContainsKey("EmailInput")) ? post["EmailInput"] : reader[4].ToString()) + "\" runat=\"server\" /></div>";
                        txt += "</div>";
                        txt += "<div class=\"wiersz\">";
                        txt += "<div class=\"pole\"><label for=\"HasloInput\">Hasło: </label><input id=\"HasloInput\" name=\"HasloInput\" type=\"password\" runat=\"server\" /></div>";
                        txt += "<div class=\"pole\"><label for=\"HasloInput2\">Powtórz hasło: </label><input id=\"HasloInput2\" name=\"HasloInput2\" type=\"password\" runat=\"server\" /></div>";
                        txt += "</div>";
                        txt += "<div><input name=\"idu\" type=\"hidden\" value=\"" + idu + "\" /><input id=\"Submit1\" type=\"submit\" value=\"Edytuj\" /></div>";

                        uname = reader[1].ToString();
                    }

                    txt = "<div id=\"tabela\">" + txt + "</div>";

                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    Blad.InnerHtml = ex.ToString();
                }
                admin.InnerHtml = "<div class=\"naglowek\">Edycja konta użytkownika " + uname + "</div>" + valid + txt;
            }
            //========================================

            else if (post.ContainsKey("delete"))
            {
                List<int> delIds = new List<int>();
                foreach (string key in post.Keys)
                    if (key.IndexOf("del_") != -1)
                    {
                        int idDel;
                        if (Int32.TryParse(key.Substring(4), out idDel))
                            delIds.Add(idDel);
                    }

                if (delIds.Count > 0)
                {
                    string lista = String.Join(", ", delIds);

                    try
                    {
                        conn.Open();

                        sql = "DELETE FROM users WHERE id in (" + lista + ");";
                        zapytanie = new MySqlCommand(sql, conn);

                        rows = zapytanie.ExecuteNonQuery();

                        conn.Close();

                        valid = "<div id=\"Blad\"><div class=\"wiersz ok\">Usunięto konta użytkowników: " + rows + "</div></div>";
                    }
                    catch (MySqlException ex)
                    {
                        Blad.InnerHtml = ex.ToString();
                    }

                }
                else Response.Redirect("./Default.aspx?action=uzytkownicy");
            }
            else
            {
                Response.Redirect("./Default.aspx?action=uzytkownicy");
            }

        }
        if (!Page.IsPostBack || rows > 0) // lista uzytkownikow
        {
            string usersBrak = "<div id=\"Blad\"><div class=\"wiersz blad\">Brak kont użytkowników</div></div>";
            try
            {
                conn.Open();

                sql = "SELECT * FROM users;";
                zapytanie = new MySqlCommand(sql, conn);

                MySqlDataReader reader = zapytanie.ExecuteReader();

                while (reader.Read())
                {
                    if (x)
                    {
                        x = false;
                        usersBrak = "";
                        txt += "<div class=\"wiersz head\">";
                        txt += "<div class=\"komorka\">Nazwa</div>";
                        txt += "<div class=\"komorka\">Imię</div>";
                        txt += "<div class=\"komorka\">Nazwisko</div>";
                        txt += "<div class=\"komorka\">Email</div>";
                        txt += "<div class=\"komorka\">Data rejestracji</div>";
                        txt += "<div class=\"komorka\">Typ</div>";
                        txt += "<div class=\"komorka\">Edycja</div>";
                        txt += "<div class=\"komorka\">Usuwanie <input id=\"delAll\" type=\"checkbox\" /></div>";
                        txt += "</div>";
                    }
                    txt += "<div class=\"wiersz\">";
                    txt += "<div class=\"komorka\">" + reader[1].ToString() + "</div>";
                    txt += "<div class=\"komorka\">" + reader[2].ToString() + "</div>";
                    txt += "<div class=\"komorka\">" + reader[3].ToString() + "</div>";
                    txt += "<div class=\"komorka\">" + reader[4].ToString() + "</div>";
                    DateTime uRej = unix.AddSeconds(Convert.ToInt32(reader[6]));
                    txt += "<div class=\"komorka\">" + date0(uRej.Day) + "-" + date0(uRej.Month) + "-" + uRej.Year + "</div>";
                    txt += "<div class=\"komorka\">" + reader[8].ToString() + "</div>";
                    txt += "<div class=\"komorka\"><input type=\"submit\" name=\"edit_" + reader[0].ToString() + "\" value=\"Edytuj\" /></div>";
                    txt += "<div class=\"komorka\"><input type=\"checkbox\" name=\"del_" + reader[0].ToString() + "\" /></div>";
                    txt += "</div>";
                }
                if (usersBrak == ""){
                    txt += "<div class=\"wiersz noborder\">";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\">&nbsp;</div>";
                    txt += "<div class=\"komorka\"><input id=\"delete\" type=\"submit\" name=\"delete\" value=\"Usuń\" /></div>";
                    txt += "</div>";
                }
                

                txt = "<div id=\"tabela\" class=\"u\">" + txt + "</div>";

                conn.Close();
            }
            catch (MySqlException ex)
            {
                Blad.InnerHtml = ex.ToString();
            }
            admin.InnerHtml = "<div class=\"naglowek\">Użytkownicy</div>" + valid + usersBrak + txt;
        }
    }
    protected void wizyty()
    {
        bool x = true;
        string txt = "";
        string wizytyBrak = "<div id=\"Blad\"><div class=\"wiersz blad\">Brak umówionych wizyt</div></div>";
        try
        {
            conn.Open();

            sql = "SELECT w.id, u.imie, u.nazwisko, w.`data`, uu.imie, uu.nazwisko, s.specjalizacja FROM wizyty w JOIN users u ON w.idu=u.id JOIN specjalisci s ON w.ids=s.id JOIN users uu ON s.uid=uu.id WHERE w.`data` >= @Dzis ORDER BY w.`data` DESC;";
            zapytanie = new MySqlCommand(sql, conn);
            zapytanie.Parameters.Add(new MySqlParameter("@Dzis", (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc) - unix).TotalSeconds.ToString()));

            MySqlDataReader reader = zapytanie.ExecuteReader();

            while (reader.Read())
            {
                if (x)
                {
                    x = false;
                    wizytyBrak = "";
                    txt += "<div class=\"wiersz head\">";
                    txt += "<div class=\"komorka\">Imię</div>";
                    txt += "<div class=\"komorka\">Nazwisko</div>";
                    txt += "<div class=\"komorka\">Termin wizyty</div>";
                    txt += "<div class=\"komorka\">Specjalista</div>";
                    txt += "</div>";
                }
                txt += "<div class=\"wiersz\">";
                txt += "<div class=\"komorka\">" + reader[1].ToString() + "</div>";
                txt += "<div class=\"komorka\">" + reader[2].ToString() + "</div>";
                DateTime czasWizyty = unix.AddSeconds(Convert.ToInt32(reader[3]));
                txt += "<div class=\"komorka\">" + date0(czasWizyty.Day) + "-" + date0(czasWizyty.Month) + "-" + czasWizyty.Year + " godz. " + date0(czasWizyty.Hour) + ":" + date0(czasWizyty.Minute) + "</div>";
                txt += "<div class=\"komorka\">" + reader[6].ToString() + " - Dr " + reader[4].ToString() + " " + reader[5].ToString() + "</div>";
                txt += "</div>";
            }

            txt = "<div id=\"tabela\">" + txt + "</div>";

            conn.Close();
        }
        catch (MySqlException ex)
        {
            Blad.InnerHtml = ex.ToString();
        }

        admin.InnerHtml = "<div class=\"naglowek\">Wizyty</div>" + wizytyBrak + txt;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            foreach (string key in Request.Form.Keys)
                post.Add(key,Request.Form[key].Trim());

        if (Request.QueryString["action"] != null)
        {
            if (Request.QueryString["action"] == "ustawienia") ustawienia();
            if (Request.QueryString["action"] == "aktualnosci") aktualnosci();
            if (Request.QueryString["action"] == "uzytkownicy") uzytkownicy();
            if (Request.QueryString["action"] == "wizyty") wizyty();
        }
        else start();

    }
}