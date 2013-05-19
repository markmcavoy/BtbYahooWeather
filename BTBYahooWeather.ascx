<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BTBYahooWeather.ascx.cs" Inherits="BiteTheBullet.DNN.Modules.BTBYahooWeather.BTBYahooWeather" %>
<asp:Panel id="pnlDropDown" runat="server" Visible="False"><p><asp:Label id="lblLocation" runat="server" resourceKey="lblLocation" CssClass="Normal"></asp:Label>&nbsp;
	<asp:DropDownList id="ddlLocations" runat="server" AutoPostBack="True"></asp:DropDownList></p></asp:Panel>

<asp:Literal id="litOutput" runat="server"></asp:Literal>
