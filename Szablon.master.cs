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
        else if (CurrentPage == "Rejestracja.aspx")
            NavbarRejestracja.Attributes["class"] = "active";
        else if (CurrentPage == "Logowanie.aspx")
            NavbarZaloguj.Attributes["class"] = "active";
        
        
        if (Session["zalogowany"] != null)
        {
            NavbarRejestracja.InnerHtml = "<a href='Wizyta.aspx'>Wizyta</a>";

            NavbarZaloguj.Attributes.Add("class", "menu");
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.InnerHtml += "<div><a href=\"./Panel.aspx\">Panel użytkownika</a></div>";
            div.InnerHtml += "<div><a href=\"./Wyloguj.aspx\">Wyloguj</a></div>";
            NavbarZaloguj.InnerHtml = "<div><a href=\"\" onclick=\"javascript: return false;\">" + Session["nazwa"] + "</a></div>"; // dodac jquery i preventdefault
            NavbarZaloguj.Controls.Add(div);
        }

        if (!IsPostBack && Request.UrlReferrer != null)
        {
            Session["PrevPage"] = Request.UrlReferrer.ToString();
        }
    }
}
