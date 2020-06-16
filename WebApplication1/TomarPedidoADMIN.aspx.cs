﻿using OrderNowDAL;
using OrderNowDAL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class TomarPedidoADMIN : System.Web.UI.Page
    {
        AlimentoDAL aDAL = new AlimentoDAL();
        PedidoDAL pDAL = new PedidoDAL();
        TrabajadorDAL tDAL = new TrabajadorDAL();
        TipoMedicionDAL tMDAL = new TipoMedicionDAL();
        AlimentoPedidoDAL aPDAL = new AlimentoPedidoDAL();
        IngredienteAlimentoDAL iADAL = new IngredienteAlimentoDAL();
        IngredientesDAL iDAL = new IngredientesDAL();
        ExtraPedidoDAL ePDAL = new ExtraPedidoDAL();

        AlimentoPedidoGrid carrito = new AlimentoPedidoGrid();
        ExtraPedidoGrid extraCarrito = new ExtraPedidoGrid();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarGridCarrito();
                ValidarSession();
                GridViewExtras.DataBind();
            }
            else
            {
                UserMessage("", "");
                UserMessageExtra("", "");
                if (txtIdAlimentoPedido.Text != "")
                {
                    ModalPopupExtender1.Show();
                }
            }
            CargarTotales();
        }

        protected void GridViewAlimentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Agregar":
                        int index = Convert.ToInt32(e.CommandArgument);
                        int idAlimento = Convert.ToInt32(((Label)GridViewAlimentos.Rows[index].FindControl("lblCodigo")).Text);

                        Alimento obj = aDAL.Find(idAlimento);
                        carrito.AgregarAlimento(obj);
                        CargarGridCarrito();
                        CargarTotales();
                        break;
                    case "Default":
                        break;
                }
            }
            catch (Exception ex)
            {
                UserMessage(ex.Message, "danger");
            }
        }

        protected void GridViewPedido_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idAlimentoPedido = Convert.ToInt32(((Label)GridViewPedido.Rows[index].FindControl("lblIdAlimentoPedido")).Text);
                AlimentoPedido objCarrito = carrito.BuscarElemento(idAlimentoPedido);
                Alimento obj = aDAL.Find((int)objCarrito.IdAlimento);

                switch (e.CommandName)
                {
                    case "Quitar":
                        carrito.EliminarAlimento(objCarrito);
                        CargarTotales();
                        break;
                    case "AgregarExtra":
                        ActivarPopUpExtra(objCarrito);
                        break;
                }
                CargarGridCarrito();
            }
            catch (Exception ex)
            {

                UserMessage(ex.Message, "danger");
            }
        }

        protected void btnIngresarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                ValidatePedidoFields();

                Pedido pedido = new Pedido()
                {
                    Trabajador = tDAL.Find((int)Session["Usuario"]).IdTrabajador,
                    IdEstadoPedido = 1,
                    IdCliente = Convert.ToInt32(cboClientes.SelectedValue),
                    IdTipoPedido = Convert.ToInt32(cboTipoPedido.SelectedValue)
                };
                pedido = pDAL.Add(pedido);

                AgregarAlimentosPorPedido(pedido);

                LimpiarPedido();
                UserMessage("Pedido Realizado", "success");
            }
            catch (Exception ex)
            {
                UserMessage(ex.Message, "danger");
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarPedido();
            }
            catch (Exception ex)
            {
                UserMessage(ex.Message, "danger");
            }
        }

        protected void cboIngrediente_TextChanged(object sender, EventArgs e)
        {
            int idIngrediente = Convert.ToInt32(cboIngrediente.SelectedValue);
            Ingrediente ingrediente = iDAL.Find(idIngrediente);

            EliminarCbo();

            if (idIngrediente != 0)
            {
                if (ingrediente.Porción != null)
                {
                    txtValorPorPorcion.Text = $"{ingrediente.Porción} {ingrediente.TipoMedicion.Descripcion}";
                    txtCantidadPorcion.Text = "1";
                    SwitchTextBox(false);
                }
                else
                {
                    txtValorPorPorcion.Text = "No establecido";
                    SwitchTextBox(true);
                    UserMessageExtra("Este Ingrediente no tiene las porciones establecidas", "danger");
                }
            }
        }

        protected void btnLimpiarExtra_Click(object sender, EventArgs e)
        {
            int idExtraPedido = Convert.ToInt32(txtIdAlimentoPedido.Text);
            LimpiarModalTodo();

            extraCarrito.DeleteAll(idExtraPedido);
            CargarGridExtras(idExtraPedido);
        }

        protected void btnCerrarModal_Click(object sender, EventArgs e)
        {
            txtIdAlimentoPedido.Text = "";
            ModalPopupExtender1.Hide();
        }

        protected void btnAgregarExtra_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateExtraFields();

                int idIngrediente = Convert.ToInt32(cboIngrediente.SelectedValue);
                cboIngrediente.Items.FindByValue(idIngrediente.ToString()).Enabled = false; //Se bloquea la opción de elegir el mismo ingrediente

                int idAlimentoPedido = Convert.ToInt32(txtIdAlimentoPedido.Text);
                int cantidad = Convert.ToInt32(txtCantidadPorcion.Text);
                int? valorExtra = string.IsNullOrEmpty(txtValorExtra.Text) ? (int?)null : Convert.ToInt32(txtValorExtra.Text);

                ExtraPedido extra = new ExtraPedido()
                {
                    IdIngrediente = idIngrediente,
                    CantidadExtra = cantidad,
                    IdAlimentoPedido = idAlimentoPedido,
                    ValorExtra = valorExtra
                };
                ExtraPedido item = extraCarrito.GetListByAlimentoPedido(idAlimentoPedido).FirstOrDefault(x => x.IdIngrediente == idIngrediente);
                if (item != null)
                {
                    int index = extraCarrito.GetList().IndexOf(item);
                    extraCarrito.Edit(index, extra);
                }
                else
                {
                    extraCarrito.Add(extra);
                }

                CargarGridExtras(idAlimentoPedido);
                LimpiarModal();
                SwitchTextBox(true);
            }
            catch (Exception ex)
            {
                UserMessageExtra(ex.Message, "danger");
            }
        }

        protected void btnGuardarExtras_Click(object sender, EventArgs e)
        {
            try
            {
                CargarTotales();

                LimpiarModalTodo();

                txtIdAlimentoPedido.Text = ""; //Cierra el Modal
                ModalPopupExtender1.Hide();
            }
            catch (Exception ex)
            {
                UserMessageExtra(ex.Message, "danger");
            }
        }

        protected void GridViewExtras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label label = e.Row.FindControl("lblIdIngrediente") as Label;
                Ingrediente ing = iDAL.Find(Convert.ToInt32(label.Text));
                label.Text = ing.Nombre;

                label = e.Row.FindControl("lblTipoMedicion") as Label;
                TipoMedicion tipoM = tMDAL.Find(Convert.ToInt32(label.Text));

                label.Text = $"{ing.Porción} {tipoM.Descripcion}";
            }
        }

        protected void GridViewExtras_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = GridViewExtras.Rows[Convert.ToInt32(e.CommandArgument)];
            switch (e.CommandName)
            {
                case "Modificar":
                    int idExtra = Convert.ToInt32((row.FindControl("lblIdExtra") as Label).Text);
                    ExtraPedido extra = extraCarrito.Find(idExtra);
                    EliminarCbo();
                    ViewState["IdExtra"] = idExtra;
                    LlenarExtraFields(extra);
                    SwitchTextBox(false);
                    break;

            }
        }



        private void ActivarPopUpExtra(AlimentoPedido objCarrito)
        {
            ActivarItemsCbo();
            int idAlimentoPedido = objCarrito.IdAlimentoPedido;
            foreach (ExtraPedido extra in extraCarrito.GetListByAlimentoPedido(idAlimentoPedido))
            {
                cboIngrediente.Items.FindByValue(extra.IdIngrediente.ToString()).Enabled = false;
            }

            Alimento obj = aDAL.Find((int)objCarrito.IdAlimento);
            txtPreparacion.Text = obj.Nombre;
            txtIdAlimentoPedido.Text = objCarrito.IdAlimentoPedido.ToString();
            CargarGridExtras(objCarrito.IdAlimentoPedido);
            ModalPopupExtender1.Show();
        }

        protected void LimpiarPedido()
        {
            carrito.EliminarAlimentos();
            extraCarrito.DeleteAll();

            CargarGridCarrito();
            LimpiarModalTodo();
            CargarTotales();

            cboClientes.SelectedValue = "0";
            cboTipoPedido.SelectedValue = "0";
        }

        protected void LimpiarModal()
        {
            cboIngrediente.SelectedValue = "0";

            txtCantidadPorcion.Text = "";
            txtValorPorPorcion.Text = "";

            txtValorExtra.Text = "";
        }

        private void LimpiarModalTodo()
        {
            ActivarItemsCbo();
            LimpiarModal();
        }

        private void ValidatePedidoFields()
        {
            if (carrito.ListarAlimentos().Count == 0)
            {
                throw new Exception("Debe Ingresar Alimentos");
            }
            if (cboClientes.SelectedValue == "0")
            {
                throw new Exception("Debe seleccionar un cliente");
            }
            if (cboTipoPedido.SelectedValue == "0")
            {
                throw new Exception("Debe Seleccionar un tipo de pedido");
            }
        }

        private void ValidateExtraFields()
        {
            if (cboIngrediente.SelectedValue == "0")
            {
                throw new Exception("Debe seleccionar un ingrediente para agregar");
            }
            if (txtCantidadPorcion.Text == "")
            {
                throw new Exception("Debe ingresar una cantidad para ser agregada");
            }
            if (Convert.ToInt32(txtCantidadPorcion.Text) == 0)
            {
                throw new Exception("Debe ingresar una cantidad para ser agregada");
            }
            if (Convert.ToInt32(txtCantidadPorcion.Text) < 0)
            {
                throw new Exception("La cantidad agregada debe ser mayor a 0");
            }
            //if (txtCantidadExtra.Text == "" && txtCantidadPorcion.Text == "")
            //{
            //    throw new Exception("Debe ingresar una cantidad para ser agregada");
            //}
            //if ((txtCantidadExtra.Text != "" && Convert.ToInt32(txtCantidadExtra.Text) < 1) || (txtCantidadPorcion.Text != "" && Convert.ToInt32(txtCantidadPorcion.Text) < 1))
            //{
            //    throw new Exception("Debe ingresar una cantidad valida para ser agregada");
            //}
        }

        private void LlenarExtraFields(ExtraPedido extra)
        {
            Ingrediente ingrediente = iDAL.Find(extra.IdIngrediente.Value);
            cboIngrediente.Items.FindByValue(ingrediente.IdIngrediente.ToString()).Enabled = true;
            cboIngrediente.SelectedValue = ingrediente.IdIngrediente.ToString();
            ViewState["IdIngrediente"] = ingrediente.IdIngrediente;

            txtCantidadPorcion.Text = extra.CantidadExtra.ToString();
            txtValorPorPorcion.Text = $"{ingrediente.Porción} {tMDAL.Find(ingrediente.IdTipoMedicion.Value).Descripcion}";

            txtValorExtra.Text = extra.ValorExtra.HasValue ? extra.ValorExtra.Value.ToString() : "";
        }

        private void AgregarAlimentosPorPedido(Pedido pedido)
        {
            int idPedido = pedido.IdPedido;
            foreach (AlimentoPedido item in carrito.ListarAlimentos())
            {
                //Agregar Alimento a la tabla AlimentoPedido
                Alimento al = aDAL.Find(Convert.ToInt32(item.IdAlimento));
                int idAlimentoPedidoLista = item.IdAlimentoPedido;
                AlimentoPedido alimentoPedido = aPDAL.Add(new AlimentoPedido()
                {
                    IdAlimento = al.IdAlimento,
                    IdPedido = idPedido
                });

                idAlimentoPedidoLista = CambiarIdListadoExtra(idAlimentoPedidoLista, alimentoPedido.IdAlimentoPedido);

                //Restar el stock del ingrediente respecto a los ingredientes del alimento
                List<IngredientesAlimento> lista = iADAL.Ingredientes(al.IdAlimento);
                foreach (IngredientesAlimento ingAl in lista)
                {
                    Ingrediente ingrediente = iDAL.Find((int)ingAl.Ingrediente);
                    ingrediente.Stock -= ingAl.Cantidad;
                    iDAL.Update(ingrediente);
                }

                AgregarExtras(idAlimentoPedidoLista);
            }
        }

        private void AgregarExtras(int idAlimentoPedido)
        {
            List<ExtraPedido> listaExtras = extraCarrito.GetListByAlimentoPedido(idAlimentoPedido);
            foreach (ExtraPedido extra in listaExtras)
            {
                ePDAL.Add(extra);

                //Restar el stock del ingrediente respecto a los ingredientes del extra
                Ingrediente ingrediente = iDAL.Find((int)extra.IdIngrediente);
                ingrediente.Stock -= extra.CantidadExtra;
                iDAL.Update(ingrediente);
            }
        }

        private int CambiarIdListadoExtra(int id, int idBDD)
        {
            // Se cambia el Id por defecto del listado de Extras, 
            // por el id obtenido de la Base de Datos 
            // para que luego sea agregado el id correcto en la Base de datos
            List<ExtraPedido> lista = extraCarrito.GetList().Where(x => x.IdAlimentoPedido == id).ToList();
            foreach (ExtraPedido item in lista)
            {
                int index = extraCarrito.GetList().IndexOf(item);
                item.IdAlimentoPedido = idBDD;
                extraCarrito.Edit(index, item);
            }
            return idBDD;
        }

        private void CargarGridExtras(int idAlimentoPedido)
        {
            GridViewExtras.DataSource = extraCarrito.GetDataTable(idAlimentoPedido);
            GridViewExtras.DataBind();
        }

        private void CargarGridCarrito()
        {
            DataTable dt = carrito.DataTableAlimentos();
            GridViewPedido.DataSource = dt;
            GridViewPedido.DataBind();
        }

        private void UserMessage(string mensaje, string type)
        {
            if (mensaje != "")
            {
                divMenssage.Attributes.Add("class", "col-md-12 text-center alert alert-" + type);
                lblMensaje.Text = mensaje;
            }
            else
            {
                divMenssage.Attributes.Add("class", "");
                lblMensaje.Text = mensaje;
            }
        }

        private void UserMessageExtra(string mensaje, string type)
        {
            if (mensaje != "")
            {
                divMenssageExtra.Attributes.Add("class", "col-md-12 text-center alert alert-" + type);
                lblMensajeExtra.Text = mensaje;
            }
            else
            {
                divMenssageExtra.Attributes.Add("class", "");
                lblMensajeExtra.Text = mensaje;
            }
        }

        private void EliminarCbo()
        {
            if (ViewState["IdIngrediente"] != null)
            {
                int idIngrediente = (int)ViewState["IdIngrediente"];
                cboIngrediente.Items.FindByValue(idIngrediente.ToString()).Enabled = false;
            }
        }

        private void ActivarItemsCbo()
        {
            foreach (ListItem item in cboIngrediente.Items)
            {
                item.Enabled = true;
            }
        }

        private void SwitchTextBox(bool desactivar)
        {
            if (desactivar)
            {
                txtValorExtra.Text = "";
                txtCantidadPorcion.Text = "";

                txtValorExtra.Enabled = false;
                txtCantidadPorcion.Enabled = false;

                btnAgregarExtra.CssClass = "btn btn-secondary btn-block";
                btnAgregarExtra.Enabled = false;
            }
            else
            {
                txtValorExtra.Enabled = true;
                txtCantidadPorcion.Enabled = true;

                btnAgregarExtra.CssClass = "btn btn-primary btn-block";
                btnAgregarExtra.Enabled = true;
            }
        }

        private void CargarTotales()
        {
            int totalAlimento = 0;
            int totalExtra = 0;

            foreach (AlimentoPedido item in carrito.ListarAlimentos())
            {
                totalAlimento += aDAL.Find(item.IdAlimento.Value).Precio.Value;
            }

            foreach (ExtraPedido extra in extraCarrito.GetList())
            {
                totalExtra += extra.ValorExtra.HasValue ? extra.ValorExtra.Value : 0;
            }


            lblTotalAlimento.Text = totalAlimento.ToString();
            lblTotalExtras.Text = totalExtra.ToString();

            int total = totalAlimento + totalExtra;
            lblTotal.Text = total.ToString();
        }

        private void ValidarSession()
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("/Login.aspx");
            }
            else
            {
                txtTrabajador.Text = tDAL.Find((int)Session["Usuario"]).Nombres;
            }
        }
    }
}