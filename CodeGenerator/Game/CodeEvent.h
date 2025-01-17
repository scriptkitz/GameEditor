#ifndef _CODE_EVENT_H_
#define _CODE_EVENT_H_

#include "CodeUtility.h"

class CodeEvent : public CodeUtility
{
public:
	static void generateEventCode(string stringDefinePath, string headerPath, string eventPath);
protected:
	static void generateHeaderFile(const myVector<pair<string, string>>& eventList, string headerPath);
	static void generateStringDefineEvent(const myVector<pair<string, string>>& eventList, string stringDefinePath);
	static void generateEventType(const myVector<pair<string, string>>& eventList, string headerPath);
	static void generateEventFile(const string& eventName, string eventPath);
};

#endif