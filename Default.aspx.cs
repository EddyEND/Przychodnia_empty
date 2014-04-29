using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Request.QueryString["action"] == "login"){ // jakis inny warunek, ale tak się będzie zmieniac navbar dla zalogowanego
            ContentPlaceHolder c = Page.Master.FindControl("Navbar") as ContentPlaceHolder;
            if (c != null){
                LiteralControl l = new LiteralControl();
                l.Text =   @"<div id='Navbar-Kalendarz'>
                                <a href='#'>Kalendarz</a>
                            </div>
                            <div id='Navbar-Rejestracja'>
                                <a href='#'>Rejestracja</a>
                            </div>
                            <div id='Navbar-Zaloguj'>
                                <a href='Default.aspx?action=logout'>Wyloguj</a>
                            </div>";
                c.Controls.Clear();
                c.Controls.Add(l);
            }
        }
    }
}