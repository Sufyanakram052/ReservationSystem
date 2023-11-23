﻿Imports System.Data.SqlClient

Public Class Places
    Inherits System.Web.UI.Page


    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")


    Dim Key = 0
    Private Sub BindDropDown()
        Dim query As String = "SELECT Id, Name FROM Parking"
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

            Dim query = "select pl.Id,pl.Name,pl.Location,pl.Price,pl.Status,pa.Name as ParkingName from Places pl left join Parking pa on pl.ParkingId = pa.Id"
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
        Dim ParkingId = Convert.ToInt16(DropDownList1.SelectedValue)

        Key = If(Integer.TryParse(Session("SelectedPlacesId")?.ToString(), Key), Key, 0)
        Dim Status As Boolean = False

        If CheckBox1.Checked Then
            Status = True
        Else
            Status = False
        End If
        If Key > 0 Then

            If NameS.Text = "" Or LocationS.Text = "" Or PriceS.Text = "" Or ParkingId = 0 Then
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
                query = "Update Places set Name='" & NameS.Text & "',Location='" & LocationS.Text & "',Price='" & PriceS.Text & "',Status='" & Status & "',ParkingId='" & ParkingId & "' where Id='" & Key & "'"
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
                NameS.Text = ""
                LocationS.Text = ""
                PriceS.Text = ""
                CheckBox1.Checked = False
                Key = 0
                Session("SelectedPlacesId") = Key
                Populate()
            End If

        Else
            Dim Name1 = NameS.Text
            Dim Location1 = LocationS.Text

            If NameS.Text = "" Or LocationS.Text = "" Or PriceS.Text = "" Or ParkingId = 0 Then
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

                    ' Query to get the maximum Id from the Places table
                    Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Places"
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
                    Dim insertQuery As String = "INSERT INTO Places (Id, Name, Location, Price, Status,ParkingId) VALUES (@Id, @Name, @Location, @Price, @Status, @ParkingId)"
                    Dim cmdInsert As New SqlCommand(insertQuery, Con)
                    cmdInsert.Parameters.AddWithValue("@Id", newId)
                    cmdInsert.Parameters.AddWithValue("@Name", Name1)
                    cmdInsert.Parameters.AddWithValue("@Location", Location1)
                    cmdInsert.Parameters.AddWithValue("@Price", PriceS.Text)
                    cmdInsert.Parameters.AddWithValue("@Status", Status)
                    cmdInsert.Parameters.AddWithValue("@ParkingId", ParkingId)

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

                    NameS.Text = ""
                    LocationS.Text = ""
                    PriceS.Text = ""
                    CheckBox1.Checked = False
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
                Dim query As String = "SELECT * FROM Places WHERE id = @Id"

                Using command As New SqlCommand(query, Con)
                    command.Parameters.AddWithValue("@Id", Id)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Data found, populate the textboxes
                            NameS.Text = reader("Name").ToString()
                            LocationS.Text = reader("Location").ToString()
                            PriceS.Text = reader("Price")
                            CheckBox1.Checked = reader("Status")
                            DropDownList1.SelectedValue = reader("ParkingId")
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
            Dim query As String = "DELETE FROM Places WHERE Id = @Id"
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
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Populate()
        If Not IsPostBack Then
            ' Only bind data if it's not a postback
            BindDropDown()
        End If
    End Sub



End Class