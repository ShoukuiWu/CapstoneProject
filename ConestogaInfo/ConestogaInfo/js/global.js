var db;
var createTime;
var dropTime;

var Type;
var Url;
var Data;
var ContentType;
var DataType;
var ProcessData;
var method;

function errorHandler(transaction, error) {
	alert("SQL error: " + error.message);
};

$(document).ready(function() {

	//open database
	db = openDatabase('sessionSum', '1.0', 'summary', 2 * 1024 * 1024);

	//Creat tables if not exists
	createTables();

	//click search button
	$("#searchButton").on("click", getHtml);

	//click sum button
	$("#summary").on("click", showSum);

	$("#searchBox").on("input", searchKeyword);
	
//	$("#emailResults").on("click", formEmail);
	
	$("#topten_btn").on("click", searchTopten);

	$("#sendEmail").on("click", formEmail);

});

// get suggestion for key words ready

function searchKeyword() {
	var input = $("#searchBox").val();

	var uesrid = "1";
	Type = "GET";
	Data = input;
	// Url = "http://localhost:52136/ServiceConeInfo.svc/GetKeywordNameAll";
	Url = "http://localhost:52136/ServiceConeInfo.svc/GetKeywordByQuery?query=" + input;
	DataType = "jsonp";
	ProcessData = false;
	method = "GetKeywordNameAll";
	CallService();
}

function CallService() {
	$.ajax({
		type : Type, //GET or POST or PUT or DELETE verb
		url : Url, // Location of the service
		data : Data, //Data sent to server
		contentType : ContentType, // content type sent to server
		dataType : DataType, //Expected data format from server
		processdata : ProcessData, //True or False
		success : function(msg) {//On Successfull service call
			putInAutocomplete(msg);
			//putInAutocomplete(msg);
		},
		error : ServiceFailed// When Service call fails
	});
}

function ServiceSucceeded(result) {

	var keyAuto = new Array();
	for ( i = 0; i < result.length; i++) {
		keyAuto[i] = result[i];
	}
	$("#question").html(keyAuto);

	if (DataType == "jsonp") {
		//if (method == "CreateEmployee") {
		if (method == "GetKeywordNameAll") {
			alert(result);
		} else {
			resultObject = result.GetEmployeeResult;
			var string = result.Name + " \n " + result.Address;
			alert(string);
		}
	}
}

function ServiceFailed(result) {
	alert('Service call failed: ' + result.status + '' + result.statusText);
	Type = null;
	Url = null;
	Data = null;
	ContentType = null;
	DataType = null;
	ProcessData = null;
}

// render returned json as key words autocomplete source
function putInAutocomplete(msg) {
	// alert(msg);
	var keyAuto = new Array();
	for ( i = 0; i < msg.length; i++) {
		keyAuto[i] = msg[i];
	}
	$("#searchBox").autocomplete({
		source : function(request, response) {
			var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");
			response($.grep(keyAuto, function(item) {
				return matcher.test(item);
			}));
		}
	});

}

/* Create table */

function createTables() {

	createTime = new Date().getTime();

	// create the table if it doesn't exist
	db.transaction(function(transaction) {
		var sqlString = "CREATE TABLE IF NOT EXISTS sum (" + " id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," + " keyWord VARCHAR ," + " question VARCHAR ," + " answer TEXT," + " time DATE" + ");";
		transaction.executeSql(sqlString, [], null, errorHandler);
	}, errorHandler);
}

// when click search button, send query to server; empty question and answer, append returned data
function getHtml() {

	var input = $("#searchBox").val();

	if (input != null && input != "") {

		var uesrid = "1";
		Type = "GET";
		Data = input;
		// Url = "http://localhost:52136/ServiceConeInfo.svc/GetKeywordNameAll";
		Url = "http://localhost:52136/ServiceConeInfo.svc/GetQuestionAnswerByQuery?query=" + input;
		DataType = "jsonp";
		ProcessData = false;
		method = "GetQuestionAnswerByQuery";
		CallServiceForAnswer();

	} else {

	}

}

function CallServiceForAnswer() {
	$.ajax({
		type : Type, //GET or POST or PUT or DELETE verb
		url : Url, // Location of the service
		data : Data, //Data sent to server
		contentType : ContentType, // content type sent to server
		dataType : DataType, //Expected data format from server
		processdata : ProcessData, //True or False
		success : function(msg) {//On Successfull service call
			renderHtml(msg);

		},
		error : ServiceFailed// When Service call fails
	});
}

//render search result in page
function renderHtml(msg) {

	var time = new Date().getTime();
	var keyWord = $("#searchBox").val();

	// remove summary
	$("#lstAll").empty();
	$("#main").empty();
	$("#lstSearch").empty();

	

	// initialise a string to hold the html line items
	var htmlStr = '';
	var aRow = null;

	if (msg.length == 0) {
		htmlStr += '<h4>No search result found.</h4>';
	}

	// read each of the rows from the results
	for (var i = 0; i < msg.length; i++) {

		aRow = msg[i];

		// Note: store the PK (id) as data-row-id attribute
		htmlStr += '<li class="listviewItem" ' + '">' + '<h4>' + aRow['question'] + '</h4>' + '<p>' + aRow['answer'] + '</p>' +  '</li>';

		var question = msg[i].question;
		var answer = msg[i].answer;

		addSummary(keyWord, question, answer, time);
	}// for

	// alert(htmlStr);
	$("#lstSearch").empty();
	$("#lstSearch").append(htmlStr);
	// $("#lstSearch").listview('refresh');

}

// add record to summary table
function addSummary(keyWord, question, answer, time) {

	// drop table if need.
	rebuildTables();

	var sqlString = "INSERT INTO sum (keyWord, question, answer, time)" + "VALUES (?, ?, ?, ?);";

	db.transaction(function(tx) {
		tx.executeSql(sqlString, [keyWord, question, answer, time], function(tx, res) {
			console.debug("DEBUG: Review record added");

		}, errorHandler);

	});
}

//rebuild table after each 2 hours
function rebuildTables() {

	dropTime = new Date().getTime();

	var eclipse = createTime - dropTime;

	// 7200 seconds, rebuild table
	if ((dropTime - createTime) / 1000 > 720) {

		try {
			var query = "DROP TABLE sum";
			db.transaction(function(transaction) {
				transaction.executeSql(query, [], null, errorHandler);

			});
			createTables;
		} catch (e) {
			alert("Error: Unable to drop table " + e + ".");
			return;
		}
	}

}

// summary button on click, show search summary or alert
function showSum() {

	db.transaction(function(tx) {
		tx.executeSql("SELECT * FROM sum", [], function(tx, res) {
			if (res.rows.length == 0) {
				alert("No search sum now.");
			} else {
				
				$("#main").empty();
				$("#lstAll").empty();

				getSum();
			}
		}, errorHandler);

	});
}

//
function getSum() {
	db.transaction(function(transaction) {
		transaction.executeSql("SELECT *FROM sum ORDER BY id", [], function(transaction, results) {
			onGetSumSuccess(results);
		}, errorHandler);
	});
}

function onGetSumSuccess(results) {
	// alert("rows: " + results.rows.length);

	// initialise a string to hold the html line items
	var htmlStr = '';
	var aRow = null;

	if (results.rows.length == 0) {
		htmlStr += '<h4>No records found.</h4>';
	}

	// read each of the rows from the results
	for (var i = 0; i < results.rows.length; i++) {
		//  htmlStr += JSON.stringify(results.rows.item(i));
		aRow = results.rows.item(i);

		// Note: store the PK (id) as data-row-id attribute
		htmlStr += '<li class="listviewItem" ' + '">' + '<h5>'+'Key word: ' +aRow['keyWord']+'<br/>'+'</h5><h4>'+'Question: ' + aRow['question'] + '</h4>' + '<p>' + aRow['answer'] + '</p>' +  '</li><br/>';
	}// for

	// alert(htmlStr);
	$("#lstSearch").empty();
	$("#lstAll").empty();
	$("#lstAll").append(htmlStr);
	// $("#lstAll").listview('refresh');
	
}

//create email
/*
function formEmail(){
	var email = $("#toCreateEmail").val();
	$("textarea#emailContent").val(email);
}
*/

//For data of E-mail
//	1)RecevierEmail
//	2)RecevierName
//	3)SenderEmail
//	4)emailContent
//Send mail to someone
function formEmail(){
	var mailData = new Array();
	mailData[0] = $("#RecevierEmail").val();
	mailData[1] = $("#RecevierName").val();
	mailData[2] = $("#SenderEmail").val();
	mailData[3] = $("#emailContent").val();
	Type = "GET";
	Data = [{"emailToAddress":mailData[0],"name":mailData[1],"address":mailData[2],"body":mailData[3]}];	
	Url = "http://localhost:52136/ServiceConeInfo.svc/SendEmail?emailToAddress="+mailData[0]+"&name="+mailData[1]+"&address="+mailData[2]+"&body="+mailData[3];
	DataType = "jsonp";
	ProcessData = false;
	method = "GetTopTenAll";
	CallSendMailService();
}

function CallSendMailService(){
	$.ajax({
		type : Type, //GET or POST or PUT or DELETE verb
		url : Url, // Location of the service
	//	data : Data, //Data sent to server
		data : { param : "value" },
		contentType : ContentType, // content type sent to server
		dataType : DataType, //Expected data format from server
		processdata : ProcessData, //True or False
		success : function(msg) {//On Successfull service call
			renderHtmlSendMail(msg);
		},
		error : ServiceFailed// When Service call fails
	});
}

function renderHtmlSendMail(msg){
	alert(msg);
	
	
}

//display Topten
function searchTopten(){
	//alert(searchTopten);
	Type = "GET";
	Url = "http://localhost:52136/ServiceConeInfo.svc/GetTopTenAll";
	//Url = "http://localhost:52136/ServiceConeInfo.svc/GetTopTenAll";
	DataType = "jsonp";
	ProcessData = false;
	method = "GetTopTenAll";
	CallToptenService();
}

function CallToptenService(){
	//alert(CallToptenService);
	$.ajax({
		type : Type, //GET or POST or PUT or DELETE verb
		url : Url, // Location of the service
		data : Data, //Data sent to server
		contentType : ContentType, // content type sent to server
		dataType : DataType, //Expected data format from server
		processdata : ProcessData, //True or False
		success : function(msg) {//On Successfull service call
			renderHtmlTopten(msg);
		},
		error : ServiceFailed// When Service call fails
	});
}

function renderHtmlTopten(msg) {
	var keyWord = $("#searchBox").val();

	// remove summary
	$("#lstAll").empty();
	$("#main").empty();
	$("#lstSearch").empty();

	// initialise a string to hold the html line items
	var htmlStr = '';
	var aRow = null;
	if (msg.length == 0) {
		htmlStr += '<h4>No search result found.</h4>';
	}

	// read each of the rows from the results
	for (var i = 0; i < msg.length; i++) {
		aRow = msg[i];
		// Note: store the PK (id) as data-row-id attribute
		var num = msg.length - i;
		var question = msg[i].question;
		var answer = msg[i].answer;
		htmlStr += '<li class="listviewItem" ' + '">' + '<h5>'+'[Top: ' + num + ']' +'<br/>' + '<p>' + '<h4>' + aRow['question'] + '</h4>' + '<p>' + aRow['answer'] + '</p>' +  '</li>';		
	}// for

	$("#lstSearch").empty();
	$("#lstSearch").append(htmlStr);
}
