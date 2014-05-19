﻿<%@ Page Title="Umów wizytę" Language="C#" MasterPageFile="~/Szablon.master" AutoEventWireup="true" CodeFile="Wizyta.aspx.cs" Inherits="Wizyta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="ramka">
        <div id="r0" runat="server"> 
            <div class="naglowek">Umów wizytę w naszej przychodni</div>
            <div id="Blad" runat="server"></div>
            <div class="wiersz">
                <div id="dataInput" class="pole">
                    <label for="DzienInput">Data: </label>
                    <select id="DzienInput" name="DzienInput" runat="server"></select>
                    <select id="MiesiacInput" name="MiesiacInput" runat="server"></select>
                    <select id="RokInput" name="RokInput" runat="server"></select>
                </div>
            </div>
            <div class="wiersz">
                <div id="specInput" class="pole"><label for="SpecjalistaInput">Specjalista: </label><select id="SpecjalistaInput" name="SpecjalistaInput" runat="server"></select></div>
            </div>
            <input type="submit" id="Submit1" value="Umów wizytę" />
        </div>
    </div>
</asp:Content>