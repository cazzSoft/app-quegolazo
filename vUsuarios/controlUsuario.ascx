<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="controlUsuario.ascx.vb" Inherits="vUsuarios.controlUsuario" %>
<style type="text/css">
    .style1
    {
        height: 26px;
    }
    .vusuariosTexto
{
	font-family: verdana;
	font-size: 13px;
	color:Black;
		
}
/*configuración estándar de espacios en tablas*/
/*espacio horizontal y vertical*/
.vusuariosEsp
{
    width: 20px;
    height: 20px;
}
/*espacio solo vertical*/
.vusuariosEspv
{
    height: 20px;
}
/*espacio horizontal*/
.vusuariosEsph
{  
    width: 20px;
}
/**bordes/
/*borde total*/
.vusuariosBordes
{
    border-right: #305876 1px solid;
    border-top: #305876 1px solid;
    border-left: #305876 1px solid;
    border-bottom: #305876 1px solid;
}
/*titulo de gridview*/
.vusuariosGrdTitulo
{
    font-family: verdana;
    font-size: 13px;
    text-align: left;
    color: White;
    font-weight: bold;
    background-color: #305876;
}
.vusuariosAlerta
{
    font-family: verdana;
	font-size: 13px;
	text-align:left;
	color: #ff0000;
	
    }
    
    /*links clase link*/
A.vusuariosLink:link
{
    TEXT-DECORATION: underline;
    font-family: verdana;
	font-size: 13px;
	text-align:left;
	color: #5088A6;  
	
}    
A.vusuariosLink:visited
{
    TEXT-DECORATION: underline;
    font-family: verdana;
	font-size: 13px;
	text-align:left;
	color: #103856; 
}
A.vusuariosLink:active 
{
    
    TEXT-DECORATION: underline;
    font-family: verdana;
	font-size: 13px;
	text-align:left;
	color: #5088A6;  
} 
A.vusuariosLink:hover 
{
    TEXT-DECORATION: none;
    font-family: verdana;
	font-size: 13px;
	text-align:left;
	color: #5088A6;  
}
</style>
<table cellpadding="0" cellspacing="0" border="0" width="450" 
    id="TBLmostrarLogin" runat="server" class="vusuariosBordes">
    <tr class="vusuariosGrdTitulo">
        <td>
            &nbsp;</td>
        <td class="vusuariosLink">
            Login</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="vusuariosEsp">
            &nbsp;</td>
        <td width="180">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblNombreUsuario" runat="server" Text="Nombre de usuario"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNombreUsuario" runat="server" Width="125px"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblPassword" runat="server" Text="Contraseña"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="125px"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            <asp:CheckBox ID="chkRecordar" runat="server" 
                Text="Recordarme en este equipo" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            &nbsp;&nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            </td>
        <td colspan="2" class="vusuariosTexto">
            <asp:Label ID="lblLogueado" runat="server" ForeColor="#305876" Visible="False" 
                Font-Bold="True">ha ingresado en el sistema</asp:Label>
        </td>
        <td>
            </td>
    </tr>
    <tr>
        <td>
            </td>
        <td>
            </td>
        <td>
            <asp:Label ID="lblErrorValidar" runat="server" ForeColor="Red" 
                Text="Usuario no válido" CssClass="vusuariosAlerta"></asp:Label>
            <asp:Label ID="lblUsuarioDesactivado" runat="server" ForeColor="Red" 
                Text="Usuario desactivado" CssClass="vusuariosAlerta" Visible="False"></asp:Label>
            <asp:Label ID="lblPasswordExpiro" runat="server" ForeColor="Red" 
                Text="Su contraseña ha expirado. Contáctese con el Dpto. Sistemas" 
                CssClass="vusuariosAlerta" Visible="False"></asp:Label>
        </td>
        <td>
            </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" Width="75px" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="vusuariosEsp">
            &nbsp;</td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0" border="0" width="450" 
    id="TBLmostrarCambioPWD" runat="server" class="vusuariosBordes">
    <tr class="vusuariosGrdTitulo">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblTituloCambio" runat="server" Text="Cambio de contraseña"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="vusuariosEsp">
            &nbsp;</td>
        <td width="180">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblExpira" runat="server" Text="Password expira el:"></asp:Label>
        &nbsp;<asp:Label ID="lblExpiraFecha" runat="server" Text="fecha"></asp:Label>
        </td>
        <td class="vusuariosLink">
            <asp:LinkButton ID="lbtnCambiarTarde" runat="server">Cambiar más tarde</asp:LinkButton>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="vusuariosEsp">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblPasswordActual" runat="server" Text="Contraseña actual"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPasswordActual" runat="server" TextMode="Password" 
                Width="125px"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblPasswordNuevo" runat="server" Text="Contraseña nueva"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPasswordNuevo" runat="server" TextMode="Password" 
                Width="125px"></asp:TextBox>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr class="vusuariosTexto">
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="lblPasswordNuevoReingreso" runat="server" 
                Text="Confirmación contraseña"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPasswordNuevoReingreso" runat="server" TextMode="Password" 
                Width="125px"></asp:TextBox>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="vusuariosAlerta">
            <asp:Label ID="lblErrorCambio" runat="server" ForeColor="Red" 
                Text="Contraseña no valida"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="vusuariosEsp">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style1">
            &nbsp;</td>
        <td class="style1">
        </td>
        <td class="style1">
            <asp:Button ID="btnCambiar" runat="server" Text="Cambiar" Width="75px" />
        </td>
        <td class="style1">
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="vusuariosEsp">
            &nbsp;</td>
    </tr>
</table>
<p>
    &nbsp;</p>
<p>
    &nbsp;</p>

