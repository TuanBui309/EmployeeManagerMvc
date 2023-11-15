$(document).ready(function () {
	loadData();
}); 
function loadData() {
	$.ajax({
		url: "/GetAllEmployee",
		type: "GET",
		contentType: "application/json;charset=utf-8",
		dataType: "json",
		success: function (result) {
			var html = '';
			if (result.statusCode == 200) {
				$.each(result.content, function (key, item) {
					html += '<tr>';
					html += '<td>' + item.Name + '</td>';
					html += '<td>' + item.DateOfBirth + '</td>';
					html += '<td>' + item.Age + '</td>';
					html += '<td>' + item.JobName + '</td>';
					html += '<td>' + item.NationName + '</td>';
					html += '<td>' + item.IdentityCardNumber + '</td>';
					html += '<td>' + item.PhoneNumber + '</td>';
					html += '<td>' + item.CityName + '</td>';
					html += '<td>' + item.DistrictName + '</td>';
					html += '<td>' + item.WardName + '</td>';
					html += '<td><a href="#" onclick="return getbyID(' + item.ID + ')">Edit</a> | <a href="#" onclick="Delele(' + item.ID + ')">Delete</a></td>';
					html += '</tr>';
				});
				$('.tbody').html(html);  
            }
			else {
				alert(result.message)
			}
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}

//Add Data Function
function Add() {
   
    var empObj = {

		Name: $('#Name').val(),
		DateOfBirth:$('#DateOfBirth'),
		Age: $('#Age').val(),
		JobId: $('#job'),
		NationId: $('#nation'),
		IdentityCardNumber: $('#identityCardNumber'),
		PhoneNumber: $('#phoneNumber'),
		CityId: $('#city'),
		DistrictId: $('#district'),
		WardId: $('#ward'),
    };
    $.ajax({
		url: "/InsertEmployee",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
		function (result) {

			if (result.statusCode == 200) {
				loadData();
				$('#myModal').modal('hide');

			}
			else {
				$('#NameError').append(`<option value="0">-- Chọn Thành Phố --</option>`);
			}
           
		},
		
		
    });
}

//Function for getting the Data Based upon Employee ID
//function getbyID(EmpID) {
//    $('#Name').css('border-color', 'lightgrey');
//    $('#Age').css('border-color', 'lightgrey');
//    $('#State').css('border-color', 'lightgrey');
//    $('#Country').css('border-color', 'lightgrey');
//    $.ajax({
//        url: "/Home/getbyID/" + EmpID,
//        typr: "GET",
//        contentType: "application/json;charset=UTF-8",
//        dataType: "json",
//        success: function (result) {
//            $('#EmployeeID').val(result.EmployeeID);
//            $('#Name').val(result.Name);
//            $('#Age').val(result.Age);
//            $('#State').val(result.State);
//            $('#Country').val(result.Country);

//            $('#myModal').modal('show');
//            $('#btnUpdate').show();
//            $('#btnAdd').hide();
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//    return false;
//}

////function for updating employee's record
//function Update() {
//    var res = validate();
//    if (res == false) {
//        return false;
//    }
//    var empObj = {
//        EmployeeID: $('#EmployeeID').val(),
//        Name: $('#Name').val(),
//        Age: $('#Age').val(),
//        State: $('#State').val(),
//        Country: $('#Country').val(),
//    };
//    $.ajax({
//        url: "/Home/Update",
//        data: JSON.stringify(empObj),
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            loadData();
//            $('#myModal').modal('hide');
//            $('#EmployeeID').val("");
//            $('#Name').val("");
//            $('#Age').val("");
//            $('#State').val("");
//            $('#Country').val("");
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}

////function for deleting employee's record
//function Delele(ID) {
//    var ans = confirm("Are you sure you want to delete this Record?");
//    if (ans) {
//        $.ajax({
//            url: "/Home/Delete/" + ID,
//            type: "POST",
//            contentType: "application/json;charset=UTF-8",
//            dataType: "json",
//            success: function (result) {
//                loadData();
//            },
//            error: function (errormessage) {
//                alert(errormessage.responseText);
//            }
//        });
//    }
//}

////Function for clearing the textboxes
//function clearTextBox() {
//    $('#EmployeeID').val("");
//    $('#Name').val("");
//    $('#Age').val("");
//    $('#State').val("");
//    $('#Country').val("");
//    $('#btnUpdate').hide();
//    $('#btnAdd').show();
//    $('#Name').css('border-color', 'lightgrey');
//    $('#Age').css('border-color', 'lightgrey');
//    $('#State').css('border-color', 'lightgrey');
//    $('#Country').css('border-color', 'lightgrey');
//}  
