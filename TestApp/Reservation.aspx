<%@ Page Title="Reservation" Language="VB" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Reservation.aspx.vb" Inherits="TestApp.Reservation" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-lg-12">
        <div class="row mt-3">
            <div class="col-sm-6">
                <div class="col d-flex align-items-baseline">
                    <div class="media">
                        <div class="media-body align-self-center ms-3">
                            <h2 class="m-0 font-20">Reservation List</h2>
                            <h6>Home / Reservation List</h6>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-3" id="ForCustomerOnly">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-body">
                        <div class="col-md-12">
                            <div class="row">
                               <div class="col-sm-12 mb-3">
                                    <label for="name" class="form-label">Select Place:</label>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30" SelectCommand="SELECT Id, Name FROM Places"></asp:SqlDataSource>
                                    <asp:DropDownList ID="DropDownList1" class="form-control col-sm-12" runat="server" DataTextField="Name" DataValueField="Id"></asp:DropDownList>
                                </div>

                                <%--<div class="col-md-12">
                                    <asp:CheckBox ID="CheckBox1" runat="server" />

                                    &nbsp;Status
                                </div>--%>
                                <div class="col-sm-3 mb-3">
                                    <asp:Button ID="saveAdmin1" class="btn btn-primary col-sm-12" runat="server" Text="Save" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <h5 class="modal-title text-white" id="exampleModalLabel">Add Admin</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="col-sm-12 mb-3">
                            <label for="PayRecipt" class="form-label">Pay Receipt:</label>
                            <asp:TextBox ID="PayReciptS" type="Text" class="form-control" runat="server" placeholder="Enter Pay Text" ViewStateMode="Enabled"></asp:TextBox>
                        </div>
                        <div class="col-sm-12 mb-3">
                            <label for="DateS" class="form-label">Date:</label>
                            <asp:TextBox ID="DateS" type="datetime-local" class="form-control" runat="server" ViewStateMode="Enabled"></asp:TextBox>
                        </div>
                        
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                         <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Save" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-5 pb-5 mt-4">
            <div class="col-md-12 mb-5 pb-5">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="table table-striped table-responsive" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" />
                        <asp:BoundField DataField="PlaceName" HeaderText="PlaceName" SortExpression="PlaceName" />
                        <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" SortExpression="CustomerName" />
                        <asp:BoundField DataField="PlacePrice" HeaderText="PlacePrice" SortExpression="PlacePrice" />
                        <asp:BoundField DataField="ParkingName" HeaderText="ParkingName" SortExpression="ParkingName" />
                        <%--<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />--%>


                        <asp:CommandField SelectText="Edit" ShowSelectButton="True" ControlStyle-CssClass="btn btn-warning" >


<ControlStyle CssClass="btn btn-warning"></ControlStyle>
                        </asp:CommandField>


                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btn btn-danger" >


<ControlStyle CssClass="btn btn-danger"></ControlStyle>
                        </asp:CommandField>


                         <asp:CommandField ShowEditButton="True" ControlStyle-CssClass="btn btn-secondary" ShowHeader="True" DeleteText="Payment" EditText="Payment" >








<ControlStyle CssClass="btn btn-secondary"></ControlStyle>
                        </asp:CommandField>








                    </Columns>
                </asp:GridView>
            </div>        
        </div>
    </div>
     <script type="text/javascript">
    function showModal() {
        // Assuming your modal element has the id 'exampleModal'
        var modal = new bootstrap.Modal(document.getElementById('exampleModal'));

        // Use Bootstrap's native JavaScript method to show the modal
        modal.show();
    }
     </script>
</asp:Content>
