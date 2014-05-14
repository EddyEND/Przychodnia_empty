<%@ Page Title="Umów wizytę" Language="C#" MasterPageFile="~/Szablon.master" AutoEventWireup="true" CodeFile="Wizyta.aspx.cs" Inherits="Wizyta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="ramka">
        <div id="r1" runat="server"> 
            <div class="naglowek">Umów wizytę w naszej przychodni</div>
            <div id="blad" runat="server"></div>
            <div class="wiersz">
                <div class="pole"><label for="DataInput">Data: </label><input id="DataInput" name="DataInput" type="text" runat="server" /></div>
                <div class="pole"><label for="GodzinaInput">Godzina: </label><input id="Godzina" name="Godzina" type="text" runat="server" /></div>
            </div>
            <div class="wiersz">
                <div class="pole"><label for="SpecjalistaInput">Specjalista: </label><select id="SpecjalistaInput" name="SpecjalistaInput" runat="server"></select></div>
            </div>
            <input type="submit" id="Submit1" value="Umów wizytę" />
        </div>
    </div>
</asp:Content>