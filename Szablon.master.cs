using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Szablon : System.Web.UI.MasterPage
{
    static string[] plMiesiace = new string[12] { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
    protected string nextWizyta()
    {
        DateTime unix = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
        MySqlConnection conn = new MySqlConnection(connStr);
        string sql;
        MySqlCommand zapytanie;
        MySqlDataReader wynik;
        string tekst = "Nie masz umówionej wizyty.";
        int specVal = 0, dataVal = 0, zarejestrowanych = 0, godzOtwarcia = 9;

        try
        {
            conn.Open();
        
            sql = "SELECT * FROM wizyty WHERE idu=@Idu AND `data` >= @Dzis ORDER BY data LIMIT 1;";
            zapytanie = new MySqlCommand(sql, conn);

            zapytanie.Parameters.Add(new MySqlParameter("@Idu", Session["id"].ToString()));
            zapytanie.Parameters.Add(new MySqlParameter("@Dzis", (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Utc) - unix).TotalSeconds.ToString()));

            wynik = zapytanie.ExecuteReader();

            if (wynik.Read()){
                unix = unix.AddSeconds(Convert.ToInt32(wynik[3]));
                //tekst = "Kolejna wizyta: " + unix.Day + " " + plMiesiace[unix.Month - 1] + " " + unix.Year;
                tekst = "Kolejna wizyta: " + unix.Day + " " + plMiesiace[unix.Month - 1] + " " + unix.Year + " o godz. " + unix.Hour + ":" + ((unix.Minute > 9) ? unix.Minute.ToString() : ("0" + unix.Minute));
            }
            wynik.Close();

            return tekst;
            conn.Close();
        }
        catch (MySqlException ex)
        {
            return tekst;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string CurrentPage = Path.GetFileName(Request.Path);

        if (CurrentPage == "Kalendarz.aspx")
            NavbarKalendarz.Attributes["class"] = "active";
        else if (CurrentPage == "Rejestracja.aspx" || CurrentPage == "Wizyta.aspx")
            NavbarRejestracja.Attributes["class"] = "active";
        else if (CurrentPage == "Logowanie.aspx")
            NavbarZaloguj.Attributes["class"] = "active";
        
        if (!IsPostBack && Request.UrlReferrer != null)
        {
            Session["PrevPage"] = Request.UrlReferrer.ToString();
        }
        
        if (Session["zalogowany"] != null)
        {
            NavbarRejestracja.InnerHtml = "<a href='Wizyta.aspx'>Wizyta</a>";
            NavbarZaloguj.InnerHtml = "<div><a href=\"./Wyloguj.aspx\">Wyloguj</a></div>";

            string ap = "<div><a href=\"./admin/\">AP</a></div>";
            if (Session["typ"].ToString() != "A") ap = "";

            NavbarDol.InnerHtml = "<div><div>Witaj, " + Session["imie"] + " " + Session["nazwisko"] + ".</div><div>" + nextWizyta() + "</div><div><a href=\"./Panel.aspx\">Panel użytkownika</a></div>" + ap + "</div>";
        }
    }
}
