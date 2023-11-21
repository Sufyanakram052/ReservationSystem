Imports System.Data.SqlClient

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
        Dim Status As Boolean = False

        'If CheckBox1.Checked Then
        '    Status = True
        'Else
        '    Status = False
        'End If
        If Key > 0 Then
            If PlaceId = 0 Or String.IsNullOrEmpty(CustomerId) Then
                MsgBox("Please fill all the fieldes.")
            Else
                Con.Open()
                Dim query As String
                query = "Update Reservation set PlaceId='" & PlaceId & "',CustomerId='" & CustomerId & "',Status='" & Status & "' where Id='" & Key & "'"
                Dim cmd As SqlCommand
                cmd = New SqlCommand(query, Con)
                cmd.ExecuteNonQuery()
                MsgBox("Reservation Updated Successfully.")
                Con.Close()
                ' CheckBox1.Checked = False
                Key = 0
                Session("SelectedPlacesId") = Key
                Populate()
            End If

        Else
            If PlaceId = 0 Or String.IsNullOrEmpty(CustomerId) Then
                MsgBox("Please fill all the fieldes.")
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

                    MsgBox("Reservation Saved Successfully.")

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
            MsgBox("Reservation Deleted Successfully.")
            Con.Close()
            Populate()
        Catch ex As Exception
            ' Handle exceptions
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub
End Class