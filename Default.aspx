<%@ Page Title="Stron główna" Language="C#" MasterPageFile="~/Szablon.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ContentPlaceholderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceholderID="Navbar" runat="server">
    <div id="Navbar-Kalendarz">
        <a href="#">Kalendarz</a>
    </div>
    <div id="Navbar-Rejestracja">
        <a href="#">Rejestracja</a>
    </div>
    <div id="Navbar-Zaloguj">
        <a href="Default.aspx?action=login">Zaloguj</a>
    </div>
</asp:Content>
<asp:Content ContentPlaceholderID="MainContent" runat="server">
    <div id="Tresc">
        <div><%=Application["i"] %></div><%-- wyświetlanie zmiennych --%>
    </div>
    <div id="Informacje-Full">
        <div>
            <div class="Informacje-Pol">
                <div>
                    <h2>Aktualności</h2>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                </div>
            </div>
            <div class="Informacje-Pol">
                <div>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                </div>
            </div>
            <div class="Informacje-Pol">
                <div>
                    <p>Trzecia Informacja</p>
                    <p>Trzecia Informacja</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>