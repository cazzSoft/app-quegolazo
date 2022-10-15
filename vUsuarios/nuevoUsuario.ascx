<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="nuevoUsuario.ascx.vb"
    Inherits="vUsuarios.nuevoUsuario" %>
<style type="text/css">
    .vusuariosTexto
    {
        font-family: verdana;
        font-size: 13px;
        color: Black;
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
        text-align: left;
        color: #ff0000;
    }
    /*links clase link*/
    A.vusuariosLink:link
    {
        text-decoration: underline;
        font-family: verdana;
        font-size: 13px;
        text-align: left;
        color: #5088A6;
    }
    A.vusuariosLink:visited
    {
        text-decoration: underline;
        font-family: verdana;
        font-size: 13px;
        text-align: left;
        color: #103856;
    }
    A.vusuariosLink:active
    {
        text-decoration: underline;
        font-family: verdana;
        font-size: 13px;
        text-align: left;
        color: #5088A6;
    }
    A.vusuariosLink:hover
    {
        text-decoration: none;
        font-family: verdana;
        font-size: 13px;
        text-align: left;
        color: #5088A6;
    }
</style>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" runat="server" id="TBPersonas">
                <tr>
                    <td class="vusuariosTexto">
                        <br />
                        <asp:Label ID="lblListaPersonas" runat="server" Text="Posibles usuarios" 
                            CssClass="vusuariosTexto"></asp:Label>
                        &nbsp;<asp:LinkButton ID="lbtnNuevoUsuario" runat="server" 
                            CssClass="vusuariosLink">Nuevo Usuario</asp:LinkButton>
                        <br />
                        &nbsp;<asp:GridView ID="gdvPersonas" runat="server" Width="100%" AllowPaging="True"
                            CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="vusuariosTexto" />
                            <Columns>
                                <asp:TemplateField HeaderText="Usuario">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPersona" runat="server" OnClick="lnkPersona_Click1" CommandArgument='<%#Eval("id")%>'> <%#Eval("Persona")%></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="fechaCreacion" DataFormatString="{0:dd/MMM/yyyy}" HeaderText="Fecha Creación" />
                                <asp:BoundField DataField="activo" HeaderText="Cuenta Activa" />
                            </Columns>
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" CssClass="vusuariosTexto" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle Font-Bold="True" ForeColor="White" CssClass="vusuariosGrdTitulo" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
   
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" runat="server" id="TBPersona"
                class="vusuariosBordes">
                <tr class="vusuariosGrdTitulo">
                    <td valign="top">
                        &nbsp;
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblPersonas" runat="server" Text="Ingrese información de la persona"></asp:Label>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="vusuariosEsp">
                        &nbsp;
                    </td>
                    <td width="100">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="vusuariosTexto">
                        <asp:Label ID="lblNombre1" runat="server" Text="Nombre"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombre1" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="vusuariosTexto">
                        <asp:Label ID="lblApellidos" runat="server" Text="Apellido"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtApellido1" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="vusuariosTexto">
                        <asp:Label ID="lblMail" runat="server" Text="Correo"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCorreo" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="vusuariosEsp">
                        &nbsp;
                    </td>
                    <td>
                        <hr />
                    </td>
                    <td>
                        <hr />
                    </td>
                    <td class="vusuariosEsp">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="vusuariosTexto" width="150">
                        <asp:Label ID="lblUsuario" runat="server" Text="Nombre de usuario"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreUsuario" runat="server"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td valign="top" class="vusuariosTexto">
                        Permisos
                    </td>
                    <td>
                       
                        <asp:TreeView ID="trvPermisos" runat="server" NodeIndent="10" ShowCheckBoxes="All"
                            ShowExpandCollapse="False">
                            <ParentNodeStyle Font-Bold="False" />
                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                VerticalPadding="0px" />
                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px"
                                NodeSpacing="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </td><td></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="vusuariosTexto">
                        <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbtlEstado" runat="server" 
                            RepeatDirection="Horizontal" CssClass="vusuariosTexto">
                            <asp:ListItem Selected="True" Value="1">Activado</asp:ListItem>
                            <asp:ListItem Value="0">Desactivado</asp:ListItem>
                        </asp:RadioButtonList>
                    </td><td></td>
                </tr>
                <tr>
                    <td class="vusuariosEsp">&nbsp;</td>
                    <td class="vusuariosEspv">
                        &nbsp;</td>
                    <td class="vusuariosTexto">
                        &nbsp;</td><td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td class="vusuariosTexto" bgcolor="#EEEEEE">
                        Clave</td>
                    <td class="vusuariosTexto" bgcolor="#EEEEEE">
                        <asp:CheckBox ID="chkAsignarClave" runat="server" Checked="True" 
                            Text="Recuperar clave" />
                    </td><td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="vusuariosEspv">
                        &nbsp;</td>
                    <td>
                        &nbsp;&nbsp; </td><td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="vusuariosEspv">
                        &nbsp;</td>
                    <td class="vusuariosTexto">
                        &nbsp;&nbsp;<asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="#FF3300" 
                            Text="Revise sus datos, no se puede crear el usuario"></asp:Label>
                    </td><td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="vusuariosEspv">
                        &nbsp;</td>
                    <td>
                        &nbsp;&nbsp;</td><td>&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnGuardarUsuario" runat="server" Text="Guardar" />
                        &nbsp;
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" />
&nbsp; <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                    </td><td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td><td class="vusuariosEsp">&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" id="TBClave" 
                runat="server" class="vusuariosBordes">
                <tr class="vusuariosGrdTitulo">
                    <td>
                        &nbsp;</td>
                    <td width="150">
                        Clave</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="vusuariosEsp">
                        &nbsp;</td>
                    <td width="150">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="vusuariosEsp">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td width="150" class="vusuariosTexto">
                        <asp:Label ID="lblClave" runat="server" Text="Clave asignada: "></asp:Label>
                    </td>
                    <td class="vusuariosTexto">
                        <asp:Label ID="lblClaveAsignada" runat="server" Text="asignada"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" />
                    </td>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                    <td colspan="2" align="center">
                        &nbsp;</td>
                    <td align="center" class="vusuariosEsp">
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
