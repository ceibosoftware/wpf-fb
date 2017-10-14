﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpfFamiliaBlanco.Proveedores;

namespace wpfFamiliaBlanco
{
    /// <summary>
    /// Interaction logic for windowModificarProveedor.xaml
    /// </summary>
    public partial class windowModificarProveedor : Window
    {

        CRUD conexion = new CRUD();
        private List<Categorias> items;
        public List<Categorias> Items { get => items; set => items = value; }
        String id;
        public windowModificarProveedor(List<Categorias> lista )
        {
            InitializeComponent();
            CargarCMB();
            LoadListaProv(lista);
            LoadListaProveedor();
            LlenarComboFiltro();


        }
        public class contacto
        {

            public String NombreContacto { get; set; }
            public String Email { get; set; }
            public String NumeroTelefono { get; set; }


            public contacto(String nomContacto, String ema, String numTelefono)
            {
                NombreContacto = nomContacto;
                Email = ema;
                NumeroTelefono = numTelefono;

            }
        }
  

        private void LoadListaProveedor()
        {

            String consulta = " Select * from categorias ";
            conexion.Consulta(consulta, ltsCategorias);
            ltsCategorias.DisplayMemberPath = "nombre";
            ltsCategorias.SelectedValuePath = "idCategorias";
        }

        private void LoadListaProv(List<Categorias> lista)
        {
            items = lista;
            ltsCatProveedores.ItemsSource = items;
            ltsCatProveedores.DisplayMemberPath = "nombre";
            ltsCatProveedores.SelectedValuePath = "id";
        }

        public void CargarCMB()
        {
            cmbRazonSocial.Items.Add("SA");
            cmbRazonSocial.Items.Add("Responsable Inscripto");
            cmbRazonSocial.Items.Add("MOnotributista");
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoria.Text))
            {
                MessageBox.Show("Falta completar campo nombre");

            }
            else if (string.IsNullOrEmpty(txtLocalidad.Text))
            {
                MessageBox.Show("falta completar campo localidad");

            }
            else if (string.IsNullOrEmpty(txtDireccion.Text))
            {
                MessageBox.Show("falta completar campo dirección");

            }
            else if (string.IsNullOrEmpty(txtCuit.Text))
            {
                MessageBox.Show("falta completar campo cuit");

            }
            else if (string.IsNullOrEmpty(txtCP.Text))
            {
                MessageBox.Show("falta completar campo código postal");

            }
            else if (ltsCatProveedores.Items.Count == 0)
            {
                MessageBox.Show("Es necesario ingresar alguna categoria del proveedor");

            }
            else if (dgvContactom.HasItems == false)
            {
                MessageBox.Show("Es necesario ingresar algun contacto al proveedor");

            }

            else
            {
                DialogResult = true;
            }
        }

        private void LlenarComboFiltro()
        {

            cmbFiltro.Items.Add("Nombre");
            cmbFiltro.Items.Add("Categoria");

        }

        private void btnCatAgregar_Click(object sender, RoutedEventArgs e)
        {

            Boolean existe = false;
            DataRow selectedDataRow = ((DataRowView)ltsCategorias.SelectedItem).Row;

            if (ltsCatProveedores.Items.Count <= 0)
            {
                items.Add(new Categorias(selectedDataRow["nombre"].ToString(), (int)ltsCategorias.SelectedValue));
                ltsCatProveedores.Items.Refresh();
            }
            else
            {
                for (int i = 0; i < ltsCatProveedores.Items.Count; i++)
                {

                    if (selectedDataRow["nombre"].ToString().CompareTo(items[i].nombre) != 0)
                    {
                        existe = false;

                    }
                    else
                    {
                        existe = true;
                        break;
                    }
                }
                if (!existe)
                {

                    items.Add(new Categorias(selectedDataRow["nombre"].ToString(), (int)ltsCategorias.SelectedValue));
                    ltsCatProveedores.Items.Refresh();


                    Console.WriteLine("categorias" + Items.Count);



                }
                else
                {
                    MessageBox.Show("Esa categoria ya fue agregada");
                }
            }
        }

        private void btnCatEliminar_Click(object sender, RoutedEventArgs e)
        {
            Items.Remove(Items.Find(item => item.id == (int)ltsCatProveedores.SelectedValue));
            ltsCatProveedores.Items.Refresh();
        }

        private void btnNuevoContacto_Click(object sender, RoutedEventArgs e)
        {
            var newW = new windowAgregarContactoProveedor();
            String idpr = pageProveedores.idProv2;
            MessageBox.Show("id" + idpr);
            newW.ShowDialog();
          
            if (newW.DialogResult == true)
            {
                Console.WriteLine("Entro");
                String telefono12 = newW.txtTelefonoContacto.Text;
                String nombreContacto12 = newW.txtNombreContacto.Text;
                String mail12 = newW.txtMailContacto.Text;
                contacto con = new contacto(nombreContacto12, mail12, telefono12);
                

                String sqlContacto;

                Console.WriteLine("telefono" + telefono12);
                Console.WriteLine("nombvre" + nombreContacto12);
                Console.WriteLine("mail" + mail12);
                sqlContacto = "insert into contactoproveedor(telefono, email, nombreContacto, FK_idProveedor) values('" + telefono12 + "', '" + mail12 + "', '" + nombreContacto12 + "', '" + idpr + "');";
                conexion.operaciones(sqlContacto);
                LoadListaProveedor();
               
            }
        }


        

    }
}
