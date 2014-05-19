using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Wizyta : System.Web.UI.Page
{
    private static string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
    private static MySqlConnection conn = new MySqlConnection(connStr);

    private string sql;
    private MySqlCommand zapytanie;

    private static string[] plMiesiace = new string[12] { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

    private int rokVal, miesiacVal, dzienVal, specVal, dataVal, zarejestrowanych;
    private static int godzOtwarcia = 9;

    private DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private TimeSpan elapsedTime;

    protected void specjalistaFill()
    {
        int specVal = 0;
        ListItem specjalista;
        if (Page.IsPostBack)
        {
            Int32.TryParse(Request.Form[SpecjalistaInput.UniqueID], out specVal);
        }
        SpecjalistaInput.Items.Clear();
        SpecjalistaInput.Items.Add(new ListItem("Brak specjalistów", ""));
        try
        {
            conn.Open();

            sql = "SELECT s.id, u.imie, u.nazwisko, s.specjalizacja FROM users u JOIN specjalisci s ON u.id=s.uid;";
            zapytanie = new MySqlCommand(sql, conn);

            MySqlDataReader wynik = zapytanie.ExecuteReader();

            bool x = true;
            while (wynik.Read())
            {
                if (x)
                {
                    SpecjalistaInput.Items.Clear();
                    SpecjalistaInput.Items.Add(new ListItem("Wybierz specjalistę...", ""));
                    x = false;
                }

                specjalista = new ListItem(wynik[3].ToString() + " - Dr " + wynik[1].ToString() + " " + wynik[2].ToString(), wynik[0].ToString());
                if (specVal == Convert.ToInt32(wynik[0]))
                    specjalista.Selected = true;
                SpecjalistaInput.Items.Add(specjalista);
            }
            
            conn.Close();
        }
        catch (MySqlException ex)
        {
            Blad.InnerHtml = ex.ToString();
        }
    }
    protected void dzienFill(DateTime dateTime){
        int dzienVal = dateTime.Day;
        if (Page.IsPostBack)
        {
            Int32.TryParse(Request.Form[DzienInput.UniqueID], out dzienVal);
        }
        DzienInput.Items.Clear();

        ListItem dzien = new ListItem("Dzień", "0");
        dzien.Attributes.Add("disabled", "true");
        DzienInput.Items.Add(dzien);

        for (int i = 0; i < 31; i++)
        {
            dzien = new ListItem((i + 1).ToString(), (i + 1).ToString());
            if (dzienVal == i + 1)
                dzien.Selected = true;
            DzienInput.Items.Add(dzien);
        }
    }
    protected void miesiacFill(DateTime dateTime)
    {
        int miesiacVal = dateTime.Month;
        if (Page.IsPostBack)
        {
            Int32.TryParse(Request.Form[MiesiacInput.UniqueID], out miesiacVal);
        }
        MiesiacInput.Items.Clear();

        ListItem miesiac = new ListItem("Miesiac", "0");
        miesiac.Attributes.Add("disabled", "true");
        MiesiacInput.Items.Add(miesiac);

        for (int i = 0; i < 12; i++)
        {
            miesiac = new ListItem(plMiesiace[i], (i + 1).ToString());
            if (miesiacVal == i + 1)
                miesiac.Selected = true;
            MiesiacInput.Items.Add(miesiac);
        }
    }
    protected void rokFill(DateTime dateTime)
    {
        int rokVal = dateTime.Year;
        if (Page.IsPostBack)
        {
            Int32.TryParse(Request.Form[RokInput.UniqueID], out rokVal);
        }
        RokInput.Items.Clear();

        ListItem rok = new ListItem("Rok", "0");
        rok.Attributes.Add("disabled", "true");
        RokInput.Items.Add(rok);
        
        for (int i = 0; i < 2; i++)
        {
            rok = new ListItem((i + dateTime.Year).ToString(), (i + dateTime.Year).ToString());
            if (rokVal == i + dateTime.Year) 
                rok.Selected = true;
            RokInput.Items.Add(rok);
        }
    }
    protected bool sprWizyta()
    {
        List<string> bledy = new List<string>();

        if (!Int32.TryParse(Request.Form[RokInput.UniqueID], out rokVal))
            bledy.Add("Nie wybrano roku lub nieprawidłowy.");
        else if (!Int32.TryParse(Request.Form[MiesiacInput.UniqueID], out miesiacVal))
            bledy.Add("Nie wybrano miesiąca lub nieprawidłowy.");
        else if (!Int32.TryParse(Request.Form[DzienInput.UniqueID], out dzienVal))
            bledy.Add("Nie wybrano dnia lub nieprawidłowy.");
        else
        {
            if (rokVal < DateTime.Today.Year || rokVal > DateTime.Today.Year + 1)
                bledy.Add("Niewłaściwy rok. [" + DateTime.Today.Year + " - " + (DateTime.Today.Year + 1) + "]");
            else if ((rokVal == DateTime.Today.Year && (miesiacVal < DateTime.Today.Month || miesiacVal > 12)) || (rokVal > DateTime.Today.Year && (miesiacVal < 1 || miesiacVal > 12)))
                bledy.Add("Niewłaściwy miesiąc. [" + plMiesiace[DateTime.Today.Month-1] + " - " + plMiesiace[11] + "]");
            else if ((miesiacVal == DateTime.Today.Month && (dzienVal < DateTime.Today.Day || dzienVal > DateTime.DaysInMonth(rokVal, miesiacVal))) || (miesiacVal > DateTime.Today.Month && (dzienVal < 1 || dzienVal > DateTime.DaysInMonth(rokVal, miesiacVal))))
                bledy.Add("Niewłaściwy dzień. " + plMiesiace[miesiacVal - 1] + " [" + ((miesiacVal == DateTime.Today.Month && (dzienVal < DateTime.Today.Day || dzienVal > DateTime.DaysInMonth(rokVal, miesiacVal))) ? DateTime.Today.Day : 1) + " - " + DateTime.DaysInMonth(rokVal, miesiacVal) + "]");
        }

        if (!Int32.TryParse(Request.Form[SpecjalistaInput.UniqueID], out specVal))
            bledy.Add("Nie wybrano specjalisty lub nieprawidłowy.");
        else
        {
            try
            {
                conn.Open();

                sql = "SELECT id FROM specjalisci WHERE id=@Id;";
                zapytanie = new MySqlCommand(sql, conn);

                zapytanie.Parameters.Add(new MySqlParameter("@Id", specVal));

                object wynik = zapytanie.ExecuteScalar();

                if (wynik == null)
                {
                    bledy.Add("Nie ma takiego specjalisty.");
                }
                else
                {
                    sql = "SELECT * FROM wizyty WHERE idu=@Idu AND `data` >= @Dzis;";
                    zapytanie = new MySqlCommand(sql, conn);

                    zapytanie.Parameters.Add(new MySqlParameter("@Idu", Session["id"].ToString()));
                    zapytanie.Parameters.Add(new MySqlParameter("@Dzis", (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc) - unix).TotalSeconds.ToString()));

                    MySqlDataReader wyn = zapytanie.ExecuteReader();

                    while (wyn.Read())
                    {
                        if (Convert.ToInt32(wyn[2]) == specVal)
                        {
                            bledy.Add("Masz już umówioną wizytę u tego specjalisty.");
                            break;
                        }
                    }
                    wyn.Close();

                    // sprawdzenie czy zdazy w tym dniu
                    sql = "SELECT COUNT(*) FROM wizyty WHERE ids=@Ids AND `data` >= @DataMin AND `data` < @DataMax;";
                    zapytanie = new MySqlCommand(sql, conn);

                    zapytanie.Parameters.Add(new MySqlParameter("@Ids", specVal));

                    elapsedTime = new DateTime(rokVal, miesiacVal, dzienVal, 0, 0, 0, DateTimeKind.Utc) - unix;
                    zapytanie.Parameters.Add(new MySqlParameter("@DataMin", elapsedTime.TotalSeconds.ToString()));
                    elapsedTime = new DateTime(rokVal, miesiacVal, dzienVal, 0, 0, 0, DateTimeKind.Utc).AddDays(1) - unix;
                    zapytanie.Parameters.Add(new MySqlParameter("@DataMax", elapsedTime.TotalSeconds.ToString()));

                    wynik = zapytanie.ExecuteScalar();

                    if (wynik != null)
                    {
                        zarejestrowanych = Convert.ToInt32(wynik);
                        if (zarejestrowanych > 15)
                            bledy.Add("W tym dniu mamy zbyt wielu pacjentów.<br />Prosimy o umówienie wizyty w innym terminie.");
                    }
                    else zarejestrowanych = 0;
                        
                }

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
            return false;
        }
        else 
        {
            Blad.InnerHtml = "";
            return true;
        } 
    }
    protected void insertWizyta()
    {
        try
        {
            conn.Open();

            sql = "INSERT INTO wizyty VALUES(@Id, @Idu, @Ids, @Data);";
            zapytanie = new MySqlCommand(sql, conn);
            zapytanie.Parameters.Add(new MySqlParameter("@Id", 0));
            zapytanie.Parameters.Add(new MySqlParameter("@Idu", Session["id"].ToString()));
            zapytanie.Parameters.Add(new MySqlParameter("@Ids", specVal));

            elapsedTime = new DateTime(rokVal, miesiacVal, dzienVal, 0, 0, 0, DateTimeKind.Utc).AddSeconds(godzOtwarcia * 3600 + zarejestrowanych * 1800) - unix;
            dataVal = (int)elapsedTime.TotalSeconds;

            zapytanie.Parameters.Add(new MySqlParameter("@Data", dataVal.ToString()));

            unix = unix.AddSeconds(elapsedTime.TotalSeconds);

            int wynik = zapytanie.ExecuteNonQuery();

            if (wynik > 0)
            {
                HtmlGenericControl div = new HtmlGenericControl("div");

                div.InnerHtml = "Jesteś umówiony/a na wizytę dnia " + unix.Day + " " + plMiesiace[unix.Month - 1] + " " + unix.Year + " o godzinie " + unix.Hour + ":" + ((unix.Minute > 9) ? unix.Minute.ToString() : ("0" + unix.Minute));
                r0.Controls.Clear();
                r0.Controls.Add(div);
            }

            conn.Close();
        }
        catch (MySqlException ex)
        {
            Blad.InnerHtml = ex.ToString();
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["zalogowany"] == null)
            Server.Transfer("Redirect.aspx?a=permission", true);

        specjalistaFill();
        dzienFill(DateTime.Now);
        miesiacFill(DateTime.Now);
        rokFill(DateTime.Now);

        if (Page.IsPostBack)
        {
            if (sprWizyta())
                insertWizyta();
        }
    }
}