<%@ Page Title="Stron główna" Language="C#" MasterPageFile="~/Szablon.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ContentPlaceholderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceholderID="MainContent" runat="server">
    <div id="TrescGlowna">
        <div id="TrescZawartosc" runat="server"></div>
    </div>
    <div id="InformacjeFull">
        <div>
            <div class="InformacjePol">
                <div id="inf1" runat="server">
                    <h2>Aktualności</h2>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                </div>
            </div>
            <div class="InformacjePol">
                <div id="inf2" runat="server">
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                </div>
            </div>
            <div class="InformacjePol">
                <div id="inf3" runat="server">
                    <p>Trzecia Informacja</p>
                    <p>Trzecia Informacja</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>