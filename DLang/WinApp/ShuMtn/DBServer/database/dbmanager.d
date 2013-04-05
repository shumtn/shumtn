module database.dbmanager;

import std.stdio;
import shu.data.database, shu.data.mysql, shu.data.pgsql;

class DBManager
{
private:
	static DBManager m_instance = null;
	static MySql m_db = null;
	//static PgSql m_db = null;

public:
	static DBManager GetApp()
	{
		if(m_instance is null) m_instance = new DBManager;
		
		return m_instance;
	}

	this()
	{
		try
		{
			m_db = new MySql("127.0.0.1", "root", "roots", "test");
			//scope(exit) delete m_db;

			//m_db = new PgSql("host=127.0.0.1 port=5432 user=postgres password=roots dbname=test");
		}
		catch(Exception e)
		{
			writefln(e.msg);
			delete m_db;
		}
	}

	void MySqlStart()
	{
		if(m_db is null) return;

		writefln("MySql===========================");

		//m_db.query("INSERT INTO users (id, name) values (?, ?)", 30, "hello mang");

		foreach(row; m_db.query("SELECT * FROM users"))
		{
			writefln("%s %s %s %s", row["id"], row[0], row[1], row[1]);
			//writefln("%s %s %s %s", row["id"], row[0], row[1], row["name"]);
		}

////		foreach(line; db.query("SELECT * FROM users"))
////		{
////			writeln(line[0], line["name"]);
////		}
	}
		
	void close()
	{
		if(m_db !is null)
		{
			delete m_db;
			m_db = null;
		}
	}
}
