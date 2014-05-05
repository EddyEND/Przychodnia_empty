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
        
        
        if (Request.QueryString["action"] == "login")
        { // jakis inny warunek, ale tak się będzie zmieniac navbar dla zalogowanego
            NavbarZaloguj.InnerHtml = "<a href='Default.aspx?action=logout'>Wyloguj</a>";
        }
    }
}
