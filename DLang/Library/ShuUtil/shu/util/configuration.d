module shu.util.configuration;

import std.xml;
import std.file;
import std.outbuffer;
import std.stdio;

class ConfigurationManager
{
	public string m_FileName = null;

	public Document m_Document = null;

	public static AppsValueCollection AppSettings = null;
	public string[string] ConnectionStrings = null; 
		
	public:	
		this ()
		{
			
		}

		this(string sql_conn)
		{
			this();
			this.Open(sql_conn);
		}

		void Open(string name)
		{
			m_FileName = cast(string)read(name ~= ".config");
			m_Document = new Document(this.m_FileName);
		}

	/// <summary>
	/// 获取 AppSettings 节点
	/// </summary>
	//static string AppSettings() 
	//{
		//AppsValueCollection avc = new AppsValueCollection();
		
	//	return "Yes";
	//}
	//void AppSettings(int newproperty) { myprop = newproperty; }

	/*public class AppSettings
	{
		string opSlice(string key);
	}*/

	public class AppsValueCollection
	{
		//string opSlice(string key);
		/*{
			if(key != null)
			{
				return key;
			}
			else
			{
				return null;
			}
		}*/

		
		private Document xmlDoc = null;
		private string m_Value = null;
		public string[string] AppsValue;

		public this()
		{
			xmlDoc = new Document(m_FileName);
		}

		public this(string xx)
		{
			this();
			writeln(xmlDoc.toString());
			//return xmlDoc.toString(); 
		}

	}

	public void ConfigCreate(string name)
	{
		auto docs = new Document(new Tag("configuration"));
		auto app = new Element("appSettings");
		auto appAdd = new Tag("add",TagType.EMPTY);
		appAdd.attr["key"] = "ServerManage";
		appAdd.attr["value"] = "         ☆☆☆天行者 ☆ 服务器端组☆☆☆";
		app ~= new Element(appAdd);		
		docs ~= app;

		auto con = new Element("connectionStrings");
		auto add = new Tag("add",TagType.EMPTY);
		add.attr["name"] = "UserDbServer";
		add.attr["connectionString"] = "Server=127.0.0.1;Port=5432;Database=UsersDb;User id=postgres;password=Windows2008;encoding=unicode;";
		add.attr["providerName"] =  "Wdd.Data.PgSql";
		con ~= new Element(add);
		docs ~= con;

		OutBuffer ob= new OutBuffer;
		ob.write(docs.toString());
		std.file.write(name ~= ".config", ob.toBytes);
	}
} 
