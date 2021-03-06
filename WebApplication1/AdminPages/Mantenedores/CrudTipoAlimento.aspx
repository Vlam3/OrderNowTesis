<%@ Page Title="Administrar Tipo de Alimento" Language="C#" MasterPageFile="~/Administrador.Master" AutoEventWireup="true" CodeBehind="CrudTipoAlimento.aspx.cs" Inherits="WebApplication1.Mantenedores.CrudTipoAlimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
</asp:Content>

<asp:Content runat="server" ID="ContentTitle" ContentPlaceHolderID="ContentPlaceHolderTitle">
    Administrar Tipo de Alimentos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="form-row">
            <asp:Label ID="Label2" runat="server" Text="Nombre:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
        </div>
        <div class="form-row">
            <asp:Label ID="Label1" runat="server" Text="Estado"></asp:Label>
            <asp:CheckBox ID="chkEstado" runat="server" Enabled="false" />
        </div>
        <div class="form-row">
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" OnClick="btnModificar_Click" Visible="false" />
            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" Visible="false" />
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
        </div>
        <div id="divMessage" runat="server">
            <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BorderStyle="None" CssClass="table table-hover table-light" HeaderStyle-CssClass="thead-light" DataKeyNames="IdTipoAlimento" DataSourceID="SqlDataSource1" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="Label7" runat="server" Text="Modificar"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lblCodigo" runat="server" Text='<%#Bind("IdTipoAlimento") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Descripcion" HeaderText="Nombre" SortExpression="Descripcion" ReadOnly="true"></asp:BoundField>
                <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" ReadOnly="true"></asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:OrderNowBDConnectionString %>' SelectCommand="SELECT * FROM [TipoAlimento]"></asp:SqlDataSource>
    </div>
</asp:Content>
