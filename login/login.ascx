<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/DesktopModules/GSS_Login/Login.ascx.cs" Inherits="GSS.AppServices.WebUI.Security.Login" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<br />
<br />
<br />
<br />
<div align="center">
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" DefaultButton="loginButton" Width="300px" >
    <asp:Label ID="messageLabel" runat="server" CssClass="NormalRed" />
    <table cellpadding="4" style="border: 1px solid #C0C0C0;" align="center" class="bodycontent">
        <tr>
            <td class="Normal">User Name:</td>
            <td>
                <asp:TextBox ID="userNameTextBox" runat="server"   Width="200px"
                    CssClass="NormalTextBox" ></asp:TextBox>
                <cc1:TextBoxWatermarkExtender runat="server" Enabled="True" 
                    ID="userNameTextBox_TextBoxWatermarkExtender" 
                    TargetControlID="userNameTextBox" 
                    WatermarkText="firstname.lastname@ubagroup.com">
                </cc1:TextBoxWatermarkExtender>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                    runat="server" CssClass="NormalRed" Display="Dynamic" 
                    ErrorMessage="*" ControlToValidate="userNameTextBox" 
                    ToolTip="Please enter your user name." />
            </td>
        </tr>
        <tr>
            <td class="Normal">Password:</td>
            <td>
                <asp:TextBox ID="passwordTextBox" runat="server" CssClass="NormalTextBox"   Width="200px"
                    TextMode="Password" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    CssClass="NormalRed" Display="Dynamic" ErrorMessage="*" 
                    ControlToValidate="passwordTextBox" 
                    ToolTip="Please enter your password." />
            </td>
        </tr>
        <tr>
            <td class="Normal">Token:</td>
            <td>
                <asp:TextBox ID="txtentrust" runat="server"  
                    CssClass="NormalTextBox"  Width="200px"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender runat="server" Enabled="True" 
                    ID="TextBoxWatermarkEntrust" 
                    TargetControlID="txtentrust" 
                    WatermarkText="Insert the generated code from your entrust token">
                </cc1:TextBoxWatermarkExtender>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RFVEntrust" 
                    runat="server" CssClass="NormalRed" Display="Dynamic" 
                    ErrorMessage="*" ControlToValidate="txtentrust" 
                    ToolTip="Token" />
            </td>
        </tr><tr>
            <td></td>
            <td align="center">
                <asp:Button ID="loginButton" runat="server" CssClass="StandardButton" 
                    Text="Login" onclick="loginButton_Click" />
            </td>
            <td></td>
        </tr>
    </table>
</asp:Panel>
    </div>