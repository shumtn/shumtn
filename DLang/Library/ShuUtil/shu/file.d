module shu.file;

import std.file;
import std.stdio;
import std.array;
import std.conv;

// 读取回调
alias void delegate (int row, int col, char[][int][int] dataRow, bool isEnd ) ReadTextTableCallback;

class JFile
{
	private:		
		char[][int][int] m_dataRow;
		int m_rowIndex = 0;
		int m_colIndex = 0;
	public:		
		@property
		{
			int rowIndex(){ return m_rowIndex; }
			int colIndex(){ return m_colIndex; }
		}
	
	this()
	{
		
	}
	
	/**读取配置表*/
	char[][int][int] readTextTable(string fileName="", int startIndex=0, ReadTextTableCallback readCallback = null)
	{		
		if( !exists(fileName) )
		{
			writefln( "Read Error=>" ~ fileName ~ " is not exists!" );
			return null;
		}
		
		std.stdio.File f = std.stdio.File(fileName);
		int tempIndex	= 0;
		foreach( strLine; f.byLine() )
        {
			if( tempIndex >= startIndex )
			{
				auto colList = split( strLine, "\t" );
				
				for( int i = 0; i < colList.length; i++ )
				{					
					m_colIndex = i;
					
					//m_dataRow[m_rowIndex][m_colIndex] ~= to!(string)(colList[m_colIndex]);
					m_dataRow[m_rowIndex][m_colIndex] ~= colList[m_colIndex];
					
					if( readCallback !is null )
						readCallback( m_rowIndex, m_colIndex, m_dataRow, false );
				}
				
				m_rowIndex++;
			}
			
			tempIndex++;
        }
		
		m_colIndex = m_colIndex <= 0 ? 0 : m_colIndex + 1;
		
		if( readCallback !is null )
			readCallback( m_rowIndex, m_colIndex, m_dataRow, true );
		
		f.close();
		
		return m_dataRow;
	}
	
	/** 关闭 */
	void close()
	{
		m_dataRow = null;
		m_rowIndex = 0;
		m_colIndex = 0;
	}
}
