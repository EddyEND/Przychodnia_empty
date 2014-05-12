using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        { // jakis inny warunek, ale tak się będzie zmieniac navbar dla zalogowanego
            NavbarRejestracja.InnerHtml = "<a href='Wizyta.aspx'>Wizyta</a>";
            NavbarZaloguj.InnerHtml = "<a href='Wyloguj.aspx'>Wyloguj</a>";
        }

        if (!IsPostBack && Request.UrlReferrer != null)
        {
            Session["PrevPage"] = Request.UrlReferrer.ToString();
        }
    }
}
