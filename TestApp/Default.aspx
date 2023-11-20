<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="TestApp._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-lg-12 ">
        <!-- Added 'center-div' class -->
        <div class="row" id="center-div">
            <div class="col-md-4">
                
                <div class="card">
                    <div class="card-body">
                        <h2 class="card-title text-center">Login</h2>
                        <div class="text-center">
                             <img src="/54474.jpg" width="100px" />
                        </div>
                       
                       <div class="col-md-12">
                           <div class="row">
                               <div class="col-sm-12 mb-3">
                                  <label for="email" class="form-label">Email:</label>
                                   <asp:TextBox ID="email" type="email" class="form-control"  placeholder="name@example.com" runat="server"></asp:TextBox>
                               </div>
                               <div class="col-sm-12 mb-3">
                                  <label for="password" class="form-label">Password:</label>
                                   <asp:TextBox ID="password" type="password" class="form-control" runat="server" placeholder="Your Password"></asp:TextBox>
                               </div>
                              
                               <div class="col-md-12 mb-3">
                                    <input type="checkbox" id="inline41">
                                    <label for="inline41">Remember Me?</label>
                                </div>
                               <div class="col-sm-12 mb-3">
                                   <asp:Button ID="ButtonLogin" class="btn btn-primary col-sm-12" runat="server" Text="Login" />
                                   
                               </div>
                           </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        #center-div {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 80vh; /* Adjust the height as needed */
        }
    </style>

    <!-- Bootstrap 5 JavaScript dependencies -->

</asp:Content>
