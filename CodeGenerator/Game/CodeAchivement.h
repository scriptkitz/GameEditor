#ifndef _CODE_ACHIVEMENT_H_
#define _CODE_ACHIVEMENT_H_

#include "CodeUtility.h"

class CodeAchivement : public CodeUtility
{
public:
	static void generateAchivementCode(string headerPath, string achivementPath);
protected:
	static void generateHeaderFile(const myVector<string>& achivementList, string headerPath);
	static void generateAchivementRegister(const myVector<string>& achivementList, string headerPath);
	static void generateAchivementFile(const string& achivement, string achivementPath);
};

#endif