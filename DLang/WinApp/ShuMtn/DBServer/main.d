module main;

import std.stdio;
import network.dblistener;
//import network.testserver;
//import network.testclient;
//import network.mytest;

void main(string[] args)
{
    DBListener dbl = new DBListener;
	dbl.start();
	
	//TestServer es = new TestServer();
	
	//TestClient tc = new TestClient();
	
	//my_test_server();
	    
    stdin.readln();
}

