Imports System.Data.SqlClient
Imports System.Reflection.Emit

Public Class Contact
    Inherits Page
    Dim Con As New SqlConnection("Data Source=DESKTOP-5DVUDJE;Initial Catalog=ParkingReservation;Integrated Security=True;Connect Timeout=30")
    Dim Key = 0

    Private Sub Populate()
        Try
            Con.Open()

            Dim query = "select c.Id,a.Name as AdminName,c.Name,c.Email,c.Password,c.Phone, c.Address,c.Status from Customers c left join Admin a on c.AdminId = a.Id"
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Populate()
    End Sub

    Protected Sub saveCustomer_Click(sender As Object, e As EventArgs) Handles saveCustomer.Click
        Key = If(Integer.TryParse(Session("SelectedAdminId")?.ToString(), Key), Key, 0)

        Dim AdminId = 0
        AdminId = If(Integer.TryParse(Session("AdminId")?.ToString(), AdminId), AdminId, 0)
        Dim Address As String = Request.Form("S1")

        Dim Status As Boolean = False

        If CheckBox1.Checked Then
            Status = True
        Else
            Status = False
        End If

        If Key > 0 Then

            If NameS.Text = "" Or EmailS.Text = "" Or PasswordS.Text = "" Or AdminId = 0 Then
                MsgBox("Please fill all the fielss.")
            Else
                Con.Open()
                Dim query As String
                query = "Update Customers set Name='" & NameS.Text & "',Email='" & EmailS.Text & "',Password='" & PasswordS.Text & "',Phone='" & PhoneS.Text & "',Address='" & Address & "',Status='" & Status & "', AdminId='" & AdminId & "' where Id='" & Key & "'"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Admin Updated Successfully.")
                Con.Close()
                NameS.Text = ""
                EmailS.Text = ""
                PasswordS.Text = ""
                Key = 0
                Populate()
            End If

        Else
            Dim Name1 = NameS.Text
            Dim Email1 = EmailS.Text
            Dim Password1 = PasswordS.Text
            If Name1 = "" Or Email1 = "" Or Password1 = "" Or AdminId = 0 Then
                MsgBox("Please fill all the fields.")
            Else
                Try
                    Con.Open()

                    ' Query to get the maximum Id from the Admin table
                    Dim getMaxIdQuery As String = "SELECT MAX(Id) FROM Customers"
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
                    Dim insertQuery As String = "INSERT INTO Customers (Id, Name, Email, Password, Phone, Address, Status, AdminId) VALUES (@Id, @Name, @Email, @Password, @Phone, @Address, @Status, @AdminId)"
                    Dim cmdInsert As New SqlCommand(insertQuery, Con)
                    cmdInsert.Parameters.AddWithValue("@Id", newId)
                    cmdInsert.Parameters.AddWithValue("@Name", Name1)
                    cmdInsert.Parameters.AddWithValue("@Email", Email1)
                    cmdInsert.Parameters.AddWithValue("@Password", Password1)
                    cmdInsert.Parameters.AddWithValue("@Phone", PhoneS.Text)
                    cmdInsert.Parameters.AddWithValue("@Address", Address)
                    cmdInsert.Parameters.AddWithValue("@Status", Status)
                    cmdInsert.Parameters.AddWithValue("@AdminId", AdminId)

                    cmdInsert.ExecuteNonQuery()

                    MsgBox("Customer Saved Successfully.")

                    NameS.Text = ""
                    EmailS.Text = ""
                    PasswordS.Text = ""
                    PhoneS.Text =
                    PhoneS.Text = ""
                    CheckBox1.Checked = False
                    Key = 0
                    Session("SelectedAdminId") = Key
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
                Dim query As String = "SELECT * FROM Customers WHERE id = @Id"

                Using command As New SqlCommand(query, Con)
                    command.Parameters.AddWithValue("@Id", Id)

                    Using reader As SqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Data found, populate the textboxes
                            NameS.Text = reader("Name").ToString()
                            EmailS.Text = reader("Email").ToString()
                            PasswordS.Text = reader("Password").ToString()
                            PhoneS.Text = reader("Phone")
                            CheckBox1.Checked = reader("Status")
                            Key = reader("Id")
                            Session("SelectedAdminId") = Key
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
            Dim query As String = "DELETE FROM Customers WHERE Id = @Id"
            Dim cmd As SqlCommand = New SqlCommand(query, Con)
            cmd.Parameters.AddWithValue("@Id", Id)
            cmd.ExecuteNonQuery()
            MsgBox("Customer Deleted Successfully.")
            Con.Close()
            Populate()
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class