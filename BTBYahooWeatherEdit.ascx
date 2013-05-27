<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnHlp" TagName="HelpButton" Src="~/controls/HelpButtonControl.ascx" %>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BTBYahooWeatherEdit.ascx.cs" Inherits="BiteTheBullet.DNN.Modules.BTBYahooWeather.BTBYahooWeatherEdit" %>
<table border="0" cellpadding="3" cellspacing="0">
	<tr>
		<td class="SubHead" valign="top"><dnn:label id="plFeedCode" runat="server" suffix=":" controlname="txtFeedCode"></dnn:label></td>
		<td>
			<asp:listbox id="lstLocations" runat="server" DataTextField="AdminDisplayName" DataValueField="WeatherId"
				Width="350px" Rows="5"></asp:listbox>
			<asp:imagebutton id="cmdDeleteImage" runat="server" ImageUrl="~/images/delete.gif" AlternateText="Delete Selected Image"
				CausesValidation="False"></asp:imagebutton>
			<dnnHlp:helpbutton id="hbtnDeleteImageHelp" runat="server" resourcekey="cmdDeleteImage"></dnnHlp:helpbutton>
		</td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plAddNewFeed" runat="server" suffix=":" controlname="txtFeedCode"></dnn:label></td>
		<td><asp:TextBox id="txtFeedCode" runat="server" Columns="10"></asp:TextBox>
			<asp:LinkButton id="lbnAddFeed" runat="server" resourceKey="lbnAddFeed" CssClass="CommandButton"></asp:LinkButton></td>
	</tr>
	<tr>
		<td colspan="2">
			<asp:RequiredFieldValidator id="rfvWeatherCode" runat="server" ControlToValidate="txtFeedCode" resourceKey="rfvWeatherCode"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plDefaultFeed" runat="server" suffix=":" controlname="rbTempC"></dnn:label></td>
		<td><asp:DropDownList id="ddlDefaultFeed" runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plUserPersist" runat="server" suffix=":" controlname="chkUserPersist"></dnn:label></td>
	    <td>
	        <asp:CheckBox ID="chkUserPersist" runat="server" resourcekey="chkUserPersist" />
	    </td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plTemperatureScale" runat="server" suffix=":" controlname="rbTempC"></dnn:label></td>
		<td>
			<asp:RadioButton id="rbTempC" runat="server" ResourceKey="rbTempC.Text" Checked="True" GroupName="TemperatureScale"
				CssClass="Normal"></asp:RadioButton>
			<asp:RadioButton id="rbTempF" runat="server" ResourceKey="rbTempF.Text" GroupName="TemperatureScale"
				CssClass="Normal"></asp:RadioButton></td>
	</tr>
    <tr>
        <td class="SubHead" valign="top"><dnn:label id="lblRenderEngine" runat="server" suffix=":" controlname="rbXlst"></dnn:label></td>
		<td>
			<asp:RadioButton id="rbXlst" runat="server" ResourceKey="rbXlst.Text" GroupName="RenderEngine"
				CssClass="Normal"></asp:RadioButton>
			<asp:RadioButton id="rbRazor" runat="server" ResourceKey="rbRazor.Text" GroupName="RenderEngine"
				CssClass="Normal"></asp:RadioButton>
    </tr>
	<tr>
		<td class="SubHead" valign="top"><dnn:label id="plDisplayType" runat="server" suffix=":" controlname="rbNormalDisplay"></dnn:label></td>
		<td>
			<asp:DropDownList runat="server" ID="ddlXsltTemplates" CssClass="ddlXsltTemplates"/>
			<asp:DropDownList runat="server" ID="ddlRazorTemplates" CssClass="ddlRazorTemplates"/>
		</td>
	</tr>
</table>
<asp:linkbutton id="cmdUpdate" CssClass="CommandButton" runat="server" BorderStyle="None" resourcekey="cmdUpdate"
	CausesValidation="False"></asp:linkbutton>
<asp:linkbutton id="cmdCancel" CssClass="CommandButton" runat="server" CausesValidation="False"
	BorderStyle="None" resourcekey="cmdCancel"></asp:linkbutton>
