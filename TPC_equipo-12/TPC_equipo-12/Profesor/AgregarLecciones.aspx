﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Profesor/ProfesorMasterPage.Master" AutoEventWireup="true" CodeBehind="AgregarLecciones.aspx.cs" Inherits="TPC_equipo_12.AgregarLecciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card w-50 my-5">
            <div class="card-body">
                <h5 class="card-title d-flex justify-content-center align-items-center">Agregar Lecciones</h5>
                <div class="mb-3">
                    <asp:Label ID="LabelNombreLeccion" runat="server" CssClass="form-label" Text="Nombre"></asp:Label>
                    <asp:TextBox ID="TextBoxNombreLeccion" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <asp:Label ID="LabelDescripcionLeccion" runat="server" CssClass="form-label" Text="Descripción"></asp:Label>
                    <asp:TextBox ID="TextBoxDescripcionLeccion" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <asp:Label ID="LabelNumeroLeccion" runat="server" CssClass="form-label" Text="Número de Lección"></asp:Label>
                    <asp:TextBox ID="TextBoxNumeroLeccion" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <asp:Button ID="ButtonAgregarMateriales" runat="server" Text="Agregar Materiales" CssClass="btn btn-primary" OnClick="ButtonAgregarMateriales_Click" />
                </div>
                <asp:Button ID="ButtonHechoLecciones" runat="server" Text="Hecho" CssClass="btn btn-primary" OnClick="ButtonHechoLecciones_Click"/>
            </div>
        </div>
    </div>
</asp:Content>
