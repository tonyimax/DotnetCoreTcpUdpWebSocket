using net_conn;

var http = new Http();
var result =  http.CheckIp("106.52.84.138");
result.Wait();