#ifndef _CODE_SQLITE_H_
#define _CODE_SQLITE_H_

#include "CodeUtility.h"

class CodeSQLite : public CodeUtility
{
public:
	static void generateSQLiteCode(string cppDataPath, string cppTablePath, string csDataPath, string csTablePath);
protected:
	//c++
	static void generateCppSQLiteDataFile(const SQLiteInfo& sqliteInfo, string dataFilePath, string tableFilePath);
	static void generateCppSQLiteTotalHeaderFile(const myVector<SQLiteInfo>& sqliteList, string filePath);
	static void generateCppSQLiteRegisteFile(const myVector<SQLiteInfo>& sqliteList, string filePath);
	static void generateCppSQLiteClassDeclare(const myVector<SQLiteInfo>& sqliteList, string filePath);
	static void generateCppSQLiteInstanceDeclare(const myVector<SQLiteInfo>& sqliteList, string filePath);
	//c#
	static void generateCSharpSQLiteDataFile(const SQLiteInfo& sqliteInfo, string dataFilePath, string tableFilePath);
	static void generateCSharpSQLiteRegisteFileFile(const myVector<SQLiteInfo>& sqliteInfo, string filePath);
};

#endif