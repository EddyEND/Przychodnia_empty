using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Szablon : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string CurrentPage = Path.GetFileName(Request.Path);

        if (CurrentPage == "Kalendarz.aspx")
            NavbarKalendarz.Attributes["class"] = "active";
        else if (CurrentPage == "Rejestracja.aspx" || CurrentPage == "Wizyta.aspx")
            NavbarRejestracja.Attributes["class"] = "active";
        else if (CurrentPage == "Logowanie.aspx")
            NavbarZaloguj.Attributes["class"] = "active";
        
        
        if (Session["zalogowany"] != null)
        {
            NavbarRejestracja.InnerHtml = "<a href='Wizyta.aspx'>Wizyta</a>";
            NavbarZaloguj.InnerHtml = "<div><a href=\"./Wyloguj.aspx\">Wyloguj</a></div>";

            string ap = "<div><a href=\"./admin/\">AP</a></div>";
            if (Session["typ"].ToString() != "A") ap = "";

            NavbarDol.InnerHtml = "<div><div>Witaj, " + Session["imie"] + " " + Session["nazwisko"] + ".</div><div>Nie masz nowych wiadomości.</div><div>Nie masz umówionej wizyty.</div><div><a href=\"./Panel.aspx\">Panel użytkownika</a></div>" + ap + "</div>";
        }

        if (!IsPostBack && Request.UrlReferrer != null)
        {
            Session["PrevPage"] = Request.UrlReferrer.ToString();
        }
    }
}
