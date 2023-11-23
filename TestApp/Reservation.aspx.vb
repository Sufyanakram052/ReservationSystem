Imports System.Data.SqlClient
Imports System.Web.DynamicData

Public Class Reservation
    Inherits Page
    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")
    Dim Key = 0
    Dim CustomerId = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Only bind data if it's not a postback
            BindDropDown()
        End If
        Populate()
    End Sub

    Private Sub BindDropDown()
        Dim query As String = "SELECT Id, Name FROM Places where status = '" & True & "'"
        Con.Open()
        Using cmd As New SqlCommand(query, Con)


            Using reader As SqlDataReader = cmd.ExecuteReader()
                DropDownList1.DataSource = reader
                DropDownList1.DataBind()
            End Using
        End Using
        Con.Close()
    End Sub

    Private Sub Populate()
        Try
            Con.Open()

            Dim CustomerId = 0
            Dim AdminId = 0
            Dim query = ""
            AdminId = If(Integer.TryParse(Session("AdminId")?.ToString(), AdminId), AdminId, 0)

            If AdminId > 0 Then
                query = "Select r.Id, p.Name As PlaceName, c.Name As CustomerName,p.Price As PlacePrice,r.Status, pr.Name As ParkingName from Reservation r left join Places p on r.PlaceId = p.Id left join Parking pr on p.ParkingId = pr.Id left join Customers c on r.CustomerId = c.Id"
            Else
                CustomerId = If(Integer.TryParse(Session("CustomerId")?.ToString(), AdminId), AdminId, 0)

                query = "Select r.Id, p.Name As PlaceName, c.Name As CustomerName,p.Price As PlacePrice,r.Status, pr.Name As ParkingName from Reservation r left join Places p on r.PlaceId = p.Id left join Parking pr on p.ParkingId = pr.Id left join Customers c on r.CustomerId = c.Id where CustomerId='" & CustomerId & "' "
            End If

            Dim adapter As SqlDataAdapter = New SqlDataAdapter(query, Con)
            Dim builder As SqlCommandBuilder = New SqlCommandBuilder(adapter)
            Dim ds As DataSet = New DataSet
            adapter.Fill(ds)

            ' Set the DataGridView DataSource
            GridView1.DataSource = ds
            GridView1.DataBind()

            ' Clear textboxes and reset key


        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)

        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()

            End If
        End Try
    End Sub

    Protected Sub saveAdmin1_Click(sender As Object, e As EventArgs) Handles saveAdmin1.Click
        Dim PlaceId = Convert.ToInt16(DropDownList1.SelectedValue)
        Key = If(Integer.TryParse(Session("SelectedPlacesId")?.ToString(), Key), Key, 0)
        CustomerId = If(Integer.TryParse(Session("CustomerId")?.ToString(), CustomerId), CustomerId, 0)
        Dim Status As Boolean = True

        'If CheckBox1.Checked Then
        '    Status = True
        'Else
        '    Status = False
        'End If
        If Key > 0 Then
            If PlaceId = 0 Or String.IsNullOrEmpty(CustomerId) Then
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Please fill all fields.',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            Else
                Con.Open()
                Dim query As String
                query = "Update Reservation set PlaceId='" & PlaceId & "',CustomerId='" & CustomerId & "',Status='" & Status & "' where Id='" & Key & "'"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Data has been Updated Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                Con.Close()
                ' CheckBox1.Checked = False
                Key = 0
                Session("SelectedPlacesId") = Key
                Populate()
            End If

        Else
            If PlaceId = 0 Or String.IsNullOrEmpty(CustomerId) Then
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Please fill all fields.',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            Else
                Try
                    Con.Open()

                    ' Query to get the maximum Id from the Reservation table
                    Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Reservation"
                    Dim cmdGetMaxId As New SqlCommand(getMaxIdQuery, Con)

                    ' ExecuteScalar is used to get a single value (in this case, the maximum Id)
                    Dim maxId As Object = cmdGetMaxId.ExecuteScalar()

                    ' Debugging information - print maxId to console
                    Console.WriteLine("Debug - maxId: " & If(maxId IsNot Nothing, maxId.ToString(), "null"))

                    ' Check if the result is DBNull before casting
                    Dim newId As Integer

                    If maxId IsNot DBNull.Value Then
                        If Integer.TryParse(maxId.ToString(), newId) Then
                            ' Increment the maximum Id to get the new Id
                            newId += 1
                        Else
                            ' Handle the case where parsing fails
                            MsgBox("Error: Unable to parse maximum Id from the database.")
                        End If
                    Else
                        ' Set newId to 0 when maxId is DBNull
                        newId = 1
                    End If

                    ' Insert the new record with the calculated Id
                    Dim insertQuery As String = "INSERT INTO Reservation (Id, PlaceId, CustomerId, Status) VALUES (@Id, @PlaceId, @CustomerId, @Status)"
                    Dim cmdInsert As New SqlCommand(insertQuery, Con)
                    cmdInsert.Parameters.AddWithValue("@Id", newId)
                    cmdInsert.Parameters.AddWithValue("@PlaceId", PlaceId)
                    cmdInsert.Parameters.AddWithValue("@CustomerId", CustomerId)
                    cmdInsert.Parameters.AddWithValue("@Status", Status)
                    cmdInsert.ExecuteNonQuery()

                    Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Data has been Added Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                    ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)

                    'CheckBox1.Checked = False
                    Key = 0
                    Session("SelectedPlacesId") = Key
                Catch ex As Exception
                    ' Handle exceptions
                    MsgBox("An error occurred: " & ex.Message & vbCrLf & ex.StackTrace)
                Finally
                    If Con.State = ConnectionState.Open Then
                        Con.Close()
                        Populate()
                    End If
                End Try
            End If
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged
        Try
            Dim Id As Integer = Integer.Parse(GridView1.SelectedRow.Cells(0).Text)
            Con.Open()
            Using Con
                Dim query As String = "SELECT * FROM Reservation WHERE id = @Id"

                Using command As New SqlCommand(query, Con)
                    command.Parameters.AddWithValue("@Id", Id)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Data found, populate the textboxes
                            DropDownList1.SelectedValue = reader("PlaceId")
                            'CheckBox1.Checked = reader("Status")
                            Key = reader("Id")
                            Session("SelectedPlacesId") = Key
                            ' You may also store the ID in a variable for further use
                            ' Key = Id
                        Else
                            ' No data found for the selected ID
                            ' Handle the case where the ID doesn't exist in the database
                            MsgBox("No data found for the selected ID.")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)

        Finally
            If Con.State = ConnectionState.Open Then
                Con.Close()
            End If
        End Try

    End Sub

    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Try
            Dim index As Integer = e.RowIndex
            Dim Id As Integer = Integer.Parse(GridView1.Rows(index).Cells(0).Text)

            Con.Open()
            Dim query As String = "DELETE FROM Reservation WHERE Id = @Id"
            Dim cmd As SqlCommand = New SqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.ExecuteNonQuery()
            Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Data has been Deleted Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

            ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            Con.Close()
            Populate()
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
        Response.Redirect(Request.Url.ToString(), True)

    End Sub


    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles GridView1.RowEditing

        Dim rowIndex As Integer = e.NewEditIndex

        Dim Id As Integer = Integer.Parse(GridView1.DataKeys(rowIndex).Values("Id").ToString())

        Con.Open()
        Using Con
            Dim query As String = "SELECT * FROM Reservation WHERE id = @Id And Status= '" & True & "'"

            Using command As New SqlCommand(query, Con)
                command.Parameters.AddWithValue("@Id", Id)

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then


                        Session("ReseverId") = Id
                        ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", "<script type='text/javascript'>showModal();</script>")
                        ' You may also store the ID in a variable for further use
                        ' Key = Id
                    Else
                        ' No data found for the selected ID
                        ' Handle the case where the ID doesn't exist in the database
                        Dim script As String = "<script>
                                           Swal.fire({
                                              title: 'Error!',
                                              text: 'You have already done the payment.',
                                              icon: 'error',
                                              confirmButtonText: 'OK'
                                           }).then(function() {
                                              window.location.href = 'Reservation.aspx';
                                           });
                                        </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)

                    End If
                End Using
            End Using
        End Using

    End Sub

    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        Populate()
        Response.Redirect(Request.Url.ToString(), True)

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim resverId = 0
        resverId = If(Integer.TryParse(Session("ReseverId")?.ToString(), resverId), resverId, 0)



        If resverId > 0 Then
            Con.Open()
            ' Query to get the maximum Id from the Reservation table
            Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Payment"
            Dim cmdGetMaxId As New SqlCommand(getMaxIdQuery, Con)

            ' ExecuteScalar is used to get a single value (in this case, the maximum Id)
            Dim maxId As Object = cmdGetMaxId.ExecuteScalar()

            ' Debugging information - print maxId to console
            Console.WriteLine("Debug - maxId: " & If(maxId IsNot Nothing, maxId.ToString(), "null"))

            ' Check if the result is DBNull before casting
            Dim newId As Integer

            If maxId IsNot DBNull.Value Then
                If Integer.TryParse(maxId.ToString(), newId) Then
                    ' Increment the maximum Id to get the new Id
                    newId += 1
                Else
                    ' Handle the case where parsing fails
                    MsgBox("Error: Unable to parse maximum Id from the database.")
                End If
            Else
                ' Set newId to 0 when maxId is DBNull
                newId = 1
            End If

            Dim insertQuery As String = "INSERT INTO Payment (Id, ReservationId, PayReceipt, PaymentDate) VALUES (@Id, @ReservationId, @PayReceipt, @PaymentDate)"
            Dim cmdInsert As New SqlCommand(insertQuery, Con)
            cmdInsert.Parameters.AddWithValue("@Id", newId)
            cmdInsert.Parameters.AddWithValue("@ReservationId", resverId)
            cmdInsert.Parameters.AddWithValue("@PayReceipt", PayReciptS.Text)
            cmdInsert.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(DateS.Text))
            cmdInsert.ExecuteNonQuery()

            Dim query As String
            query = "Update Reservation set Status = '" & False & "' where Id= '" & resverId & "'"
            Dim cmd As SqlCommand
            cmd = New SqlCommand(query, Con)
            cmd.ExecuteNonQuery()

            Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Data has been added successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       }).then(function() {
                                              window.location.href = 'Reservation.aspx';
                                           });
                                    </script>"

            ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)

            Con.Close()
        End If



    End Sub
End Class