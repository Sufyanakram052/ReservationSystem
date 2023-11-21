Imports System.Data.SqlClient

Public Class About
    Inherits Page
    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")


    Dim Key = 0
    Protected Sub saveAdmin1_Click(sender As Object, e As EventArgs) Handles saveAdmin1.Click
        Key = If(Integer.TryParse(Session("SelectedAdminId")?.ToString(), Key), Key, 0)

        If Key > 0 Then
            If NameS.Text = "" Or EmailS.Text = "" Or PasswordS.Text = "" Then
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Please fill all the fields.',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            Else
                Con.Open()
                Dim query As String
                query = "Update Admin set Name='" & NameS.Text & "',Email='" & EmailS.Text & "',Password='" & PasswordS.Text & "' where Id='" & Key & "'"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Admin Updated Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                Con.Close()
                NameS.Text = ""
                EmailS.Text = ""
                PasswordS.Text = ""
                Key = 0
                Session("SelectedAdminId") = Key
                Populate()
            End If
        Else
            Dim Name1 = NameS.Text
            Dim Email1 = EmailS.Text
            Dim Password1 = PasswordS.Text
            If Name1 = "" Or Email1 = "" Or Password1 = "" Then
                Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Please fill all the fields.',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                'MsgBox("Please fill all the fields.")
            Else
                Try
                    Con.Open()

                    ' Check if the email already exists in the Admin table
                    If EmailExistsInTable("Admin", Email1) Then
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Email already exists',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                        'MsgBox("Error: Email already exists in the Admin table.")
                    ElseIf EmailExistsInTable("Customers", Email1) Then
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Error!',
                                          text: 'Email already exists',
                                          icon: 'error',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                        'MsgBox("Error: Email already exists in the Customer table.")
                    Else
                        ' Continue with the insertion
                        ' Query to get the maximum Id from the Admin table
                        Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Admin"
                        Dim cmdGetMaxId As New SqlCommand(getMaxIdQuery, Con)

                        Dim maxId As Object = cmdGetMaxId.ExecuteScalar()

                        Console.WriteLine("Debug - maxId: " & If(maxId IsNot Nothing, maxId.ToString(), "null"))

                        Dim newId As Integer

                        If maxId IsNot DBNull.Value Then
                            If Integer.TryParse(maxId.ToString(), newId) Then
                                newId += 1
                            Else
                                MsgBox("Error: Unable to parse maximum Id from the database.")
                            End If
                        Else
                            newId = 1
                        End If

                        Dim insertQuery As String = "INSERT INTO Admin (Id, Name, Email, Password) VALUES (@Id, @Name, @Email, @Password)"
                        Dim cmdInsert As New SqlCommand(insertQuery, Con)
                        cmdInsert.Parameters.AddWithValue("@Id", newId)
                        cmdInsert.Parameters.AddWithValue("@Name", Name1)
                        cmdInsert.Parameters.AddWithValue("@Email", Email1)
                        cmdInsert.Parameters.AddWithValue("@Password", Password1)

                        cmdInsert.ExecuteNonQuery()
                        Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Admin Saved Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

                        ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
                        'MsgBox("Admin Saved Successfully.")

                        NameS.Text = ""
                        EmailS.Text = ""
                        PasswordS.Text = ""
                        Key = 0
                        Session("SelectedAdminId") = Key
                    End If
                Catch ex As Exception
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

    ' Function to check if the email exists in a given table
    Private Function EmailExistsInTable(tableName As String, email As String) As Boolean
        Dim emailExistsQuery As String = $"SELECT COUNT(*) FROM {tableName} WHERE Email = @Email"
        Dim cmdEmailExists As New SqlCommand(emailExistsQuery, Con)
        cmdEmailExists.Parameters.AddWithValue("@Email", email)

        Dim emailExistsCount As Integer = Convert.ToInt32(cmdEmailExists.ExecuteScalar())

        Return emailExistsCount > 0
    End Function


    Private Sub Populate()
        Try
            Con.Open()

            Dim query = "SELECT * FROM Admin"
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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populate()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged
        Try
            Dim Id As Integer = Integer.Parse(GridView1.SelectedRow.Cells(0).Text)
            Con.Open()
            Using Con
                Dim query As String = "SELECT * FROM Admin WHERE id = @Id"

                Using command As New SqlCommand(query, Con)
                    command.Parameters.AddWithValue("@Id", Id)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Data found, populate the textboxes
                            NameS.Text = reader("Name").ToString()
                            EmailS.Text = reader("Email").ToString()
                            PasswordS.Text = reader("Password").ToString()
                            Key = reader("Id")
                            Session("SelectedAdminId") = Key
                            ClientScript.RegisterStartupScript(Me.GetType(), "ShowModal", "<script type='text/javascript'>showModal();</script>")
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
            Dim query As String = "DELETE FROM Admin WHERE Id = @Id"
            Dim cmd As SqlCommand = New SqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.ExecuteNonQuery()
            Dim script As String = "<script>
                                       Swal.fire({
                                          title: 'Success!',
                                          text: 'Admin Deleted Successfully.',
                                          icon: 'success',
                                          confirmButtonText: 'OK'
                                       });
                                    </script>"

            ClientScript.RegisterStartupScript(Me.GetType(), "SweetAlert", script)
            'MsgBox("User Deleted Successfully.")
            Con.Close()
            Populate()
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class