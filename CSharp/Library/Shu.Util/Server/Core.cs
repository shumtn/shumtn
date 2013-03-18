using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Text;

namespace Shu.Util
{
	public static class Core
	{
		private static string m_BaseDirectory = null;
        private static string m_ExePath = null;
        private static Assembly m_Assembly = Assembly.GetEntryAssembly();
        public static Assembly Assembly { get { return m_Assembly; } set { m_Assembly = value; } }
		
		public static string ExePath
		{
			get
			{
				if( m_ExePath == null )
				{
					m_ExePath = Assembly.Location;
					//System.Environment.CurrentDirectory;
					//m_ExePath = Process.GetCurrentProcess().MainModule.FileName;
				}

				return m_ExePath;
			}
		}
		
		public static string BaseDirectory
		{
			get
			{
				if( m_BaseDirectory == null )
				{
					try
					{
						m_BaseDirectory = ExePath;

						if( m_BaseDirectory.Length > 0 )
							m_BaseDirectory = Path.GetDirectoryName( m_BaseDirectory );
					}
					catch
					{
						m_BaseDirectory = "";
					}
				}

				return m_BaseDirectory;
			}
		}
		
#if Framework_4_0
		public static readonly bool Is64Bit = Environment.Is64BitProcess;
#else
		public static readonly bool Is64Bit = (IntPtr.Size == 8);	//Returns the size for the current /process/
#endif
		
		public static Version Version { get { return m_Assembly.GetName().Version; } }
		
		private static bool m_VBdotNET = false;
		public static bool VBdotNet { get { return m_VBdotNET; } }
		
		
		
		
		
		
		
		private static int m_ItemCount, m_MobileCount;

		public static int ScriptItems { get { return m_ItemCount; } }
		public static int ScriptMobiles { get { return m_MobileCount; } }
		public static void VerifySerialization()
		{
			m_ItemCount = 0;
			m_MobileCount = 0;

			VerifySerialization( Assembly.GetCallingAssembly() );

			//for( int a = 0; a < ScriptCompiler.Assemblies.Length; ++a )
			//	VerifySerialization( ScriptCompiler.Assemblies[a] );
		}
		
		private static void VerifySerialization( Assembly a )
		{
			if( a == null )
				return;

#if Framework_4_0
			Parallel.ForEach(a.GetTypes(), t => 
				{
					VerifyType(t);
				});
#else
			foreach (Type t in a.GetTypes())
			{
				VerifyType(t);
			}
#endif
		}
		
		private static readonly Type[] m_SerialTypeArray = new Type[1] { typeof(Serial) };
		
		private static void VerifyType( Type t )
		{
			bool isItem = t.IsSubclassOf(typeof(Item));

			//if (isItem || t.IsSubclassOf(typeof(Mobile)))
			if (isItem)
			{
				if (isItem)
				{
					//++m_ItemCount;
					Interlocked.Increment(ref m_ItemCount);
				}
				else
				{
					//++m_MobileCount;
					Interlocked.Increment(ref m_MobileCount);
				}

				StringBuilder warningSb = null;

				try
				{
					/*
					if( isItem && t.IsPublic && !t.IsAbstract )
					{
						ConstructorInfo cInfo = t.GetConstructor( Type.EmptyTypes );

						if( cInfo == null )
						{
							if (warningSb == null)
								warningSb = new StringBuilder();

							warningSb.AppendLine("       - No zero paramater constructor");
						}
					}*/

					if (t.GetConstructor(m_SerialTypeArray) == null)
					{
						if (warningSb == null)
							warningSb = new StringBuilder();

						warningSb.AppendLine("       - No serialization constructor");
					}

					if (t.GetMethod("Serialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) == null)
					{
						if (warningSb == null)
							warningSb = new StringBuilder();

						warningSb.AppendLine("       - No Serialize() method");
					}

					if (t.GetMethod("Deserialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly) == null)
					{
						if (warningSb == null)
							warningSb = new StringBuilder();

						warningSb.AppendLine("       - No Deserialize() method");
					}

					if (warningSb != null && warningSb.Length > 0)
					{
						Console.WriteLine("Warning: {0}\n{1}", t, warningSb.ToString());
					}
				}
				catch
				{
					Console.WriteLine("Warning: Exception in serialization verification of type {0}", t);
				}
			}
		}
	}
}

